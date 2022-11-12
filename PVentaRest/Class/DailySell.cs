using System;
namespace PVentaRest.Class
{
    public class DailySell
    {
        public int Id { get; set; }
        public DateTime Hour { get; set; }
        public int SellTotal { get; set; }
        public string CustomerName { get; set; }
    }
}

