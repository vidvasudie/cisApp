using cisApp.library;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace cisApp.Core
{
    public static class StoreadProcedure
    {
        static IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();
        public static List<T> GetAllStored<T>(string nameProd, SqlParameter[] parameter) where T : new()
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection Connection = new SqlConnection(configuration.GetConnectionString("MSSqlConnection")))
                {
                    Connection.Open();
                    IDbCommand command = Connection.CreateCommand();
                    command.CommandText = nameProd;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 86400;
                    foreach (SqlParameter r in parameter)
                    {
                        command.Parameters.Add(r);
                    }
                    IDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    Connection.Close();
                }
                return dt.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable GetAllStoredDataTable(string nameProd, SqlParameter[] parameter)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection Connection = new SqlConnection(configuration.GetConnectionString("MSSqlConnection")))
                {
                    Connection.Open();
                    IDbCommand command = Connection.CreateCommand();
                    command.CommandText = nameProd;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 86400;
                    foreach (SqlParameter r in parameter)
                    {
                        command.Parameters.Add(r);
                    }
                    IDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    Connection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<T> GetAllStoredNonparam<T>(string nameProd) where T : new()
        {
            DataTable dt = new DataTable();
            using (SqlConnection Connection = new SqlConnection(configuration.GetConnectionString("MSSqlConnection")))
            {
                Connection.Open();
                IDbCommand command = Connection.CreateCommand();
                command.CommandText = nameProd;
                command.CommandType = CommandType.StoredProcedure;
                IDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                Connection.Close();
            }
            return dt.ToList<T>();
        }

    }
}
