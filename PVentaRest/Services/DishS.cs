using Firebase.Database.Query;
using PVentaRest.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVentaRest.Services
{
    public class DishS : Firebase
    {
        public DishS () : base()
        {

        }

        public async Task<string> Add(Dish dish)
        {
            return await this.Post("dishes",dish);
        }

        public async Task<List<Dish>> List()
        {
            var items = await this.firebaseClient
                  .Child("dishes")
                  .OnceAsync<Dish>();
            List<Dish> dishes = new List<Dish>();
            foreach (var item in items)
            {
                dishes.Add(new Dish { 
                    ID = item.Key, 
                    Name = item.Object.Name, 
                    Price = item.Object.Price
                });
            }
            return dishes.OrderBy(x => x.Name).ToList();
        }

        public async Task Delete(string Key)
        {
            await this.DeleteItem("dishes", Key);
        }

        public async Task Update(string key, Dish dish)
        {
            await this.UpdateItem("dishes", key, dish);
        }
    }
}
