using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyLogRepository.DbContext;
using EasyLogRepository.Models;
using Microsoft.Extensions.DependencyInjection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyLogRepository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        public LogController(IServiceScopeFactory factory)
        {
            _dbContext = factory.CreateScope().ServiceProvider.GetRequiredService<MyDbContext>();
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "亲，不要试了，这个接口没漏洞" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] LogItem value)
        {
            try
            {
                var logs = new List<TableLogs>();
                foreach (var logDetail in value.Logs)
                {
                    var time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(logDetail.Timestamp);
                    logs.Add(new TableLogs()
                    {
                        CreateTime = time,
                        SessionId = value.SessionId,
                        LogType = logDetail.LogType,
                        Message = logDetail.Message
                    });
                }

                foreach (var log in logs)
                {
                    _dbContext.Logs.Add(log);
                }
                var count = _dbContext.SaveChanges();
                if (count != logs.Count)
                {
                    return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 1, Message = "数据写入失败" });
                }
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 0, Message = "提交成功" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new JiuLing.CommonLibs.Model.JsonResult() { Code = 999, Message = $"系统错误。{ex.Message}" });
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
