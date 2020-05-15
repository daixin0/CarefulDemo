using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFCommonLib.Helpers;

namespace WPFCommonLib.Utility
{
    public class RpcInvoker
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="fullQulifiedName">方法所属类的完整名称</param>
        /// <param name="parameters">调用方法需要的参数</param>
        public static void Invoke(string methodName, string fullQulifiedName, params object[] parameters)
        {
            Invoke<string>(methodName, fullQulifiedName, parameters);
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="methodName"></param>
        /// <param name="fullQulifiedName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Invoke<T>(string methodName, string fullQulifiedName, params object[] parameters)
        {
            var result = default(T);
            var targetType = ApplicationHelper.GetTargetType(fullQulifiedName);

            var instance = Activator.CreateInstance(targetType);
            if (instance != null)
            {
                var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

                if (method == null)
                {
                    throw new Exception($"在类型 {fullQulifiedName} 中找不到指定的方法: {methodName}");
                }

                var returnValue = method.Invoke(instance, parameters);

                if (returnValue is T)
                {
                    result = (T)returnValue;
                }
            }

            return result;
        }
    }
}
