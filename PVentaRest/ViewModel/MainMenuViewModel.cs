using PVentaRest.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class MainMenuViewModel : BaseViewModel
    {
        public MainMenuViewModel(INavigation navigation)
        {
            Navigation = navigation;

        }

        public ICommand LogoutCommand => new Command((obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            Application.Current.MainPage = new NavigationPage(new LoginPage());
            Preferences.Remove("logged");
            IsBusy = false;

        });

        public ICommand OpenMenuManagementCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            await Navigation.PushAsync(new MenuManagementPage());
            IsBusy = false;

        });

        public ICommand OpenNewOrderCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            await Navigation.PushAsync(new NewOrderPage());
            IsBusy = false;
        });

        public ICommand OpenDailySellsCommand => new Command(async () =>
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await Navigation.PushAsync(new DailySells());
            IsBusy = false;
        });
    }
}
