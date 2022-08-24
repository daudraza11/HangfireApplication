using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum eSystemConfigEntry : int
    {
        //====== IpayLog Configs
        EID_AccessBank = 1,
        EID_UPSA = 2,
        EID_Regent = 3,
        LogPaymentPath = 148,
        LogServicePath = 149,
        LogServiceUser = 150,
        LogServicePass = 151,
        LogServiceInstance = 161,
    }

}
