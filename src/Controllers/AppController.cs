using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Web;
using EasyLogRepository.DbContext;
using Microsoft.Extensions.DependencyInjection;
using JiuLing.CommonLibs.ExtensionMethods;

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
        public IActionResult Get()
        {
            //TODO 暂时兼容下老接口，最终需要删除
            return Get("MusicPlayerOnline", "android");
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
                var obj = new { appInfo.VersionCode, appInfo.VersionName, DownloadUrl = downloadUrl };
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult<object> { Code = 0, Message = "", Data = obj });
            }
            catch (Exception ex)
            {
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 999, Message = $"系统错误。{ex.Message}" });
            }
        }
    }
}
