using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericDAL.GenericDB
{
    public class CommonDAL
    {
        public Int32 GetIntVal(object columnValue)
        {
            string RegexNumeric = "^-?[0-9]*$";
            if (new Regex(RegexNumeric).IsMatch(Convert.ToString(columnValue)))
            {
                Int32 NumberValue = 0;
                bool IsConverted = Int32.TryParse(Convert.ToString(columnValue), out NumberValue);
                return NumberValue;
            }
            return 0;
        }
        public Int64 GetInt64Val(object columnValue)
        {
            string RegexNumeric = "^-?[0-9]*$";
            if (new Regex(RegexNumeric).IsMatch(Convert.ToString(columnValue)))
            {
                Int64 NumberValue = 0;
                bool IsConverted = Int64.TryParse(Convert.ToString(columnValue), out NumberValue);
                return NumberValue;
            }
            return 0;
        }
        public double GetDoubleVal(object columnValue)
        {
            string RegexNumeric = "^-?([0-9]+)?([,.][0-9]+)?$";
            if (new Regex(RegexNumeric).IsMatch(Convert.ToString(columnValue)))
            {
                double NumberValue = 0;
                bool IsConverted = double.TryParse(Convert.ToString(columnValue), out NumberValue);
                return NumberValue;
            }
            return 0;
        }
        public decimal GetDecimalVal(object columnValue)
        {
            double val = GetDoubleVal(columnValue);
            return (decimal)val;
        }
        public DateTime GetDateTimeVal(object columnValue)
        {
            if (Convert.ToString(columnValue) != "null")
            {
                DateTime dt = default(DateTime);
                bool IsConverted = DateTime.TryParse(Convert.ToString(columnValue), out dt);
                return dt;
            }
            return default(DateTime);
        }
        public static DateTime GetStringToDateTimeVal(object columnValue, string inputFormat) // inputFormat = "ddMMyyyy" | "MMddyyyy" 
        {
            if (columnValue != null && !string.IsNullOrEmpty(Convert.ToString(columnValue)) && Convert.ToString(columnValue) != "0" && Convert.ToString(columnValue) != "null")
            {
                string dateTimeString = Convert.ToString(columnValue);
                dateTimeString = Regex.Replace(dateTimeString, @"[^\u0000-\u007F]", string.Empty);
                DateTime dt = DateTime.ParseExact(dateTimeString, inputFormat, CultureInfo.InvariantCulture);
                return dt;
            }
            return default(DateTime);
        }
        public string GetStringVal(object columnValue)
        {
            if (Convert.ToString(columnValue) != "null")
            {
                return Convert.ToString(columnValue);
            }
            return "";
        }
        public Boolean GetBoolVal(object columnValue)
        {
            if (Convert.ToString(columnValue) != "null")
            {
                return Convert.ToBoolean(columnValue);
            }
            return false;
        }
        public object GetDBNullIfEmpty(object obj, string param = "")
        {
            if (param.ToUpper().Equals("P_LOG_ID"))
            {
                if (obj == null || string.IsNullOrEmpty(Convert.ToString(obj)))
                {
                    return DBNull.Value;
                }
                return obj;
            }
            if (obj == null || string.IsNullOrEmpty(Convert.ToString(obj)) || Convert.ToString(obj) == "0")
            {
                return DBNull.Value;
            }
            return obj;
        }

        public object GetDBNullIfDateEmpty(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(Convert.ToString(obj)) || Convert.ToDateTime(obj) == DateTime.MinValue)
            {
                return DBNull.Value;
            }
            return obj;
        }
        public object GetDBNullIfIntZero(object obj)
        {
            if (obj == null || obj.Equals(0))
            {
                return DBNull.Value;
            }
            return obj;
        }
        public object GetDBNullIfEmpty(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(Convert.ToString(obj)) || Convert.ToString(obj) == "0")
            {
                return DBNull.Value;
            }
            return obj;
        }
    }

}
