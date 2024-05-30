using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WindSync.BLL.Services.AlertService;
using WindSync.BLL.Services.Auth;
using WindSync.BLL.Services.TurbineDataService;
using WindSync.BLL.Services.TurbineService;
using WindSync.BLL.Services.WindFarmService;
using WindSync.BLL.Utils;
using WindSync.Core.Models;
using WindSync.Core.Utils;
using WindSync.DAL.DB;
using WindSync.DAL.Repositories.AlertRepository;
using WindSync.DAL.Repositories.TurbineDataRepository;
using WindSync.DAL.Repositories.TurbineRepository;
using WindSync.DAL.Repositories.WindFarmRepository;

namespace WindSync.PL.Configuration;

public static class Configuration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Standart services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IAlertService, AlertService>();
        builder.Services.AddScoped<ITurbineDataService, TurbineDataService>();
        builder.Services.AddScoped<ITurbineService, TurbineService>();
        builder.Services.AddScoped<IWindFarmService, WindFarmService>();

        // Helpers
        builder.Services.AddScoped<ITurbineDataHelper, TurbineDataHelper>();

        // Repositories
        builder.Services.AddScoped<IAlertRepository, AlertRepository>();
        builder.Services.AddScoped<ITurbineDataRepository, TurbineDataRepository>();
        builder.Services.AddScoped<ITurbineRepository, TurbineRepository>();
        builder.Services.AddScoped<IWindFarmRepository, WindFarmRepository>();

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

        // CORS settings
        var corsName = builder.Configuration[Constants.AngularCorsName];
        var corsOrigin = builder.Configuration[Constants.AngularCorsOrigin];

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: corsName,
                builder =>
                {
                    builder.WithOrigins(corsOrigin)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
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
