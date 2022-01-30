using System.Data;
using System.Data.SqlClient;

namespace GestionHospital.DataAccess
{
    public class DataParameterManager
    {
        public static IDbDataParameter CreateParameter(string name, SqlDbType sqlDbType, int size, ParameterDirection direction = ParameterDirection.Input)
        {
            return CreateSqlParameter(name, sqlDbType, size, direction);
        }

        public static IDbDataParameter CreateParameter(string name, SqlDbType sqlDbType, int size, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            return CreateSqlParameter(name, sqlDbType, size, value, direction);
        }

        private static IDbDataParameter CreateSqlParameter(string name, SqlDbType sqlDbType, int size, ParameterDirection direction)
        {
            return new SqlParameter
            {
                SqlDbType = sqlDbType,
                Size = size,
                ParameterName = name,
                Direction = direction
            };
        }

        private static IDbDataParameter CreateSqlParameter(string name, SqlDbType sqlDbType, int size, object value, ParameterDirection direction)
        {
            return new SqlParameter
            {
                SqlDbType = sqlDbType,
                Size = size,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }
    }
}