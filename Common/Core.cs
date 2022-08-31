using Hangfire.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class IPayLogsCore
    {
        private static List<ENTITY_3RDPARTY_API_CONFIG> _Configs { get; set; }

        public static List<ENTITY_3RDPARTY_API_CONFIG> Configs
        {
            get
            {
                if (_Configs != null && _Configs.Count() > 0)
                    return _Configs;
                else
                {
                    _Configs = new API_LOG_DAL().GetAllAPIConfigs();
                    return _Configs;
                }
            }
            set { _Configs = value; }
        }
        public static bool IsInitialized = false;
        public static string IsLive = ConfigurationManager.AppSettings["IsLiveLogs"];
        public static void Initialize()
        {
            if (!IsInitialized)
            {
                if (IsLive == "1")
                {
                    LogPaymentPath = "https://logs.interpayafrica.com/Service/Log.svc/LogPayment";
                    LogServicePath = "https://logs.interpayafrica.com/Service/Log.svc/LogService";
                    LogServiceUser = "Interpay@Log";
                    LogServicePass = "1nterp@y{!@#}";
                    LogInstance = "Live";
                    IsInitialized = true;
                }
                else
                {
                    LogPaymentPath = "https://devlogs.interpayafrica.com/Service/Log.svc/LogPayment";
                    LogServicePath = "https://devlogs.interpayafrica.com/Service/Log.svc/LogService";
                    LogServiceUser = "Interpay@Log";
                    LogServicePass = "1nterp@y{!@#}";
                    LogInstance = "DEV";
                    IsInitialized = true;
                }
            }
        }
        static string LogPaymentPath { get; set; }
        static string LogServicePath { get; set; }
        static string LogServiceUser { get; set; }
        static string LogServicePass { get; set; }
        static string LogInstance { get; set; }

        private static void LogPaymentRequest(LogRequestCore data)
        {
            Initialize();
            data = AssignConfigs(data);
            Task.Run(() => LogAsync(JsonConvert.SerializeObject(data), LogPaymentPath));
        }
        private static LogRequestCore AssignConfigs(LogRequestCore data)
        {
            data.UserName = LogServiceUser;
            data.Password = LogServicePass;
            data.FROM_IP = UserIP.TerminalIP;
            data.APPLICATION_TIME = DateTime.Now.ToString();
            data.INSTANCE = LogInstance;
            return data;
        }

        public static void Log3rdPartyRequest(LogRequestCore data, Exception exp = null)
        {
            if (exp != null)
            {
                data.MESSAGE = (exp.InnerException != null ? (exp.Message + " - " + exp.InnerException.Message) : exp.Message) +
                    data.MESSAGE;
            }
            data.LOG_TYPE = "THIRDPARTY";
            if (data.TRANS_REF.Length > 50)
            {
                data.TRANS_REF = data.TRANS_REF.Substring(0, 50);
            }
            LogPaymentRequest(data);
        }

        public static async void LogAsync(string Text, string ServicePath)
        {
            try
            {
                int NumberOfRetries = 5;
                int DelayOnRetry = 500;
                bool IsLogged = false;
                int RetryCount = 0;
                do
                {
                    try
                    {
                        string strResponse = await Task.Run(() => HttpCall(Text, ServicePath));
                        IPayLogsResponseCore resp = JsonConvert.DeserializeObject<IPayLogsResponseCore>(strResponse);
                        IsLogged = resp.Status == "1";
                    }
                    catch (Exception ex)
                    {
                        string expMsg = ex.InnerException != null ? (ex.InnerException.Message + ex.Message) : ex.Message;
                        IsLogged = false;
                        if (RetryCount < NumberOfRetries)
                        {
                            RetryCount++;
                            Thread.Sleep(DelayOnRetry);
                        }
                        else
                        {
                            throw new Exception(expMsg);
                        }
                    }
                } while (!IsLogged);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.InnerException != null ? (ex.InnerException.Message + ex.Message) : ex.Message, eErrorLogTypes.FileLogger, 0, eLogCallSource.appInterpay);
            }
        }


        private static string HttpCall(string data, string reqURL)
        {
            using (var wb = new WebClient())
            {
                wb.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var strResponse = wb.UploadString(reqURL, data);
                return strResponse;
            }
        }

    }
    public class IPayLogsResponseCore
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class IPayLogsRequestCore
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LogRequestCore : IPayLogsRequestCore
    {
        public string LOG_TYPE { get; set; }
        public string SOURCE { get; set; }
        public string MESSAGE_TYPE { get; set; }
        public string FUNCTION_NAME { get; set; }
        public string FROM_IP { get; set; }
        public string APPLICATION_TIME { get; set; }
        public string MESSAGE { get; set; }
        public string INSTANCE { get; set; }
        public string TRANSID { get; set; }
        public string TRANS_REF { get; set; }
        public double TIMETAKEN { get; set; }
    }



    public static class UserIP
    {
        public static string TerminalIP
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                catch (Exception ex)
                {
                    return "localhost";
                }
            }
        }
    }
}
