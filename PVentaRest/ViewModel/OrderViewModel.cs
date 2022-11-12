using PVentaRest.Class;
using PVentaRest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        public OrderViewModel(INavigation navigation,string orderNumber = null)
        {
            Navigation = navigation;
            OrderNumber = orderNumber;
            Dishes = new ObservableCollection<DishViewModel>();
            GetOrder();
            
        }

        private string _OrderNumber;
        public string OrderNumber
        {
            get
            {
                return _OrderNumber;
            }
            set
            {
                SetValue(ref _OrderNumber, value);
            }
        }

        private string _CustomerName;
        public string CustomerName { get { return _CustomerName; } set { SetValue(ref _CustomerName, value); } }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { 
                SetValue(ref _Status, value);
                if (Status == "Pagada")
                {
                    EnableActiveOrderBtn = false;
                    EnableCancelOrderBtn = true;
                }
                else
                {
                    EnableCancelOrderBtn = false;
                    EnableActiveOrderBtn = true;
                }
            }

        }

        private string _Total;
        public string Total { get { return _Total; } set { SetValue(ref _Total, value); } }

        private DateTime _OrderDate;
        public DateTime OrderDate { get { return _OrderDate; } set { SetValue(ref _OrderDate, value); }}

        private bool _EnableActiveOrderBtn;
        public bool EnableActiveOrderBtn { get { return _EnableActiveOrderBtn; } set { SetValue(ref _EnableActiveOrderBtn, value);  } }
    
        private bool _EnableCancelOrderBtn;
        public bool EnableCancelOrderBtn{ get { return _EnableCancelOrderBtn; } set { SetValue(ref _EnableCancelOrderBtn, value); } }

        private Order _Order { get; set; }

        public ObservableCollection<DishViewModel> Dishes { get; set; }

        private async Task GetOrder()
        {
            Dishes.Clear();

            Console.WriteLine("GET ORDER");
            OrderS orderS = new OrderS();
            //List<Order> orders = new List<Order>();
            try
            {
                Order order = await orderS.GetById(OrderNumber);
                _Order = order;
                Status = order.Status;
                Total = order.Total.ToString();
                OrderDate = order.Hour;
                CustomerName = order.CustomerName;
                foreach (Dish item in order.Dishes)
                {
                    Dishes.Add(new DishViewModel()
                    {
                        Name = item.Name,
                        Price = item.Total,
                        Quantity = item.Quantity,
                    });
                }

            }
            catch (Exception)
            {

                await DisplayAlert("Error", "Hubo un problema al listar los datos.", "OK");
            }
        }

        public ICommand CancelOrderCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            
            bool cancel = await App.Current.MainPage.DisplayAlert("Advertencia", "¿Desea marcar como cancelada la orden?", "Si","Cancelar");
            if(cancel)
            {
                OrderS orderS = new OrderS();
                try
                {
                    Status = "Cancelado";
                    _Order.Status = "Cancelado";
                    await orderS.Update(OrderNumber, _Order);
                    await DisplayAlert("Info", "Orden Cancelada", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Error al intentar cancelar la orden", "OK");
                }
            }
            IsBusy = false;
        
        });

        public ICommand ActiveOrderCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            
            bool activate = await App.Current.MainPage.DisplayAlert("Advertencia", "¿Desea marcar como pagada la orden?", "Si", "Cancelar");
            if (activate)
            {
                OrderS orderS = new OrderS();
                try
                {
                    Status = "Pagada";
                    _Order.Status = "Pagada";
                    await orderS.Update(OrderNumber, _Order);
                    await DisplayAlert("Info", "Orden Marcada como Pagada", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Error al intentar activar la orden", "OK");
                }
            }
            IsBusy = false;
        });

        public ICommand GoToReportCommand => new Command(async (obj) =>
        {
            await PdfClickHandler();
        });

        private async Task  PdfClickHandler()
        {
            await Browser.OpenAsync("https://www.africau.edu/images/default/sample.pdf", BrowserLaunchMode.SystemPreferred);
        }


    }
}
