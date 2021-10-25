using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.DeskBookService.Web.Ioc
{
    public interface IServicesInstaller
    {
        void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}
