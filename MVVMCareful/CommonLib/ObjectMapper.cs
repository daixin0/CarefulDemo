using CommonLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    /// <summary>
    /// 对象映射器
    /// </summary>
    public class ObjectMapper
    {
        
        /// <summary>
        /// 创建一个指定类型的新对象，并且将将当前对象的值，映射到新对象，要求两个对象至少有一个属性名称和属性类型相同。不支持集合对象的映射
        /// </summary>
        /// <typeparam name="T">要映射的对象的类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>新对象</returns>
        public static T CopyTo<T>(object source) where T : class, new()
        {
            T target = new T();
            return CopyTo(source, target);
        }
        /// <summary>
        /// 将将源对象的值，映射到目标对象，要求两个对象至少有一个属性名称和属性类型相同。不支持集合对象的映射
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <returns>目标对象</returns>
        /// <remarks>注意日期类型属性将会转换成默认值</remarks>
        private static T CopyTo<T>(object source, T target) where T : class, new()
        {
            T result = source.CopyTo<T>(target);
            return result;
        }

    }
}
