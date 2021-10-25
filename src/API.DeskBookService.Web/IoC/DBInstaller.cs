using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Data.Context;
using API.DeskBookService.Data.DataSettings;
using API.DeskBookService.Web.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace API.DeskBookService.Web.IoC
{
    public class DBInstaller:IServicesInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DeskDatabaseSettings>(
            configuration.GetSection(nameof(DeskDatabaseSettings)));
                    services.AddSingleton<IDeskDatabaseSettings>(sp =>
                        sp.GetRequiredService<IOptions<DeskDatabaseSettings>>().Value);
                    services.AddTransient<IDeskBookerDataContext, DeskBookerDataContext>();
        }


    }
}
