using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RosenHCMC.VPN.Contract.DTOs
{
    public class WallpaperDTO
    {
        public int Id { get; set; }
        public string WallpaperFileName { get; set; }
        public string WallpaperFileType { get; set; }
        public string WallpaperContent { get; set; }
        public bool WallpaperIncluded => WallpaperContent != null;
    }
}
