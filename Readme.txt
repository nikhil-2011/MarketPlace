This project focuses on Market Place problem, user gets the feature to use the application as a Buyer or a Seller. 
As a Buyer:
	-User can view the items uploaded by other sellers
	-Add items to cart,buy the items
	-View his cart and orders

Web app features to add/delete items from cart and user can place order afterwards too. If one user adds any item to his cart it will be visible to another user unless and untill the first user places an order but for him the quantity of item will get reduced. If in mean time second user places the order and item comes out of stock, the first user will get an error message while placing the order. If any item is removed from the cart, quantity of items is updated accordingly.

As s Seller:
	-User can add new catalog
	-User can update existing catalog
	-Disable/Enable his catalog categories

Web app features to add catalog with values: Category,Sub-Category,Item,Price,Quantity which can be updated afterwards. If for a Category/Sub-Category any item exists in the user's catalog only the price and quantity values are updated. Category status can be changed to Enabled/Disabled and for the Disabled categories items are not shown to the buyer.

Steps to Run WebApp:
	-Install Microsoft Visual Studio 2017 
	-Install MS SQL-Server 2012
	-Right click 'Databases'->Add new database and Run the DB creation script MarketPlaceDB which will create tables,sequences and procedures that are used in 	 project
	-Run InsertScript to insert initial values in the DB
	-Open MarketPlace.sln in Visual Studio
	-Delete the existing edmx file in MarketPlaceDAL class library and create a new one: (to generate the new connection string as the DB server gets changed)
		-Right Click on MarketPlaceDAL and add a new item i.e ADO.NET Entity Model
		-Choose EF from database and select your server and DB
		-Copy connection string from App.config to the Web.config file in MVC project
	-Now you can run the MVC project

