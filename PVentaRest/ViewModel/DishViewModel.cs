using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PVentaRest.ViewModel
{
    public class DishViewModel : BaseViewModel
    {

        public DishViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public DishViewModel()
        {
 
        }

        private string _ID;
        public string ID { get { return _ID; } set { SetValue(ref _ID, value); } }
        
        private string _Name;
        public string Name { get { return _Name; } set { SetValue(ref _Name, value); } }

        private int _Price;
        public int Price { get { return _Price; } set { SetValue(ref _Price, value); } }


        private int _Quantity;
        public int Quantity { get { return _Quantity; } set { SetValue(ref _Quantity, value); } }
    }
}
