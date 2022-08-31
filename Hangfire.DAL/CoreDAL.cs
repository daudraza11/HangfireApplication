using GenericDAL.GenericDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hangfire.DAL
{
    public class ENTITY_3RDPARTY_API_CONFIG
    {
        public Int64 RULE_ID { get; set; }
        public string API_CODE { get; set; }
        public string API_NAME { get; set; }
        public Int64 PROVIDER_ENTITY_ID { get; set; }
        public Int64 METHOD_NUMBER { get; set; }
        public string METHOD_NAME { get; set; }
        public Int64 IS_LOGGING_ENABLED { get; set; }
    }
    public class API_LOG_DAL
    {
        public ENTITY_3RDPARTY_API_CONFIG PopulateRecord(IDataRecord dr)
        {
            ENTITY_3RDPARTY_API_CONFIG objModel = new ENTITY_3RDPARTY_API_CONFIG();

            objModel.RULE_ID = GetIntVal(dr["RULE_ID"]);
            objModel.API_CODE = Convert.ToString(dr["API_CODE"]);
            objModel.API_NAME = Convert.ToString(dr["API_NAME"]);
            objModel.PROVIDER_ENTITY_ID = GetIntVal(dr["PROVIDER_ENTITY_ID"]);
            objModel.METHOD_NUMBER = GetIntVal(dr["METHOD_NUMBER"]);
            objModel.METHOD_NAME = Convert.ToString(dr["METHOD_NAME"]);
            objModel.IS_LOGGING_ENABLED = GetIntVal(dr["IS_LOGGING_ENABLED"]);
            return objModel;
        }
        public Int32 GetIntVal(object columnValue)
        {
            string RegexNumeric = "^[0-9]*$";
            if (new Regex(RegexNumeric).IsMatch(Convert.ToString(columnValue)))
            {
                Int32 NumberValue = 0;
                bool IsConverted = Int32.TryParse(Convert.ToString(columnValue), out NumberValue);
                return NumberValue;
            }
            return 0;
        }
        public List<ENTITY_3RDPARTY_API_CONFIG> GetAllAPIConfigs()
        {
            var command = DALHelperOracle.GetCommand();
            command.CommandText = "select * from ENTITY_3RDPARTY_API_CONFIG";
            return DALHelperOracle.GetPopulatedData<ENTITY_3RDPARTY_API_CONFIG>(command, PopulateRecord).ToList();
        }
    }
}
