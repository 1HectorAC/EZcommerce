
using EZcommerce.Models.Settings;
using EZcommerce.Web.Models.Session;
using Sprache;
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Options;

namespace EZcommerce.Web.Services;

public class CheckoutService
{
    private readonly StripeSettings _stripeSettings;
    private readonly StripeClient _client;

    public CheckoutService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
        _client = new StripeClient(_stripeSettings.SecretKey);

    }

    public async Task<Session> CreateCheckoutSession(List<CartItem> cartItems, string orderId)
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

        // Used to get Order in webhook
        var metaData = new Dictionary<string, string>
        {
            {"orderId", orderId}
        };

        var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = "http://localhost:5191/Checkout/Success",
                CancelUrl = "http://localhost:5191/Checkout/Cancel",
                Metadata = metaData,
                LineItems = sessionItems
            };

        Session session = await _client.V1.Checkout.Sessions.CreateAsync(options);

        return session;
    }

    
}