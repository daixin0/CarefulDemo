using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCommonLib.Extensions;

namespace WPFCommonLib.Utility
{
    public class ApplicationHelper
    {
        public static T GetWindow<T>() where T : Window
        {
            return Application.Current.Windows.OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetWindows<T>() where T : Window
        {
            return Application.Current.Windows.OfType<T>();
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        public static Window GetCurrentActivatedWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(m => m.IsActive);
        }

        public static Type GetTargetType(string fullQulifiedName, Func<Assembly, bool> condition = null)
        {
            // 忽略 C1 以及 PWMIS 开头的类库，可能会引起错误
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().AsEnumerable().Where(m => !m.FullName.StartsWith("C1.") && !m.FullName.StartsWith("PWMIS"));

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

            if (type == null)
            {
                throw new Exception("找不到指定的 Type");
            }

            return type;
        }
    }
}
