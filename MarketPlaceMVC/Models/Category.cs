using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlaceMVC.Models
{
    public class Category
    {
        public string catid { get; set; }
        public string name { get; set; }
        public string addedby { get; set; }
        public string status { get; set; }
    }
}