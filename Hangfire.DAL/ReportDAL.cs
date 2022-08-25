using GenericDAL.GenericDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.DAL
{
    public class ReportDAL
    {
        public DataTable GetCustomersV1Data()
        {

            var Command = DALHelperSQL.GetCommand();
            Command.CommandText = "Select  ifnull(iduser,' ') as ID, ifnull(username,' ')  as 'Full Name',  ifnull(mobile,' ')  as 'Mobile Number' ,ifnull(reg_date,' ')  as 'Registration Date' from customers_v1  order by reg_date desc";
            Command.CommandType = CommandType.Text;
            DataTable dt = DALHelperSQL.GetDataTable(Command.CommandText);
            return dt;
        }
        public DataTable GetCustomersV3Data()
        {

            var Command = DALHelperSQL.GetCommand();
            Command.CommandText = "select iduser as ID, username as 'Full Name', age as Age, mobile as 'Mobile Number' ,reg_date as 'Registration Date' from customers_v3  order by reg_date desc";
            Command.CommandType = CommandType.Text;
            DataTable dt = DALHelperSQL.GetDataTable(Command.CommandText);
            return dt;
        }
        public DataTable GetCustomersV4Data()
        {

            var Command = DALHelperSQL.GetCommand();
            Command.CommandText = "Select  ifnull(iduser,' ') as ID, ifnull(username,' ')  as 'Full Name',  ifnull(mobile,' ')  as 'Mobile Number' ,ifnull(reg_date,' ')  as 'Registration Date' from customers_v4  order by reg_date desc";
            Command.CommandType = CommandType.Text;
            DataTable dt = DALHelperSQL.GetDataTable(Command.CommandText);
            return dt;
        }
    }
}
