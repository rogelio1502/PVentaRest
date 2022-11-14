using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PVentaRest.View;
using Xamarin.Essentials;

namespace PVentaRest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
            if (Preferences.Get("logged", "") == "yes")
            {
                Application.Current.MainPage = new NavigationPage(new MainMenu());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

  

    }
}
