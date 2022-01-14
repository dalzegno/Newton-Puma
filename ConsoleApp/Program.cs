using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
            //BuildConfiguration();

            //#region Ensuring appsettings.json is in the right location
            //Console.WriteLine($"Configuration Directory: {DbConnectionsDirectory()}");

            //var connectionString = _configuration.GetConnectionString("puma");
            //if (!string.IsNullOrEmpty(connectionString))
            //    Console.WriteLine($"Connection string to Database: {connectionString}");
            //else
            //{
            //    Console.WriteLine($"Please copy the 'DbConnections.json' to this location");
            //    return;
            //}
            //#endregion

            //#region Uncomment after scaffolding
            ////BuildOptions();
            ////QueryDatabaseAsync().Wait();
            ////QueryDatabaseWithLinq();
            //#endregion

            var encryptionService = new EncryptionService();

            //var hash = encryptionService.Encrypt("hej hej");
            //Console.WriteLine("Sträng innan kryptering: 'hej hej'");
            //Console.WriteLine($"Sträng efter kryptering: {hash}");

            Console.WriteLine("Skapar ny användare..");

            var peter = new UserDto()
            {
                Id = 10,
                Password = "hej",
                FirstName = "Peter",
                DisplayName = "PeterPeter",
                Email = "peter",
                IsActive = true
            };

            Console.Write("Hashar lösenord...");
            peter.Password = encryptionService.Encrypt(peter.Password);

            var users = new List<UserDto>
            {
                peter
            };

            Console.WriteLine("Användare skapad och sparad i databasen!");
            Console.WriteLine("----------------------------------");

            Console.Write("Logga in: ");
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Lösenord: ");
            string password = Console.ReadLine();

            var hashedPassword = encryptionService.Encrypt(password);

            var foundUser = users.FirstOrDefault(u => u.Password == hashedPassword && u.Email == email);

            if (foundUser != null)
            {
                Console.WriteLine("Hittade användare!");
                Console.WriteLine($"Förnamn: {foundUser.FirstName}");
            }
            else
            {
                Console.WriteLine("Hittade inte användaren");
            }

        }

        //static void BuildConfiguration()
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(DbConnectionsDirectory())
        //        .AddJsonFile("DbConnections.json", optional: true, reloadOnChange: true);
        //    _configuration = builder.Build();
        //}
        //static string DbConnectionsDirectory()
        //{
        //    //LocalApplicationData is a good place to store configuration files.
        //    var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    documentPath = Path.Combine(documentPath, "AOOP2", "EFC", "DbConnections");
        //    if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
        //    return documentPath;
        //}

        //#region Uncomment after scaffolding
        ///*
        //private static void BuildOptions()
        //{
        //    _optionsBuilder = new DbContextOptionsBuilder<PumaDbContext>();
        //    var connectionString = _configuration.GetConnectionString("puma");
        //    _optionsBuilder.UseSqlite(connectionString);
        //}
        //private static async Task QueryDatabaseAsync()
        //{
        //    using (var db = new PumaDbContext(_optionsBuilder.Options))
        //    {
        //        var usersCount = await db.Users.CountAsync();
        //        Console.WriteLine($"Nr of Users: {usersCount}");
        //    }
        //}
        //private static void QueryDatabaseWithLinq()
        //{
        //    using (var db = new PumaDbContext(_optionsBuilder.Options))
        //    {
        //        var users = db.Users;

        //        Console.WriteLine("\n\nQuery Database with Linq");
        //        Console.WriteLine("------------------------");
        //        Console.WriteLine($"Nr of users: {users.Count()}");
        //    }
        //}
        //*/
        //#endregion
    }
}
