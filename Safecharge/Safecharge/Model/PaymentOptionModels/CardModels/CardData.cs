using Safecharge.Utils;

namespace Safecharge.Model.PaymentOptionModels.CardModels
{
    /// <summary>
    /// Holder for credit/debit/prepaid card data.
    /// </summary>
    public class CardData
    {
        private string cardNumber;
        private string cardHolderName;
        private string expirationMonth;
        private string expirationYear;
        private string ccTempToken;
        private string cVV;

        /// <summary>
        /// The credit/debit card number.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthCardNumber"/>.</remarks>
        public string CardNumber
        {
            get { return this.cardNumber; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthCardNumber, nameof(this.CardNumber));
                this.cardNumber = value;
            }
        }

        /// <summary>
        /// The name of the cardholder as it appears on the card.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthCardHolderName"/>.</remarks>
        public string CardHolderName
        {
            get { return this.cardHolderName; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthCardHolderName, nameof(this.CardHolderName));
                this.cardHolderName = value;
            }
        }

        /// <summary>
        /// The card's expiration month (e.g., "03", "12").
        /// </summary>
        /// <remarks>Max length is 2.</remarks>
        public string ExpirationMonth
        {
            get { return this.expirationMonth; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 2, nameof(this.ExpirationMonth));
                this.expirationMonth = value;
            }
        }

        /// <summary>
        /// The card's expiration year (e.g., "2023", "23").
        /// </summary>
        /// <remarks>Max length is 4.</remarks>
        public string ExpirationYear
        {
            get { return this.expirationYear; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 4, nameof(this.ExpirationYear));
                this.expirationYear = value;
            }
        }

        /// <summary>
        /// A temporary token for the card details, often used in tokenization schemes.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringId"/>.</remarks>
        public string CcTempToken
        {
            get { return this.ccTempToken; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringId, nameof(this.CcTempToken));
                this.ccTempToken = value;
            }
        }

        /// <summary>
        /// The Card Verification Value (CVV, CVC2, CID).
        /// </summary>
        /// <remarks>Length must be between 3 and 4 characters.</remarks>
        public string CVV
        {
            get { return this.cVV; }
            set
            {
                Guard.RequiresLengthBetween(value?.Length, 3, 4, nameof(this.CVV));
                this.cVV = value;
            }
        }
    }
}
