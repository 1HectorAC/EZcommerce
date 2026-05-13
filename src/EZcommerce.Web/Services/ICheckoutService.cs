
using EZcommerce.Web.Models.Session;
using Stripe;
using Stripe.Checkout;

namespace EZcommerce.Web.Services;

public interface ICheckoutService
{
    Task<Session> CreateCheckoutSession(List<CartItem> cartItems, string orderId);

    Task<Charge?> GetChargeAsync(string paymentIntentId);
}