using System.Collections.Generic;
using HappyImmigrant.Mobile.Data.Contracts;
using HappyImmigrant.Mobile.Data.Models;
using SQLite.Net;
using Xamarin.Forms;


namespace HappyImmigrant.Mobile.Data
{

    public class CountriesDb
    {

        private readonly SQLiteConnection _database;
        private static readonly object _locker = new object();


        public CountriesDb()
        {
            _database = DependencyService.Get<ISqLite>().GetConnection();
            // create the tables
            //database.CreateTable<TodoItem>();
        }


        public IEnumerable<Country> GetAll()
        {
            lock (_locker)
            {
                return _database.Query<Country>("SELECT * FROM [Countries] ORDER BY [CountryName]");
            }
        }
    }
}
