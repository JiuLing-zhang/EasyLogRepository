using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using Microsoft.Extensions.Hosting;

namespace EasyLogRepository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : Controller
    {

        private readonly IHostEnvironment _hostEnvironment;
        public DownloadController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("{path}")]
        public IActionResult Get(string path)
        {
            var file = HttpUtility.UrlDecode(path);
            file = file.Replace("/", "\\");
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, file);
            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(file));
        }
    }
}
