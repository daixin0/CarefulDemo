using System;
using System.Net;

namespace Careful.Tool.Helpers
{
    public class ResponseExceptionBase
    {
        public bool IsSuccess { get; set; } //是否成功，成功则不提示

        public bool NeedTip { get; set; } = true; //是否需要错误提示

        public int Code { get; set; } // -1 = 未知异常，-2 =业务异常。

        public string Message { get; set; } //提示信息

        public WebException WebException { get; set; }

        public Exception Exception { get; set; }

        public ResponseExceptionBase(WebException ex)
        {
            this.WebException = ex;
            this.Code = (int)ex.Status;
            this.Message = this.GetMessage(ex.Status,ex.Message);
        }

        public ResponseExceptionBase (Exception ex)
        {
            this.Exception = ex;
            this.Code = -1;
            this.Message = ex.Message;//"未知异常";
        }

        public ResponseExceptionBase(HttpStatusCode code)
        {
            this.Exception = new Exception("http code exception");
            this.Code = (int)code;
            this.Message = this.GetMessage(code);
        }

        public ResponseExceptionBase(string code)
        {
            this.Exception = new Exception("bussiness exception,code = " + code);
            this.Code = -2;
            this.Message = this.GetMessage(code);
        }

        private string GetMessage(WebExceptionStatus code,string msg)
        {
            string message = "异常";
            IsSuccess = false;
            switch (code)
            {
                case WebExceptionStatus.Success:
                    message = "成功";
                    IsSuccess = true;
                    break;
                case WebExceptionStatus.NameResolutionFailure:
                case WebExceptionStatus.ConnectFailure:
                    message = "请检测网络"; // 无法解析服务器名称
                    break;
                case WebExceptionStatus.ReceiveFailure:
                case WebExceptionStatus.SendFailure:
                case WebExceptionStatus.PipelineFailure:
                case WebExceptionStatus.ProtocolError:
                case WebExceptionStatus.ConnectionClosed:
                case WebExceptionStatus.TrustFailure:
                case WebExceptionStatus.SecureChannelFailure:
                case WebExceptionStatus.ServerProtocolViolation:
                case WebExceptionStatus.KeepAliveFailure:
                case WebExceptionStatus.ProxyNameResolutionFailure:
                case WebExceptionStatus.UnknownError:
                case WebExceptionStatus.MessageLengthLimitExceeded:
                case WebExceptionStatus.CacheEntryNotFound:
                case WebExceptionStatus.RequestProhibitedByCachePolicy:
                case WebExceptionStatus.RequestProhibitedByProxy:
                    message = msg; //"发生未知类型的异常";
                    break;
                case WebExceptionStatus.RequestCanceled:
                    message = "请求被取消";
                    break;
                case WebExceptionStatus.Timeout:
                    message = "请求超时";
                    break;
                case WebExceptionStatus.Pending:
                    IsSuccess = true;
                    break;
            }
            return message;
        }

        private string GetMessage(HttpStatusCode code)
        {
            string message = "异常";
            int codeInt = (int)code;
            if(codeInt < 200)
            {
                this.IsSuccess = true;
                this.Message = "提示信息";
            }
            else if(codeInt < 300)
            {
                this.IsSuccess = true;
                this.Message = "请求成功";

            }
            else if(codeInt < 400)
            {
                this.IsSuccess = true;
                this.Message = "请求被重定向";
            }
            else if(codeInt < 500)
            {
                this.IsSuccess = false;
                this.Message = "客户端请求错误";
            }
            else if(codeInt < 600)
            {
                this.IsSuccess = false;
                this.Message = "服务处理错误";
            }
            else
            {
                this.IsSuccess = false;
                this.Message = "未知错误";
            }
            return message;
        }

        private string GetMessage(string code)
        {
            string message = "异常";
            IsSuccess = false;
            switch (code)
            {
                case "0":
                    message = "成功";
                    IsSuccess = true;
                    break;
                case "3002":
                    message = "未登录";
                    break;
                default:
                    IsSuccess = true;
                    break;
            }
            return message;
        }
    }
}
