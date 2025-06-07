using Safecharge.Model.Common;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Request.Common.Payment
{
    /// <summary>
    /// Abstract base class for payment requests that include order details and common payment parameters.
    /// It extends <see cref="SafechargeOrderDetailsRequest"/>.
    /// </summary>
    public abstract class SafechargePaymentRequest : SafechargeOrderDetailsRequest
    {
        private string customSiteName;
        private string productId;
        private string customData;
        private string relatedTransactionId;

        /// <summary>
        /// Empty constructor used for mapping from config file.
        /// </summary>
        public SafechargePaymentRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargePaymentRequest"/> class with essential parameters.
        /// </summary>
        /// <param name="merchantInfo">Merchant information. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="checksumOrderMapping">The order of fields used for checksum calculation. See <see cref="Utils.Enum.ChecksumOrderMapping"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="currency">The three-letter ISO currency code.</param>
        /// <param name="amount">The transaction amount as a string.</param>
        public SafechargePaymentRequest(
            MerchantInfo merchantInfo,
            ChecksumOrderMapping checksumOrderMapping,
            string sessionToken,
            string currency,
            string amount)
            : base(merchantInfo, checksumOrderMapping, sessionToken, currency, amount)
        {
        }

        /// <summary>
        /// Transaction type of the request (e.g., "Auth", "Sale", "PreAuth").
        /// Constants for common transaction types can be found in <see cref="ApiConstants"/>.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// The merchant's site name. Useful for merchants operating multiple websites.
        /// Risk rules and traffic management rules can be built based on this field.
        /// </summary>
        /// <remarks>Max length is 50.</remarks>
        public string CustomSiteName
        {
            get { return this.customSiteName; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 50, nameof(this.CustomSiteName));
                this.customSiteName = value;
            }
        }

        /// <summary>
        /// A free text field to identify the product or service sold.
        /// If not sent or empty, it may be concatenated from item names.
        /// Risk rules and traffic management rules can be built based on this field.
        /// </summary>
        /// <remarks>Max length is 50.</remarks>
        public string ProductId
        {
            get { return this.productId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 50, nameof(this.ProductId));
                this.productId = value;
            }
        }

        /// <summary>
        /// Custom data that can be passed with the request.
        /// This data is passed to the payment gateway and is visible in transaction reporting.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringDefault"/>.</remarks>
        public string CustomData
        {
            get { return this.customData; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringDefault, nameof(this.CustomData));
                this.customData = value;
            }
        }

        /// <summary>
        /// The ID of a related, previous transaction (e.g., for rebilling or referencing a prior authorization).
        /// </summary>
        /// <remarks>Max length is 19.</remarks>
        public string RelatedTransactionId
        {
            get { return this.relatedTransactionId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 19, nameof(this.RelatedTransactionId));
                this.relatedTransactionId = value;
            }
        }
    }
}
