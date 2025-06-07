using Safecharge.Utils;

namespace Safecharge.Model.PaymentOptionModels
{
    /// <summary>
    /// Base class for payment option details, primarily holding the User Payment Option ID (UPO ID).
    /// </summary>
    public class BasePaymentOption
    {
        private string userPaymentOptionId;

        /// <summary>
        /// The unique identifier for a stored User Payment Option (UPO).
        /// This ID can be used to process transactions with a previously saved payment method.
        /// </summary>
        /// <remarks>This field is mandatory if not providing full payment details. Max length is defined by <see cref="Constants.MaxLengthStringId"/>.</remarks>
        public string UserPaymentOptionId
        {
            get { return this.userPaymentOptionId; }
            set
            {
                Guard.RequiresNotNull(value, nameof(UserPaymentOptionId));
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringId, nameof(this.UserPaymentOptionId));
                this.userPaymentOptionId = value;
            }
        }
    }
}
