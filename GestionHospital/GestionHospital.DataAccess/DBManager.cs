using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace GestionHospital.DataAccess
{
    public class DBManager
    {
        private DatabaseHandlerFactory dbFactory;

        private IDatabaseHandler database;

        public DBManager(string connectionStringName = "GestionHospitalDB")
        {
            dbFactory = new DatabaseHandlerFactory(connectionStringName);
            database = dbFactory.CreateDatabase();
        }

        public IDbConnection GetDatabaseConnection()
        {
            return database.CreateConnection();
        }

        public void CloseConnection(IDbConnection connection)
        {
            database.CloseConnection(connection);
        }

        public IDbDataParameter CreateParameter(string name, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(name, value, dbType, ParameterDirection.Input);
        }

        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(name, size, value, dbType, ParameterDirection.Input);
        }

        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return DataParameterManager.CreateParameter(name, size, value, dbType, direction);
        }

        public DataTable GetDataTable(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var dataset = new DataSet();
                    var dataAdaper = database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);

                    return dataset.Tables[0];
                }
            }
        }

        public DataSet GetDataSet(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    var dataset = new DataSet();
                    var dataAdaper = database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);

                    return dataset;
                }
            }
        }

        public IDataReader GetDataReader(string commandText, CommandType commandType, IDbDataParameter[] parameters, out IDbConnection connection)
        {
            IDataReader reader = null;
            connection = database.CreateConnection();
            connection.Open();

            var command = database.CreateCommand(commandText, commandType, connection);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            reader = command.ExecuteReader();

            return reader;
        }

        public void Delete(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public int Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out int lastId)
        {
            lastId = 0;
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt32(newId);
                }
            }

            return lastId;
        }

        public long Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out long lastId)
        {
            lastId = 0;
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt64(newId);
                }
            }

            return lastId;
        }

        public void InsertWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void Update(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters)
        {
            IDbTransaction transactionScope = null;
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public object GetScalarValue(string commandText, CommandType commandType, IDbDataParameter[] parameters = null)
        {
            using (var connection = database.CreateConnection())
            {
                connection.Open();

                using (var command = database.CreateCommand(commandText, commandType, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    return command.ExecuteScalar();
                }
            }
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }

            return obj;
        }

        public List<T> ConsultarDatos<T>(string commandText, IDbDataParameter[] parameters = null)
        {
            var respuesta = GetDataTable(commandText, CommandType.StoredProcedure, parameters);

            return ConvertDataTable<T>(respuesta);
        }
    }
}