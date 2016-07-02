using JC.DataAccess.IntegrationTests.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace JC.DataAccess.IntegrationTests
{
    internal static class TestDB
    {
        private static string _ServerName = ".";
        private static string _DatabaseName = "TestDB";

        public static string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        public static string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }

        public static string ConnectionString
        {
            get
            {
                string applicationName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
                return new StringBuilder()
                            .AppendFormat("Server={0};", _ServerName)
                            .AppendFormat("Database={0};", _DatabaseName)
                            .Append("Integrated Security=true;")
                            .AppendFormat("Application Name={0};", applicationName)
                            .AppendFormat("Workstation ID={0}", Environment.MachineName)
                            .ToString();
            }
        }

        public static void Create()
        {
            string scriptText = Resources.CreateDatabase;
            using (var db = new SqlConnection())
            {
                db.ConnectionString = string.Format("Server={0}; Database=master; Integrated Security=true", _ServerName);
                db.Open();
                scriptText = Regex.Replace(scriptText, @"{\s*DB_NAME\s*}", _DatabaseName);
                foreach (var commandText in SqlScript.Parse(scriptText))
                {
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandText = commandText;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        
        public static bool Exists()
        {
            using (var db = new SqlConnection())
            {
                db.ConnectionString = string.Format("Server=.; Database=master; Integrated Security=true", _ServerName);
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT 1 FROM sys.databases WHERE name='{0}';", _DatabaseName);
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar() is int;
                }
            }
        }

        public static void TruncateTable(string tableName)
        {
            using (var db = new SqlConnection())
            {
                db.ConnectionString = ConnectionString;
                db.Open();
                string commandText = string.Format("TRUNCATE TABLE {0};", tableName);
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
