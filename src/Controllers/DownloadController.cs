using System;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using EasyLogRepository.DbContext;
using EasyLogRepository.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EasyLogRepository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly IHostEnvironment _hostEnvironment;
        public DownloadController(IHostEnvironment hostEnvironment, IServiceScopeFactory factory)
        {
            _hostEnvironment = hostEnvironment;
            _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<MyDbContext>();
        }

        [HttpGet("{path}")]
        public IActionResult Get(string path)
        {
            var log = new TableLogs()
            {
                CreateTime = DateTime.Now,
                LogType = LogTypeEnum.消息,
                SessionId = "Download",
                Message = "文件下载"
            };
            _dbContext.Logs.Add(log);
            _dbContext.SaveChanges();

            var file = HttpUtility.UrlDecode(path);
            file = file.Replace("/", "\\");
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, file);
            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(file));
        }
    }
}
