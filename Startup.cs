using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RosenHCMC.VPN.DAL;
using RosenHCMC.VPN.Native;

namespace RosenHCMC.VPN
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            bool hide = bool.Parse(Configuration["HideOnStart"]);
            if (hide)
            {
                SysCall.Hide();
            }

            bool seed = bool.Parse(Configuration["SeedOnStart"]);
            if (seed)
            {
                DatabaseService databaseService = new DatabaseService(Configuration);
                databaseService.RegisterWallpaper(new Wallpaper() { WallpaperFileName = "test.png", WallpaperFileType = "png" });
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppServices.RosenVPNServiceCollection.RegisterServices(services);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
