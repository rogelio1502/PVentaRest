using PVentaRest.Class;
using PVentaRest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class NewOrderViewModel : BaseViewModel
    {
        public NewOrderViewModel(INavigation navigation)
        {
            Navigation = navigation;
            GetDishes();

            FoodDishesSelected = new ObservableCollection<DishViewModel>();
            EnableCloseOrderBtn = false;
            AddDishBtnEnabled = false;

        }

        private async Task GetDishes()
        {
            FoodDishesPickerDataSource = new ObservableCollection<DishViewModel>();

            Console.WriteLine("GET DISHES");
            DishS dishService = new DishS();
            List<Dish> dishes = new List<Dish>();
            try
            {
                dishes = await dishService.List();
            }
            catch (Exception)
            {

                await DisplayAlert("Error", "Hubo un problema al listar los datos.", "OK");
            }

            foreach (Dish item in dishes)
            {
                Console.WriteLine(item.ID);
                FoodDishesPickerDataSource.Add(
                    new DishViewModel() { 
                        ID = item.ID, Name = item.Name, Price = item.Price 
                    }
                );
            }

            FoodDishesPickerDataSource.OrderBy(c => c.Name).ToList();
            if(FoodDishesPickerDataSource.Count > 0)
            {
                AddDishBtnEnabled = true;
            }
        }

        //private List<DishViewModel> _FoodDishesPickerDataSource;

        private bool _AddDishBtnEnabled;
        public bool AddDishBtnEnabled
        {
            get
            {
                return _AddDishBtnEnabled;
            }
            set
            {
                SetValue(ref _AddDishBtnEnabled, value);
            }
        }

        public ObservableCollection<DishViewModel> FoodDishesPickerDataSource
        {
            set;
            get;
        }

        private DishViewModel _SelectedFoodDish;
        public DishViewModel SelectedFoodDish
        {
            get
            {
                return _SelectedFoodDish;
            }
            set
            {
                SetProperty(ref _SelectedFoodDish, value);

            }
        }

        public ObservableCollection<DishViewModel> FoodDishesSelected { get; set; }

        private int _AccountTotal;

        public int AccountTotal
        {
            get
            {
                return _AccountTotal;
            }
            set
            {
                SetValue(ref _AccountTotal, value);
                if (AccountTotal > 0)
                {
                    EnableCloseOrderBtn = true;

                }
                else
                {
                    EnableCloseOrderBtn = false;

                }
            }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get
            {
                return _CustomerName;
            }
            set
            {
                SetValue(ref _CustomerName, value);
            }
        }


        private bool _EnableCloseOrderBtn;

        public bool EnableCloseOrderBtn
        {
            get
            {
                return _EnableCloseOrderBtn;
            }
            set
            {
                SetValue(ref _EnableCloseOrderBtn, value);
            }
        }


        private void ComputeAccountTotal()
        {
            int total = 0;
            foreach (DishViewModel item in FoodDishesSelected)
            {
                int subtotal = (int)(item.Quantity * item.Price);
                total = total + subtotal;
            }
            AccountTotal = total;
        }


        public ICommand BackCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            if (FoodDishesSelected.Count > 0)
            {
                bool areYouSure = await Application.Current.MainPage.DisplayAlert("Advertencia", "¿Deseas volver?, perderas la orden?", "Si", "Cancelar");
                if (areYouSure)
                    await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();

            }
            IsBusy = false;

        });

        public ICommand AddDishToListCommand => new Command(async (obj) => {
            if (IsBusy || SelectedFoodDish == null)
                return;

            IsBusy = true;
            if ((FoodDishesSelected.Where(c => SelectedFoodDish.ID == c.ID).Count() == 0) == true)
            {
                SelectedFoodDish.Quantity = 1;
                FoodDishesSelected.Add(SelectedFoodDish);
            }
            else
            {
                await DisplayAlert("Info", "Ya has agregado este item, editalo.", "OK");
            }
            SelectedFoodDish = null;

            ComputeAccountTotal();
            IsBusy = false;

        });

        public ICommand ClearOrderViewCommand => new Command(async (obj) =>
        {
            if (IsBusy)
                return;
            IsBusy = true;
            bool areYouSure = await Application.Current.MainPage.DisplayAlert("Advertencia", "¿Deseas limpiar la orden?", "Si", "Cancelar");
            if (areYouSure)
                FoodDishesSelected.Clear();
            SelectedFoodDish = null;
            CustomerName = "";
            ComputeAccountTotal();

            IsBusy = false;
        });

        public ICommand IncrementQuantityCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            DishViewModel dishVM = (DishViewModel)obj;
            dishVM.Quantity++;
            ComputeAccountTotal();

            IsBusy = false;

        });

        public ICommand DecrementQuantityCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            DishViewModel dishVM = (DishViewModel)obj;
            if (dishVM.Quantity == 1)
            {

                bool areYouSure = await Application.Current.MainPage.DisplayAlert(
                   "Advertencia",
                   String.Format(
                       "¿Deseas eliminar el platillo {0}?"
                       , dishVM.Name
                       ),
                   "Si", "Cancelar");
                if (areYouSure)
                {
                    List<DishViewModel> dishViewModels = FoodDishesSelected.Where(x => x.ID != dishVM.ID).ToList();
                    FoodDishesSelected.Clear();

                    foreach (DishViewModel item in dishViewModels)
                    {
                        FoodDishesSelected.Add(item);
                    }
                }



            }
            else
            {
                dishVM.Quantity--;
                ComputeAccountTotal();
            }
            IsBusy = false;

        });


        public ICommand RemoveItemCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            DishViewModel dishVM = (DishViewModel)obj;
            bool areYouSure = await Application.Current.MainPage.DisplayAlert(
                "Advertencia",
                String.Format(
                    "¿Deseas eliminar el platillo {0}?"
                    , dishVM.Name
                    ),
                "Si", "Cancelar");
            if (areYouSure)
            {
                List<DishViewModel> dishViewModels = FoodDishesSelected.Where(x => x.ID != dishVM.ID).ToList();
                FoodDishesSelected.Clear();

                foreach (DishViewModel item in dishViewModels)
                {
                    FoodDishesSelected.Add(item);
                }

                ComputeAccountTotal();

            }
            IsBusy = false;
        });

        public ICommand CloseOrderCommand => new Command(async(obj)=>{
            if (IsBusy)
                return;
            IsBusy = true;
            if(CustomerName != null && CustomerName != "")
            {
                bool areYouSure = await Application.Current.MainPage.DisplayAlert("Atención", "¿Deseas guardar la orden?", "Si", "Cancelar");
                if (areYouSure)
                {
                    bool receiveMoney = await Application.Current.MainPage.DisplayAlert("Total", "El total es $" + AccountTotal.ToString(), "Guardar", "Cancelar");
                    if (receiveMoney)
                    {
                        // send data to db
                        List<Dish> dishes = new List<Dish>();
                        OrderS orderS = new OrderS();
                        foreach (DishViewModel item in FoodDishesSelected)
                        {
                            dishes.Add(new Dish() { 
                                Name = item.Name,
                                ID = item.ID,
                                Price = item.Price,
                                Total = item.Price * item.Quantity,
                                Quantity = item.Quantity,
                                
                                
                            
                            });
                        }
                        await orderS.Add(
                            new Order() { 
                                CustomerName = CustomerName, 
                                Hour = DateTime.Now, 
                                Total = AccountTotal,
                                Dishes = dishes,
                                Status = "Pagada"
                            }
                        );
                        await DisplayAlert("INFO", "Orden Generada con éxito", "OK");

                        ClearOrderViewCommand.Execute(null);
                        await Navigation.PopAsync();
                    }
                }
            }
            else
            {
                await DisplayAlert("Advertencia", "Ingresa el nombre del cliente", "OK");
            }
            IsBusy = false;
        });

        public ICommand RefreshCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            IsBusy = false;
            IsRefreshing = false;

        });

    }
}
