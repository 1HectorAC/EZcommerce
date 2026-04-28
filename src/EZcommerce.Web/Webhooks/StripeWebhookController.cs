
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
                
            await HandleCheckoutCompleted(session);
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
        
        var charge = await _checkoutService.GetChargeAsync(session.PaymentIntentId);
                                            
        
        /*
        Console.WriteLine("customer name:" + session.CustomerDetails.Name);
        Console.WriteLine("Total amount:" + session.AmounTotal);
        Console.WriteLine("payment type:" + session.PaymentIntent.LatestCharge.PaymentMethodDetails.Type);
        */
        /*
        var orderIdString = session.Metadata.GetValueOrDefault("orderId");
        
        if(orderIdString is null)
            throw new Exception("orderId metadata in stripe session not set");
        var orderId = Int32.Parse(orderIdString);

        Console.WriteLine("Start HandleChekcoutCompleted function");
        var order = new Order
        {
            CustomerName = session.CustomerDetails.Name,
            CustomerEmail = session.CustomerDetails.Email,
            CustomerPhone = session.CustomerDetails.Phone ?? "",
            ShippingAddressLine1 = session.CollectedInformation.ShippingDetails.Address.Line1,
            ShippingAddressLine2 = session.CollectedInformation.ShippingDetails.Address.Line2,
            City = session.Customer.CollectedInformation.ShippingDetails.Address.City,
            State = session.Customer.CollectedInformation.ShippingDetails.Address.State,
            ZipCode = session.Customer.CollectedInformation.ShippingDetails.Address.PostalCode,
            Country = session.Customer.CollectedInformation.ShippingDetails.Address.Country,
            Status = "Paid"
        };
        _service.OrderUpdate(orderId, order);
        
        var payment = new Payment
        {
            OrderId = orderId,
            Amount = session.AmountTotal,
            Method = charge!.PaymentMethod,
            Status = "Paid",
            TransactionReference = charge!.Id
        };
        _service.PaymentCreate(payment);
    */
        // send email
    }
}