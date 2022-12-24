using PD_Diary.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PD_Diary.Services
{
    public class NutrientRepository
    {

        SQLiteConnection database;
        public NutrientRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Nutrient>();
        }
        public IEnumerable<Tuple<int, string>> GetItems<T>() where T : INamed, new()
        {
            return (from i in database.Table<T>() select new Tuple<int, string>(i.Id,i.Name)).ToList();
        }
        public async Task<bool> UpdateItemAsync(Nutrient item)
        {
            database.Update(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddItemAsync(Nutrient item)
        {
            database.Insert(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            database.Delete<Nutrient>(id);
            return await Task.FromResult(true);
        }

        public async Task<Nutrient> GetItemAsync(int id)
        {
            return await Task.FromResult(database.Get<Nutrient>(id));
        }
        public async Task<IEnumerable<Tuple<int, string>>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(GetItems<Nutrient>());
        }

        internal IEnumerable GetNutrientsByCategory(int categoryId)
        {
            return (from i in database.Table<Nutrient>()  where i.CategoryID == categoryId
                    select new Tuple<int, string>(i.Id, i.Name)).ToList();
        }

        public delegate void ProcessCommandResultDelegate(object acceptor, string message);
        public static void ExecuteQuery(string commandText, ProcessCommandResultDelegate processFnc)
        {
            List<string> ImportedFiles = new List<string>();
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(App.DATABASE_NAME);
            SQLiteConnection connect = new SQLiteConnection(databasePath);
            {
                SQLiteCommand fmd = connect.CreateCommand(commandText);
                {
                    fmd.ExecuteQuery<Nutrient>();
                }
            }
            connect.Close();
        }

        public Nutrient GetItem(int id)
        {
            return database.Get<Nutrient>(id);
        }
        public Nutrient GetCategory(int id)
        {
            return database.Get<Nutrient>(id);
        }
    }

    public interface INamed
    {
       int Id { get; set; }
       string Name { get; set; }
    }
}
