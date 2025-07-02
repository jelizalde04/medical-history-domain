using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using delete_medical.Data;
using delete_medical.Models;
using System.Text;

namespace delete_medical
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Load();

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new InvalidOperationException("JWT_SECRET not set.");

            var petDbName = Environment.GetEnvironmentVariable("PET_DB_NAME")
                ?? throw new InvalidOperationException("PET_DB_NAME not set.");
            var medicalDbName = Environment.GetEnvironmentVariable("MEDICAL_DB_NAME")
                ?? throw new InvalidOperationException("MEDICAL_DB_NAME not set.");

            string medicalConnectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={medicalDbName};";
            string petConnectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={petDbName};";

            builder.Services.AddDbContext<PetContext>(options =>
                options.UseNpgsql(petConnectionString));

            builder.Services.AddDbContext<PetMedicalContext>(options =>
                options.UseNpgsql(medicalConnectionString));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                    };
                });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PetMedicalHistoryAPI",
                    Version = "v1",
                    Description = "API for viewing pet medical history records."
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                                    Enter 'Bearer' [space] and then your token in the text input below.
                                    Example: 'Bearer eyJhb...'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetMedicalHistoryAPI v1");
                    c.RoutePrefix = "api-docs-deleteMedical";
                });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.MapControllers();
            
            app.MapGet("/health", () => Results.Ok("Healthy"));

            app.Run();
        }
    }
}
