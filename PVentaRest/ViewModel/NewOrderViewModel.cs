using PVentaRest.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class NewOrderViewModel : BaseViewModel
    {
        public NewOrderViewModel(INavigation navigation)
        {
            Navigation = navigation;
            // get and list data from db and 
            FoodDishesPickerDataSource = new List<DishViewModel>
            {
                new DishViewModel
                {
                    ID = 1,
                    Name = "Hamburguesa con Queso",
                    Price = 20,

                },
                new DishViewModel
                {
                    ID = 2,
                    Name = "Pizza Peperoni (8 rebanadas)",
                    Price = 100,
                },
                new DishViewModel
                {
                    ID = 3,
                    Name = "Pizza Peperoni (4 rebanadas)",
                    Price = 100,
                },
                new DishViewModel
                {
                    ID = 4,
                    Name = "Pizza Jamón (8 rebanadas)",
                    Price = 100,
                }
            }.OrderBy(c => c.Name).ToList();

            FoodDishesSelected = new ObservableCollection<DishViewModel>();

        }

        private List<DishViewModel> _FoodDishesPickerDataSource;

        public List<DishViewModel> FoodDishesPickerDataSource
        {
            set
            {
                SetValue(ref _FoodDishesPickerDataSource, value);
            }
            get { return _FoodDishesPickerDataSource; }
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
            bool areYouSure = await Application.Current.MainPage.DisplayAlert("Atención", "¿Deseas guardar la orden?", "Si", "Cancelar");
            if (areYouSure)
            {
                bool receiveMoney = await Application.Current.MainPage.DisplayAlert("Total", "El total es $" + AccountTotal.ToString(), "Guardar", "Cancelar");
                if (receiveMoney)
                {
                    // send data to db
                    await DisplayAlert("INFO","Orden Generada con éxito","OK");

                    ClearOrderViewCommand.Execute(null);
                    await Navigation.PopAsync();
                }

            }
            IsBusy = false;


        
        });

    }
}
