using Microsoft.Extensions.DependencyInjection.Extensions;
using Orcamentaria.EquipmentService.Application.Services;
using Orcamentaria.EquipmentService.Application.Validators;
using Orcamentaria.EquipmentService.Domain.Mappers;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Domain.Services;
using Orcamentaria.EquipmentService.Domain.Services.Internal;
using Orcamentaria.EquipmentService.Infrastructure.Contexts;
using Orcamentaria.EquipmentService.Infrastructure.Repositories;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.Lib.Infrastructure.Configures;

namespace Orcamentaria.EquipmentService.API
{
    public class Startup
    {
        private readonly string _serviceName = "Orcamentaria.EquipmentService";
        private readonly string _apiVersion = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = services.ResolveConfigs(Configuration, _serviceName);

            services.Replace(ServiceDescriptor.Singleton(Configuration));

            services.AddServiceRegistryHosted(Configuration);

            services.ResolveCommonServicesWithMySql<MySqlContext>(configuration: Configuration,
                serviceName: _serviceName,
                apiVersion: _apiVersion,
                customServices: () =>
                {
                    //Mappers
                    services.AddAutoMapper(_ => { },
                        typeof(EquipmentMapper),
                        typeof(EquipmentTypeMapper),
                        typeof(EquipmentMaintenanceMapper));

                    //Repositories
                    services.AddScoped<IEquipmentRepository, EquipmentRespository>();
                    services.AddScoped<IEquipmentTypeRepository, EquipmentTypeRespository>();
                    services.AddScoped<IEquipmentMaintenanceRepository, EquipmentMaintenanceRespository>();

                    //Services
                    services.AddScoped<IEquipmentService, Application.Services.EquipmentService>();
                    services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
                    services.AddScoped<IEquipmentMaintenanceService, EquipmentMaintenanceService>();

                    //Internal services 
                    services.AddScoped<IEquipmentInternalService, Application.Services.EquipmentService>();
                    services.AddScoped<IEquipmentTypeInternalService, EquipmentTypeService>();

                    //Validators
                    services.AddScoped<IValidatorEntity<Equipment>, EquipmentValidator>();
                    services.AddScoped<IValidatorEntity<EquipmentType>, EquipmentTypeValidator>();
                    services.AddScoped<IValidatorEntity<EquipmentMaintenance>, EquipmentMaintenanceValidator>();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app.ConfigureCommon(env, _serviceName, _apiVersion);
    }
}
