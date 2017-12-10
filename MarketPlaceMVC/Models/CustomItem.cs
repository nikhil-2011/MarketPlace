using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlaceMVC.Models
{
    public class CustomItem
    {
        public string itemid { get; set; }
        public string itemname { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> quantity { get; set; }
        public string subcatname { get; set; }
        public string catname { get; set; }
        public string seller { get; set; }
        
    }
}