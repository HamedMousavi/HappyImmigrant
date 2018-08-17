using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DatabaseManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;


namespace DatabaseManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            LoadLists();

            InitializeComponent();
            DataContext = this;
        }

        private void LoadLists()
        {
            DatabaseCountries = new List<ViewModels.Country>();
            FileCountries = new List<ViewModels.Country>();

            var files = Directory.EnumerateFiles(@"E:\Programming\Graphics\Icons\Free\1\flags_2\FlagsPNG").ToList();
            foreach (var file in files)
            {
                FileCountries.Add(new ViewModels.Country
                {
                    FlagIcon = new BitmapImage(new Uri(file)),
                    Name = Path.GetFileNameWithoutExtension(file)
                });
            }

            var countries = LoadCountries();
            foreach (var country in countries)
            {
                DatabaseCountries.Add(new ViewModels.Country
                {
                    FlagIcon = country.FlagImage == null ? null : CreateImage(country.FlagImage),
                    Name = country.Name,
                    Id = country.Id,
                    PersianName = country.PersianName
                });
            }
        }


        private static BitmapImage CreateImage(byte[] imageData)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            return bitmapImage;
        }


        public List<ViewModels.Country> FileCountries { get; set; }
        public List<ViewModels.Country> DatabaseCountries { get; set; }


        public ViewModels.Country SelectedFileCountry { get; set; }
        public ViewModels.Country SelectedDatabaseCountry { get; set; }


        private void StoreFlagIconsButtonClick(object sender, RoutedEventArgs e)
        {
            // get a list of image files
            var files = Directory.EnumerateFiles(@"E:\Programming\Graphics\Icons\Free\1\flags_2\FlagsPNG").ToList();
            if (files == null) throw new InvalidOperationException("Cannot find flag images.");

            var countries = LoadCountries();

            foreach (var country in countries)
            {
                var name = country.Name.Replace(" ", "-");

                var count = files.Count(f => f.ToLower().Contains(name.ToLower()));
                if (count > 1 || count <= 0)
                {
                    Debug.WriteLine(name);
                }
                else
                {
                    var icon = files.FirstOrDefault(f => f.ToLower().Contains(name.ToLower()));
                    if (icon != null)
                    {
                        InsertIcon(country, icon);
                    }
                }
            }

        }


        private static IEnumerable<Country> LoadCountries()
        {
            var countries = new List<Country>();

            var ctx = new AdoNetUnitOfWork(@"data source=E:\Programming\Hamed\Business\Immigration\Data\HI.db3");
            ctx.CreateCommand(false, CommandType.Text, "SELECT * FROM [Countries] ORDER BY [CountryName]", null);
            ctx.Execute(reader =>
            {
                while (reader.Read())
                {
                    countries.Add(new Country
                    {
                        Id = AdoConverter.Read<Int64>(reader, "CountryId", -1),
                        Name = AdoConverter.Read(reader, "CountryName", string.Empty),
                        FlagImage = reader["FlagImage"] as byte[],
                    });
                }
            });

            return countries;
        }


        private static void InsertIcon(Country country, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                InsertBlob(country, new BinaryReader(stream).ReadBytes((Int32)stream.Length));
            }
        }


        private static void InsertBlob(Country country, byte[] data)
        {
            //ctx.CreateCommand(false, CommandType.Text, "UPDATE [Countries] SET [] = {0} WHERE CountryId = {1}");
            var ctx = new AdoNetUnitOfWork(@"data source=E:\Programming\Hamed\Business\Immigration\Data\HI.db3");
            ctx.CreateCommand(false, CommandType.Text, "UPDATE [Countries] SET [FlagImage] = @FlagImage WHERE [CountryId] = @CountryId",
                new List<SQLiteParameter>
                {
                    new SQLiteParameter("@CountryId", country.Id),
                    new SQLiteParameter("@FlagImage", data)
                });
            ctx.ExecuteScalar();

            country.FlagImage = data;
        }


        private void ReplaceFlagIconsButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedDatabaseCountry == null || SelectedFileCountry == null) return;

            var filePath = Path.Combine(@"E:\Programming\Graphics\Icons\Free\1\flags_2\FlagsPNG", SelectedFileCountry.Name + ".png");
            if (!File.Exists(filePath)) return;

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                InsertBlob(SelectedDatabaseCountry, new BinaryReader(stream).ReadBytes((Int32)stream.Length));
            }

            FirePropertyChanged("DatabaseCountries");
            FirePropertyChanged("SelectedDatabaseCountry");
        }



        public event PropertyChangedEventHandler PropertyChanged;


        protected void FirePropertyChanged(string propertyName)
        {
            FirePropertyChanged(this, propertyName);
        }


        protected void FirePropertyChanged(object sender, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
