using API.DeskBookService.Core.Services;
using API.DeskBookService.Web.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.DeskBookService.Web.IoC
{
    public class ServiceInstaller : IServicesInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDeskService,DeskService>();
            services.AddSingleton<IBookingService,BookingService>();
            services.AddHttpContextAccessor();
        }
    }
}
