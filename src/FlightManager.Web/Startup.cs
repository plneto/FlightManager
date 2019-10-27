using System;
using AutoMapper;
using FlightManager.Application.AutoMapperProfiles;
using FlightManager.Data.Repositories;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Contracts.Services;
using FlightManager.Domain.Services;
using FlightManager.Web.Extensions;
using FlightManager.Web.SampleData;
using FlightManager.Web.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace FlightManager.Web
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
            services.AddAutoMapper(typeof(ApplicationProfile));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllersWithViews();

            services.ConfigureSettings<MongoSettings>(Configuration);

            services.AddMongoDb(Configuration.GetSettings<MongoSettings>());

            services.AddTransient<IFlightRepository, FlightRepository>();
            services.AddTransient<IAirportRepository, AirportRepository>();
            services.AddTransient<IReportItemRepository, ReportItemRepository>();

            services.AddTransient<IFlightDomainService, FlightDomainService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMongoDatabase mongoDatabase)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            mongoDatabase.AirportSeed();
        }
    }
}