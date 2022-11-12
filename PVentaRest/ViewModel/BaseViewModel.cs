using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public INavigation Navigation;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnpropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ImageSource photo;
        public ImageSource Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnpropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task DisplayAlert(string title, string message, string txt_btn)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, txt_btn);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                SetProperty(ref _title, value);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                SetProperty(ref _isBusy, value);
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing { get { return isRefreshing; } set { SetValue(ref isRefreshing, value); } }

       

        protected void SetValue<T>(ref T backingFieled, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, backingFieled))
            {
                return;

            }
            backingFieled = value;
            OnPropertyChanged(propertyName);
        }

        #region UtilCustomMethods
        //public NavigationPage GoToHome => new NavigationPage(new MainTabbedPage()) { BarBackgroundColor = Color.FromHex("#ff69b4") };

        //public NavigationPage GoToRegister => new NavigationPage(new RegisterPage()) { BarBackgroundColor = Color.FromHex("#ff69b4") };

        //public NavigationPage GoToLogin => new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.FromHex("#ff69b4") };

        #endregion
    }
}