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
