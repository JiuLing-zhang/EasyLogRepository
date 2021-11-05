![](https://img.shields.io/badge/build-passing-brightgreen)
![](https://img.shields.io/github/license/JiuLing-zhang/EasyLogRepository)  

# 说明
一个自用的简单的日志存储项目。  
项目使用的`Blazor Server`，仅仅是在`Hello, world!`的基础上加了个日志接口。  
一来最近在学习`Blazor Server`方面的东西，二来这个日志的功能是自用的，目前没啥追求，应急用一下，连接口都是在裸奔哒。所以这个仓库就这么诞生了。  

# 代码说明  

代码运行时需要手动添加`appsettings.json`配置文件  

```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "MySQL": "server=xx;userid=xx;password=xx;database=xx;"
  }
}
```