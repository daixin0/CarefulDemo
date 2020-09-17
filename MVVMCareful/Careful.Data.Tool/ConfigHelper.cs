using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Data.Tool
{
    public static class ConfigHelper
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? String.Empty;
        }

        public static NameValueCollection GetConfigSection(string name)
        {
            return (NameValueCollection)ConfigurationManager.GetSection(name) ?? null;
        }
    }
}
