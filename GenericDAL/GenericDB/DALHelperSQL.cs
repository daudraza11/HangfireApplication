using GenericDAL.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GenericDAL.GenericDB
{
    public class DALHelperSQL
    {
        private static readonly object _syncLock = new object();
        public static MySqlConnection GetConnection()
        {
            return DBConnectionBroker.GetInstance().Connection;
        }
        public static ICommand GetCommand()
        {
            return DBConnectionBroker.GetInstance().Command;
        }

        public static ICommand GetCommand(CommandType ctype = CommandType.Text)
        {
            var command = DBConnectionBroker.GetInstance().Command;
            command.CommandType = ctype;
            return command;
        }

        public static void Execute(string SQL)
        {
            ICommand Command = GetCommand();
            Command.CommandText = SQL;
            using (MySqlConnection Conn = GetConnection())
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
        public static void Execute(ref ICommand Command)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                using (MySqlConnection Conn = GetConnection())
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
        public static int Execute(MySqlCommand Command)
        {
            int id;
            TransactionOptions transactionOption = new TransactionOptions();
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                using (MySqlConnection Conn = GetConnection())
                {
                    try
                    {
                        Command.Connection = Conn;
                        Conn.Open();
                        id = Convert.ToInt32(Command.ExecuteScalar());
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
            return id;
        }
        public static DataTable GetDataTable(string SQL)
        {
            DataTable dt = new DataTable();
            MySqlDataReader dr = null;
            ICommand Command = GetCommand();
            Command.CommandText = SQL;
            using (MySqlConnection Conn = GetConnection())
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
        public static DataTable GetDataTable(MySqlCommand Command)
        {
            DataTable DT = new DataTable();
            MySqlDataReader dr = null;
            using (MySqlConnection Conn = GetConnection())
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
        public static List<T> GetPopulatedData<T>(string SQL, Func<MySqlDataReader, T> Populate)
        {
            DataTable dt = new DataTable();
            MySqlDataReader dr = null;
            ICommand Command = GetCommand();
            Command.CommandText = SQL;
            List<T> lst = new List<T>();
            using (MySqlConnection Conn = GetConnection())
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
        public static List<T> GetPopulatedData<T>(ICommand Command, Func<MySqlDataReader, T> Populate)
        {
            DataTable DT = new DataTable();
            MySqlDataReader dr = null;
            List<T> lst = new List<T>();
            using (MySqlConnection Conn = GetConnection())
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
        public static T2 Execute<T, T2>(T param, Action<T, ICommand> FuncCommand, Func<ICommand, T2> Outparams)
        {
            ICommand Command = GetCommand();
            FuncCommand(param, Command);
            TransactionOptions transactionOption = new TransactionOptions();
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                using (MySqlConnection Conn = GetConnection())
                {
                    try
                    {
                        Command.Connection = Conn;
                        Conn.Open();
                        Command.ExecuteNonQuery();
                        T2 ret = Outparams(Command);
                        objTrnScope.Complete();
                        Conn.Close();
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("ORA-00001"))
                        {
                            throw new Exception("Insert operation failed: Record already exist");
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    finally
                    {
                        if (Conn.State == ConnectionState.Open)
                            Conn.Close();
                        if (Command != null)
                            Command.Dispose();
                    }
                }
            }
        }

        public static DataSet GetDataSet(MySqlCommand Command)
        {
            DataSet DS = new DataSet();
            MySqlDataReader dr = null;
            using (MySqlConnection Conn = GetConnection())
            {
                try
                {
                    Command.Connection = Conn;
                    Conn.Open();
                    MySqlDataAdapter adap = new MySqlDataAdapter(Command);
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
            ICommand Command = GetCommand();
            Command.CommandText = SQL;
            using (MySqlConnection Conn = GetConnection())
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
        public static Object GetFieldValue(ref MySqlCommand Command)
        {
            Object result = null;
            using (MySqlConnection Conn = GetConnection())
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

    }

}

