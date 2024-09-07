using EduLink.Data;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
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

            builder.Services.AddScoped<IVolunteer, VolunteerService>();

            builder.Services.AddScoped<IAdmin, AdminService>();


            builder.Services.AddScoped<JwtTokenService>();

            // Configure Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // Add custom options if needed
                // options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                // options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddEntityFrameworkStores<EduLinkDbContext>()
            .AddDefaultTokenProviders(); // Ensure default token providers are registered

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

            // Configure Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
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

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
