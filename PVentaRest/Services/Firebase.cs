using Firebase.Database;
using Firebase.Database.Query;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVentaRest.Services
{
    public class Firebase
    {
        protected FirebaseClient firebaseClient { get; set; }
        protected string Database { get; set; }
        public Firebase()
        {
            string auth = "cVUpmlqCJa7tjYkpTcn6WvPNjvphlj0tSNecvq9L"; // your app secret
            this.firebaseClient = new FirebaseClient(
              "https://xamarin-5ffdb-default-rtdb.firebaseio.com",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(auth)
              });
      
    
        }

        protected async Task<string> Post(string coll,object Instance)
        {
            Console.WriteLine("Llega aquí");
            var result = await this.firebaseClient
              .Child(coll)
              .PostAsync(Instance);
            return result.Key.ToString();
        }

        protected async Task DeleteItem(string ChildName, string Key)
        {
            await firebaseClient.Child(ChildName).Child(Key).DeleteAsync();
        }

        public async Task UpdateItem(string ChildName, string Key, object Instance)
        {
            await firebaseClient
                .Child(ChildName)
                .Child(Key)
                .PutAsync(Instance);
        }




    }
}
