using CarStore.ServicesAbstraction;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.Services
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly PaymentIntentService _paymentIntentService;

        public StripePaymentService(IConfiguration configuration)
        {
            var secretKey = configuration["Stripe:SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException(
                    "Stripe Secret Key is missing from configuration.");
            }

            var stripeClient = new StripeClient(secretKey);

            _paymentIntentService =
                new PaymentIntentService(stripeClient);
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(
            decimal amount,
            int reservationId)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Payment amount must be greater than zero.");
            }

            if (reservationId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(reservationId),
                    "Reservation ID must be greater than zero.");
            }

            var options = new PaymentIntentCreateOptions
            {
                Amount = decimal.ToInt64(amount * 100m),
                Currency = "egp",

                AutomaticPaymentMethods =
                    new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true
                    },

                Metadata = new Dictionary<string, string>
                {
                    ["ReservationId"] = reservationId.ToString()
                }
            };

            return await _paymentIntentService.CreateAsync(options);
        }
    }
}
