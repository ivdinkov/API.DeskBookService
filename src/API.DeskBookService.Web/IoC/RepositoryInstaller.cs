using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Data.Repository;
using API.DeskBookService.Web.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.DeskBookService.Web.IoC
{
    public class RepositoryInstaller : IServicesInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IDeskRepository, DeskRepository>();
        }
    }
}
