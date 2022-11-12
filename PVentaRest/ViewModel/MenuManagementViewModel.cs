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
    public class MenuManagementViewModel : BaseViewModel
    {
        public MenuManagementViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FoodDishes = new ObservableCollection<DishViewModel>
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
            };
            ShowForm = false;

        }

        public ObservableCollection<DishViewModel> FoodDishes { get; set; }

        private bool _ShowForm;
        public bool ShowForm
        {
            get
            {
                return _ShowForm;
            }
            set
            {
                SetValue(ref _ShowForm, value);
            }
        }

        private string _DishPrice;
        public string DishPrice
        {
            get
            {
                return _DishPrice;
            }
            set
            {

                SetValue(ref _DishPrice, value);

                
            }
        }

        private string _DishName;
        public string DishName
        {
            get
            {
                return _DishName;
            }
            set
            {
                SetValue(ref _DishName, value);
            }
        }

        private int EditingId { get; set; }

        public ICommand OpenFormToEditCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            DishViewModel dishViewModel = (DishViewModel)obj;
            Console.WriteLine(dishViewModel.Name);
            EditingId = dishViewModel.ID;
            DishName = dishViewModel.Name.ToString();
            DishPrice = dishViewModel.Price.ToString();
            ShowForm = true;
            IsBusy = false;
        
        });

        public ICommand OpenFormToAddCommand => new Command(async () => {
            if (IsBusy)
                return;
            IsBusy = true;
            EditingId = 0;
            DishName = "".ToString();
            DishPrice = 0.ToString();
            ShowForm = true;
                
            IsBusy = false;

        });

        public ICommand SaveDishCommand => new Command(async () => {
            if (IsBusy)
                return;
            IsBusy = true;
            if (int.Parse(DishPrice) > 0)
            {
                bool save = await Application.Current.MainPage.DisplayAlert(
                    "Atención", String.Format("¿Desea guardar el platillo?\nNombre: {0}\nPrecio: {1}",DishName,DishPrice),
                    "Guardar", "Cancelar");
                if (save)
                {
                    if(EditingId != 0)
                    {
                        Console.WriteLine("Edit");
                        foreach (DishViewModel item in FoodDishes)
                        {
                            if(item.ID == EditingId)
                            {
                                item.Name = DishName;
                                item.Price = int.Parse(DishPrice);
                            }
                        }



                    }
                    else
                    {
                        Console.WriteLine("Save");
                        FoodDishes.Add(new DishViewModel { Name = DishName, Price = int.Parse(DishPrice), ID = DateTime.Now.Millisecond });

                    }

                    ShowForm = false;
                }
               

            }
            else
            {
                await DisplayAlert("Advertencia", "Precio es inválido.", "OK");

            }
            IsBusy = false;

        });

        



    }
}
