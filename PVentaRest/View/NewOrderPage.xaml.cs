using PVentaRest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PVentaRest.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrderPage : ContentPage
    {
        public NewOrderPage()
        {
            InitializeComponent();
            BindingContext = new NewOrderViewModel(Navigation);
        }

   


        protected override bool OnBackButtonPressed()

        {
            return true;
        }


    }
}