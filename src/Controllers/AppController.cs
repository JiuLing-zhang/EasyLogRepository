﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Web;
using EasyLogRepository.DbContext;
using Microsoft.Extensions.DependencyInjection;

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

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var appInfo = _dbContext.AppInfo.OrderByDescending(x => x.CreateTime).FirstOrDefault();
                if (appInfo == null)
                {
                    return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 1, Message = "未找到App信息" });
                }

                string downloadUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/api/download/{HttpUtility.UrlEncode(appInfo.FilePath)}";
                var obj = new { appInfo.VersionCode, appInfo.VersionName, DownloadUrl = downloadUrl };
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult<object> { Code = 0, Message = "提交成功", Data = obj });
            }
            catch (Exception ex)
            {
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 999, Message = $"系统错误。{ex.Message}" });
            }
        }

        public IActionResult Download()
        {
            var addrUrl = @"D:\F\学习\vs2017\netcore\Study.AspNetCore\WebApp02-1\wwwroot\bak\love.apk";
            var stream = System.IO.File.OpenRead(addrUrl);
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(addrUrl));
        }
    }
}
