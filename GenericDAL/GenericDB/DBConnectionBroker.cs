using GenericDAL.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDAL.GenericDB
{
    public class DBConnectionBroker
    {
        ///    <value>
        ///   _Instance property is the single instance of the class that can only
        ///      be instantiate
        ///     </value>
        private static DBConnectionBroker _Instance;

        ///   <value>
        ///   ConnectionString represent the actual connection string for a
        ///    specific Database System for which Factory is initialize
        ///   </value>
        protected string ConnectionString;

        ///   <summary>
        ///   The Constructor of the class will Initialize the Factory using the
        ///   the database provider in configuration file as
        ///     <remarks>
        ///     e.g. :
        ///         <appSettings>
        ///	            <add key="CurrentFactory" value="Oracle.DataAccess.Client"/>
        ///         </appSettings>
        ///     </remarks>
        /// </summary>

        private DBConnectionBroker()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnStrMySql"].ConnectionString;
            //ConnectionString = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("ConnString");
            //ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[1].ToString();
        }

        public static DBConnectionBroker GetInstance()
        {
            if (_Instance == null)
                _Instance = new DBConnectionBroker();

            return _Instance;
        }

        public MySqlConnection Connection
        {
            get
            {
                try
                {
                    MySqlConnection Conn = new MySqlConnection(ConnectionString.ToString());

                    return Conn;
                }
                catch (NullReferenceException ex)
                {
                    throw (new NullReferenceException("Factory should be initialised before getting connection: " + ex.Message));
                }
            }
        }

        ///   <summary>
        ///   This function destroys the MySqlConnection for the 
        ///   specified factory. 
        ///       <remarks>
        ///           <returns>void</returns>
        ///       </remarks> 
        ///   </summary>
        public void closeConnection(MySqlConnection conn)
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

  
        public ICommand Command
        {
            get
            {
                //Read Configuration and return mysql or oracle command or any other db command
                return new MySQLCommandLocal();
            }
        }
    }
}
