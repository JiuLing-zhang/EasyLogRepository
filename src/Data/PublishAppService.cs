using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyLogRepository.DbContext;
using EasyLogRepository.Models;
using EasyLogRepository.Models.Dto;
using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EasyLogRepository.Data
{
    public class PublishAppService
    {
        private readonly MyDbContext _dbContext;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        public PublishAppService(IServiceScopeFactory factory, IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<MyDbContext>();
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        private long maxFileSize = 1024 * 1024 * 200;
        public async Task<JiuLing.CommonLibs.Model.JsonResult> UploadFile(IBrowserFile file, string key, AppInfoDto dto)
        {
            var result = new JiuLing.CommonLibs.Model.JsonResult();
            try
            {
                if (key.IsEmpty() || dto.AppName.IsEmpty() || dto.Platform.IsEmpty() || dto.VersionName.IsEmpty())
                {
                    result.Code = 1;
                    result.Message = "参数错误";
                    return result;
                }

                string publishAppKey = _configuration.GetSection("PublishAppKey").Value;
                if (publishAppKey != key)
                {
                    result.Code = 2;
                    result.Message = "Key值不正确";
                    return result;
                }
                //获取扩展名
                var extension = "";
                var index = file.Name.LastIndexOf(".");
                if (index >= 0)
                {
                    extension = file.Name.Substring(index + 1);
                }

                var fileName = $"{dto.AppName}-{dto.Platform}-{dto.VersionName}.{extension}";
                string id = JiuLing.CommonLibs.Security.MD5Utils.GetLowerValue(fileName);
                var directoryPath = "uploads";
                var directory = Path.Combine(_hostEnvironment.ContentRootPath, directoryPath);
                if (!System.IO.Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var path = Path.Combine(directory, fileName);

                await using FileStream fs = new(path, FileMode.Create);
                await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

                var appInfo = new AppInfo()
                {
                    Id = id,
                    CreateTime = DateTime.Now,
                    AppName = dto.AppName,
                    Platform = dto.Platform,
                    VersionCode = dto.VersionCode,
                    VersionName = dto.VersionName,
                    MinVersionName = dto.MinVersionName,
                    FilePath = $"{directoryPath}/{fileName}"
                };
                _dbContext.AppInfo.Add(appInfo);
                var count = await _dbContext.SaveChangesAsync();
                if (count == 0)
                {
                    result.Code = 3;
                    result.Message = "数据写入失败";
                    return result;
                }

                result.Code = 0;
                result.Message = "发布成功";
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 999;
                result.Message = $"系统错误：{ex.Message}";
                return result;
            }

        }
    }
}
