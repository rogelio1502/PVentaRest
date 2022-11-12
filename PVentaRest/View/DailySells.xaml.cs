using System;
using System.Collections.Generic;
using PVentaRest.ViewModel;
using Xamarin.Forms;

namespace PVentaRest.View
{
    public partial class DailySells : ContentPage
    {
        DailySellViewModel vm;
        public DailySells()
        {
            InitializeComponent();
            vm = new DailySellViewModel(Navigation);
            BindingContext = vm;
        }

        
    }
}

