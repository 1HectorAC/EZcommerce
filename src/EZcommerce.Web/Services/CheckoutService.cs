
using Stripe;
using Stripe.Checkout;

namespace EZcommerce.Web.Services;

public class CheckoutService
{
    
    //private readonly StripeClient _client;

    public CheckoutService(IConfiguration config)
    {
        //_client = new StripeClient(config["STRIPE:SECRETKEY"]);

    }
    
/*
    public async Task<Session> CreateCheckoutSession()
    {
        var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = "/Checkout/Success",
                CancelUrl = "/Checkout/Cancel",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 2000, // $20
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Test Product"
                            }
                        }
                    }
                }
            };

        Session session = await _client.V1.Checkout.Sessions.CreateAsync(options);

        return session;
    }
*/
    
}