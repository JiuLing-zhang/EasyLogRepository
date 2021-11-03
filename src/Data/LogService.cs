using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyLogRepository.DbContext;
using EasyLogRepository.Models;
using JiuLing.CommonLibs.ExtensionMethods;

namespace EasyLogRepository.Data
{
    public class LogService
    {
        private readonly MyDbContext _dbContext;
        public LogService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<LogVO>> GetLogsAsync()
        {
            List<TableLogs> dbList;
            using (_dbContext)
            {
                dbList = _dbContext.Logs.ToList();
            }

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

            return Task.FromResult(result);
        }
    }
}
