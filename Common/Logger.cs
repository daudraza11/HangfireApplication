using Elmah;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class Logger
    {
        public static bool LogErrorMessage(string APIName, string FunctionName, string Data, string MessageType, string TransID = "0", string TransRef = "0")
        {
            if (IPayLogsCore.Configs.Where(a => a.API_CODE.ToLower() == APIName.ToLower() && a.METHOD_NAME.ToLower() == FunctionName.ToLower() && a.IS_LOGGING_ENABLED == 1).Count() == 0)
                return true;
            string fMessage = "{\"Time\":\"" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss tt") + "\",\"ApiName\":\"" + APIName + "\",\"ReqType\":\"" + "Request" + "\",\"Function\":\"" + FunctionName + "\",\"URL\":\"" + URL + "\",\"Data\":\"" + Data + "\",\"Headers\":\"" + Headers + "\"}";
            if (string.IsNullOrEmpty(TransID))
                TransID = "0";
            if (string.IsNullOrEmpty(TransRef))
                TransRef = "0";
            IPayLogsCore.Log3rdPartyRequest(new LogRequestCore() { MESSAGE = fMessage, FUNCTION_NAME = FunctionName, MESSAGE_TYPE = "ThirdPartyApi", SOURCE = APIName, TRANSID = TransID, TRANS_REF = TransRef });
            //GetMyLogger(APIName).Info(fMessage);
            return true;
        }
    }
}
