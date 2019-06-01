﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PD_Diary.Models;
using Xamarin.Forms;

namespace PD_Diary.Services
{
    public class MockDataStore : IDataStore<Nutrient>
    {
        static List<Nutrient> items = new List<Nutrient>
            {
                new Nutrient { Id = id1, Text = "Свинина тушенная",
                    Components = new List<Component> {
                        new Component{Id = ComponentType.Protein, Per100gramm = 14.9},
                        new Component{Id = ComponentType.Fat, Per100gramm = 32.2},
                        new Component{Id = ComponentType.Carbohydrate, Per100gramm = 0},
                        new Component{Id = ComponentType.Sodium, Per100gramm = 456},
                        new Component{Id = ComponentType.Potassium, Per100gramm = 253},
                        new Component{Id = ComponentType.Phosphates, Per100gramm = 160},
                        new Component{Id = ComponentType.Calories, Per100gramm = 349}
                    } },
                new Nutrient { Id = id2, Text = "Сыр Пармезан",
                    Components = new List<Component> {
                        new Component{Id = ComponentType.Protein, Per100gramm = 35.6},
                        new Component{Id = ComponentType.Fat, Per100gramm = 30},
                        new Component{Id = ComponentType.Carbohydrate, Per100gramm = 0},
                        new Component{Id = ComponentType.Sodium, Per100gramm = 705},
                        new Component{Id = ComponentType.Potassium, Per100gramm = 130},
                        new Component{Id = ComponentType.Phosphates, Per100gramm = 840},
                        new Component{Id = ComponentType.Calories, Per100gramm = 412}
                    } },
                new Nutrient { Id = id3, Text = "Банан",
                    Components = new List<Component> {
                        new Component{Id = ComponentType.Protein, Per100gramm = 1.2},
                        new Component{Id = ComponentType.Fat, Per100gramm = 0},
                        new Component{Id = ComponentType.Carbohydrate, Per100gramm = 22.4},
                        new Component{Id = ComponentType.Sodium, Per100gramm = 1},
                        new Component{Id = ComponentType.Potassium, Per100gramm = 395},
                        new Component{Id = ComponentType.Phosphates, Per100gramm = 30},
                        new Component{Id = ComponentType.Calories, Per100gramm = 94}
                    } }
            };
        private const string id1 = "FBFBC500-209F-4A8B-88CF-A6EB8DF01192";
        private const string id2 = "010223F7-094D-4CAE-A3B2-4DF65BE47479";
        private const string id3 = "F8D9A4A4-891F-49F6-8123-80CAD3EDDC85";

        public DailyRecord GetDailyRecord(DateTime date)
        {
            DailyRecord dailyRecord = new DailyRecord { Date = date };
            dailyRecord.AddConsumption(MealType.Breakfast, id1, 200);
            dailyRecord.AddConsumption(MealType.Lunch, id2, 100);
            dailyRecord.AddConsumption(MealType.Lunch, id3, 300);
            return dailyRecord;
        }
        public async Task<bool> AddItemAsync(Nutrient item)
        {
            if (item.Components.Sum(x => x.Per100gramm) != 0)
            {
                if (string.IsNullOrEmpty(item.Text)) { item.Text = "Новый продукт"; }
                if (string.IsNullOrEmpty(item.Id)) { item.Id = Guid.NewGuid().ToString(); }

                items.Add(item);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Nutrient item)
        {
            var oldItem = items.Where((Nutrient arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Nutrient arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Nutrient> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Nutrient>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

    }
}