using PVentaRest.Services;
using PVentaRest.Class;
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
    public class MenuManagementViewModel : BaseViewModel
    {
        public MenuManagementViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FoodDishes = new ObservableCollection<DishViewModel>();
            GetDishes();
            ShowForm = false;
            ShowCancelBtn = false;
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

        private bool _ShowCancelBtn;
        public bool ShowCancelBtn
        {
            get
            {
                return _ShowCancelBtn;
            }
            set
            {
                SetValue(ref _ShowCancelBtn, value);
            }
        }

        private string EditingId { get; set; }

        private async Task GetDishes()
        {
            Console.WriteLine("Hola mudni");
            DishS dishService = new DishS();
            try
            {
                List<Dish> dishes = await dishService.List();

                Console.WriteLine("Llega acá");
                FoodDishes.Clear();
                foreach (Dish item in dishes)
                {
                    Console.WriteLine("Hola mundo");
                    FoodDishes.Add(new DishViewModel { ID = item.ID, Name = item.Name, Price = item.Price });
                }
            }
            catch (Exception)
            {

                await DisplayAlert("Error", "Hubo un problema al listar los datos.", "OK");
            }
            

            

        }

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
            ShowCancelBtn = true;
            IsBusy = false;
        
        });

        public ICommand OpenFormToAddCommand => new Command(async () => {
            if (IsBusy)
                return;
            IsBusy = true;
            EditingId = null;
            DishName = "";
            DishPrice = "";
            ShowForm = true;
            ShowCancelBtn = true;
                
            IsBusy = false;

        });
        
        public ICommand SaveDishCommand => new Command(async () => {
            if (IsBusy)
                return;
            IsBusy = true;
            if (DishName != null && DishName != "" && DishPrice != null && DishPrice != "")
            {

                bool isNumeric = false;
                int number;
                isNumeric = int.TryParse(DishPrice, out number);
                if (!isNumeric)
                {
                    await DisplayAlert("Advertencia", "Precio invalido.", "OK");

                }
                else
                {

                    bool save = await Application.Current.MainPage.DisplayAlert(
                        "Atención", String.Format("¿Desea guardar el platillo?\nNombre: {0}\nPrecio: {1}", DishName, DishPrice),
                        "Guardar", "Cancelar");
                    if (save)
                    {
                        if (EditingId != null)
                        {
                            DishS dishService = new DishS();
                            try
                            {
                                await dishService.Update(EditingId, new Dish() { Name = DishName, Price = int.Parse(DishPrice) });
                            }
                            catch (Exception)
                            {
                                IsBusy = false;
                                await DisplayAlert("Error", "Hubo un problema al actualizar los datos, intenta más tarde","OK");
                            
                            }
                            await GetDishes();


                        }
                        else
                        {

                            DishS dishService = new DishS();
                            Dish dish = new Dish()
                            {
                                Name = DishName,
                                Price = int.Parse(DishPrice),
                            };
                            try
                            {
                                string demoResult = await dishService.Add(dish);

                            }
                            catch (Exception)
                            {
                                IsBusy = false;
                                await DisplayAlert("Error", "Hubo un problema al guardar los datos, intenta más tarde", "OK");

                            }

                            await GetDishes();
                        }
                        ShowCancelBtn = false;

                        ShowForm = false;
                    }


                }
            }
            else
            {
                await DisplayAlert("Advertencia", "Datos incompletos.", "OK");

            }
            IsBusy = false;

        });

        public ICommand CloseFormCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            EditingId = null;
            DishName = "".ToString();
            DishPrice = 0.ToString();
            ShowForm = false;
            ShowCancelBtn = false;
            IsBusy = false;
        });

        public ICommand RemoveItemCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            bool remove = await Application.Current.MainPage.DisplayAlert(
                "Advertencia",
                String.Format(
                    "Deseas eliminar el item con nombre {0}",
                    ((DishViewModel)obj).Name
                ),
                "Aceptar",
                "Cancelar"
            );
            if (remove)
            {
                DishS dishService = new DishS();
                try
                {
                    await dishService.Delete(((DishViewModel)obj).ID);
                    await DisplayAlert("Info", "El item ha sido eliminado", "OK");
                }
                catch (Exception)
                {
                    IsBusy = false;
                    await DisplayAlert("Error", "Hubo un problema al eliminar registro, intenta más tarde", "OK");
                }
                
                await GetDishes();
                
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
