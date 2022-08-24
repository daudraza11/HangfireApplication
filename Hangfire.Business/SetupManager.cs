using Common;
using Hangfire.DAL;
using Hnagfire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Business
{
    public class SetupManager
    {
        public List<Entity_System_ConfigModel> GetSystemSettings()
        {
            return new SystemSetup_DAL().GetSystemSettings();
        }
        public void InitSystemSettings()
        {
            var lst = GetSystemSettings();
            eEntityIDs.Initialize(lst);
        }
    }
}
