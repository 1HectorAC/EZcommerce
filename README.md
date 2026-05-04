# EZcommerce
A simple ecommerce platform with secure payments and admin management tools. 

## Flow
- User can add a products to their cart
- On the shopping cart page, they can review their order and then checkout
- User gets redirected to stripe hosted checkout page for payment processing
- User is then redirected to confirmation page on completion

## Features
- There are admin pages to manage Products, Orders, and Payments
- Payment processing is done with Stripe API
- There are webhooks setup to manage Order/Payment status changes

## Tools/Language/Other:
- ASP.NET Core MVC
- ASP.NET Core Identity (for authentication)
- C#
- Microsoft SQL Server
- Stripe API

## How to run:
1) Have .NET Runtime and SQL Server installed
2) Main project is in src/EZcommerce.Web folder. Any following commands in done in this folder
2) Add .env file. There is and Example file (ExampleEnv.txt), Add db connection string and stripe api keys
3) Update database based on migration files. Can be done with dotnet ef cli tool:
`dotnet ef database update`
4) Run the application with:
`dotnet run`