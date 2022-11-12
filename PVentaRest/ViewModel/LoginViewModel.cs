using PVentaRest.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
        }

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public ICommand ValidateCredentialsCommand => new Command((obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            Application.Current.MainPage = new NavigationPage(new MainMenu());
            IsBusy = false;
        });
    }
}
