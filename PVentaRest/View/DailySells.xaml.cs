using System;
using System.Collections.Generic;
using PVentaRest.ViewModel;
using Xamarin.Forms;

namespace PVentaRest.View
{
    public partial class DailySells : ContentPage
    {
        public DailySells()
        {
            InitializeComponent();
            BindingContext = new DailySellViewModel(Navigation);
        }
    }
}

