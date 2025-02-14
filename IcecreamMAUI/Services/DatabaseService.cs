using SQLite;

namespace IcecreamMAUI.Services;

public class DatabaseService : IAsyncDisposable
{
    private const string DatabaseName = "Icecream.db3";

    private static readonly string _databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);

    private SQLiteAsyncConnection? _connection;
    private SQLiteAsyncConnection Database => _connection ??= new SQLiteAsyncConnection(_databasePath,
            SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache
        );


    public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
    
}
