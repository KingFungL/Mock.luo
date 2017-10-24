using Autofac;
using Autofac.Integration.Mvc;
using Mock.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.App_Start
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

            builder.RegisterAssemblyTypes(service).AsImplementedInterfaces();

            //builder.RegisterType<RedisHelper>().As<IRedisHelper>().SingleInstance();

            //创建一个Autofac的容器
            var container = builder.Build();
            //将MVC的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}