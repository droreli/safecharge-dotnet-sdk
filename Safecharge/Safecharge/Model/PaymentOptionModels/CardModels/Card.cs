using Safecharge.Model.PaymentOptionModels.ThreeDModels;
using Safecharge.Utils;

namespace Safecharge.Model.PaymentOptionModels.CardModels
{
    /// <summary>
    /// Represents credit/debit card details for a payment or an order.
    /// This class extends <see cref="CardData"/> with additional card processing information.
    /// </summary>
    public class Card : CardData
    {
        private string acquirerId;

        /// <summary>
        /// External token provider information, if the card details are tokenized by a third-party.
        /// </summary>
        public ExternalToken ExternalToken { get; set; }

        /// <summary>
        /// Details about stored credentials for recurring or merchant-initiated transactions.
        /// </summary>
        public StoredCredentials StoredCredentials { get; set; }

        /// <summary>
        /// The identifier of the acquirer that processed the transaction. Max length: 2.
        /// </summary>
        public string AcquirerId
        {
            get { return this.acquirerId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 2, nameof(this.AcquirerId));
                this.acquirerId = value;
            }
        }

        /// <summary>
        /// 3D Secure (Three Domain Secure) information for the transaction.
        /// </summary>
        public ThreeD ThreeD { get; set; }
    }
}
