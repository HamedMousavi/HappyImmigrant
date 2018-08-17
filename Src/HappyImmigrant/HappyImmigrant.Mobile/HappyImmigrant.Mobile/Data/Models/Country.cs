using SQLite.Net.Attributes;

namespace HappyImmigrant.Mobile.Data.Models
{
    public class Country
    {
        [PrimaryKey, AutoIncrement]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        //public string PersianName { get; set; }

        public byte[] FlagImage { get; set; }
    }
}
