using System;
using Xamarin.Forms;

namespace PVentaRest.Class
{
    public class DailySell
    {
        public string ID { get; set; }
        public DateTime Hour { get; set; }
        public int SellTotal { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public Color BorderColorFrame { get; set; }
    }
}

