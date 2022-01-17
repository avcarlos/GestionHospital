using System.Data;
using System.Data.SqlClient;

namespace GestionHospital.DataAccess
{
    public class DataParameterManager
    {
        public static IDbDataParameter CreateParameter(string name, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            return CreateSqlParameter(name, value, dbType, direction);
        }

        public static IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            return CreateSqlParameter(name, size, value, dbType, direction);
        }

        private static IDbDataParameter CreateSqlParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }

        private static IDbDataParameter CreateSqlParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                Size = size,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }
    }
}