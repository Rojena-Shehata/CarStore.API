using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace CarStore.ServicesAbstraction
{
    public interface IStripePaymentService
    {
       Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount,int reservationId);

    }
}
