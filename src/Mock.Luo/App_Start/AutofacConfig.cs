using Autofac;
using Autofac.Integration.Mvc;
using Mock.Code.Helper;
using Mock.Code.Mail;
using System.Reflection;
using System.Web.Mvc;
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
            //把当前程序集中的所有非抽象类型的ActionFilterAttribute都注册（这样我们在所有ActionFilterAttribute及子类中都可以使用属性注入）
            //前提：注册这个过滤的时候，这个过滤器的对象不能直接new出来，而是要去IOC容器中得到这个过滤类的对象。
            //例如：需要这样注册过滤：filters.Add(DependencyResolver.Current.GetService<CheckLoginAttribute>());

            builder.RegisterAssemblyTypes(typeof(WebApiApplication).Assembly).Where(r => typeof(HandlerErrorAttribute).IsAssignableFrom(r) && !r.IsAbstract).PropertiesAutowired();


            //创建一个Autofac的容器
            var container = builder.Build();
            //将MVC的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }


}