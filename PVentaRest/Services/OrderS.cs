using Firebase.Database.Query;
using PVentaRest.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVentaRest.Services
{
    public class OrderS : Firebase
    {
        public OrderS() : base()
        {

        }

        public async Task<string> Add(Order order)
        {
            return await this.Post("orders", order);
        }

        public async Task<List<Order>> List()
        {
            Console.WriteLine("orders");
            var items = await this.firebaseClient
                  .Child("orders")
                  .OnceAsync<Order>();
            List<Order> orders = new List<Order>();
            Console.WriteLine("Entra");
            foreach (var item in items)
            {
                orders.Add(
                    new Order { 
                        ID = item.Key, 
                        Hour = item.Object.Hour, 
                        CustomerName = item.Object.CustomerName, 
                        Total = item.Object.Total,
                        Status = item.Object.Status,
                    }
                );
            }

            return orders.OrderByDescending(x => x.Hour).ToList();

        }


        public async Task<Order> GetById(string key)
        {
            var item = await this.firebaseClient
                  .Child("orders").Child(key)
                  .OnceSingleAsync<Order>();

            //List<Order> orders = new List<Order>();
            Console.WriteLine(item.CustomerName);
            return item;


        }

        public async Task Update(string Key,Order order)
        {
            await this.UpdateItem("orders", Key,order);
        }
 
    }
}
