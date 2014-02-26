using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _dbConnection;
        private readonly SQLiteConnectionString _connectionParameters;
        private readonly SQLiteConnectionPool _sqliteConnectionPool;

        public Database(ISQLitePlatform platform, string databasePath)
        {
            _connectionParameters = new SQLiteConnectionString(databasePath, false);
            _sqliteConnectionPool = new SQLiteConnectionPool(platform);

            _dbConnection = new SQLiteAsyncConnection(() => _sqliteConnectionPool.GetConnection(_connectionParameters));
        }

        public async Task Initialize()
        {
            await _dbConnection.CreateTableAsync<ToDo>();
        }

        public async Task<int> AddNewToDo(ToDo item)
        {
            var result = await _dbConnection.InsertAsync(item);
            return result;
        }

        public async Task<int> UpdateToDo(ToDo item)
        {
            var result = await _dbConnection.UpdateAsync(item);
            return result;
        }

        public async Task<int> DeleteToDo(ToDo item)
        {
            var result = await _dbConnection.DeleteAsync(item);
            return result;
        }

        public async Task<List<ToDo>> GetAllToDos()
        {
            var result = await _dbConnection.Table<ToDo>().OrderByDescending(t => t.TimeStamp).ToListAsync();
            return result;
        }

        public async Task<ToDo> GetToDoById(int id)
        {
            var result = await _dbConnection.Table<ToDo>().Where(t => t.Id == id).FirstOrDefaultAsync();
            return result;
        }
    }
}
