using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLogRepository.Models
{
    public class LogItem
    {
        public string SessionId { get; set; }
        public List<LogDetail> Logs { get; set; }
    }

    public class LogDetail
    {
        public Int64 Timestamp { get; set; }
        public LogTypeEnum LogType { get; set; }
        public string Message { get; set; }
    }

    public enum LogTypeEnum
    {
        消息 = 0,
        警告 = 1,
        错误 = 2
    }
}
