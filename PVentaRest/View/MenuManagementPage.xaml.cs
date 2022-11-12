using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVentaRest.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PVentaRest.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuManagementPage : ContentPage
    {
        public MenuManagementPage()
        {
            InitializeComponent();
            BindingContext = new MenuManagementViewModel(Navigation);
        }
    }
}