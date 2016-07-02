using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace JC.DataAccess.MSSQL
{
    /// <summary>
    /// Implements <see cref="IDatabase"/> for the case of Microsoft SQL Server.
    /// </summary>
    public sealed class SqlServerDatabase : IDatabase, IDisposable
    {
        private readonly IColumnDescriptorProvider _ColumnDescriptors;
        private readonly string _ConnectionString;
        private SqlConnection _Connection;

#if DEBUG
        //
        //  These events are only available in debug builds because they are
        //  a testibility provision.
        private event EventHandler _ConnectionOpened;
        private event EventHandler _ConnectionClosed;
#endif
        private readonly IParameterDescriptorProvider _ParameterDescriptors;
        private SqlTransaction _TX;


        public SqlServerDatabase(string connectionString)
            : this(connectionString, new ParameterDescriptorProvider(),
                 new ColumnDescriptorProvider())
        {
        }

        public SqlServerDatabase(string connectionString,
            IParameterDescriptorProvider parameterDescriptors,
            IColumnDescriptorProvider columnDescriptors)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            if (parameterDescriptors == null)
                throw new ArgumentNullException(nameof(parameterDescriptors));
            if (columnDescriptors == null)
                throw new ArgumentNullException(nameof(columnDescriptors));

            _ConnectionString = connectionString;
            _ParameterDescriptors = parameterDescriptors;
            _ColumnDescriptors = columnDescriptors;
        }

#if DEBUG
        public event EventHandler ConnectionOpened
        {
            add { _ConnectionOpened += value; }
            remove { _ConnectionOpened -= value; }
        }

        public event EventHandler ConnectionClosed
        {
            add { _ConnectionClosed += value; }
            remove { _ConnectionClosed -= value; }
        }
#endif

        void IDatabase.Abort()
        {
            if (_TX == null)
                throw new InvalidOperationException("Not in a transaction");

            try
            {
                _TX.Rollback();
            }
            finally
            {
                _TX.Dispose();
                _TX = null;
            }
            CloseSqlConnection(_Connection);
            _Connection = null;
        }

        void IDatabase.BeginTransaction()
        {
            (this as IDatabase).BeginTransaction(IsolationLevel.ReadCommitted);
        }

        void IDatabase.BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_TX != null)
                throw new InvalidOperationException("Already in a transaction");

            if (_Connection == null)
            {
                _Connection = NewSqlConnection(_ConnectionString);
                _Connection.Open();
            }
            _TX = _Connection.BeginTransaction(isolationLevel);
        }

        void IDatabase.Commit()
        {
            if (_TX == null)
                throw new InvalidOperationException("Not in a transaction");

            try
            {
                _TX.Commit();
            }
            finally
            {
                _TX.Dispose();
                _TX = null;
            }
            CloseSqlConnection(_Connection);
            _Connection = null;
        }

        int IDatabase.Execute(string command, CommandType commandType)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            return (this as IDatabase).Execute(command, commandType, null);
        }

        int IDatabase.Execute(string command, CommandType commandType,
            object parameters)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            SqlConnection connection = null;
            if (_Connection == null)
            {
                connection = NewSqlConnection(_ConnectionString);
                connection.Open();
            }
            else
            {
                connection = _Connection;
            }
            try
            {
                IEnumerable<ParameterDescriptor> parameterDescriptors = null;
                int returnValue = -1;
                var cmd = BuildCommand(_ParameterDescriptors, connection, _TX,
                    command, commandType, parameters, out parameterDescriptors);
                using (cmd)
                {
                    returnValue = cmd.ExecuteNonQuery();
                    BackAssignReturnValue(cmd, parameterDescriptors, parameters);
                    BackAssignOutputParameters(cmd, parameterDescriptors, parameters);
                }
                return returnValue;
            }
            finally
            {
                if (connection != _Connection)
                {
                    CloseSqlConnection(connection);
                }
            }
        }

        IEnumerable<T> IDatabase.Query<T>(string command, CommandType commandType)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            return (this as IDatabase).Query<T>(command, commandType, null);
        }

        IEnumerable<T> IDatabase.Query<T>(string command, CommandType commandType,
            object parameters)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            Func<IDataReader, T> defaultMapper = resultset =>
                    MappingFunction.Default<T>(_ColumnDescriptors, resultset);
            return (this as IDatabase).Query<T>(command, commandType, parameters,
                defaultMapper);
        }

        IEnumerable<T> IDatabase.Query<T>(string command, CommandType commandType,
            object parameters, Func<IDataReader, T> mapper)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            SqlConnection connection = null;
            if (_Connection == null)
            {
                connection = NewSqlConnection(_ConnectionString);
                connection.Open();
            }
            else
            {
                connection = _Connection;
            }
            try
            {
                IEnumerable<ParameterDescriptor> parameterDescriptors = null;
                var cmd = BuildCommand(_ParameterDescriptors, connection, _TX,
                    command, commandType, parameters, out parameterDescriptors);
                using (cmd)
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            yield return mapper(dataReader);
                        }
                    }
                }
            }
            finally
            {
                if (connection != _Connection)
                {
                    CloseSqlConnection(connection);
                }
            }
        }

        T IDatabase.QueryScalar<T>(string command, CommandType commandType)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            return (this as IDatabase).QueryScalar<T>(command, commandType, null);
        }

        T IDatabase.QueryScalar<T>(string command, CommandType commandType, object parameters)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            SqlConnection connection = null;
            if (_Connection == null)
            {
                connection = NewSqlConnection(_ConnectionString);
                connection.Open();
            }
            else
            {
                connection = _Connection;
            }
            try
            {
                IEnumerable<ParameterDescriptor> parameterDescriptors = null;
                var cmd = BuildCommand(_ParameterDescriptors, connection, _TX,
                    command, commandType, parameters, out parameterDescriptors);
                using (cmd)
                {
                    object returnValue = cmd.ExecuteScalar();
                    return (T)returnValue;
                }
            }
            finally
            {
                if (connection != _Connection)
                {
                    CloseSqlConnection(connection);
                }
            }
        }

        bool IDatabase.SupportsTransactions
        {
            get { return true; }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }


        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Connection != null)
                {
                    try
                    {
                        _Connection.Close();
                    }
                    catch
                    {
                    }
                    try
                    {
                        _Connection.Dispose();
                    }
                    catch
                    {
                    }
                    _Connection = null;
                }
            }
        }
        
#if DEBUG
        private void OnConnectionOpened()
        {
            var handlers = _ConnectionOpened;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }

        private void OnConnectionClosed()
        {
            var handlers = _ConnectionClosed;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }
#endif

        private SqlConnection NewSqlConnection(string connectionString)
        {
            var connection = new SqlConnection();
            connection.ConnectionString = connectionString;
#if DEBUG
            connection.StateChange += (sender, e) =>
            {
                if (e.CurrentState == ConnectionState.Open)
                {
                    OnConnectionOpened();
                }
                else if (e.CurrentState == ConnectionState.Closed)
                {
                    OnConnectionClosed();
                }
            };
#endif
            return connection;
        }

        private void CloseSqlConnection(SqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            finally
            {
                connection.Dispose();
            }
        }

        private static SqlCommand BuildCommand(IParameterDescriptorProvider paramDescriptorProvider,
            SqlConnection connection, SqlTransaction tx, string commandText,
            CommandType commandType, object parameters,
            out IEnumerable<ParameterDescriptor> paramDescriptors)
        {
            paramDescriptors = null;
            var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            if (tx != null)
            {
                cmd.Transaction = tx;
            }
            if (parameters != null)
            {
                paramDescriptors = paramDescriptorProvider.GetParameters(parameters.GetType());
                if (paramDescriptors.Count(p => p.Direction == ParameterDirection.ReturnValue) > 1)
                {
                    //
                    //  There can be only one!
                    throw new DataException("Multiple return value parameters specified");
                }
                var returnValueParamDesc = paramDescriptors.FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);
                if (returnValueParamDesc != null && (returnValueParamDesc.DbType != DbType.Int32))
                {
                    throw new DataException("A return value parameter, if specified, must be of type Int32");
                }
                foreach (var descriptor in paramDescriptors)
                {
                    var value = descriptor.Property.GetValue(parameters) ?? DBNull.Value;
                    var parameter = new SqlParameter();
                    if (descriptor.ParameterName[0] == '@')
                    {
                        parameter.ParameterName = descriptor.ParameterName;
                    }
                    else
                    {
                        parameter.ParameterName = string.Concat("@", descriptor.ParameterName);
                    }
                    if (value is DataTable)
                    {
                        parameter.SqlDbType = SqlDbType.Structured;
                    }
                    else
                    {
                        parameter.DbType = descriptor.DbType;
                    }
                    parameter.Direction = descriptor.Direction;
                    parameter.IsNullable = descriptor.IsNullable;
                    if (descriptor.Size != -1)
                    {
                        parameter.Size = descriptor.Size;
                    }
                    parameter.Value = value;
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        private static void BackAssignReturnValue(SqlCommand cmd,
            IEnumerable<ParameterDescriptor> parameterDescriptors, object parameters)
        {
            if (parameterDescriptors == null || parameters == null)
                return;

            var returnValueParamDesc = parameterDescriptors.FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);
            if (returnValueParamDesc != null)
            {
                var returnValueParam = GetReturnValueParameter(cmd);
                returnValueParamDesc.Property.SetValue(parameters, returnValueParam.Value);
            }
        }

        private static void BackAssignOutputParameters(SqlCommand cmd,
            IEnumerable<ParameterDescriptor> parameterDescriptors, object parameters)
        {
            if (parameterDescriptors == null || parameters == null)
                return;

            var outputParamDescriptors = parameterDescriptors.Where(p => p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output);
            foreach (var outputParamDescriptor in outputParamDescriptors)
            {
                var sqlParam = cmd.Parameters[outputParamDescriptor.ParameterName];
                outputParamDescriptor.Property.SetValue(parameters, sqlParam.Value);
            }
        }

        private static SqlParameter GetReturnValueParameter(SqlCommand command)
        {
            foreach (SqlParameter parameter in command.Parameters)
            {
                if (parameter.Direction == ParameterDirection.ReturnValue)
                {
                    return parameter;
                }
            }
            return null;
        }

    }
}
