using Safecharge.Model.Common;
using Safecharge.Model.PaymentOptionModels;
using Safecharge.Model.PaymentOptionModels.CardModels;
using Safecharge.Request.Common;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Request
{
    /// <summary>
    /// Request to execute payout.
    /// </summary>
    /// <remarks>
    ///  This method is intended for merchants implementing payout for Credit cards and APMs(paypal, skrill, neteller and so on).
    ///  Using a native mobile application is not required to use the payout client SDK.
    ///  However, they are able to use this <see cref="PayoutRequest"/> directly.
    ///  Merchants are able to use the payout client SDK, according to the instructions<a href="https://www.safecharge.com/docs/api/?java#payout"> here</a>, 
    ///  which performs credit card tokenization for them.
    ///  See <a href="https://www.safecharge.com/docs/api/?java#payout">documentation</a>.
    /// </remarks>
    public class PayoutRequest : SafechargeRequest
    {
        private string userTokenId;
        private string clientUniqueId;
        private string amount;
        private string currency;
        private string comment;
        private string phone;
        private string userPmId;

        /// <summary>
        /// Empty constructor used for mapping from config file.
        /// </summary>
        public PayoutRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayoutRequest"/> with the required parameters.
        /// </summary>
        /// <param name="merchantInfo">Merchant's data (E.g. secret key, the merchant id, the merchant site id, etc.)</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="userTokenId">(Required) ID of the user in the merchant's system.</param>
        /// <param name="clientUniqueId">(Required) ID of the transaction in the merchant’s system. This must be sent to perform future actions, such as reconciliation.</param>
        /// <param name="amount">(Required) The transaction amount as a string (e.g., "10.00").</param>
        /// <param name="currency">(Required) The three-letter ISO currency code of the transaction (e.g., "USD").</param>
        /// <param name="userPaymentOption">(Required) User payment option details. See <see cref="Model.PaymentOptionModels.UserPaymentOption"/>.</param>
        public PayoutRequest(
            MerchantInfo merchantInfo,
            string sessionToken,
            string userTokenId,
            string clientUniqueId,
            string amount,
            string currency,
            UserPaymentOption userPaymentOption)
            : base(merchantInfo, ChecksumOrderMapping.ApiGenericChecksumMapping, sessionToken)
        {
            this.UserTokenId = userTokenId;
            this.ClientUniqueId = clientUniqueId;
            this.Amount = amount;
            this.Currency = currency;
            this.UserPaymentOption = userPaymentOption;
            this.RequestUri = this.CreateRequestUri(ApiConstants.PayoutUrl);
        }

        /// <summary>
        /// ID of the user in merchant system.
        /// </summary>
        public string UserTokenId
        {
            get { return this.userTokenId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringDefault, nameof(this.UserTokenId));
                this.userTokenId = value;
            }
        }

        /// <summary>
        /// ID of the transaction in merchant system.
        /// </summary>
        public string ClientUniqueId
        {
            get { return this.clientUniqueId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringId, nameof(this.ClientUniqueId));
                this.clientUniqueId = value;
            }
        }

        /// <summary>
        /// The transaction amount.
        /// </summary>
        /// <remarks>Max length is 12.</remarks>
        public string Amount
        {
            get { return this.amount; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 12, nameof(this.Amount));
                this.amount = value;
            }
        }

        /// <summary>
        /// The three-letter ISO currency code of the transaction.
        /// </summary>
        /// <remarks>Max length is 3.</remarks>
        public string Currency
        {
            get { return this.currency; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 3, nameof(this.Currency));
                this.currency = value;
            }
        }

        /// <summary>
        /// Gets or sets the user payment option details for this payout.
        /// </summary>
        public UserPaymentOption UserPaymentOption { get; set; }

        /// <summary>
        /// Enables the addition of a free text comment to the request.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringDefault"/>.</remarks>
        public string Comment
        {
            get { return this.comment; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringDefault, nameof(this.Comment));
                this.comment = value;
            }
        }

        /// <summary>
        /// Gets or sets the dynamic descriptor for the transaction, which appears on the user's bank statement.
        /// </summary>
        public DynamicDescriptor DynamicDescriptor { get; set; }

        /// <summary>
        /// Gets or sets optional custom merchant-specific details.
        /// </summary>
        public MerchantDetails MerchantDetails { get; set; }

        /// <summary>
        /// Gets or sets URL details for notifications or redirection.
        /// </summary>
        public UrlDetails UrlDetails { get; set; }

        /// <summary>
        /// Gets or sets credit/debit card data if applicable for the payout.
        /// </summary>
        public CardData CardData { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthPhone"/>.</remarks>
        public string Phone
        {
            get { return this.phone; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthPhone, nameof(this.Phone));
                this.phone = value;
            }
        }

        /// <summary>
        /// Gets or sets the User Payment Method ID.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringId"/>.</remarks>
        public string UserPmId
        {
            get { return this.userPmId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringId, nameof(this.UserPmId));
                this.userPmId = value;
            }
        }
    }
}
