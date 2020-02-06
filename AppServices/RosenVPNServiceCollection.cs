using System;
using Microsoft.Extensions.DependencyInjection;
using RosenHCMC.VPN.Contract;
using RosenHCMC.VPN.DAL;

namespace RosenHCMC.VPN.AppServices
{
    internal class RosenVPNServiceCollection
    {
        internal static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<DatabaseService>();
            services.AddTransient<IWallpaperService, WallpaperService>();
            services.AddTransient<Mapper>();

            services.AddHostedService<DiscoveryService>();
        }
    }
}