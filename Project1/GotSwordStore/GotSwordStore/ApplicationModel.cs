using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotSwordStore
{
    public class ApplicationModel
    {
        UserAuthetication user = new UserAuthetication();
        UserType userType = new UserType();
    }

    // House to store data from database
    // Created classes for each table in database
    // Cart & CartItems are temporary as only used during checkout
    // once checkout runs it takes items from order and disposes them
    // Store added on final edit
    // due to quantity not updating properly
    // and created view to join items that are needed
    // together to update qty based on product and store location
    public class UserAuthetication
    {
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; } 
        public int UserTypeID { get; set; }
        public string Address { get; set; }
    }

    public class UserType
    {
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
    }

    public class Stock
    {
        public int StockID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public int StoreID { get; set; }

    }

    public class Locations
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
    }

    public class Cart
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CartItems> CartItems { get; set; }
        public List<Product> Products { get; set; }
    }

    public class CartItems
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
    }

    public class CustomerOrders
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime DateCreated { get; set; }
        public double CartTotal { get; set; }
        public List<CustomerOrderDetails> OrderDetails { get; set; }
        public List<Product> Products { get; set; }

    }

    public class CustomerOrderDetails
    {
        public int OrderDetailsID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
    }

    public class Store
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public int StockID { get; set; }
        public int Qty { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
    }

}

