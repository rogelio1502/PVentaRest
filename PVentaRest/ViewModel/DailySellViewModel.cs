using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using PVentaRest.Class;
using PVentaRest.View;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class DailySellViewModel : BaseViewModel
    {
        public DailySellViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Total = 100;
            DailySells = new ObservableCollection<DailySell>
            {
                new DailySell
                {
                    CustomerName = "Rogelio",
                    Hour = DateTime.Parse("2022-11-11 00:00:00"),
                    SellTotal = 134,
                    Id = 1
                },
                new DailySell
                {
                    CustomerName = "Susana",
                    Hour = DateTime.Parse("2022-11-11 00:00:00"),
                    SellTotal = 134,
                    Id = 3
                },
                new DailySell
                {
                    CustomerName = "Angela",
                    Hour = DateTime.Parse("2022-11-11 00:00:00"),
                    SellTotal = 134,
                    Id = 2
                },
            };


        }

        private int _Total;
        public int Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetValue(ref _Total, value);
            }
        }

        public ObservableCollection<DailySell> DailySells { get; set; }


        public ICommand RefreshCommand => new Command(async (obj) => {
            if (IsBusy)
                return;
            IsBusy = true;
            IsBusy = false;
            IsRefreshing = false;

        });
    }
}
