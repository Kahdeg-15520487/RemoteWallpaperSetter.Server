using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RosenHCMC.VPN.Contract;
using RosenHCMC.VPN.Contract.DTOs;

namespace RosenHCMC.VPN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WallpaperController : ControllerBase
    {
        private readonly IWallpaperService wallpaperService;

        public WallpaperController(IWallpaperService wallpaperService)
        {
            this.wallpaperService = wallpaperService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(wallpaperService.GetWallpaper(id));
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(wallpaperService.ListWallpaper());
        }

        [HttpGet("content")]
        public IActionResult ListContent()
        {
            return Ok(wallpaperService.ListWallpaper(true));
        }

        [HttpPut("{id}")]
        public IActionResult Set(int id)
        {
            try
            {
                wallpaperService.SetWallpaper(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Upload(WallpaperUploadDTO dto)
        {
            return Ok(wallpaperService.UploadWallpaper(dto));
        }
    }
}
