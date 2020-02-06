using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using LiteDB;
using System.Linq;

namespace RosenHCMC.VPN.DAL
{
    public class DatabaseService
    {
        private readonly IConfiguration configuration;

        public DatabaseService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Wallpaper GetWallpaper(int id)
        {
            using (LiteDatabase db = new LiteDatabase(configuration["ConnectionStrings:DefaultConnection"]))
            {
                ILiteCollection<Wallpaper> wallpapers = db.GetCollection<Wallpaper>("wallpaper");
                Wallpaper wp = wallpapers.FindById(new BsonValue(id));
                return wp;
            }
        }

        public IEnumerable<Wallpaper> GetWallpapers()
        {
            using (LiteDatabase db = new LiteDatabase(configuration["ConnectionStrings:DefaultConnection"]))
            {
                ILiteCollection<Wallpaper> wallpapers = db.GetCollection<Wallpaper>("wallpaper");
                IEnumerable<Wallpaper> wallpaper = wallpapers.FindAll();
                return wallpaper.ToList();
            }
        }

        public int RegisterWallpaper(Wallpaper wallpaper)
        {

            using (LiteDatabase db = new LiteDatabase(configuration["ConnectionStrings:DefaultConnection"]))
            {
                ILiteCollection<Wallpaper> wallpapers = db.GetCollection<Wallpaper>("wallpaper");
                return wallpapers.Insert(wallpaper).AsInt32;
            }
        }
    }
}
