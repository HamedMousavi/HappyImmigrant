using System.Windows.Media.Imaging;

namespace DatabaseManager.ViewModels
{
    public class Country : Models.Country
    {
        public BitmapImage FlagIcon { get; set; }
    }
}
