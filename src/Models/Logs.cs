using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLogRepository.Models
{
    [Table("Logs")]
    public class TableLogs
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; }
        public DateTime CreateTime { get; set; }
        public LogTypeEnum LogType { get; set; }
        public string Message { get; set; }
    }
}
