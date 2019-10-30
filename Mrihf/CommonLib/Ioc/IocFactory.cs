using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Ioc
{
    /// <summary>
    /// 依赖注入容器工厂
    /// </summary>
    public class IocFactory
    {
        static readonly IObjectContainer container = new DefaultObjectContainer();
        /// <summary>
        /// 获取默认的对象构造器
        /// </summary>
        public static IObjectContainer Default
        {
            get {
                return container;
            }
        }
    }

    /// <summary>
    /// 对象容器接口
    /// </summary>
    public interface IObjectContainer
    {
        /// <summary>
        /// 获取类型的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T:class;
    }

    class DefaultObjectContainer:IObjectContainer
    {

        public T Resolve<T>() where T : class
        {
           return  SimpleIoc.Default.GetInstance<T>();
        }
    }
}
