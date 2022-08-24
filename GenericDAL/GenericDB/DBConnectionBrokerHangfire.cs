using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDAL.GenericDB
{
    public class DBConnectionBrokerHangfire
    {
        private static DBConnectionBrokerHangfire _Instance;

        protected string ConnectionString;

        private DBConnectionBrokerHangfire()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["MailerDb"].ConnectionString;
            //ConnectionString = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("ConnString");
            //ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[1].ToString();
        }


        public static DBConnectionBrokerHangfire GetInstance()
        {
            if (_Instance == null)
                _Instance = new DBConnectionBrokerHangfire();

            return _Instance;
        }

        public SqlConnection Connection
        {
            get
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                    {
                        // connection.Open();
                        connection.ConnectionString = this.ConnectionString;
                        return connection;
                    }
                }
                catch (NullReferenceException ex)
                {
                    throw (new NullReferenceException("Factory should be initialised before getting connection: " + ex.Message));
                }
            }
        }

        ///   <summary>
        ///   This function destroys the OracleConnection for the 
        ///   specified factory. 
        ///       <remarks>
        ///           <returns>void</returns>
        ///       </remarks> 
        ///   </summary>
        public void closeConnection(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public SqlCommand Command
        {
            get
            {
                return new SqlCommand();
            }
        }
    }

}
