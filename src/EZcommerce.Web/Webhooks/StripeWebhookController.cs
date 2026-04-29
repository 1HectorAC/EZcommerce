
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

    private readonly EZcommerce.Web.Services.CheckoutService _checkoutService;

    private readonly IEZcommerceService _service;

    public StripeWebhookcontroller(IOptions<StripeSettings> stripeSettings, IEZcommerceService service, EZcommerce.Web.Services.CheckoutService checkoutService)
    {
        _stripeSettings = stripeSettings.Value;
        _checkoutService = checkoutService;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        Console.WriteLine("1) Stripe Webhook called");
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
            Console.WriteLine("2) CheckoutSessionCompleted event received ");
            var session = stripeEvent.Data.Object as Session;
            if(session is null)
            {
                Console.WriteLine("stripe Session is null error in Webhook");
                return BadRequest();
            }
            try
            {
                await HandleCheckoutCompleted(session);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error hit in HandleCheckoutComplete call: " + ex.Message);
                return BadRequest();
            }
            
        }
        else if(stripeEvent.Type == EventTypes.CheckoutSessionExpired)
        {
            Console.WriteLine("2) CheckoutSessionExpired received");
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

        Console.WriteLine("3) Finished Stripe webhook call.");
        return Ok();
    }

    public async Task HandleCheckoutCompleted(Session session)
    {
        Console.WriteLine("Start HandleCheckoutCompleted function");

        // Get charge for pay method and transactionReference
        var charge = await _checkoutService.GetChargeAsync(session.PaymentIntentId);

        // Get orderId for updating order                  
        var orderIdString = session.Metadata.GetValueOrDefault("orderId");
        if(orderIdString is null)
            throw new Exception("orderId metadata in stripe session not set");
        var orderId = Int32.Parse(orderIdString);

        
        var order = new Order
        {
            CustomerName = session.CustomerDetails.Name,
            CustomerEmail = session.CustomerDetails.Email,
            CustomerPhone = session.CustomerDetails.Phone,
            ShippingAddressLine1 = session.CollectedInformation.ShippingDetails.Address.Line1,
            ShippingAddressLine2 = session.CollectedInformation.ShippingDetails.Address.Line2,
            City = session.CollectedInformation.ShippingDetails.Address.City,
            State = session.CollectedInformation.ShippingDetails.Address.State,
            ZipCode = session.CollectedInformation.ShippingDetails.Address.PostalCode,
            Country = session.CollectedInformation.ShippingDetails.Address.Country,
            Status = "Paid"
        };
        _service.OrderUpdate(orderId, order);
        
        decimal amound = Math.Round(session.AmountTotal / 100m ?? 0.00m,2);
        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amound,
            Method = charge!.PaymentMethod,
            Status = "Paid",
            TransactionReference = charge!.Id
        };
        _service.PaymentCreate(payment);
        
        // send email
    }
}