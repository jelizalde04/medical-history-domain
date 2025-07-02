using create_medical.Data;
using create_medical.Models;

using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace create_medical

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables from the .env file
            Env.Load();

            // Get the credentials from the .env file
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET no est� definido en el entorno.");

            // Get the database names from the environment
            var petDbName = Environment.GetEnvironmentVariable("PET_DB_NAME") ?? throw new InvalidOperationException("PET_DB_NAME no est� definido en el entorno.");
            var medicalDbName = Environment.GetEnvironmentVariable("MEDICAL_DB_NAME") ?? throw new InvalidOperationException("MEDICAL_DB_NAME no est� definido en el entorno.");

            // connecting to the medical database
            string medicalConnectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={medicalDbName};";

            // connecting to the pet database
            string petConnectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={petDbName};";

            // Add both contexts with their connection strings
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

                    // To accept the token directly without the word 'Bearer'
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var authorization = context.Request.Headers["Authorization"].ToString();
                            if (!string.IsNullOrEmpty(authorization))
                            {
                                var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                                    ? authorization.Substring("Bearer ".Length).Trim()
                                    : authorization.Trim();

                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
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

            // Configure Swagger with custom route and lock for JWT without "Bearer"
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PetMedicalHistoryAPI", Version = "v1" });
                c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Introduce solo el token JWT, sin la palabra 'Bearer'.",
                    Scheme = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWT"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Middleware Configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetMedicalHistoryAPI v1");
                    c.RoutePrefix = "api-docs-createMedical";
                });
            }

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
