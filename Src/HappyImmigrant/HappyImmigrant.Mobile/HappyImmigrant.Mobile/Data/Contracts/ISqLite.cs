using SQLite.Net;

namespace HappyImmigrant.Mobile.Data.Contracts
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection();
    }
}
