using API.DeskBookService.Web.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace API.DeskBookService.Web.IoC
{
    public class AppInstaller : IServicesInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "DeskBooking API", Version = "v1" });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentfullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                x.IncludeXmlComments(xmlCommentfullPath);
                var xmlCommentfullPath2 = Core.ReadXml.GetXml();
                x.IncludeXmlComments(xmlCommentfullPath2);
            });

            services.AddControllers();
        }
    }
}
