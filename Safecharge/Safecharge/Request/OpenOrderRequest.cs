using Safecharge.Model.Common;
using Safecharge.Model.PaymentOptionModels.OpenOrder;
using Safecharge.Request.Common.OpenOrder;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Request
{
    /// <summary>
    /// Represents a request to open an order in the Safecharge system.
    /// </summary>
    /// <remarks>
    /// This request creates or updates an order with items, amounts, and user details.
    /// It does not process a payment but rather establishes an order that can be paid later.
    /// It extends <see cref="Common.OpenOrder.OrderRequestWithDetails"/>.
    /// </remarks>
    public class OpenOrderRequest : OrderRequestWithDetails
    {
        private string productId;
        private string isRebilling;
        private string preventOverride;

        /// <summary>
        /// Empty constructor used for mapping from config file.
        /// </summary>
        public OpenOrderRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenOrderRequest"/> class with essential merchant and transaction details.
        /// </summary>
        /// <param name="merchantInfo">Merchant information. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="currency">The three-letter ISO currency code (e.g., "USD").</param>
        /// <param name="amount">The total order amount as a string (e.g., "10.00").</param>
        public OpenOrderRequest(
            MerchantInfo merchantInfo,
            string sessionToken,
            string currency,
            string amount)
            : base(merchantInfo, ChecksumOrderMapping.ApiGenericChecksumMapping, sessionToken, currency, amount)
        {
            Guard.RequiresValidCurrencyCode(currency, nameof(currency));
            this.RequestUri = this.CreateRequestUri(ApiConstants.OpenOrderUrl);
        }

        /// <summary>
        /// The merchant's custom site name.
        /// </summary>
        public string CustomSiteName { get; set; }

        /// <summary>
        /// An identifier for the product being sold.
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
        /// Payment option details specific to opening an order.
        /// </summary>
        public OpenOrderPaymentOption PaymentOption { get; set; }

        /// <summary>
        /// The type of transaction for this order (e.g., "Sale", "Auth").
        /// See <see cref="ApiConstants"/> for predefined transaction types.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Indicates if this order is part of a rebilling (recurring) sequence.
        /// Expected values: "0" for false, "1" for true.
        /// </summary>
        public string IsRebilling
        {
            get { return this.isRebilling; }
            set
            {
                if (value != null)
                {
                    Guard.RequiresBool(value, nameof(this.IsRebilling));
                }

                this.isRebilling = value;
            }
        }

        /// <summary>
        /// When set to "1", this flag prevents overriding the billing address with the one from the UPO (User Payment Option).
        /// Expected values: "0" for false, "1" for true.
        /// </summary>
        public string PreventOverride
        {
            get { return this.preventOverride; }
            set
            {
                if (value != null)
                {
                    Guard.RequiresBool(value, nameof(this.PreventOverride));
                }

                this.preventOverride = value;
            }
        }
    }
}
