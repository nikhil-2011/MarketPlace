using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlaceMVC.Models
{
    public class User
    {
        public string userid { get; set; }
        public string username { get; set; }
        public string userpassword { get; set; }
        public Nullable<long> contact { get; set; }
    }
}