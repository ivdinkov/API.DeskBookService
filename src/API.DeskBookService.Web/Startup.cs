using API.DeskBookService.Core.DataInterfaces;
using API.DeskBookService.Data.Context;
using API.DeskBookService.Data.DataSettings;
using API.DeskBookService.Data.Repository;
using API.DeskBookService.Web.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace API.DeskBookService.Web
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constrctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DeskDatabaseSettings>(
                Configuration.GetSection(nameof(DeskDatabaseSettings)));

            services.AddSingleton<IDeskDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DeskDatabaseSettings>>().Value);

            services.AddTransient<IDeskBookerDataContext, DeskBookerDataContext>();
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IDeskRepository, DeskRepository>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "DeskBooking API", Version = "v1" });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentfullPath = Path.Combine(AppContext.BaseDirectory,xmlCommentsFile);
                x.IncludeXmlComments(xmlCommentfullPath);
                var xmlCommentfullPath2 = Core.ReadXml.GetXml();
                x.IncludeXmlComments(xmlCommentfullPath2);
            });

            services.AddControllers();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });

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
