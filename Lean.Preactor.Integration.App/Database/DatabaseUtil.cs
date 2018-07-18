using Preactor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
namespace LSB.App.Database
{
    public class DatabaseUtil : IDisposable
    {
        private IPreactor _preactor;
        private IDbConnection _connection;
        public DatabaseUtil(IPreactor preactor)
        {
            _preactor = preactor;
            _connection = GetConnection();
        }

        public DatabaseUtil(string connectionString)
        {
            _connection = GetConnection(connectionString);
        }

        private IDbConnection GetConnection()
        {
            // Get the connection string
            string connectionString = _preactor.ParseShellString("{DB CONNECT STRING}");
            return GetConnection(connectionString);
        }

        private IDbConnection GetConnection(string connectionString)
        {
            // Create a connection to the database
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void ExecuteStoredProcedure(string storedProcedure, int timeout = 30)
        {
            try
            {
                var threadStart = new ThreadStart(() =>
                {
                    try
                    {
                        var command = _connection.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storedProcedure;
                        command.CommandTimeout = timeout;
                        command.ExecuteNonQuery();
                        if (OnExecuteStoredProcedureComplete != null)
                            OnExecuteStoredProcedureComplete.Invoke(this, storedProcedure);
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Error(ex.ToString());
                        if (OnExecuteStoredProcedureError != null)
                            OnExecuteStoredProcedureError.Invoke(this, storedProcedure);
                    }
                });
                var trd = new Thread(threadStart);
                trd.Start();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex.ToString());
                throw;
            }
        }

        public event EventHandler<string> OnExecuteStoredProcedureComplete;
        public event EventHandler<string> OnExecuteStoredProcedureError;
        
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }

    public class DatabaseUtil<T> : IDisposable
    {
        private IPreactor _preactor;
        private IDbConnection _connection;
        public DatabaseUtil(IPreactor preactor)
        {
            _preactor = preactor;
            _connection = GetConnection();
        }

        public DatabaseUtil(string connectionString)
        {
            _connection = GetConnection(connectionString);
        }

        private IDbConnection GetConnection()
        {
            // Get the connection string
            string connectionString = _preactor.ParseShellString("{DB CONNECT STRING}");
            return GetConnection(connectionString);
        }

        private IDbConnection GetConnection(string connectionString)
        {
            // Create a connection to the database
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public event EventHandler<IEnumerable<T>> OnExecuteQueryComplete;
        public event EventHandler<string> OnExecuteQueryError;

        public void ExecuteQuery(string query)
        {
            try
            {
                var threadStart = new ThreadStart(() =>
                {
                    try
                    {
                        //var result = _connection.Query<T>(query);
                        var result = _connection.Query<T>(query);
                        if (OnExecuteQueryComplete != null)
                            OnExecuteQueryComplete.Invoke(this, result);
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Error(ex.ToString());
                        if (OnExecuteQueryError != null)
                            OnExecuteQueryError.Invoke(this, ex.ToString());
                    }
                });
                var trd = new Thread(threadStart);
                trd.Start();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex.ToString());
                throw;
            }
        }

        public void Save(T entity)
        {
            var result = _connection.Update(entity);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
