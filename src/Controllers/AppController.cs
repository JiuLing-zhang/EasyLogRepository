using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Web;
using EasyLogRepository.DbContext;
using Microsoft.Extensions.DependencyInjection;
using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Model;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace EasyLogRepository.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AppController : Controller
    {
        private readonly MyDbContext _dbContext;
        public AppController(IServiceScopeFactory factory)
        {
            _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<MyDbContext>();
        }

        [HttpGet("{appName}/{platform}")]
        public IActionResult Get(string appName, string platform)
        {
            try
            {
                if (appName.IsEmpty() || platform.IsEmpty())
                {
                    return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 1, Message = $"非法请求" });
                }
                var appInfo = _dbContext.AppInfo.Where(x => x.AppName == appName && x.Platform == platform).OrderByDescending(x => x.CreateTime).FirstOrDefault();
                if (appInfo == null)
                {
                    return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 2, Message = "未找到App信息" });
                }
                
                string downloadUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/api/download/{appInfo.Id}";
                return new JsonResult(new AppUpgradeInfo
                {
                    Name = appName,
                    Version = appInfo.VersionName,
                    MinVersion = appInfo.MinVersionName,
                    DownloadUrl = downloadUrl,
                    CreateTime = appInfo.CreateTime,
                    Log = ""
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 999, Message = $"系统错误。{ex.Message}" });
            }
        }
    }
}
