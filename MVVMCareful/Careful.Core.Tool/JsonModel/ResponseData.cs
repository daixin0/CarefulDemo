using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Tool.JsonModel
{
    public class ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 课程数据
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string timestamp { get; set; }

        public object data { get; set; }
    }
    public class ResponseData<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 课程数据
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string timestamp { get; set; }

        public T data { get; set; }
    }
}
