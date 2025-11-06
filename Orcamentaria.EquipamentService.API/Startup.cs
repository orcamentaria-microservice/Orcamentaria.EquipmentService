using Microsoft.Extensions.DependencyInjection.Extensions;
using Orcamentaria.EquipamentService.Application.Services;
using Orcamentaria.EquipamentService.Domain.Mappers;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.EquipamentService.Infrastructure.Contexts;
using Orcamentaria.EquipamentService.Infrastructure.Repositories;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.Lib.Infrastructure;
using Orcamentaria.PersonService.Application.Validators;

namespace Orcamentaria.EquipamentService.API
{
    public class Startup
    {
        private readonly string _serviceName = "Orcamentaria.EquipamentService";
        private readonly string _apiVersion = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = CommonDI.ResolveConfigs(_serviceName, services, Configuration);
            services.Replace(ServiceDescriptor.Singleton(Configuration));

            CommonDI.AddServiceRegistryHosted(services, Configuration);

            CommonDI.ResolveCommonServicesWithMySql<MySqlContext>(
                serviceName: _serviceName,
                apiVersion: _apiVersion,
                services: services,
                configuration: Configuration,
                customServices: () =>
                {
                    //Mappers
                    services.AddAutoMapper(
                        typeof(EquipamentMapper),
                        typeof(EquipamentTypeMapper),
                        typeof(EquipamentMaintenanceMapper));

                    //Repositories
                    services.AddScoped<IEquipamentRepository, EquipamentRespository>();
                    services.AddScoped<IEquipamentTypeRepository, EquipamentTypeRespository>();
                    services.AddScoped<IEquipamentMaintenanceRepository, EquipamentMaintenanceRespository>();

                    //Services
                    services.AddScoped<IEquipamentService, Application.Services.EquipamentService>();
                    services.AddScoped<IEquipamentTypeService, EquipamentTypeService>();
                    services.AddScoped<IEquipamentMaintenanceService, EquipamentMaintenanceService>();

                    //Validators
                    services.AddScoped<IValidatorEntity<Equipament>, EquipamentValidator>();
                    services.AddScoped<IValidatorEntity<EquipamentType>, EquipamentTypeValidator>();
                    services.AddScoped<IValidatorEntity<EquipamentMaintenance>, EquipamentMaintenanceValidator>();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
			=> CommonDI.ConfigureCommon(_serviceName, _apiVersion, app, env);
    }
}
