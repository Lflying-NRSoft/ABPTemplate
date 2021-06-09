# ABPTemplate
ABP VNext 的空模板

## 快速入门

- 拉取模板代码，修改代码并提交
- 重新拉取最新代码到新目录下，然后进行zip压缩，压缩名为：module-3.3.2.zip。
- 执行命令：abp new Acme.BookStore -t module --template-source 'E:\01 Work\01 ABP\templates' -v 3.3.2
- 将项目HttpApi.Host设为启动项目。

````bash
> abp new Acme.BookStore -t module --template-source 'C:\02 github\ABPTemplate' -v 3.3.2
````
或者
````bash
> abp new Acme.BookStore -t module --template-source http://192.168.149.72:8900/baison/module-3.3.2.zip -v 3.3.2 --skip-cli-version-check
````

### ABP命令行
> 请点击链接 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) 查看所有有效的选项。


## 数据库迁移
## 创建数据库

### 连接字符串

检查 `.HttpApi.Host` 项目下 `appsettings.json` 文件中的 **链接字符串**:

````json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=BookStore;Trusted_Connection=True"
}
````

该解决方案配置为**Entity Framework Core**与**MS SQL Server**一起使用. EF Core支持[各种](https://docs.microsoft.com/en-us/ef/core/providers/)数据库提供程序,因此你可以使用任何受支持的DBMS. 请参阅[Entity Framework集成文档](https://docs.abp.io/en/abp/latest/Entity-Framework-Core)了解如何切换到另一个DBMS.

### 数据库连接字符串

查看`.Web`项目下`appsettings.json`文件中的 **连接字符串**:

````json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=BookStore;Trusted_Connection=True"
  }
}
````

解决方案使用 **Entity Framework Core** 和 **MS SQL Server**. EF Core支持[各种](https://docs.microsoft.com/zh-cn/ef/core/providers/)数据库提供程序,因此你可以根据实际需要使用其他DBMS. 如果需要,请更改连接字符串.

### 应用迁移

#### 使用EF Core Update-Database命令

Ef Core具有`Update-Database`命令, 可根据需要创建数据库并应用挂起的迁移. 右键单击`.Web`项目并选择**设置为启动项目**:

{{ if UI == "MVC" }}

右键单击{{if Tiered == "Yes"}}`.IdentityServer`{{else}}`.Web`{{end}}项目并选择**设置为启动项目**:

{{ else if UI != "MVC" }}

右键单击`.HttpApi.Host`项目并选择**设置为启动项目**:

{{ end }}

![set-as-startup-project](images/set-as-startup-project.png)

打开**包管理器控制台(Package Manager Console)**, 选择`.EntityFrameworkCore.DbMigrations`项目作为**默认项目**并运行`Update-Database`命令:

![package-manager-console-update-database](images/package-manager-console-update-database.png)

这将基于配置的连接字符串创建新数据库.

> **使用`.DbMigrator`工具是建议的方法**, 因为它能初始化初始数据能够正确运行Web应用程序.
>
> 如果你只是使用 `Update-Database` 命令,你会得到一个空数据库,所以你无法登录到应用程序因为数据库中没有初始管理用户. 不需要种子数据库时,可以在开发期间使用 `Update-Database` 命令. 但是使用 `.DbMigrator` 应用程序会更简单,你始终可以使用它来迁移模式并为数据库添加种子.

{{ else if DB == "Mongo" }}

````json
"ConnectionStrings": {
  "Default": "mongodb://localhost:27017/BookStore"
}
````

该解决方案被配置为在你的本地计算机中使用 **MongoDB**,因此你需要启动并运行一个MongoDB服务器实例或者将连接字符串更改为另一个MongoDB服务器.

