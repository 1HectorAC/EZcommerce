
using EZcommerce.Models.Settings;
using EZcommerce.Web.Models;
using EZcommerce.Web.Services;
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

    private readonly IEZcommerceService _service;

    public StripeWebhookcontroller(IOptions<StripeSettings> stripeSettings, IEZcommerceService service)
    {
        _stripeSettings = stripeSettings.Value;
        _service = service;
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
        else if(stripeEvent.Type == EventTypes.CheckoutSessionExpired)
        {
            var session = stripeEvent.Data.Object as Session;
            if(session is null)
            {
                Console.WriteLine("stripe Session is null error in Webhook");
                return BadRequest();
            }
            var orderIdString = session.Metadata.GetValueOrDefault("orderId");
            if(orderIdString is null)
                return BadRequest("orderId metadata in stripe session not set");

            var orderId = Int32.Parse(orderIdString);

            await _service.OrderInventoryRollback(orderId);

            _service.OrderRemove(orderId);
            
        }

        Console.WriteLine("completed Stripe webhook call.");
        return Ok();
    }

    public static async Task HandleCheckoutCompleted(Session session)
    {
        Console.WriteLine("Start HandleChekcoutCompleted function");
        

        // Edit Order: add fields, mark as "Paid"
        // Create Payement
        // send email
        

    }
}