using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyLogRepository.DbContext;
using EasyLogRepository.Models;
using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;

namespace EasyLogRepository.Data
{
    public class LogService
    {
        private readonly MyDbContext _dbContext;
        public LogService(IServiceScopeFactory factory)
        {
            _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<MyDbContext>();
        }

        public Task<List<LogVO>> GetLogsAsync()
        {
            return Task.Run(() =>
            {
                List<TableLogs> dbList = _dbContext.Logs.ToList();

                var result = new List<LogVO>();
                foreach (var item in dbList)
                {
                    result.Add(new LogVO()
                    {
                        SessionId = item.SessionId,
                        CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        LogType = item.LogType.GetDescription(),
                        Message = item.Message
                    });
                }

                return result;
            });
         
        }
    }
}
