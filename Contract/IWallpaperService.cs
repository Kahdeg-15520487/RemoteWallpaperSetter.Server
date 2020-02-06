using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RosenHCMC.VPN.Contract.DTOs;

namespace RosenHCMC.VPN.Contract
{
    public interface IWallpaperService
    {
        WallpaperDTO GetWallpaper(int id);
        void SetWallpaper(int id);
        IEnumerable<WallpaperDTO> ListWallpaper(bool includeContent = false);

        WallpaperDTO UploadWallpaper(WallpaperUploadDTO dto);
    }
}
