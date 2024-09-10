using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace TodoApi.Services
{
  public class ToDoService
  {
    private readonly IMongoCollection<ToDoItem> _toDoItems;

    public ToDoService(IOptions<MongoSettings> mongoSettings)
    {
      var settings = mongoSettings.Value;

      // Retrieves the connection string to MongoDB from the settings
      var mongoClient = new MongoClient(settings.ConnectionString);

      // Retrieves the database name from the settings and obtains the MongoDB database
      var mongoDatabase = mongoClient.GetDatabase(settings.Database);

      // Retrieves ToDoItem document collection from database
      _toDoItems = mongoDatabase.GetCollection<ToDoItem>("ToDoItems");
    }

    public async Task<List<ToDoItem>> GetAsync() =>
        await _toDoItems.Find(item => true).ToListAsync();

    public async Task<ToDoItem> GetAsync(string id) =>
        await _toDoItems.Find<ToDoItem>(item => item.Id == id).FirstOrDefaultAsync();

    public async Task<ToDoItem> CreateAsync(ToDoItem newToDo)
    {
      await _toDoItems.InsertOneAsync(newToDo);
      return newToDo;
    }

    public async Task UpdateAsync(string id, ToDoItem updateToDo) =>
        await _toDoItems.ReplaceOneAsync(item => item.Id == id, updateToDo);

    public async Task RemoveAsync(string id) =>
        await _toDoItems.DeleteOneAsync(item => item.Id == id);

    public async Task RemoveAsync(ToDoItem oldToDo) =>
        await _toDoItems.DeleteOneAsync(item => item.Id == oldToDo.Id);
  }
}