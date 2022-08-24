using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnagfire.Model
{
    public partial class Entity_System_ConfigModel
    {

        private Int32 _Config_ID;
        private String _Config_Key;
        private String _Config_Value;
        private String _Config_Status;

        public Int32 Config_ID
        {
            get { return _Config_ID; }
            set { _Config_ID = value; }
        }
        public String Config_Key
        {
            get { return _Config_Key; }
            set { _Config_Key = value; }
        }
        public String Config_Value
        {
            get { return _Config_Value; }
            set { _Config_Value = value; }
        }
        public String Config_Status
        {
            get { return _Config_Status; }
            set { _Config_Status = value; }
        }

    }
}
