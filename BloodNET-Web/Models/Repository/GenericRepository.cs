﻿using BloodNET_Web.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace BloodNET_Web.Models.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    {
        private readonly string connectionString;
        public GenericRepository(string c)
        {
            connectionString = c;
        }

        public void Add(TEntity entity)
        {
            //get table name
            var tablename = typeof(TEntity).Name;

            var properties =
                typeof(TEntity).GetProperties().Where(p => p.Name != "Id" && p.Name!= "Image");
            var columnNames = string.Join(",", properties.Select(x => x.Name));
            var parameterName =
                string.Join(",", properties.Select(y => "@" + y.Name));

            var query = $"insert into {tablename} ({columnNames}) values({parameterName}) ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var comm = new SqlCommand(query, connection);
                foreach (var prop in properties)
                {
                    comm.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity));
                }
                comm.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            var tablename = typeof(TEntity).Name;

            var query = $"delete from {tablename} where id = @id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public TEntity Get(int id)
        {
            var tablename = typeof(TEntity).Name;

            var query = $"select * from {tablename} where id = @id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                var t = Activator.CreateInstance<TEntity>();
                Map(reader, t);
                return t;
            }

        }

        public void Map(SqlDataReader reader, TEntity entity)
        {
            reader.Read();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var propertyName = reader.GetName(i);
                var property = typeof(TEntity).GetProperty(propertyName);
                property.SetValue(entity, reader.GetValue(i));
            }
        }

    }
}