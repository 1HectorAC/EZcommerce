
using EZcommerce.Models.Settings;
using EZcommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;


namespace EZcommerce.Web.Webhooks;

[ApiController]
[Route("webhooks/stripe")]
public class StripeWebhookcontroller: ControllerBase
{

    private readonly StripeSettings _stripeSettings;

    public StripeWebhookcontroller(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        Console.WriteLine("Stripe Webhook called");
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _stripeSettings.WebhookSecret
            );
        }
        catch(Exception ex){ 
            Console.WriteLine($"Webhook signiture verification failed. {ex.Message}");
            return BadRequest();
            }

        // Handle event
        if(stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
        {
            var session = stripeEvent.Data.Object as Session;
            if(session is null)
            {
                Console.WriteLine("stripe Session is null error in Webhook");
                return BadRequest();
            }
                
            await HandleCheckoutCompleted(session);
        }

        Console.WriteLine("completed Stripe webhook call.");
        return Ok();
    }

    public static async Task HandleCheckoutCompleted(Session session)
    {
        Console.WriteLine("Start HandleChekcoutCompleted function");
        /*
        var payment = new Payment
        {
            OrderId = 0,
            Amount = 0,
            Method = "",
            Status = "Succeeded",
            TransactionReference = ""
        };
        var orderItem = new OrderItem
        {
            OrderId = 0,
            ProductId = 0,
            Quantity = 0,
            PriceAtPurchase = 0
        };

        var order = new Order
        {
            CustomerName = "",
            CustomerEmail = "",
            CustomerPhone = "",
            ShippingAddressLine1 = "",
            ShippingAddressLine2 = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
            TotalAmmount = 0,
            Status = "Paid",
            CreatedAt = DateTime.UtcNow,
            OrderItems = 
        };

        // load order
        // validate inventory
        // Mark order as "Paid"
        // Create Payement
        //reduce inventory
        // send email
        */

    }
}