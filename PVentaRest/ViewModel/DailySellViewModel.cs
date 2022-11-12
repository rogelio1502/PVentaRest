using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PVentaRest.Class;
using PVentaRest.Services;
using PVentaRest.View;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class DailySellViewModel : BaseViewModel
    {
        public DailySellViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Total = 100;
            DailySells = new ObservableCollection<DailySell>();
            Hour = DateTime.Now;
            BorderColorFrame = Color.Gray;
        }

        private int _Total;
        public int Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetValue(ref _Total, value);
            }
        }

        private DateTime _Hour;
        public DateTime Hour
        {
            get
            {
                return _Hour;
            }
            set
            {
                SetValue(ref _Hour, value);
                GetOrders();
            }
        }

        public async Task GetOrders()
        {
            DailySells.Clear();

            Console.WriteLine("GET ORDERS");
            OrderS orderS = new OrderS();
            List<Order> orders = new List<Order>();
            try
            {
                orders = await orderS.List();
            }
            catch (Exception)
            {

                await DisplayAlert("Error", "Hubo un problema al listar los datos.", "OK");
            }

            foreach (Order item in orders)
            {
                if(item.Hour.Date == Hour.Date)
                {
                    Console.WriteLine(item.Status);
                    DailySells.Add(
                        new DailySell()
                        {
                            ID = item.ID,
                            Hour = item.Hour,
                            CustomerName = item.CustomerName,
                            SellTotal = item.Total,
                            Status = item.Status ?? "Pagada",
                            BorderColorFrame = item.Status == "Pagada" ? Color.Green : Color.Red
                        }
                    ); 
                }   
            }
        }


        private Color _BorderColorFrame;
        public Color BorderColorFrame
        {
            get
            {
                return _BorderColorFrame;
            }
            set
            {
                SetValue(ref _BorderColorFrame, value);
            }
        }

        public ObservableCollection<DailySell> DailySells { get; set; }

        public ICommand RefreshCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            await GetOrders();
            IsBusy = false;
            IsRefreshing = false;

        });

        public ICommand WatchOrderCommand => new Command(
            async (obj) => {
                Console.WriteLine("Hola mundi");
                DailySell ds = (DailySell)obj;
                await Navigation.PushAsync(new OrderPage(ds.ID));
                
            }
        );



    }
}
