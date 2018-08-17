using HappyImmigrant.Mobile.Data;
using HappyImmigrant.Mobile.Data.Contracts;
using HappyImmigrant.Mobile.Droid;
using SQLite.Net;
using System;
using System.IO;
using Xamarin.Forms;


[assembly: Dependency(typeof(SqLiteAndroid))]
namespace HappyImmigrant.Mobile.Droid
{
    public class SqLiteAndroid : ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, Constants.DatabaseFileName);

            // This is where we copy in the prepopulated database
            Console.WriteLine(path);
            if (!File.Exists(path))
            {
                var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.Hi);  // RESOURCE NAME ###

                // create a write stream
                var writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                // write to the stream
                ReadWriteStream(s, writeStream);
            }

            var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var conn = new SQLiteConnection(plat, path);

            // Return the database connection 
            return conn;
        }

        /// <summary>
        /// helper method to get the database out of /raw/ and into the user filesystem
        /// </summary>
        static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            const int length = 256;
            var buffer = new Byte[length];
            var bytesRead = readStream.Read(buffer, 0, length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }
            readStream.Close();
            writeStream.Close();
        }
    }
}