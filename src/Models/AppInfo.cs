using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLogRepository.Models
{
    [Table("AppInfo")]
    public class AppInfo
    {
        /// <summary>
        /// 文件名的MD5
        /// </summary>
        [Key]
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string AppName { get; set; }
        public string Platform { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public string MinVersionName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }
}
