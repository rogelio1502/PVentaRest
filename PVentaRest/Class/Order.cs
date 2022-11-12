using System;
using System.Collections.Generic;
using System.Text;

namespace PVentaRest.Class
{
    public class Order
    {
        public string ID { get; set; }
        public DateTime Hour { get; set; }
        public string CustomerName { get; set; }
        public int Total { get; set; }
        public List<Dish> Dishes {get;set;}
        public string Status { get; set; }
    }
}
