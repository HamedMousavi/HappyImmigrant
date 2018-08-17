using HappyImmigrant.Mobile.Data;
using HappyImmigrant.Mobile.Data.Contracts;
using HappyImmigrant.Mobile.WinPhone;
using SQLite.Net;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;


[assembly: Dependency(typeof(SqLiteWinPhone))]
namespace HappyImmigrant.Mobile.WinPhone
{
    public class SqLiteWinPhone : ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, Constants.DatabaseFileName);

            var plat = new SQLite.Net.Platform.WindowsPhone8.SQLitePlatformWP8();
            var conn = new SQLiteConnection(plat, path);

            // Return the database connection 
            return conn;
        }
    }
}
