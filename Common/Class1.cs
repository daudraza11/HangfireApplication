using Hnagfire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class eEntityIDs
    {
        public static int Interpay { get { return 1; } }
        private static bool isinit = false;
        public static bool isIntialized { get { return isinit; } }
        public static int UPSA { get; private set; }
        public static int Regent { get; private set; }
        public static int AccessBank { get; private set; }


        public static void Initialize(List<Entity_System_ConfigModel> lstConfig)
        {
            AccessBank = ConfigFuncs.GetIntValue(eSystemConfigEntry.EID_AccessBank, lstConfig);
            UPSA = ConfigFuncs.GetIntValue(eSystemConfigEntry.EID_UPSA, lstConfig);
            Regent = ConfigFuncs.GetIntValue(eSystemConfigEntry.EID_Regent, lstConfig);

            isinit = true;
        }
    }
}
