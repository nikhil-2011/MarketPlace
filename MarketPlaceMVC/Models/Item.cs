using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlaceMVC.Models
{
    public class Item
    {
        public string itemid { get; set; }
        public string catid { get; set; }
        public string subid { get; set; }
        public string addedby { get; set; }
        public string name { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> quantity { get; set; }
    }
}