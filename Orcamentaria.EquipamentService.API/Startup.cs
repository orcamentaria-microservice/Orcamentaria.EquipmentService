using Microsoft.Extensions.DependencyInjection.Extensions;
using Orcamentaria.EquipamentService.Application.Services;
using Orcamentaria.EquipamentService.Application.Validators;
using Orcamentaria.EquipamentService.Domain.Mappers;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Domain.Services;
using Orcamentaria.EquipamentService.Infrastructure.Contexts;
using Orcamentaria.EquipamentService.Infrastructure.Repositories;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.Lib.Infrastructure.Configures;

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
            => app.ConfigureCommon(env, _serviceName, _apiVersion);
    }
}
