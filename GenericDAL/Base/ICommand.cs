using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDAL.Base
{
    public enum CommandTypes { MYSQL = 1 }
    public abstract class ICommand
    {
        public ICommand GeCommand(CommandTypes cmdType)
        {
            if (cmdType == CommandTypes.MYSQL)
                return new MySQLCommandLocal();
            else
                return null;
        }
        public abstract CommandType CommandType { get; set; }
        public abstract string CommandText { get; set; }
        public abstract MySqlConnection Connection { get; set; }
        public abstract int ExecuteNonQuery();
        public abstract object ExecuteScalar();
        public abstract MySqlDataReader ExecuteReader(CommandBehavior beh);
        public abstract MySqlDataReader ExecuteReader();
        public abstract void Dispose();
        public abstract MySqlParameterCollection Parameters { get; }


    }
}
