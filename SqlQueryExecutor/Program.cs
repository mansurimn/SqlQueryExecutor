using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SqlQueryExecutor
{
    internal class SqlQueryExecutor
    {
        private static Timer _timer;
        private static string _connectionString;
        private static string _sqlQuery;
        private static string _logFilePath;
        private static int _intervalSeconds;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SqlQueryExecutor));
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            // Load settings from configuration
            LoadConfiguration();

            _logger.Info("Service stopped");

            // Initialize the timer with the interval from config
            _timer = new Timer(_intervalSeconds * 1000); // Convert seconds to milliseconds

            _timer.Elapsed += ExecuteSqlQuery;
            _timer.AutoReset = true;
            _timer.Enabled = true;


            Console.WriteLine("Press [Enter] to exit the program...");
            Console.ReadLine();
        }

        private static void LoadConfiguration()
        {
            try
            {
                _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                _sqlQuery = ConfigurationManager.AppSettings["SqlQuery"];
                _logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
                _intervalSeconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);

                Console.WriteLine($"Loaded Configuration: \nInterval: {_intervalSeconds}s\nLog File: {_logFilePath}\nSQL Query: {_sqlQuery}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
        }

        private static void ExecuteSqlQuery(object sender, ElapsedEventArgs e)
        {
            _logger.Info("Start  ExecuteSqlQuery");
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(_sqlQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            LogQueryResults(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            _logger.Info("End  ExecuteSqlQuery");
        }

        private static void LogQueryResults(SqlDataReader reader)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    _logger.Info($"{reader.GetName(i)}: {reader[i]}");
                }
            }
        }

    }
}
