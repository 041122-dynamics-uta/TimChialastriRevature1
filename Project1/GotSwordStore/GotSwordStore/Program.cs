using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GotSwordStore
{
    internal class Program
    {
		// Fields 
        
        static UserAuthetication user = new UserAuthetication();
        static List<Locations> locations = new List<Locations>();
        static List<CustomerOrders> customerOrders = new List<CustomerOrders>();
        static List<Product> products = new List<Product>();
        static List<Cart> carts = new List<Cart>();
        static List<Store> stores = new List<Store>();
        static double cartTotal = 0;
        static string userStateName = "";
        static int userStoreID = 0;
        static List<string> existUsername = new List<string>();

        public static string connectionString = GetConnectionString();

        // Method to open our sql connection
        static void Main(string[] args)
        {
            string option = "";

            while (option != "0")
            {
                Console.WriteLine("The Game of Thrones Store");
                Console.WriteLine("Select one option below");
                Console.WriteLine("1: Register");
                Console.WriteLine("2: Login");
                Console.WriteLine("0: Exit\n");

                Console.Write("Option: ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        LogIn();
                        break;
                }

                if (user.UserID != 0)
                {
                    while (option != "0")
                    {
                        Console.WriteLine("1: View Locations");
                        Console.WriteLine("2: Orders");
                        Console.WriteLine("3: Products");
                        Console.WriteLine("4: Cart");
                        Console.WriteLine("5: Checkout");
                        Console.WriteLine("6: Stores");
                        Console.WriteLine("0: Exit\n");

                        Console.Write("Option: ");
                        option = Console.ReadLine();

                        switch (option)
                        {
                            case "1":
                                GetLocations();
                                break;
                            case "2":
                                GetPastOrders();
                                break;
                            case "3":
                                GetAllProducts(0);
                                break;
                            case "4":
                                Cart();
                                break;
                            case "5":
                                Checkout();
                                break;
                            case "6":
                                GetStore();
                                break;
                        }
                    }
                    break;
                }
            }
        }


        static private string GetConnectionString()
        {
            return "Server=tcp:timchialastriserver.database.windows.net,1433;Initial Catalog=GOTSwordDb;Persist Security Info=False;User ID=TimChialastriRevature;Password=Selah0416!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=true;";
        }


        //  grabs maximum user id from database and it adds 1 number to it.
        //  this is needed for registration 
        //  so that user does not duplicate
        //  \n to print new line
        //  ExecuteReader method to execute a SQL Command or storedprocedure returns a set of rows from the database
        static private int GetMaxUserID()
        {
            int maxUserID = 0;
            string message = "";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = "SELECT MAX(UserID) As MaxUserID FROM UserAuthentication";

                SqlCommand command = new SqlCommand(query, connection);

                // try catch for errors 
                // try this first and if broken send into the catch
                // catch: display the message of error to user, stop app from running, or add custom msg of error
                // return success if correct

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            maxUserID = reader.GetInt32(0);
                            maxUserID++;
                            // Console.WriteLine("Your value is " + maxUserID + "\n");
                            // GetInt32 Gets the value of the specified column as a 32-bit signed integer.
                            // No conversions are performed; therefore, the data retrieved must already be a 32-bit signed integer
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Rows Found.");
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            return maxUserID;
        }

        //create method so that user can register
        // declare variables needed at top
        static public string Register()
        {
            string full_name = "";
            string password = "";
            string username = "";
            string passwordConfirm = "";
            string address = "";
            int userTypeID = 1;
            GetUsers();

            // Asking user for information 
            Console.Write("Please Enter your username: ");
            username = Console.ReadLine();

            // If the user already exist, continue entering username till the username entered doesn't match one already in the DB.
            // while loop to notify user if username id a duplicate and prompts user to try again
            // IMPORTANT: ADDRESS INPUT ADDED 
            // TO ENSURE THAT DATABASE IS SYNCED WITH PROPER STORE LOCATION
            // TO MAKE SURE THAT CART IS CORRECT FOR BOTH USER AND STORE LOCATION TO COMMUNICATE
            // IF STATEMENT TO NOTIFY USER IF PW DOES NOT MATCH
            while (existUsername.Contains(username))
            {
                Console.Write("Username already exists, enter another username: ");
                username = Console.ReadLine();
            }

            Console.Write("Please enter your full name: ");
            full_name = Console.ReadLine();

            Console.Write("Please enter your current address: Street, State = Example: 111 main st, CA");
            address = Console.ReadLine();

            Console.Write("Please enter your password: ");
            password = Console.ReadLine();

            Console.Write("Please confirm your password: ");
            passwordConfirm = Console.ReadLine();
            
            // IF STATEMENT TO NOTIFY USER IF PW DOES NOT MATCH
            // !STRING IS NOT EQUAL THEN NOTIFY USER
            if (!string.Equals(password, passwordConfirm))
            {
                Console.WriteLine("Passwords must match to continue!!");
                return "Failed!";
            }

            // REMEMBER THAT SQL MUST BE CONNECTED PROPERLY FOR COMMAND TO WORK
            // 5-9-2022 APP FAILED BECAUSE VALUES START AT 0,1,2,3,4,5 
            // ERROR FIXED
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;

                connection.Open();

                var query = string.Format("Insert into UserAuthentication (UserID, fullname, username, password, UserTypeID, Address) " +
                    "values({0}, '{1}', '{2}', '{3}', {4}, '{5}')", GetMaxUserID(), full_name, username, password, userTypeID, address);

                // ExecuteNonQuery Executes statement against the connection and returns the number of rows affected. For UPDATE, INSERT, and DELETE statements,
                // the return value is the number of rows affected by the command.
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("User Saved!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return "Success";
        }

        // METHOD FOR USER LOGIN
        // also add try catch for errors 
        // try this first and if broken send into the catch
        // catch: display the message of error to user, stop app from running, or add custom msg of error
        // return message
        static public string LogIn()
        {
            string username = "";
            string password = "";
            string message = "";

            Console.Write("Please enter your username: ");
            username = Console.ReadLine();

            Console.Write("Please enter your Password: ");
            password = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = string.Format("Select UserID, Username, Password, UserTypeID, Address from UserAuthentication where username = '{0}' AND password = '{1}'", username, password);

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    var information = command.ExecuteReader();
                    if (information != null)
                    {
                        message = "Success\n";
                        Console.WriteLine(message);
                    }
                    else
                    {
                        message = "User not found";
                    }

                    // Added this in order to add user authentication later.
                    // added and done
                    // The Split() method returns an array of strings generated by splitting of original string separated by the delimiters
                    // passed as a parameter in Split() method. The delimiters can be a character or an array of characters or an array of strings
                    // used while loop to 
                    while (information.Read())
                    {
                        user.UserID = information.GetInt32(0);
                        user.Username = information.GetString(1);
                        user.UserTypeID = information.GetInt32(3);
                        user.Address = information.GetString(4);
                    }

                    // Get address location later
                    //
                    var addressSplit = user.Address.Split(',');
                    int addressLength = addressSplit.Length;
                    userStateName = addressSplit[--addressLength];
                    userStateName = userStateName.Replace(" ", "");
                    GetStoreID();

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                Console.WriteLine("\n");

            }
            return message;
        }

        // METHOD FOR GETLOCATIONS
        // // I need to create a new instance in each iteration to avoid the locations list to be overriden.
        // NOTE: CRETED VIEW FOR STORE ID TO BE JOINED WITH
        // QTY , PRODUCT , PRICE ETC
        // SO THAT THESE TABLE COMMUNICATE WITH EACH OTHER
        // W/OUT THIS FEATURE THE SAME ERROR WOULD OCCUR
        // WHICH WOULD NOT UPDATE THE QTY WITH THE 
        //CORRECT LOCATION
        static public void GetLocations()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = "SELECT StoreID, StoreName, StoreLocation FROM Locations";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // We need to create a new instance in each iteration to avoid the locations list to be overriden.
                            Locations location = new Locations();

                            location.StoreID = reader.GetInt32(0);
                            location.StoreName = reader.GetString(1);
                            location.StoreLocation = reader.GetString(2);

                            locations.Add(location);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found!");
                    }

                    Console.WriteLine("Locations: ");
                    foreach (var loc in locations)
                    {
                        Console.WriteLine(String.Format("\tStore Name: {0}" +
                                                        "\n\tStore Location: {1}\n", loc.StoreName, loc.StoreLocation));
                    }
                    Console.WriteLine("\n");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        static public void GetPastOrders()
        {
            double total = 0;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = string.Format("SELECT c.OrderID, UserID, DateCreated, s.ProductID, s.Qty, s.Price," +
                                                "p.ProductName, p.ProductDescription " +
                                                "FROM CustomerOrders as c " +
                                                "INNER JOIN CustomerOrderDetails as s ON c.OrderId = s.OrderId " +
                                                "INNER JOIN Product as p ON s.ProductID = p.ProductID " +
                                                "Where UserID = {0}", user.UserID);

                // NOTE: CRETED VIEW FOR STORE ID TO BE JOINED WITH
                // QTY , PRODUCT , PRICE ETC
                // SO THAT THESE TABLE COMMUNICATE WITH EACH OTHER
                // W/OUT THIS FEATURE THE SAME ERROR WOULD OCCUR
                // WHICH WOULD NOT UPDATE THE QTY WITH THE 
                //CORRECT LOCATION

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (!string.IsNullOrEmpty(user.UserID.ToString()))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                
                                CustomerOrders customerOrder = new CustomerOrders();
                                CustomerOrderDetails customerOrderDetails = new CustomerOrderDetails();
                                Product product = new Product();

                                customerOrder.OrderID = reader.GetInt32(0);
                                customerOrder.UserID = user.UserID;
                                customerOrder.DateCreated = reader.GetDateTime(2);
                                customerOrderDetails.ProductID = reader.GetInt32(3);
                                customerOrderDetails.Qty = reader.GetInt32(4);
                                customerOrderDetails.Price = reader.GetDouble(5);
                                product.ProductName = reader.GetString(6);
                                product.ProductDescription = reader.GetString(7);

                                customerOrder.OrderDetails = new List<CustomerOrderDetails>();
                                customerOrder.Products = new List<Product>();

                                customerOrder.OrderDetails.Add(customerOrderDetails);
                                customerOrder.Products.Add(product);


                                customerOrders.Add(customerOrder);

                            }

                            //total = customerOrders.QTY
                            // FOR LOOP IN CUSTOMER ORDERS
                            // DISPLAY TO USER THE ORDERID
                            // DISPLAY TO USER THE DATE
                            // DISPLAY TO USER THE USER STATE NAME **
                            // USERSTATENAME ADDED TO MAKE USER UNIQUE
                            // THOUGHT THIS WOULD ALSO BE USED TO
                            // HELP USER FIND CLOSEST STORE TO THEIR LOCATION FOR SHIP
                            // BUT NOT NECESSARY FOR THIS APPLICATION
                        }
                        else
                        {
                            Console.WriteLine("No rows found!");
                        }

                        Console.WriteLine("Orders: ");

                        foreach (var ord in customerOrders)
                        {
                            Console.WriteLine(String.Format("\tOrder Number: {0}" +
                                                "\n\tDate: {1}" +
                                                "\n\tStore Location: {2}",
                                                ord.OrderID, ord.DateCreated, userStateName));

                            Console.WriteLine(String.Format("\tProduct Name: {0}" +
                                                            "\n\t\tProduct Description: {1}" +
                                                            "\n\t\tQty: {2}" +
                                                            "\n\t\tPrice: {3}",
                                                            ord.Products.FirstOrDefault().ProductName,
                                                            ord.Products.FirstOrDefault().ProductDescription, ord.OrderDetails.FirstOrDefault().Qty,
                                                            ord.OrderDetails.FirstOrDefault().Price));

                            total = ord.OrderDetails.FirstOrDefault().Price;

                            Console.WriteLine("\tTotal: $" + total);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Login to be able to see your orders.");
                    }
                    Console.WriteLine("\n");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        static public void GetAllProducts(int? locationID)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = "SELECT ProductID, ProductName, ProductDescription, Price FROM Product";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (products.Count <= 0)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.ProductID = reader.GetInt32(0);
                                product.ProductName = reader.GetString(1);
                                product.ProductDescription = reader.GetString(2);
                                product.Price = reader.GetDouble(3);

                                products.Add(product);

                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found!");
                        }

                        Console.WriteLine("Products: ");
                        foreach (var pro in products)
                        {
                            Console.WriteLine(String.Format("\tProduct ID: {0}" +
                                                            "\n\tProduct Name: {1}" +
                                                            "\n\tProduct Description: {2}" +
                                                            "\n\tPrice: ${3}\n", pro.ProductID, pro.ProductName, pro.ProductDescription, pro.Price));
                        }
                        Console.WriteLine("\n");

                    }
                    else
                    {
                        Console.WriteLine("Products: ");
                        foreach (var pro in products)
                        {
                            Console.WriteLine(String.Format("\tProduct ID: {0}" +
                                                            "\n\tProduct Name: {1}" +
                                                            "\n\tProduct Description: {2}" +
                                                            "\n\tPrice: ${3}\n", pro.ProductID, pro.ProductName, pro.ProductDescription, pro.Price));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        // Need a temporary method for carts that is only active while using 
        // and then disposed of unlike other tables
        // Also added a delete method to verify that what im trying to delete actually exists,
        // if it does then conitune to delete it,
        // if not show user that it does not exist.
        // Display items in cart with list
        // if not equal to null 
        // While loop 
        static public void Cart()
        {
            int item = 1;
            string choice = "";
            int itemId = 1;
            string deleteId = "";
            Cart cart = new Cart();
            cart.CartItems = new List<CartItems>();
            cart.Products = new List<Product>();

            if (carts != null)
            {
                carts = new List<Cart>();
            }

            try
            {
                while (item > 0)
                {
                    Console.WriteLine("Please select the items you would like to add to your cart: (Based on product id)");
                    Console.WriteLine("Enter 0 to go back to shopping or remove an item from your cart");
                    GetAllProducts(0);
                    Console.Write("Product: ");
                    choice = Console.ReadLine();
                    item = Int32.Parse(choice);

                    if (item != 0)
                    {
                        CartItems items = new CartItems();
                        Product product = new Product();

                        cart.CartID = 1;
                        cart.UserID = user.UserID;
                        cart.DateCreated = DateTime.Now;
                        items.CartID = cart.CartID;
                        items.CartItemID = itemId++; //PostFix: CartItemID = ItemID Then ItemId = ItemID + 1 //  PreFix: ++ItemID == ItemID = ItemID + 1 Then CartItemID = ItemID
                        items.ProductID = item;
                        product.ProductID = item;
                        product.ProductName = products.Where(x => x.ProductID == item).Select(x => x.ProductName).FirstOrDefault().ToString();
                        product.ProductDescription = products.Where(x => x.ProductID == item).Select(x => x.ProductDescription).FirstOrDefault().ToString();
                        product.Price = Convert.ToDouble(products.Where(x => x.ProductID == item).Select(x => x.Price).FirstOrDefault().ToString());

                        cart.CartItems.Add(items);
                        cart.Products.Add(product);

                        Console.WriteLine("The Item was added to your cart");
                    }

                }
                carts.Add(cart);

                if (cart.CartID > 0)
                {
                    Console.WriteLine(String.Format("\tCart ID: {0}" +
                                                            "\n\tCustomer Name {1}\n", cart.CartID, user.Username));
                    var cartItemID = 0;
                    foreach (var ite in carts.FirstOrDefault().Products)
                    {
                        foreach (var citem in carts.FirstOrDefault().CartItems)
                        {
                            cartItemID += citem.CartItemID;
                            break;
                        }

                        Console.WriteLine(String.Format("\t\tItem ID: {0}" +
                                                            "\n\t\tProduct ID: {1}" +
                                                            "\n\t\tProduct Name: {2}" +
                                                            "\n\t\tProduct Description: {3}" +
                                                            "\n\t\tPrice: ${4}\n", cartItemID //carts.FirstOrDefault().CartItems.FirstOrDefault().CartItemID
                                                            , ite.ProductID, ite.ProductName,
                                                            ite.ProductDescription, ite.Price));


                        cartTotal += ite.Price;
                    }
                    Console.WriteLine(String.Format("\tTotal: ${0}\n\n", cartTotal));

                    Console.Write("Would you like to delete an item? Enter the Item ID and Product ID divided by space, ex: 1 2 or 1 1: ");
                    deleteId = Console.ReadLine();

                    if (!string.Equals(deleteId, "0 0"))
                    {
                        var splitID = deleteId.Split(' ');
                        DeleteItem(splitID);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static public void Checkout()
        {
            // Insert into Customer orders from Cart.
            // Grab cart products ids for the customer order details
            // Grap order once created for the customer order details table.

            int productID = 0;
            int qty = 1;
            int stockQty = 1;
            double price = 0;

            Console.WriteLine("Checkout Page");
            Console.WriteLine(String.Format("Checkout Total: ${0}", cartTotal));
            Console.Write("Do you wish to continue; type Yes or No? ");
            string choice = Console.ReadLine();

            if (string.Equals(choice.ToLowerInvariant(), "yes"))
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;

                    connection.Open();

                    // Customer Orders
                    var ordersQuery = string.Format("Insert into CustomerOrders (UserID, DateCreated, CartTotal) " +
                        "values({0}, '{1}', {2})", user.UserID, DateTime.Now, carts.FirstOrDefault().Products.Count);

                    SqlCommand command = new SqlCommand(ordersQuery, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    // Customer Details.
                    foreach (var c in carts.FirstOrDefault().Products)
                    {
                        var getProductIDQuery = string.Format("Select ProductID, Price From CustomerOrderDetails " +
                                                            "Where ProductID = {0} AND OrderID = {1}", c.ProductID, GetOrderID());
                        SqlCommand id = new SqlCommand(getProductIDQuery, connection);

                        SqlDataReader reader = id.ExecuteReader();

                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                productID = reader.GetInt32(0);
                                qty++;
                                price = reader.GetDouble(1) * qty;
                            }
                        }

                        if (productID <= 0)
                        {
                            var detailsQuery = string.Format("Insert into CustomerOrderDetails (OrderID, ProductID, Qty, Price) " +
                                                "values({0}, {1}, {2}, {3})", GetOrderID(), c.ProductID, 1, c.Price);

                            SqlCommand details = new SqlCommand(detailsQuery, connection);
                            try
                            {
                                details.ExecuteNonQuery();
                                Console.WriteLine("Order Saved!");
                                productID = 0;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            productID = 0;
                            UpdateOrderDetails(qty, price, c.ProductID);
                        }
                        UpdateStock(c.ProductID, stockQty);
                    }


                }
            }

        }

        static public int GetOrderID()
        {
            int maxOrderID = 0;
            string message = "";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = "SELECT MAX(OrderID) As MaxOrderID FROM CustomerOrders";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            maxOrderID = reader.GetInt32(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Rows Found.");
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            return maxOrderID;
        }

        static public void UpdateOrderDetails(int qty, double price, int productID)
        {

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;

                connection.Open();

                var updateQuery = string.Format("Update CustomerOrderDetails Set Qty = {0} Where OrderID = {1} AND ProductID = {2}", qty, GetOrderID(), productID);

                SqlCommand update = new SqlCommand(updateQuery, connection);
                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                updateQuery = string.Format("Update CustomerOrderDetails Set Price = {0} Where OrderID = {1} AND ProductID = {2}", price, GetOrderID(), productID);

                update = new SqlCommand(updateQuery, connection);
                try
                {
                    update.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        static public void DeleteItem(string[] cartItemID)
        {
            try
            {
                int itID = Convert.ToInt32(cartItemID[0]);
                int prID = Convert.ToInt32(cartItemID[1]);

                // Delete Item
                carts.FirstOrDefault().CartItems.RemoveAt(--itID);

                carts.FirstOrDefault().Products.RemoveAt(--prID);

                Console.WriteLine("Your Item was deleted.");

                Console.WriteLine(String.Format("\tCart ID: {0}" +
                                                            "\n\tCustomer Name {1}\n", 1, user.Username));
                var itemID = 0;
                cartTotal = 0;

                foreach (var ite in carts.FirstOrDefault().Products)
                {
                    foreach (var citem in carts.FirstOrDefault().CartItems)
                    {
                        itemID += citem.CartItemID;
                        break;
                    }

                    Console.WriteLine(String.Format("\t\tItem ID: {0}" +
                                                        "\n\t\tProduct ID: {1}" +
                                                        "\n\t\tProduct Name: {2}" +
                                                        "\n\t\tProduct Description: {3}" +
                                                        "\n\t\tPrice: ${4}\n", itemID
                                                        , ite.ProductID, ite.ProductName,
                                                        ite.ProductDescription, ite.Price));


                    cartTotal += ite.Price;
                }
                Console.WriteLine(String.Format("\tTotal: ${0}\n\n", cartTotal));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static public void UpdateStock(int productID, int qty)
        {
            string message = "";
            int stockQty = 0;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;

                connection.Open();

                string query = string.Format("Select Qty From Stock Where StoreID = '{0}' AND ProductID = {1}", userStoreID, productID);

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    var information = command.ExecuteReader();
                    if (information != null)
                    {
                        message = "Success\n";
                    }
                    else
                    {
                        message = "Stock Qty not found";
                    }

                    // Added this for user authentication later.
                    while (information.Read())
                    {
                        stockQty = information.GetInt32(0);
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                stockQty -= qty;

                //StockID
                string updateStock = string.Format("Update Stock Set Qty = {0} Where ProductID = {1} AND StoreID = {2}", stockQty, productID, userStoreID);

                SqlCommand updateCommand = new SqlCommand(updateStock, connection);
                try
                {
                    updateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        static public void GetStoreID()
        {
            string message = "";

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = string.Format("Select StoreID From Locations Where StoreState = '{0}'", userStateName);

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    var information = command.ExecuteReader();
                    if (information != null)
                    {
                        message = "Success\n";
                    }
                    else
                    {
                        message = "State Name not found";
                    }

                    // Added this in order to user user authentication later.
                    while (information.Read())
                    {
                        userStoreID = information.GetInt32(0);
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
        }

        static public void GetStore()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = "SELECT StoreID, StoreName, StoreLocation, StockID, Qty, ProductID, ProductName, ProductDescription, Price FROM StoreView";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // I created a new instance in each iteration to avoid the locations list becoming overriden.
                            // Method Overriding is a technique that allows the invoking of functions from another class (base class) in the derived class.
                            // Creating a method in the derived class with the same signature as a method in the base class is called as method overriding. 
                            // geeksforgeeks.org may use 
                            Store store = new Store();

                            store.StoreID = reader.GetInt32(0);
                            store.StoreName = reader.GetString(1);
                            store.StoreLocation = reader.GetString(2);
                            store.StockID = reader.GetInt32(3);
                            store.Qty = reader.GetInt32(4);
                            store.ProductID = reader.GetInt32(5);
                            store.ProductName = reader.GetString(6);
                            store.ProductDescription = reader.GetString(7);
                            store.Price = reader.GetDouble(8);

                            stores.Add(store);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found!");
                    }

                    Console.WriteLine("Store: ");
                    foreach (var sto in stores)
                    {
                        Console.WriteLine(String.Format("\tStore Name: {0}" +
                                                        "\n\tStore Location: {1}" +
                                                        "\n\tProduct Name: {2}" +
                                                        "\n\tProduct Description: {3}" +
                                                        "\n\tProduct Price: {4}" +
                                                        "\n\tStock Qty: {5}\n", sto.StoreName, sto.StoreLocation, sto.ProductName,
                                                        sto.ProductDescription, sto.Price, sto.Qty));
                    }
                    Console.WriteLine("\n");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // VERIFY THAT USERNAMES EXISTs
        static public void GetUsers()
        {
            string message = "";
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string query = string.Format("Select Username from UserAuthentication");

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    var information = command.ExecuteReader();

                    // Added this FOR user authentication later.
                    while (information.Read())
                    {
                        existUsername.Add(information.GetString(0));
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
        }
    }
}
