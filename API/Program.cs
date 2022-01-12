using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PumaDbLibrary;

namespace NorthwindApplication
{
    class Program
    {
        private static IConfigurationRoot _configuration;
       

        #region Uncomment after scaffolding
        //private static DbContextOptionsBuilder<PumaDbContext> _optionsBuilder;
        #endregion

        static void Main(string[] args)
        {
            BuildConfiguration();

            #region Ensuring appsettings.json is in the right location
            Console.WriteLine($"Configuration Directory: {DbConnectionsDirectory()}");

            var connectionString = _configuration.GetConnectionString("puma");
            if (!string.IsNullOrEmpty(connectionString))
                Console.WriteLine($"Connection string to Database: {connectionString}");
            else
            {
                Console.WriteLine($"Please copy the 'DbConnections.json' to this location");
                return;
            }
            #endregion

            #region Uncomment after scaffolding
            //BuildOptions();
            //QueryDatabaseAsync().Wait();
            //QueryDatabaseWithLinq();
            #endregion
        }

        static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(DbConnectionsDirectory())
                .AddJsonFile("DbConnections.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }
        static string DbConnectionsDirectory()
        {
            //LocalApplicationData is a good place to store configuration files.
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = Path.Combine(documentPath, "AOOP2", "EFC", "DbConnections");
            if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
            return documentPath;
        }

        #region Uncomment after scaffolding
        /*
        private static void BuildOptions()
        {
            _optionsBuilder = new DbContextOptionsBuilder<PumaDbContext>();
            var connectionString = _configuration.GetConnectionString("puma");
            _optionsBuilder.UseSqlite(connectionString);
        }
        private static async Task QueryDatabaseAsync()
        {
            using (var db = new PumaDbContext(_optionsBuilder.Options))
            {
                var usersCount = await db.Users.CountAsync();
                Console.WriteLine($"Nr of Users: {usersCount}");
            }
        }
        private static void QueryDatabaseWithLinq()
        {
            using (var db = new PumaDbContext(_optionsBuilder.Options))
            {
                var users = db.Users;
                
                Console.WriteLine("\n\nQuery Database with Linq");
                Console.WriteLine("------------------------");
                Console.WriteLine($"Nr of users: {users.Count()}");
            }
        }
        */
        #endregion
    }
}
