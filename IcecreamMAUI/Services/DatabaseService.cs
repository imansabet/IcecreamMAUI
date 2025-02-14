using IcecreamMAUI.Data;
using IcecreamMAUI.Models;
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

    public async Task AddCartItem(CartItemEntity entity) 
    {
        await Database.CreateTableAsync<CartItemEntity>();
        await Database.InsertAsync(entity);
    }
    public async Task UpdateCartItem(CartItemEntity entity)
    {
        
        await Database.UpdateAsync(entity);

    }
    public async Task DeleteCartItem(CartItemEntity entity)
    {
        await Database.DeleteAsync(entity); 
    }


    public async Task<CartItemEntity> GetCartItemAsync(int id)
    {
       return await Database.GetAsync<CartItemEntity>(id);
    }



    public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
    
}
