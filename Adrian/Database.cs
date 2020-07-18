using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Adrian
{
    public interface IDatabase
    {
        IEnumerable<T> ExecuteQuery<T>(string sql, SqlParameter[] parameters = null);
        int Insert(string sql, SqlParameter[] parameters = null);
        void ExecuteNonQuery(string sql, SqlParameter[] parameters = null);
    }

    public class Database : IDatabase
    {
        private readonly SqlConnectionStringBuilder builder;

        public Database()
        {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "adriangil.database.windows.net";
            builder.UserID = "agil";
            builder.Password = "adnextech13!";
            builder.InitialCatalog = "adrianDb";
        }

        public IEnumerable<T> ExecuteQuery<T>(string sql, SqlParameter[] parameters = null)
        {
            var results = new List<T>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();
                            foreach (var property in typeof(T).GetProperties())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                {
                                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                    property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                                }
                            }
                            results.Add(item);
                        }
                    }
                }
            }
            return results;
        }

        public int Insert(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.Parameters.Add("@ID", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public void ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}