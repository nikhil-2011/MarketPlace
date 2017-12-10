using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlaceMVC.Models
{
    public class Cart
    {
        public string cartid { get; set; }
        public string userid { get; set; }
        public string itemid { get; set; }
        public string itemname { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> price { get; set; }
    }
}