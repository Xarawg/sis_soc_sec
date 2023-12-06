using AutoMapper.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security_Service_AspNetCore.Services;
using SecurityService_AspNetCore.Configurations;
using SecurityService_AspNetCore.Configurations.Mappings;
using SecurityService_Core.Interfaces;
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
        /// <returns></returns>
        public static IServiceCollection AddExtensions(this IServiceCollection services, ProjectOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder.UseDatabase("CustomerContext", options);
            var connectionOptions = options.ConnectionStrings["CustomerContext"];
            services.AddScoped(x => new CustomerContext(optionsBuilder.Options));

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

            services.AddScoped<AdministratorService>();
            services.AddScoped<OperatorService>();
            services.AddScoped<UserService>();

            services.AddScoped<IAdministratorStore, AdministratorStore>();
            services.AddScoped<IOperatorStore, OperatorStore>();
            services.AddScoped<IUserStore, UserStore>();

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
