﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WindSync.BLL.Services;
using WindSync.BLL.Utils;
using WindSync.Core.Models;
using WindSync.Core.Utils;
using WindSync.DAL.DB;

namespace WindSync.PL.Configuration;

public static class Configuration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Standart services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Custom services
        builder.Services.AddScoped<IAuthService, AuthService>();

        // Auto Mapper configuration
        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        // Routing configuration
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        // JWT configuration for auth service
        var jwtConfiguration = new JwtConfiguration
        {
            Key = builder.Configuration[Constants.JwtKeyProperty],
            Issuer = builder.Configuration[Constants.JwtIssuerProperty],
            Audience = builder.Configuration[Constants.JwtAudienceProperty]
        };
        builder.Services.AddSingleton(jwtConfiguration);

        // Db Context configuration
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));

        // Identity User configuration
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        // Authentication configuration
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        // Jwt Bearer configuration
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration[Constants.JwtAudienceProperty],
                ValidIssuer = builder.Configuration[Constants.JwtIssuerProperty],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Constants.JwtKeyProperty]))
            };
        });
    }

    public static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "WindSync API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[]{}
                }
            });
        });
    }
}