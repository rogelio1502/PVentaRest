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
        private string _Username;
        public string Username { get { return _Username; } set { SetValue(ref _Username, value); } }
        private string _Password;
        public string Password { get { return _Password; } set { SetValue(ref _Password, value); } }
        public ICommand ValidateCredentialsCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            if(Username == "admin" && Password == "admin")
            {
                Application.Current.MainPage = new NavigationPage(new MainMenu());
            }
            else
            {
                await DisplayAlert("Error", "Credenciales Inválidas", "OK");
            }
            
           
            IsBusy = false;
        });
    }
}
