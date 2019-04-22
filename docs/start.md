# 开始

该文档用于指导你如何使用该项目。
### 工具
1. VS2015、VS2017
2. SQL server 2008,2012,2017
3. Redis
4. redis-desktop-manager.exe
![安装包下载](/docs/images/Install-Software.png)
博客里面有redis,sql server 2012,redis-desktop-manager.exe的安装包
[https://blog.csdn.net/q710777720/article/details/80358932](https://blog.csdn.net/q710777720/article/details/80358932)



### 1.还原数据库
数据库：SQL server 2008,2012,2017都可以。

1 将docs目录中的“Mock数据bak文件20171129.zip”解压后还原，这是一个bak文件，还原过程就不BB了。

    1.1  备选还原方案，新建一个数据库Mock，将“mock数据库备选方案还原.zip”解压后，在SQL Server Managerment中执行一个里面的SQL语句就可以了

2 由于项目中使用的Code First的方式，以上还原方式是为了生成一些基础数据。由于代码有所变更，数据表结构可能存在问题，所以还需要执行以下命令,可参考下图,设置Mock.luo为启动项目，选择Mock.Data这个默认项目，执行如下命令.若执行出错，执行二次试试。

~~~
Update-Database -Force
~~~

![/docs/images/Update-Database.png](/docs/images/Update-Database.png)



### 配置项

web.config中sql server 密码根据自己本地密码设置 redis未设置密码，

~~~
  <connectionStrings>
    <!--本地-->
    <add name="RedisConnectionString" connectionString="127.0.0.1:6379" />
    <add name="MockDbConnection" connectionString="Server=.;Initial Catalog=Mock;User ID=sa;Password=您的密码" providerName="System.Data.SqlClient" />
  </connectionStrings>
~~~

如果redis设置了密码，可使用如下配置
~~~
  <add name="Abp.Redis.Cache" connectionString="127.0.0.1:6379,password=您的密码" />
~~~

Generic/Configs/log4net.config文件用于配置log4net 要插入的数据库。这里同MockDbConnection中的配置,uid与pwd
![/docs/images/log4net-Config.png](/docs/images/log4net-Config.png)
~~~
  <connectionString value="server=.;database=Mock;uid=sa;pwd=您的密码;MultipleActiveResultSets=True" />
~~~

其他关于redis密码配置 [https://blog.csdn.net/q710777720/article/details/89449196](https://blog.csdn.net/q710777720/article/details/89449196)
