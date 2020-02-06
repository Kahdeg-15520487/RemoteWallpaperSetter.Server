using RosenHCMC.VPN.Contract.DTOs;
using RosenHCMC.VPN.DAL;
using RosenHCMC.VPN.Native;

namespace RosenHCMC.VPN.AppServices
{
    public class Mapper
    {
        public WallpaperDTO MapToDTO(Wallpaper wp, bool includeContent = false)
        {
            return new WallpaperDTO()
            {
                Id = wp.Id,
                WallpaperFileName = wp.WallpaperFileName,
                WallpaperFileType = wp.WallpaperFileType,
                WallpaperContent = includeContent ? SysCall.LoadContent(wp.WallpaperFileName) : null
            };
        }
    }
}
