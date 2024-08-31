using EduLink.Data;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace EduLink
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            // Configure JSON options to handle object cycles
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });



            // Get the connection string settings 
            string ConnectionStringVar = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<EduLinkDbContext>(opt => opt.UseSqlServer(ConnectionStringVar));

            // Register repositories
            //Ex:
           builder.Services.AddScoped<IVolunteer, VolunteerService>();

            builder.Services.AddScoped<IStudent, StudentService>();

            //For JWT Later
            //builder.Services.AddScoped<JwtTokenService>();


            // Swagger Config
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Tunify API",
                    Version = "v1",
                    Description = "API for managing playlists, songs, and artists in the Tunify Platform"
                });
                //HERE For JWT
                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "Please enter user token below."
                //});

                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //    {
                //        {
                //            new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //                }
                //            },
                //            Array.Empty<string>()
                //        }
                //    });

            });

            //Configure Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                   .AddEntityFrameworkStores<EduLinkDbContext>();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            // add auth service to the app using jwt

            //builder.Services.AddAuthentication(
            //    options =>
            //    {
            //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    }
            //    ).AddJwtBearer(
            //    options =>
            //    {
            //        options.TokenValidationParameters = JwtTokenService.ValidateToken(builder.Configuration);
            //    });


            var app = builder.Build();

            // Use Swagger Service
            app.UseSwagger(
             options =>
             {
                 options.RouteTemplate = "api/{documentName}/swagger.json";
             });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/v1/swagger.json", "EduLink API v1");
                options.RoutePrefix = "EduLinkSwagger";
            });

            // Add redirection from root URL to Swagger UI
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/EduLinkSwagger/index.html");
                }
                else
                {
                    await next();
                }
            });

            //Authentication
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.MapControllers();

            //app.MapGet("/", () => "Hello World!");

            app.Run();

        }
    }
}
