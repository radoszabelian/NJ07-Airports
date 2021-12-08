using Aiports_Model;
using Airports_IO.Services;
using Airports_Logic.Services;
using Airports_Logic.Services.FlightsService;
using Airports_Logic.Services.GeoLocation;
using Airports_Settings.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Airports_Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddMvc();

            services.AddSingleton<IAirportsService, AirportsService>();
            services.AddSingleton<IDataAccessor, DataAccessor>();
            services.AddSingleton<Airports_Logic.Services.ILogger, LoggerService>();
            services.AddSingleton<IConfig>(new ConfigService("appSettings.json"));
            services.AddSingleton<IAirportsDataConverter, AirportsDataConverter>();
            services.AddSingleton<IGeoLocationService, GeoLocationService>();
            services.AddSingleton<ICsvHelper, CsvHelper>();
            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<Logger>(NLog.LogManager.GetCurrentClassLogger());
            services.AddSingleton<IFlightService, FlightsSearchService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseMvc();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
