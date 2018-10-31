using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Mock.Code;
using Mock.Code.Helper;
using Mock.Code.Mail;
using Mock.Domain.Interface;
using Mock.Luo.Generic.Filters;

namespace Mock.Luo
{
    public class AutofacConfig
    {
        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数),接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register()
        {  
            //实例化一个autofac的创建容器
            ContainerBuilder builder = new ContainerBuilder();
            var service = Assembly.Load("Mock.Domain");

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(service).AsImplementedInterfaces().PropertiesAutowired();

            builder.RegisterType<RedisHelper>().As<IRedisHelper>().SingleInstance();
            builder.RegisterType<MailHelper>().As<IMailHelper>().SingleInstance().PropertiesAutowired();

            //注入特性
            builder.RegisterFilterProvider();

            //创建一个Autofac的容器
            var container = builder.Build();
            //将MVC的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }


}