using Careful.Tool;
using Careful.Tool.JsonModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Careful.Tool.Helpers
{
    public class HttpHelper
    {
        #region "单例"
        private static HttpHelper instance;
        private static object objLock = new object();
        public static HttpHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objLock)
                    {
                        if (instance == null)
                        {
                            instance = new HttpHelper();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion



        public Encoding ResponseEncoding { get; set; } = Encoding.UTF8;
        public string ContentType { get; set; }
        public string TGT { get; set; }
        public string Method { get; set; }
        public int TimeOut { get; set; } = 90000;
        public int ReadWriteTimeout { get; set; } = 180000;

        #region "变量"
        private HttpClient client;
        private HttpClientHandler handle;
        #endregion

        /// <summary>
        /// 判断网络状况的方法,返回值true为连接，false为未连接  
        /// </summary>
        /// <param name="conState"></param>
        /// <param name="reder"></param>
        /// <returns></returns>
        [DllImport("wininet")]
        public extern static bool InternetGetConnectedState(out int conState, int reder);
        public bool IsInternet
        {
            get
            {
                int i;
                return InternetGetConnectedState(out i, 0);
            }

        }
        #region "构造函数"
        private HttpHelper()
        {
            handle = new HttpClientHandler();
            client = new HttpClient(handle);
            //三十秒延时
            client.Timeout = new TimeSpan(0, 0, 30);
        }

        #endregion

        #region "HttpGet"

        private byte[] GetResponseBuffter(Stream stream)
        {
            List<byte> resultList = new List<byte>();
            byte[] readBuf = new byte[5120];
            var result = stream.Read(readBuf, 0, readBuf.Length);
            while (result > 0)
            {
                byte[] buf = new byte[result];
                Buffer.BlockCopy(readBuf, 0, buf, 0, result);
                resultList.AddRange(buf);
                result = stream.Read(readBuf, 0, readBuf.Length);
            }
            return resultList.ToArray();
        }


        public T HttpGet<T>(string baseUrl, string APIUrl, IDictionary<string, string> queryStrings = null)
        {
            var result = this.HttpGet(baseUrl + APIUrl, queryStrings);
            T obj = JsonHelper.Deserialize<T>(result);
            return obj;

        }

        public Task<T> HttpGetAsync<T>(string baseUrl, string APIUrl, IDictionary<string, string> queryStrings = null)
        {
            return Task.Run(() =>
            {
                var result = this.HttpGet(baseUrl + APIUrl, queryStrings);
                T obj = JsonHelper.Deserialize<T>(result);
                return obj;
            });

        }

        public string HttpGet(string url, IDictionary<string, string> queryStrings = null)
        {
            string result = "";
            try
            {
                string fullUrl = url;
                if (queryStrings != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var query in queryStrings)
                    {
                        if (query.Value != null)
                        {
                            sb.Append(string.Format("&{0}={1}", query.Key, query.Value));
                        }
                    }
                    if (sb.Length > 0)
                    {
                        fullUrl = string.Format("{0}?{1}", url, sb.ToString().Trim('&'));
                    }
                }

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(fullUrl);
                request.Method = Method;
                request.ContentType = ContentType;
                if (!string.IsNullOrEmpty(TGT))
                {
                    if (TGT.Contains("CASTGC"))
                    {
                        request.Headers.Add("Cookie", TGT);
                    }
                    else
                    {
                        request.Headers.Add("TGT", TGT);
                    }
                }

                request.Timeout = this.TimeOut;
                request.ReadWriteTimeout = this.ReadWriteTimeout;
                request.MaximumAutomaticRedirections = 100;
                var response = (HttpWebResponse)request.GetResponse();
                ResponseExceptionBase responseExceptionBase = new ResponseExceptionBase(response.StatusCode);
                if (responseExceptionBase.IsSuccess)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var resultBuffer = this.GetResponseBuffter(stream);
                        result = ResponseEncoding.GetString(resultBuffer);
                    }
                }
                response.Close();
                if (!responseExceptionBase.IsSuccess)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region "HttpPost"

        public string HttpPost(string url, string strJson)
        {
            string result = "";
            try
            {
                if (!IsInternet)
                {
                    ResponseData responseData = new ResponseData();
                    responseData.result = "";
                    responseData.message = "当前不存在网络，请使用离线模式。";
                    return JsonHelper.Serialize(responseData);
                }
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                Stream dataStream = null;


                request.Method = Method;
                request.ContentType = ContentType;
                if (!string.IsNullOrEmpty(TGT))
                {
                    request.Headers.Add("Cookie", TGT);

                }

                request.Timeout = this.TimeOut;
                request.ReadWriteTimeout = this.ReadWriteTimeout;
                request.MaximumAutomaticRedirections = 100;
                byte[] bytes = Encoding.UTF8.GetBytes(strJson);
                using (dataStream = request.GetRequestStream())
                {
                    dataStream.Write(bytes, 0, bytes.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                ResponseExceptionBase responseExceptionBase = new ResponseExceptionBase(response.StatusCode);
                if (responseExceptionBase.IsSuccess)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var resultBuffer = this.GetResponseBuffter(stream);
                        result = ResponseEncoding.GetString(resultBuffer);
                    }
                }
                response.Close();
                if (!responseExceptionBase.IsSuccess)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                ResponseData responseData = new ResponseData();
                responseData.result = "1111";
                responseData.message = ex.Message;
                return JsonHelper.Serialize(responseData);
            }
        }
        #endregion


        #region 上传下载


        #region 下载

        private long fileLength;
        public long downLength { get; set; }
        private int readSize;
        public bool stopDown { get; set; }


        public void DownloadFile(string url, string fileName, Action<int> progressBar)
        {
            stopDown = false;

            long lStartPos = 0;
            Stream fs = null, ns = null;

            fileLength = getDownLength(url);

            if (fileLength > 0)
            {
                if (File.Exists(fileName))
                {
                    fs = File.OpenWrite(fileName);
                    lStartPos = downLength = fs.Length;

                    progressBar((int)(Convert.ToDouble(downLength) / Convert.ToDouble(fileLength) * 100));

                    if (downLength == fileLength)
                    {
                        fs.Close();
                        fs.Dispose();
                        return;
                    }
                    else
                    {
                        fs.Seek(lStartPos, SeekOrigin.Current);
                    }
                }
                else
                {
                    fs = new System.IO.FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                    lStartPos = 0;
                }

                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    if (lStartPos > 0)
                    {
                        request.AddRange((int)lStartPos);
                    }
                    ns = request.GetResponse().GetResponseStream();
                    readSize = 1024 * 1024;
                    byte[] nbytes = new byte[readSize];
                    int nReadSize = 0;
                    nReadSize = ns.Read(nbytes, 0, readSize);
                    while (nReadSize > 0)
                    {
                        if (stopDown)
                            break;

                        //if (0 == getDownLength(url))
                        //{
                        //    stopDown = true;
                        //    break;
                        //}
                        //else
                        //{
                        downLength += nReadSize;
                        fs.Write(nbytes, 0, nReadSize);
                        nReadSize = ns.Read(nbytes, 0, readSize);
                        progressBar((int)(Convert.ToDouble(downLength) / Convert.ToDouble(fileLength) * 100));

                        //Application.DoEvents();
                        //}
                    }

                    fs.Close();
                    fs.Dispose();
                    ns.Close();
                    ns.Dispose();
                }
                catch (Exception ex)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    if (ns != null)
                    {
                        ns.Close();
                        ns.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// 获取下载文件大小
        /// </summary>
        /// <param name="url">连接</param>
        /// <returns>文件长度</returns>
        public long getDownLength(string url)
        {
            try
            {
                WebRequest wrq = WebRequest.Create(url);
                WebResponse wrp = (WebResponse)wrq.GetResponse();
                wrp.Close();
                return wrp.ContentLength;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Stream> HttpGet(string url)
        {
            try
            {

                HttpResponseMessage msg = client.GetAsync(url).Result;


                Stream strTemp = await msg.Content.ReadAsStreamAsync();

                return strTemp;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion


        #region 上传

        public string HttpPostData(string url, string fileKeyName,
                                    string filePath, NameValueCollection stringDict)
        {
            if (!IsInternet)
            {
                ResponseData responseData = new ResponseData();
                responseData.result = "";
                responseData.message = "当前不存在网络，请稍后再试。";
                return JsonHelper.Serialize(responseData);
            }
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");

            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            FileStream fileStream = null;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }

            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            memStream.Write(beginBoundary, 0, beginBoundary.Length);

            webRequest.Method = Method;
            webRequest.Timeout = 60000;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            if (!string.IsNullOrEmpty(TGT))
            {
                webRequest.Headers.Add("Cookie", TGT);

            }

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: image/png;image/jpg;\r\n\r\n";
                var header = string.Format(filePartHeader, fileKeyName, Path.GetFileName(filePath));
                var headerbytes = Encoding.UTF8.GetBytes(header);
                memStream.Write(headerbytes, 0, headerbytes.Length);
            }

            var buffer = new byte[1024];
            int bytesRead; // =0  
            if (fileStream != null)
            {
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }
            }

            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            try
            {
                var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                                Encoding.GetEncoding("utf-8")))
                {
                    responseContent = httpStreamReader.ReadToEnd();
                }
                if (fileStream != null)
                    fileStream.Close();
                httpWebResponse.Close();
                webRequest.Abort();
                return responseContent;
            }
            catch (Exception ex)
            {
                ResponseData responseData = new ResponseData();
                responseData.result = "1111";
                responseData.message = "当前网络存在问题，请稍后再试。";
                return JsonHelper.Serialize(responseData);
            }
        }


        #endregion

        #endregion
    }
}
