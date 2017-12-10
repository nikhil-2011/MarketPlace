using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceDAL
{
    public class MarketPlaceRepository
    {
        private MarketPlaceDBContext context;
        public MarketPlaceRepository()
        {
            context = new MarketPlaceDBContext();
        }

        public List<Category> GetCategory(string userid)

        {
            List<Category> m = new List<Category>();
            try
            {
                m = (from c in context.Categories where c.addedby==userid select c).ToList();
                
            }
            catch (Exception)
            {
                m = null;
            }
            return m;
        }
        
        public void UpdateStatus(string id)
        {
            Category cat = new Category();
            cat = (from c in context.Categories where c.catid == id select c).First();
            if (cat.status.Equals("Enabled"))
            {
                cat.status = "Disabled";
            }
            else
            {
                cat.status = "Enabled";
            }
            context.SaveChanges();
        }

        public int CheckUser(string username,string pwd)
        {
            User u = new User();
            u = (from c in context.Users where c.username == username && c.userpassword == pwd select c).FirstOrDefault();
            if (u!=null)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public string GetUserID(string username)
        {
            string id;
            id = (from c in context.Users where c.username == username select c.userid).FirstOrDefault();
            return id;
        }

        public List<Item> GetItems(string userid)
        {
            
            List<Item> result = new List<Item>();
            List<Item> result2 = new List<Item>();
            result = (from c in context.Items where c.addedby != userid select c).ToList();
            var cartList = CheckCart(userid);
            foreach (var item in result)
            {
                foreach (var cartitem in cartList)
                {
                    if (item.itemid.Equals(cartitem.itemid))
                    {
                        item.quantity -= cartitem.quantity;
                    }
                }
            }
            foreach (var item in result)
            {
                if (GetStatus(item.catid)=="Enabled" && item.quantity>0)
                {
                    result2.Add(item);
                }
            }
            return result2;
        }

        public List<Item> GetItemSeller(string userid)
        {
            List<Item> result = new List<Item>();
            result = (from c in context.Items where c.addedby == userid select c).ToList();
            return result;
        }

        public string AddCategory(string catname,string userid)
        {
            Category c = new Category();
            
            var catIdparameter = new ObjectParameter("catid", typeof(string));
            int retVal = context.usp_GetCategoryId(catIdparameter);
            c.catid = catIdparameter.Value.ToString();
            c.addedby = userid;
            c.name = catname;
            c.status = "Enabled";
            context.Categories.Add(c);
            context.SaveChanges();
            return c.catid;
        }

        public string GetCategoryName(string catid)
        {
            string name = (from c in context.Categories where c.catid == catid select c.name).FirstOrDefault();
            return name;
        }

        public string GetSubCatName(string subid)
        {
            string name = (from c in context.SubCategories where c.subid == subid select c.subname).FirstOrDefault();
            return name;
        }

        public string GetUserName(string userid)
        {
            string name = (from c in context.Users where c.userid == userid select c.username).FirstOrDefault();
            return name;
        }
        public string AddSubCategory(string catid, string subcatname)
        {
            SubCategory c = new SubCategory();
            c.catid = catid;
            var subIdparameter = new ObjectParameter("subid", typeof(string));
            int retVal = context.usp_GetSubCatId(subIdparameter);
            c.subid = subIdparameter.Value.ToString();
            c.subname = subcatname;
            context.SubCategories.Add(c);
            context.SaveChanges();
            return c.subid;
        }


        public string AddItemDB(string catid,string subid,string userid,string itemname,string price,int quantity)
        {
            Item c = new Item();
            c.catid = catid;
            c.subid = subid;
            c.addedby = userid;
            var itemIdparameter = new ObjectParameter("itemid", typeof(string));
            int retVal = context.usp_GetItemId(itemIdparameter);
            c.itemid = itemIdparameter.Value.ToString();
            c.name = itemname;
            c.price = Convert.ToDecimal(price);
            c.quantity = Convert.ToInt32(quantity);
            context.Items.Add(c);
            context.SaveChanges();
            return c.catid;
        }

        public int AddUser(string username,string password,string contact)
        {
            int retVal;
            try
            {
                User u = new User();
                string name = (from c in context.Users where c.username == username select c.username).FirstOrDefault();
                if (name == null)
                {
                    var userIdparameter = new ObjectParameter("userid", typeof(string));
                    retVal = context.usp_GetUserId(userIdparameter);
                    u.userid = userIdparameter.Value.ToString();

                    u.username = username;
                    u.userpassword = password;
                    u.contact = Convert.ToInt32(contact);
                    context.Users.Add(u);
                    context.SaveChanges();
                    return 1;
                }

                else
                {
                    return 2;
                }
            }
            catch (Exception)
            {

                return 0;
            }
           


        }

        public string GetStatus(string catid)
        {
            string status = (from c in context.Categories where c.catid == catid select c.status).FirstOrDefault();
            return status;
        }

        public string CheckCategory(string catname,string userid)
        {
            string result = (from c in context.Categories where c.addedby == userid && c.name == catname select c.catid).FirstOrDefault();
            return result;
        }

        public string CheckSubCategory(string catid,string subcatname)
        {
            string result = (from c in context.SubCategories where c.catid == catid && c.subname == subcatname select c.subid).FirstOrDefault();
            return result;
        }

        public int AddToCart(Cart car,string userid)
        {
            try
            {
                
                var chk = (from c in context.Carts where c.itemid == car.itemid && c.userid==userid select c).FirstOrDefault();
                if (chk!=null)
                {
                    chk.quantity += car.quantity;
                    var price = GetItemPrice(car.itemid);
                    chk.price = chk.price * chk.quantity;
                }
                else
                {
                    var cartIdparameter = new ObjectParameter("cartid", typeof(string));
                    int retVal = context.usp_GetCartId(cartIdparameter);
                    car.cartid = cartIdparameter.Value.ToString();
                    car.userid = userid;
                    context.Carts.Add(car);
                }
                
                context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
         

        }

        public List<Cart> GetCart(string userid)

        {
            List<Cart> m = new List<Cart>();
            try
            {
                m = (from c in context.Carts where c.userid == userid select c).ToList();

            }
            catch (Exception)
            {
                m = null;
            }
            return m;
        }

        public string CheckQuantity(string itemid,int? q)
        {
            int quant =Convert.ToInt32((from c in context.Items where c.itemid == itemid select c.quantity).FirstOrDefault());
            if (quant>=q)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }

        public void ReduceQuantity(string itemid,int? q)
        {
            Item d = new Item();
             d = (from c in context.Items where c.itemid == itemid select c).FirstOrDefault();
            d.quantity = d.quantity - q;
            context.SaveChanges();
        }

        public int CheckOut(string userid)
        {
            List<Cart> cartList = new List<Cart>();
            int cancel = 0;
            cartList = (from c in context.Carts where c.userid == userid select c).ToList();
            foreach (var item in cartList)
            {
                if (CheckQuantity(item.itemid,item.quantity)=="N")
                {
                    cancel = 1;
                }
            }

            if (cancel==1)
            {
                return 1;
            }
            else
            {
                var orderIdparameter = new ObjectParameter("orderid", typeof(string));
                int retVal = context.usp_GetOrderId(orderIdparameter);
                string orderid = orderIdparameter.Value.ToString();
                foreach (var item in cartList)
                {
                    Order ord = new Order();
                    ord.orderid = orderid;
                    ord.itemid = item.itemid;
                    ord.itemname = item.itemname;
                    ord.price = item.price;
                    ord.userid = userid;
                    ord.quantity = item.quantity;
                    context.Orders.Add(ord);


                }

                foreach (var item in cartList)
                {
                    var i = (from c in context.Carts where c.itemid == item.itemid select c).FirstOrDefault();
                    ReduceQuantity(item.itemid, item.quantity);
                    context.Carts.Remove(i);
                }



                context.SaveChanges();
                return 0;
            }
           
        }

        public List<Order> GetOrder(string userid)
        {
            var result = (from c in context.Orders where c.userid == userid select c).ToList();
            return result;
        }

        public string CheckItem(string catid,string subid,string itemname,string userid)
        {
            string id = (from c in context.Items where c.catid == catid && c.subid == subid && c.name == itemname && c.addedby == userid select c.itemid).FirstOrDefault();
            return id;
        }

        public void IncreaseItemCount(string itemid,int q)
        {
            var res = (from c in context.Items where c.itemid == itemid select c).FirstOrDefault();
            res.quantity += q;
            context.SaveChanges();
        }

        public string GetCategoryID(string catname)
        {
            string res = (from c in context.Categories where c.name == catname select c.catid).FirstOrDefault();
            return res;
        }
        public string GetSubCatID(string catid,string subcatname)
        {
            string res = (from c in context.SubCategories where c.catid == catid && c.subname==subcatname select c.subid).FirstOrDefault();
            return res;
        }

       public string GetItemID(string catid,string subcatid,string userid,string itemname)
        {
            string res = (from c in context.Items where c.catid == catid && c.subid == subcatid && c.addedby == userid && c.name == itemname select c.itemid).FirstOrDefault();
            return res;
        }

        public void UpdateItem(string itemid,decimal? price,int? quantity)
        {
            Item item = new Item();
             item = (from c in context.Items where c.itemid == itemid select c).FirstOrDefault();
            item.price = price;
            item.quantity = quantity;
            context.SaveChanges();
        }

        public int? RemoveItem(string cartid)
        {
            Cart car = new Cart();
            car = (from c in context.Carts where c.cartid == cartid select c).FirstOrDefault();
            int? q = car.quantity;
            context.Carts.Remove(car);
            context.SaveChanges();
            return q;
        }

        public decimal? GetItemPrice(string itemid)
        {
            var res = (from c in context.Items where c.itemid == itemid select c.price).FirstOrDefault();
            return res;
        }

        public List<Cart> CheckCart(string userid)
        {
            var res = (from c in context.Carts where c.userid == userid select c).ToList();
            return res;
        }
    }
}
