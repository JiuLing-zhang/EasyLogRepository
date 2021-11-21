using System;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using EasyLogRepository.DbContext;
using EasyLogRepository.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

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

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var appInfo = _dbContext.AppInfo.Where(x => x.Id == id).FirstOrDefault();
            if (appInfo == null)
            {
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 1, Message = "非法请求" });
            }

            var log = new TableLogs()
            {
                CreateTime = DateTime.Now,
                LogType = LogTypeEnum.消息,
                SessionId = "Download",
                Message = "文件下载"
            };
            _dbContext.Logs.Add(log);
            _dbContext.SaveChanges();

            var file = HttpUtility.UrlDecode(appInfo.FilePath);
            file = file.Replace("/", "\\");
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, file);
            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, appInfo.ContentType, Path.GetFileName(file));
        }
    }
}
