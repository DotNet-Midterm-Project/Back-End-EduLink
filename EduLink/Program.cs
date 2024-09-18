using EduLink.Data;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace EduLink
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure JSON options to handle object cycles
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Get the connection string settings 
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Configure database context
            builder.Services.AddDbContext<EduLinkDbContext>(opt => opt.UseSqlServer(connectionString));

            // Register services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAccount, IdentityAccountService>();
            builder.Services.AddScoped<HelperService>();
            builder.Services.AddScoped<IStudent, StudentService>();
            builder.Services.AddScoped<ICommon, CommonService>();
            builder.Services.AddScoped<IComment, CommentService>();
            builder.Services.AddScoped<IVolunteer, VolunteerService>();
            builder.Services.AddScoped<IAdmin, AdminService>();
            builder.Services.AddScoped<IFile, FileService>();
            builder.Services.AddScoped<IGroup, GroupService>();
            builder.Services.AddScoped<JwtTokenService>();
            builder.Services.AddScoped<IMeetingService, MeetingService>();  

            builder.Services.AddScoped<ITask, TaskService>();
            // Configure Identity
            builder.Services.AddIdentity<User, IdentityRole>(options => { })
                .AddEntityFrameworkStores<EduLinkDbContext>()
                .AddDefaultTokenProviders();

            // Configure authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenService.ValidateToken(builder.Configuration);
            });


            builder.Services.AddAuthorization(options =>
            {
                // You can define policies if needed, or use the default policy
                options.AddPolicy("DefaultPolicy", policy =>
                    policy.RequireAuthenticatedUser());
            });

            // Enable CORS for all origins (adjust for production)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSwaggerUI", policy =>
                {
                    policy.WithOrigins("http://localhost:5085") // Replace {PORT} with the actual port of your API, e.g., 5000
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            // Configure Hangfire
            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connectionString); 
            });

            // add Hangfire Dashboard
            builder.Services.AddHangfireServer();


            // Configure Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EduLink API",
                    Version = "v1",
                    Description = "API for managing student, volunteer, and courses in the EduLink Platform"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter user token below."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            //Cors
            app.UseCors("AllowSwaggerUI");

            // Use Swagger
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/v1/swagger.json", "EduLink API v1");
                options.RoutePrefix = "EduLink";
            });

            // Redirect root URL to Swagger UI
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/EduLink/index.html");
                }
                else
                {
                    await next();
                }
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
          Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
                RequestPath = "/Resources"
            });

            // Hangfire Dashboard
            app.UseHangfireDashboard();
            // Schedule tasks using Hangfire
            RecurringJob.AddOrUpdate<HelperService>(
                "update-events-statuses",
                service => service.UpdateEventStatusesAsync(),
                Cron.Hourly); // The task is executed every hour
            RecurringJob.AddOrUpdate<HelperService>(
                "update-Sessions-statuses",
                service => service.UpdateSessionStatusesAsync(),
                Cron.Hourly); // The task is executed every hour
            RecurringJob.AddOrUpdate<HelperService>(
                "update-booking-statuses",
                service => service.UpdateBookingStatusesAsync(),
                Cron.Minutely); // The task is executed every hour

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
