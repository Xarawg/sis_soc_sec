using AutoMapper.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_AspNetCore.Configurations.Mappings;
using SecurityService_Core.Interfaces;
using SecurityService_Core.Security;
using SecurityService_Core_Stores;
using SecurityService_Core_Stores.Stores;
using System.Reflection;

namespace Security_Service_AspNetCore
{
    /// <summary>
    /// Класс расширения сервисов проекта
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Метод расширения сервисов program.cs
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="tokenOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddExtensions(this IServiceCollection services, ProjectOptions options, TokenOptions tokenOptions)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder.UseDatabase("CustomerContext", options);
            var connectionOptions = options.ConnectionStrings["CustomerContext"];
            services.AddScoped(x => new CustomerContext(optionsBuilder.Options));
            services.AddHttpContextAccessor();
            services.AddControllers().AddNewtonsoftJson();

            services.AddCors((options) =>
            {
                options.AddPolicy("DefaultPolicyWithOrigin", (builder) =>
                {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(host => true)
                    .WithOrigins(tokenOptions.AllowedAudiences);
                });
            });

            services.AddCors((options) =>
            {
                options.AddPolicy("DefaultPolicyWithOrigin", (builder) =>
                {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(host => true);
                });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Сервер использует аутентификацию на основе JWT токена авторизации. 
                        Добавьте токен авторизации в виде: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper(conf =>
            {
                conf.AddDataReaderMapping(false);
                conf.AddProfile<UserMap>();
                conf.AddProfile<OrderMap>();
                conf.AddProfile<DocscanMap>();
            });

            var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,  // указывает, будет ли валидироваться издатель при валидации токена
                        ValidateAudience = true, // будет ли валидироваться потребитель токена
                        ValidateLifetime = true, // будет ли валидироваться время существования
                        ValidateIssuerSigningKey = true, // валидация ключа безопасности
                        ValidIssuer = tokenOptions.Issuer, // строка, представляющая издателя
                        ValidAudiences = tokenOptions.AllowedAudiences, // установка потребителей токена
                        IssuerSigningKey = signingConfigurations.SecurityKey, // установка ключа безопасности
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            services.AddAuthorization();

            services.AddScoped<AdministratorService>();
            services.AddScoped<OperatorService>();
            services.AddScoped<UserService>();

            services.AddScoped<IAdministratorStore, AdministratorStore>();
            services.AddScoped<IOperatorStore, OperatorStore>();
            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<ITokenHandler, SecurityService_Core.Security.TokenHandler>();

            return services;
        }

        /// <summary>
		/// Конфигурация DbContext для использования базы данных, в зависимости от настроек.
		/// </summary>
		/// <param name="optionsBuilder">Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.</param>
		/// <param name="connectionStringName">Наименование строки соединения, которая будет применена из настроек web-приложения.</param>
		/// <param name="options">Настройки web-приложения.</param>
		private static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder optionsBuilder, string connectionStringName, ProjectOptions options)
        {
            if (options.ConnectionStrings.TryGetValue(connectionStringName, out ConnectionStringOptions? value))
            {
                var connectionOptions = value;
                optionsBuilder
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseNpgsql(connectionOptions.ConnectionString, (builder) =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });
            }
            return optionsBuilder;
        }
    }
}
