using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MarketPlaceDAL;
namespace MarketPlaceMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(FormCollection obj)
        {
            string username = obj["userid"];
            string password = obj["pwd"];

            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            int status=dal.CheckUser(username, password);
            
            if (status==1)
            {
                System.Web.HttpContext.Current.Session["userid"] = dal.GetUserID(username);
                Session["username"] = username;
                return View("LoginPage");
                
            }
            else
            {
                return View("WrongPassword");
            }


        }

        public ActionResult BuyerPane()
        {
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            var itemList = dal.GetItems(userid);
            var customList = new List<Models.CustomItem>();
            foreach (var item in itemList)
            {
                var c =new  Models.CustomItem();
                c.catname = dal.GetCategoryName(item.catid);
                c.itemid = item.itemid;
                c.itemname = item.name;
                c.price = item.price;
                c.quantity = item.quantity;
                c.subcatname = dal.GetSubCatName(item.subid);
                c.seller = dal.GetUserName(item.addedby);
                customList.Add(c);
            }
            return View(customList);
        }

        public ActionResult SellerPane()
        {
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            var itemList = dal.GetItemSeller(userid);
            var customList = new List<Models.CustomItem>();
            foreach (var item in itemList)
            {
                var c = new Models.CustomItem();
                c.catname = dal.GetCategoryName(item.catid);
                c.itemname = item.name;
                c.itemid = item.itemid;
                c.price = item.price;
                c.quantity = item.quantity;
                c.subcatname = dal.GetSubCatName(item.subid);
                c.seller = dal.GetUserName(item.addedby);
                customList.Add(c);
            }
            return View(customList);
        }
        
        public ActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddItem(FormCollection obj)
        {
            string catname = obj["catname"];
            string subcatname = obj["subcatname"];
            string itemname = obj["itemname"];
            string price = obj["price"];
            int quantity =Convert.ToInt32(obj["quantity"]);

            var dal = new MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            string catid;
            catid=dal.CheckCategory(catname, userid);
            if (catid==null)
            {
                catid = dal.AddCategory(catname, userid);
            }

            string subid;
            subid = dal.CheckSubCategory(catid,subcatname);
            if (subid==null)
            {
                subid = dal.AddSubCategory(catid, subcatname);
            }

            string itemid;
            itemid = dal.CheckItem(catid, subid, itemname,userid);
            if (itemid==null)
            {
                dal.AddItemDB(catid, subid, userid, itemname, price, quantity);
            }
            else
            {
                dal.IncreaseItemCount(itemid,quantity);
            }
               

            return RedirectToAction("SellerPane");
        }

        public ActionResult LoginPage()
        {
            return View("LoginPage");
        }

        public ActionResult SignUP()
        {
            return View("SignUP");
        }

        [HttpPost]
        public ActionResult SignUP(FormCollection obj)
        {
            var dal = new MarketPlaceRepository();
            string username = obj["username"];
            string password = obj["userpassword"];
            
            string contact = obj["contact"];
            int status=dal.AddUser(username,password,contact);
            if (status==1)
            {
                System.Web.HttpContext.Current.Session["userid"] = dal.GetUserID(username);
                Session["username"] = username;

                return View("LoginPage");
            }
            else if(status==2)
            {
                return View("UserExists");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult CategoryIndex()
        {

            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            var categoryList = dal.GetCategory(userid);
            var categories = new List<Models.Category>();
            foreach (var item in categoryList)
            {
                categories.Add(Mapper.Map<Models.Category>(item));
            }
            return View(categories);
        }




        public ActionResult ChangeStatus(Models.Category item)
        {
            try
            {
                // TODO: Add update logic here
                var dal = new MarketPlaceDAL.MarketPlaceRepository();
                dal.UpdateStatus(item.catid);
                return RedirectToAction("CategoryIndex");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LogoutUser()
        {
            Session.RemoveAll();
            Session.Abandon();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetNoStore();
            //FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

       public ActionResult AddToCart(Models.CustomItem item)
        {
            var c = new Models.Cart();
            
            c.itemid = item.itemid;
            c.itemname = item.itemname;
            c.price = item.price;
            c.quantity = 1;
            c.userid = Session["userid"].ToString();

           
            return View(c);
            
           
        }

        public ActionResult AddToCart2(Models.CustomItem item)
        {
            var c = new Models.Cart();

            c.itemid = item.itemid;
            c.itemname = item.itemname;
            c.price = item.price;
            c.quantity = 1;
            c.userid = Session["userid"].ToString();


            return View(c);


        }
        [HttpPost]
        public ActionResult BuyItem(FormCollection obj)
        {
            var c = new Models.Cart();
            c.cartid = "";
            c.itemid = obj["itemid"].ToString();
            c.userid = obj["userid"].ToString();
            c.quantity = Convert.ToInt32(obj["quantity"]);
            c.price =Convert.ToDecimal(obj["price"])*c.quantity;
            c.itemname = obj["itemname"].ToString();
            //return View("trial",c);
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = c.userid;
            if (dal.CheckQuantity(c.itemid, c.quantity) == "Y")
            {
                int status = dal.AddToCart(Mapper.Map<MarketPlaceDAL.Cart>(c),userid);
                //dal.ReduceQuantity(c.itemid, c.quantity);
                if (status == 1)
                {
                    return RedirectToAction("MyCart");

                }
                else
                {
                    return View("Error");
                }
            }

            else
            {
                return RedirectToAction("AddToCart2", c);
            }


        }

        public ActionResult MyCart()
        {
            
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            var cartList = dal.GetCart(userid);
            var cartItems = new List<Models.Cart>();
            foreach (var item in cartList)
            {
                cartItems.Add(Mapper.Map<Models.Cart>(item));
            }
            if (cartItems.Count>0)
            {
                ViewBag.status = 1;
            }
            return View(cartItems);

        }

        public ActionResult MyCart2()
        {

            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            var cartList = dal.GetCart(userid);
            var cartItems = new List<Models.Cart>();
            foreach (var item in cartList)
            {
                cartItems.Add(Mapper.Map<Models.Cart>(item));
            }
            if (cartItems.Count > 0)
            {
                ViewBag.status = 1;
            }
            //List<string> cancelList = new List<string>();
            //cancelList = l;
            string error = "Some items are out of stock.";
            //foreach (var item in cancelList)
            //{
            //    error = error + item.itemname + " ";
            //}
            ViewBag.error = error;
            return View(cartItems);

        }

        public ActionResult PlaceOrder()
        {
            string userid = Session["userid"].ToString();
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            int cancel=dal.CheckOut(userid);
            if (cancel==0)
            {
                return View("OrderPlaced");
            }
            else
            {
                return RedirectToAction("MyCart2");
            }

            
        }
        
        public ActionResult ViewOrder()
        {
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            string userid = Session["userid"].ToString();
            var orderList = dal.GetOrder(userid);
            var orders = new List<Models.Order>();
            foreach (var item in orderList)
            {
                orders.Add(Mapper.Map<Models.Order>(item));
            }
            return View(orders);
        }

        public ActionResult EditItem(Models.CustomItem item)
        {
            return View("EditItem",item);
        }
        [HttpPost]
        public ActionResult EditItemDB(FormCollection obj)
        {
          //  var c = new Models.CustomItem();
           string itemid = obj["itemid"].ToString();
            decimal price = Convert.ToDecimal(obj["price"]);
            int quantity = Convert.ToInt32(obj["quantity"]);
            //string itemid = ViewBag.itemid;
            //ViewBag.itemid = itemid;
            var dal = new MarketPlaceDAL.MarketPlaceRepository();

            //string catid = dal.GetCategoryID(item.catname);
            //string subcatid = dal.GetSubCatID(catid, item.subcatname);
            //string userid = Session["userid"].ToString();
            //string itemid = dal.GetItemID(catid, subcatid, userid, item.itemname);
            // return View("trial");
            dal.UpdateItem(itemid,price, quantity);
            return RedirectToAction("SellerPane");

        }

        public ActionResult DeleteFromCart(Models.Cart cart)
        {
            var dal = new MarketPlaceDAL.MarketPlaceRepository();
            int q=Convert.ToInt32(dal.RemoveItem(cart.cartid));
           // dal.IncreaseItemCount(cart.itemid, q);
            return RedirectToAction("MyCart");
        }
     
    }
}