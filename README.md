# RushHour
ASP.NET MVC based application for managing various appointments

# Setup
1. Change your Windows Regional Format to Bulgaria (Bulgarian) and change it's Decimal symbol to "." instead of ","
2. Please change the connection string in the Web.config file (RushHour project) to the corresponding one of your machine.
3. Run `update-database` in the Package Manager Console with Default project DataAccess to run the migrations against your database.

You have some seeded users that you can use in the Seed method of the Configuration class (DataAccess project).
