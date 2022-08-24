using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDAL.Base
{
    public class MySQLCommandLocal : ICommand
    {
        private MySqlCommand _command = new MySqlCommand();
        public override MySqlParameterCollection Parameters
        {
            get
            {
                return _command.Parameters;
            }
        }
        public override CommandType CommandType
        {
            get
            {
                return _command.CommandType;
            }
            set
            {
                _command.CommandType = value;
            }
        }
        public override string CommandText
        {
            get
            {
                return _command.CommandText;
            }
            set
            {
                _command.CommandText = value;
            }
        }
        public override MySqlConnection Connection
        {
            get
            {
                return _command.Connection;
            }
            set
            {
                _command.Connection = value;
            }
        }
        public override int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }
        public override object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }
        public override MySqlDataReader ExecuteReader(CommandBehavior beh)
        {
            return _command.ExecuteReader(beh);
        }
        public override MySqlDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }
        public override void Dispose()
        {
            _command.Dispose();
        }
    }

}
