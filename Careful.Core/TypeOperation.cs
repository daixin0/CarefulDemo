using Careful.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core
{
    public class TypeOperation
    {
        public static Type GetTargetType(string fullQulifiedName, Func<Assembly, bool> condition = null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().AsEnumerable();
            if (condition != null)
            {
                assemblies = assemblies.Where(condition);
            }

            Type type = null;
            try
            {
                type = assemblies?.SelectMany(a => a.GetTypes()).SingleOrDefault(t => t.FullName == fullQulifiedName);
            }
            catch (Exception ex)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var loadException = ex as ReflectionTypeLoadException;

                    if (loadException.LoaderExceptions.HasItems())
                    {
                        StringBuilder sb = new StringBuilder();
                        StringBuilder sbTitle = new StringBuilder();
                        foreach (var item in loadException.LoaderExceptions.ToList())
                        {
                            sb.AppendLine(item.ToString());
                            sbTitle.AppendLine(item.Message);
                        }

                        throw new Exception("获取已加载程序集信息时出错，详细信息:" + Environment.NewLine + sbTitle.ToString(), new Exception(sb.ToString()));
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw new Exception("获取已加载程序集信息时出错", ex);
                }
            }

            //if (type == null)
            //{
            //    throw new Exception("找不到指定的 Type: " + fullQulifiedName);
            //}

            return type;
        }
    }
}
