using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Extensions
{
    public static class ModelCastExtension
    {
        public static T CopyTo<T>(this object source, T target) where T : class, new()
        {
            Type tFrom = source.GetType();
            Type tTo = target.GetType();
            PropertyInfo[] arrFromPro = tFrom.GetProperties();
            PropertyInfo[] arrToPro = tTo.GetProperties();

            foreach (PropertyInfo proFrom in arrFromPro)
            {
                foreach (PropertyInfo proTo in arrToPro)
                {
                    if (proFrom.Name == proTo.Name && proFrom.PropertyType == proTo.PropertyType)
                    {

                        proTo.SetValue(target, proFrom.GetValue(source, null), null);
                        break;
                    }
                }
            }
            return target;
        }
    }
}
