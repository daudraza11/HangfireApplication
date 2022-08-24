using GenericDAL.GenericDB;
using Hnagfire.Model;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.DAL
{
    public class SystemSetup_DAL
    {
        public List<Entity_System_ConfigModel> GetSystemSettings()
        {
            string SQL = "select * from ENTITY_SYSTEM_CONFIG";
            //return DALHelper.FillDataGeneric<Entity_System_ConfigModel>(SQL);
            return DALHelperOracle.GetPopulatedData<Entity_System_ConfigModel>(SQL, PopulateRecordEntitySystemConfig);
        }
        private Entity_System_ConfigModel PopulateRecordEntitySystemConfig(OracleDataReader dr)
        {
            Entity_System_ConfigModel ObjEntity_System_ConfigModel = new Entity_System_ConfigModel();

            ObjEntity_System_ConfigModel.Config_ID = dr["CONFIG_ID"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["CONFIG_ID"]);
            ObjEntity_System_ConfigModel.Config_Key = dr["CONFIG_KEY"] == DBNull.Value ? default(String) : Convert.ToString(dr["CONFIG_KEY"]);
            ObjEntity_System_ConfigModel.Config_Value = dr["CONFIG_VALUE"] == DBNull.Value ? default(String) : Convert.ToString(dr["CONFIG_VALUE"]);
            ObjEntity_System_ConfigModel.Config_Status = dr["CONFIG_STATUS"] == DBNull.Value ? default(String) : Convert.ToString(dr["CONFIG_STATUS"]);

            return ObjEntity_System_ConfigModel;
        }
    }
}
