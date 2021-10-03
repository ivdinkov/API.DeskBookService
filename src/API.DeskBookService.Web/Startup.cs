
using API.DeskBookService.Core.Interfaces;
using API.DeskBookService.Core.Processor.Interfaces;
using API.DeskBookService.DataAccess;
using API.DeskBookService.DataAccess.Interfaces;
using API.DeskBookService.DataAccess.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.DeskBookService.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DeskDatabaseSettings>(
                Configuration.GetSection(nameof(DeskDatabaseSettings)));

            services.AddSingleton<IDeskDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DeskDatabaseSettings>>().Value);

            services.AddSingleton<IDeskBookerDataContext, DeskBookerDataContext>();
            services.AddTransient<IDeskBookingRepository, DeskBookingRepository>();
            services.AddTransient<IDeskRepository, DeskRepository>();
            services.AddTransient<IDeskBookingRequestProcessor, DeskBookingRequestProcessor>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
