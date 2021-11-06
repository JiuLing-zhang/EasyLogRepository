using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.StaticFiles;

namespace EasyLogRepository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : Controller
    {
        [HttpGet("{path}")]
        public IActionResult Get(string path)
        {
            var file = HttpUtility.UrlDecode(path);
            var stream = System.IO.File.OpenRead(file);
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(file));
        }
    }
}
