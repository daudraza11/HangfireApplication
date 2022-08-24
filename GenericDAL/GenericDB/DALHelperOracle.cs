using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace GenericDAL.GenericDB
{
    public class DALHelperOracle
    {
        private static readonly object _syncLock = new object();
        public static OracleConnection GetConnection()
        {
            return DBConnectionBrokerOracle.GetInstance().Connection;
        }
        //public static SqlConnection GetConnectionHangfire()
        //{
        //    return DBConnectionBrokerHangfire.GetInstance().Connection;
        //}
        public static OracleCommand GetCommand()
        {
            return DBConnectionBrokerOracle.GetInstance().Command;
        }
        public static SqlCommand GetCommandHangfire()
        {
            return DBConnectionBrokerHangfire.GetInstance().Command;
        }
        public static SqlConnection GetConnectionHangfire()
        {
            return DBConnectionBrokerHangfire.GetInstance().Connection;
        }
        //public static OracleCommand GetCommandBindByName(CommandType ctype = CommandType.Text)
        //{
        //    var command = DBConnectionBrokerOracle.GetInstance().Command;
        //    command.CommandType = ctype;
        //    command.BindByName = true;
        //    return command;
        //}

        public static void Execute(string SQL)
        {
            OracleCommand Command = GetCommand();
            Command.CommandText = SQL;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    Command.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static void Execute(ref OracleCommand Command)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                using (OracleConnection Conn = GetConnection())
                {
                    try
                    {
                        Command.Connection = Conn;
                        Conn.Open();
                        Command.ExecuteNonQuery();
                        objTrnScope.Complete();
                        Conn.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (Conn.State == ConnectionState.Open)
                        {
                            Conn.Close();
                        }
                        Command.Dispose();
                    }
                }
            }
        }
        public static DataTable GetDataTable(string SQL)
        {
            DataTable dt = new DataTable();
            OracleDataReader dr = null;
            OracleCommand Command = GetCommand();
            Command.CommandText = SQL;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    dr = Command.ExecuteReader(CommandBehavior.SingleResult);
                    dt.Load(dr);
                    Conn.Close();
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static DataTable GetDataTableHangfire(string SQL)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr = null;
            SqlCommand Command = GetCommandHangfire();
            Command.CommandText = SQL;
            using (SqlConnection Conn = GetConnectionHangfire())
            {
                try
                {
                    Command.Connection = Conn;
                    Command.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["MailerDb"].ConnectionString;
                    Conn.Open();
                    dr = Command.ExecuteReader(); //CommandBehavior.SingleResult
                    dt.Load(dr);
                    Conn.Close();
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static DataTable GetDataTable(OracleCommand Command)
        {
            DataTable DT = new DataTable();
            OracleDataReader dr = null;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    dr = Command.ExecuteReader(CommandBehavior.SingleResult);
                    DT.Load(dr);
                    return DT;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static List<T> GetPopulatedData<T>(string SQL, Func<OracleDataReader, T> Populate)
        {
            DataTable dt = new DataTable();
            OracleDataReader dr = null;
            OracleCommand Command = GetCommand();
            Command.CommandText = SQL;
            List<T> lst = new List<T>();
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    dr = Command.ExecuteReader();
                    while (dr.Read())
                    {
                        lst.Add(Populate(dr));
                    }
                    Conn.Close();
                    return lst;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static List<T> GetPopulatedData<T>(OracleCommand Command, Func<OracleDataReader, T> Populate)
        {
            DataTable DT = new DataTable();
            OracleDataReader dr = null;
            List<T> lst = new List<T>();
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    dr = Command.ExecuteReader();
                    while (dr.Read())
                    {
                        lst.Add(Populate(dr));
                    }
                    return lst;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }

        public static DataSet GetDataSet(OracleCommand Command)
        {
            DataSet DS = new DataSet();
            OracleDataReader dr = null;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    OracleDataAdapter adap = new OracleDataAdapter(Command);
                    adap.Fill(DS);
                    return DS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static Object GetFieldValue(string SQL)
        {
            Object result = null;
            OracleCommand Command = GetCommand();
            Command.CommandText = SQL;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    result = Command.ExecuteScalar();
                    Conn.Close();
                    if (result != DBNull.Value)
                        return result;
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }
        public static Object GetFieldValueResult(ref OracleCommand Command, string paramname)
        {
            Object result = null;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    Command.ExecuteScalar();
                    Conn.Close();
                    if (Command.Parameters[paramname] != null)
                        result = Command.Parameters[paramname].Value;
                    if (result != DBNull.Value)
                        return result;
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }

        [Obsolete]
        public static Object GetFieldValue(ref OracleCommand Command)
        {
            Object result = null;
            using (OracleConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    result = Command.ExecuteScalar();
                    Conn.Close();
                    if (result != DBNull.Value)
                        return result;
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    Command.Dispose();
                }
            }
        }


        public static Int32 GetIntVal(OracleParameter param)
        {
            if (param != null && param.Value != null)
            {
                string RegexNumeric = "^-?[0-9]*$";
                if (new Regex(RegexNumeric).IsMatch(Convert.ToString(param.Value)))
                {
                    Int32 NumberValue = 0;
                    bool IsConverted = Int32.TryParse(Convert.ToString(param.Value), out NumberValue);
                    return NumberValue;
                }
            }
            return 0;
        }
        public static string GetStringVal(OracleParameter param)
        {
            if (param != null && param.Value != null)
            {
                if (Convert.ToString(param.Value) != "null")
                {
                    return Convert.ToString(param.Value);
                }
            }
            return "";
        }

        #region Private Methods

        [DebuggerStepThrough]
        private static object GetValue(object o)
        {
            if (o.GetType() == typeof(DBNull))
                return null;
            else
                return o;
        }

        public static void SetValue(ref PropertyInfo[] propertyArray, object entity, object value, string PropertyName)
        {
            PropertyInfo propertyInfo = propertyArray.FirstOrDefault(p => p.Name == PropertyName);
            if (propertyInfo != null)
                propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyInfo.PropertyType));
        }


        #endregion
        public class DBConnectionBrokerOracle
        {
            private static DBConnectionBrokerOracle _Instance;

            protected string ConnectionString;


            private DBConnectionBrokerOracle()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            }

            public static DBConnectionBrokerOracle GetInstance()
            {
                if (_Instance == null)
                    _Instance = new DBConnectionBrokerOracle();

                return _Instance;
            }


            public OracleConnection Connection
            {
                get
                {
                    try
                    {
                        OracleConnection Conn = new OracleConnection(ConnectionString.ToString());

                        return Conn;
                    }
                    catch (NullReferenceException ex)
                    {
                        throw (new NullReferenceException("Factory should be initialised before getting connection: " + ex.Message));
                    }
                }
            }

            public void closeConnection(OracleConnection conn)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            public OracleCommand Command
            {
                get
                {
                    return new OracleCommand();
                }
            }
        }

    }
}
