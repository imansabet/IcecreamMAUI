﻿using IcecreamMAUI.Data;
using IcecreamMAUI.Models;
using SQLite;

namespace IcecreamMAUI.Services;

public class DatabaseService : IAsyncDisposable
{
    private const string DatabaseName = "IcecreamDb.db3";

    private static readonly string _databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);

    private SQLiteAsyncConnection? _connection;
    private SQLiteAsyncConnection Database => _connection ??= new SQLiteAsyncConnection(_databasePath,
            SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache
        );

    private async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> actionOnDb)
    {
        await Database.CreateTableAsync<CartItemEntity>();
        return await actionOnDb.Invoke();

    }


    public async Task<int> AddCartItem(CartItemEntity entity) => 
        await ExecuteAsync(async () => await Database.InsertAsync(entity));
    
    public async Task UpdateCartItem(CartItemEntity entity) =>
        await ExecuteAsync(async () => await Database.UpdateAsync(entity));
   
    public async Task DeleteCartItem(CartItemEntity entity) =>
        await ExecuteAsync(async () => await Database.DeleteAsync(entity));


    public async Task<CartItemEntity> GetCartItemAsync(int id) =>
        await ExecuteAsync(async () => await Database.GetAsync<CartItemEntity>(id));


    public async Task<List<CartItemEntity>> GetAllCartItemsAsync() =>
        await ExecuteAsync(async () => await Database.Table<CartItemEntity>().ToListAsync());

    public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
    
}
