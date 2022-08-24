using Hnagfire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class Extensions
    {
    }
    public static class ConfigFuncs
    {
        public static string GetValue(eSystemConfigEntry ESConfigEntry, List<Entity_System_ConfigModel> Configs)
        {
            Int32 ConfigID = (Int32)ESConfigEntry;
            if (Configs.Where(a => a.Config_ID == ConfigID).Count() > 0)
                return Configs.Where(a => a.Config_ID == ConfigID).FirstOrDefault().Config_Value;
            throw new Exception("Entity System Config Value Not Found. ID = " + ConfigID.ToString());
        }
        public static Int64 GetInt64Value(eSystemConfigEntry ESConfigEntry, List<Entity_System_ConfigModel> Configs)
        {
            return Convert.ToInt64(GetValue(ESConfigEntry, Configs));
        }
        public static Int32 GetIntValue(eSystemConfigEntry ESConfigEntry, List<Entity_System_ConfigModel> Configs)
        {
            return Convert.ToInt32(GetValue(ESConfigEntry, Configs));
        }
        public static bool GetBoolValue(eSystemConfigEntry ESConfigEntry, List<Entity_System_ConfigModel> Configs)
        {
            Int32 val = Convert.ToInt32(GetValue(ESConfigEntry, Configs));
            if (val == 0)
                return false;
            else
                return true;
        }
    }

}
