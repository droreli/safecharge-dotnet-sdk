using System.Collections.Generic;
using Safecharge.Model.PaymentOptionModels.CardModels;

namespace Safecharge.Model.PaymentOptionModels
{
    /// <summary>
    /// Represents the payment option chosen by the user. This can be a card, an alternative payment method, etc.
    /// </summary>
    public class PaymentOption : BasePaymentOption
    {
        /// <summary>
        /// Card details if the payment option is a card.
        /// </summary>
        public Card Card { get; set; }

        /// <summary>
        /// Alternative payment method details. The keys and values are specific to the APM.
        /// </summary>
        public Dictionary<string, string> AlternativePaymentMethod { get; set; }

        /// <summary>
        /// Sub-method details, if applicable (e.g., for some specific APMs or card processing flows).
        /// </summary>
        public SubMethod Submethod { get; set; }
    }
}
