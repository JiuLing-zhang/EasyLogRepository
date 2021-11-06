using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLogRepository.Models
{
    [Table("AppInfo")]
    public class AppInfo
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public string FilePath { get; set; }
    }
}
