using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RosenHCMC.VPN.Contract;
using RosenHCMC.VPN.Contract.DTOs;
using RosenHCMC.VPN.DAL;
using RosenHCMC.VPN.Native;

namespace RosenHCMC.VPN.AppServices
{
    public class WallpaperService : IWallpaperService
    {
        private readonly DatabaseService dbservice;
        private readonly IConfiguration configuration;
        private readonly Mapper mapper;

        public WallpaperService(DatabaseService dbservice, IConfiguration configuration, Mapper mapper)
        {
            this.dbservice = dbservice;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        public WallpaperDTO GetWallpaper(int id)
        {
            return mapper.MapToDTO(dbservice.GetWallpaper(id));
        }

        public IEnumerable<WallpaperDTO> ListWallpaper(bool includeContent = false)
        {
            return dbservice.GetWallpapers().ToList().Select(wp => mapper.MapToDTO(wp, includeContent));
        }

        public void SetWallpaper(int id)
        {
            Wallpaper wp = this.dbservice.GetWallpaper(id);
            Roller.PaintWall(wp.WallpaperFileName, Roller.Style.Stretch);
        }

        public WallpaperDTO UploadWallpaper(WallpaperUploadDTO dto)
        {
            string filename = SysCall.SaveContent(dto.WallpaperFileName, dto.WallpaperContent);
            int id = dbservice.RegisterWallpaper(new Wallpaper()
            {
                WallpaperFileName = dto.WallpaperFileName,
                WallpaperFileType = Path.GetExtension(dto.WallpaperFileName).Substring(1)
            });
            return mapper.MapToDTO(dbservice.GetWallpaper(id));
        }
    }
}
