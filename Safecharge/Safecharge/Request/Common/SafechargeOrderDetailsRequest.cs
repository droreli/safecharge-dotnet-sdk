using System.Collections.Generic;
using Safecharge.Model.Common;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Request.Common
{
    /// <summary>
    /// Abstract base class for requests that involve order details, such as payment requests or opening an order.
    /// It extends <see cref="SafechargeRequest"/> by adding common order and transaction parameters like currency, amount, items, and user details.
    /// </summary>
    public abstract class SafechargeOrderDetailsRequest : SafechargeRequest
    {
        private string userTokenId;
        private string clientUniqueId;

        /// <summary>
        /// Empty constructor used for mapping from config file.
        /// </summary>
        public SafechargeOrderDetailsRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeOrderDetailsRequest"/> class with essential parameters.
        /// </summary>
        /// <param name="merchantInfo">Merchant information. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="checksumOrderMapping">The order of fields used for checksum calculation. See <see cref="Utils.Enum.ChecksumOrderMapping"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="currency">The three-letter ISO currency code (e.g., "USD").</param>
        /// <param name="amount">The transaction amount as a string (e.g., "10.00").</param>
        public SafechargeOrderDetailsRequest(
            MerchantInfo merchantInfo,
            ChecksumOrderMapping checksumOrderMapping,
            string sessionToken,
            string currency,
            string amount)
            : base(merchantInfo, checksumOrderMapping, sessionToken)
        {
            this.Currency = currency;
            this.Amount = amount;
        }

        /// <summary>
        /// The three character ISO currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The transaction amount.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// List of items that will be purchased.
        /// </summary>
        public List<Item> Items { get; set; } = new List<Item>();

        /// <summary>
        /// Details about the user which include the user's name, email, address, etc.
        /// </summary>
        public CashierUserDetails UserDetails { get; set; }

        /// <summary>
        /// Shipping address related to a user's order.
        /// </summary>
        public UserAddress ShippingAddress { get; set; }

        /// <summary>
        /// Billing address related to a user payment option. Since order can contain only one payment option billing address is part of the order parameters.
        /// </summary>
        public UserAddress BillingAddress { get; set; }

        /// <summary>
        /// Merchant descriptor - this is the message(Merchant's name and phone) that the user will see in his payment bank report
        /// </summary>
        public DynamicDescriptor DynamicDescriptor { get; set; }

        /// <summary>
        /// Optional custom fields.
        /// </summary>
        public MerchantDetails MerchantDetails { get; set; }

        /// <summary>
        /// Although DMN response can be configured per merchant site, it will allow to dynamically return the DMN to the provided address per request.
        /// </summary>
        public UrlDetails UrlDetails { get; set; }

        /// <summary>
        /// ID of the user in merchant system.
        /// </summary>
        public string UserTokenId
        {
            get { return this.userTokenId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringId, nameof(this.UserTokenId));
                this.userTokenId = value;
            }
        }

        /// <summary>
        /// ID of the transaction in the merchant's system.
        /// This must be sent to perform future actions like reconciliation or identifying the transaction in case of issues.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringId"/>.</remarks>
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
        /// Additional details about the transaction amount, such as tax, discount, or shipping.
        /// </summary>
        public AmountDetails AmountDetails { get; set; }
    }
}
