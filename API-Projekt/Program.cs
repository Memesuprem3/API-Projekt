using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services;
using Microsoft.OpenApi.Models;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using System.Collections.Generic;
using System.Windows.Input;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
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

        // Repositories
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddScoped<IAuthentication, AuthenticationRepo>();
        builder.Services.AddScoped<IUser, UserRepo>();
        builder.Services.AddScoped<ICustomer, CustomerRepo>();
        builder.Services.AddScoped<ICompany, CompanyRepo>();
        builder.Services.AddScoped<IAppointment, AppointmentRepo>();

        // DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

        // JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        // Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
            options.AddPolicy("CustomerOrCompanyPolicy", policy => policy.RequireRole("Customer", "Company"));
            options.AddPolicy("CompanyPolicy", policy => policy.RequireRole("Company"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}