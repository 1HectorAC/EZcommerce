
using EZcommerce.Web.Models.Session;
using Stripe;
using Stripe.Checkout;

namespace EZcommerce.Web.Services;

public class CheckoutService
{
    
    private readonly StripeClient _client;

    public CheckoutService(IConfiguration config)
    {
        _client = new StripeClient(config["STRIPE:SECRETKEY"]);

    }
    

    public async Task<Session> CreateCheckoutSession(List<CartItem> cartItems)
    {
        var sessionItems = cartItems.Select(i => new SessionLineItemOptions
        {
            Quantity = i.Quantity,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (int)(i.PriceSnapshot * 100), // $20
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = i.Name
                            }
                        }
        }).ToList();

        var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = "http://localhost:5191/Checkout/Success",
                CancelUrl = "http://localhost:5191/Checkout/Cancel",
                LineItems = sessionItems
            };

        Session session = await _client.V1.Checkout.Sessions.CreateAsync(options);

        return session;
    }

    
}