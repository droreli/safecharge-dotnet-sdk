using Safecharge.Model.Common;
using Safecharge.Model.PaymentModels;
using Safecharge.Model.PaymentOptionModels;
using Safecharge.Request.Common.Payment;
using Safecharge.Utils;
using Safecharge.Utils.Enum;
using System.Collections.Generic; // For List<Item>
using Safecharge.Model.Common.Addendum; // For Addendums

namespace Safecharge.Request
{
    /// <summary>
    /// Represents a request to perform a payment (e.g., Sale or Auth transaction).
    /// This request is used for card transactions (credit/debit), 3D Secure, and Alternative Payment Methods.
    /// It extends <see cref="Authorize3dAndPaymentRequest"/> with payment-specific properties.
    /// </summary>
    public class PaymentRequest : Authorize3dAndPaymentRequest
    {
        private string orderId;
        private string isPartialApproval;

        /// <summary>
        /// Empty constructor used for mapping from config file.
        /// </summary>
        public PaymentRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRequest"/> with essential merchant and transaction details.
        /// </summary>
        /// <param name="merchantInfo">Merchant information including keys and site ID. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="currency">The three-letter ISO currency code (e.g., "USD").</param>
        /// <param name="amount">The transaction amount as a string (e.g., "10.00").</param>
        /// <param name="paymentOption">The payment option details. See <see cref="Model.PaymentOptionModels.PaymentOption"/>.</param>
        public PaymentRequest(
            MerchantInfo merchantInfo,
            string sessionToken,
            string currency,
            string amount,
            PaymentOption paymentOption)
            : base(merchantInfo, ChecksumOrderMapping.ApiGenericChecksumMapping, sessionToken, currency, amount, paymentOption)
        {
            Guard.RequiresValidCurrencyCode(currency, nameof(currency));
            this.RequestUri = this.CreateRequestUri(ApiConstants.PaymentUrl);
        }

        /// <summary>
        /// The unique identifier of the order in the merchant's system.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringId"/>.</remarks>
        public string OrderId
        {
            get { return this.orderId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringId, nameof(this.OrderId));
                this.orderId = value;
            }
        }

        /// <summary>
        /// Indicates if the transaction is a Mail Order/Telephone Order (MOTO).
        /// Expected values: "0" for false, "1" for true.
        /// </summary>
        public string IsMoto { get; set; }

        /// <summary>
        /// Contains details for specific sub-payment methods, if applicable.
        /// </summary>
        public SubMethodDetails SubMethodDetails { get; set; }

        /// <summary>
        /// Indicates if partial approval is allowed for this transaction.
        /// Partial approval occurs when a transaction is processed for an amount lower than requested due to insufficient funds.
        /// </summary>
        /// <remarks>
        /// This feature is only supported by Nuvei acquiring and requires prior configuration by Nuvei's Integration Support Team.
        /// Possible values: "1" (allow partial approval) or "0" (do not allow partial approval). Max length is 1.
        /// </remarks>
        public string IsPartialApproval
        {
            get { return this.isPartialApproval; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, 1, nameof(this.IsPartialApproval));
                this.isPartialApproval = value;
            }
        }

        /// <summary>
        /// Contains details for currency conversion (DCC - Dynamic Currency Conversion) if applicable.
        /// </summary>
        public CurrencyConversion CurrencyConversion { get; set; }

        /// <summary>
        /// Creates a new builder instance for <see cref="PaymentRequest"/>.
        /// </summary>
        /// <param name="merchantInfo">Merchant information including keys and site ID. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="currency">The three-letter ISO currency code (e.g., "USD").</param>
        /// <param name="amount">The transaction amount as a string (e.g., "10.00").</param>
        /// <param name="paymentOption">The payment option details. See <see cref="Model.PaymentOptionModels.PaymentOption"/>.</param>
        /// <returns>A new instance of the <see cref="Builder"/>.</returns>
        public static Builder CreateBuilder(MerchantInfo merchantInfo, string sessionToken, string currency, string amount, PaymentOption paymentOption)
        {
            return new Builder(merchantInfo, sessionToken, currency, amount, paymentOption);
        }

        /// <summary>
        /// Fluent builder for <see cref="PaymentRequest"/>.
        /// </summary>
        public class Builder // Removed static
        {
            private readonly MerchantInfo _merchantInfo;
            private readonly string _sessionToken;
            private readonly string _currency;
            private readonly string _amount;
            private readonly PaymentOption _paymentOption;

            private string _orderId;
            private string _isMoto;
            private SubMethodDetails _subMethodDetails;
            private string _isPartialApproval;
            private CurrencyConversion _currencyConversion;
            private int? _isRebilling;
            private bool _autoPayment3D;
            private string _transactionType;
            private string _customSiteName;
            private string _productId;
            private string _customData;
            private string _relatedTransactionId;
            private List<Item> _items = new List<Item>();
            private CashierUserDetails _userDetails;
            private UserAddress _shippingAddress;
            private UserAddress _billingAddress;
            private DynamicDescriptor _dynamicDescriptor;
            private MerchantDetails _merchantDetails;
            private UrlDetails _urlDetails;
            private string _userTokenId;
            private string _clientUniqueId;
            private AmountDetails _amountDetails;
            private string _rebillingType;
            private string _authenticationTypeOnly;
            private SubMerchant _subMerchant;
            private Addendums _addendums;
            private string _userId;
            private DeviceDetails _deviceDetails;
            private string _clientRequestId; // From SafechargeBaseRequest
            private string _internalRequestId; // From SafechargeBaseRequest

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/>.
            /// </summary>
            public Builder(MerchantInfo merchantInfo, string sessionToken, string currency, string amount, PaymentOption paymentOption)
            {
                _merchantInfo = merchantInfo;
                _sessionToken = sessionToken;
                _currency = currency;
                _amount = amount;
                _paymentOption = paymentOption;
            }

            public Builder WithOrderId(string orderId) { _orderId = orderId; return this; }
            public Builder WithIsMoto(string isMoto) { _isMoto = isMoto; return this; }
            public Builder WithSubMethodDetails(SubMethodDetails subMethodDetails) { _subMethodDetails = subMethodDetails; return this; }
            public Builder WithIsPartialApproval(string isPartialApproval) { _isPartialApproval = isPartialApproval; return this; }
            public Builder WithCurrencyConversion(CurrencyConversion currencyConversion) { _currencyConversion = currencyConversion; return this; }
            public Builder WithIsRebilling(int? isRebilling) { _isRebilling = isRebilling; return this; }
            public Builder WithAutoPayment3D(bool autoPayment3D) { _autoPayment3D = autoPayment3D; return this; }
            public Builder WithTransactionType(string transactionType) { _transactionType = transactionType; return this; }
            public Builder WithCustomSiteName(string customSiteName) { _customSiteName = customSiteName; return this; }
            public Builder WithProductId(string productId) { _productId = productId; return this; }
            public Builder WithCustomData(string customData) { _customData = customData; return this; }
            public Builder WithRelatedTransactionId(string relatedTransactionId) { _relatedTransactionId = relatedTransactionId; return this; }
            public Builder WithItems(List<Item> items) { _items = items ?? new List<Item>(); return this; }
            public Builder WithUserDetails(CashierUserDetails userDetails) { _userDetails = userDetails; return this; }
            public Builder WithShippingAddress(UserAddress shippingAddress) { _shippingAddress = shippingAddress; return this; }
            public Builder WithBillingAddress(UserAddress billingAddress) { _billingAddress = billingAddress; return this; }
            public Builder WithDynamicDescriptor(DynamicDescriptor dynamicDescriptor) { _dynamicDescriptor = dynamicDescriptor; return this; }
            public Builder WithMerchantDetails(MerchantDetails merchantDetails) { _merchantDetails = merchantDetails; return this; }
            public Builder WithUrlDetails(UrlDetails urlDetails) { _urlDetails = urlDetails; return this; }
            public Builder WithUserTokenId(string userTokenId) { _userTokenId = userTokenId; return this; }
            public Builder WithClientUniqueId(string clientUniqueId) { _clientUniqueId = clientUniqueId; return this; }
            public Builder WithAmountDetails(AmountDetails amountDetails) { _amountDetails = amountDetails; return this; }
            public Builder WithRebillingType(string rebillingType) { _rebillingType = rebillingType; return this; }
            public Builder WithAuthenticationTypeOnly(string authenticationTypeOnly) { _authenticationTypeOnly = authenticationTypeOnly; return this; }
            public Builder WithSubMerchant(SubMerchant subMerchant) { _subMerchant = subMerchant; return this; }
            public Builder WithAddendums(Addendums addendums) { _addendums = addendums; return this; }
            public Builder WithUserId(string userId) { _userId = userId; return this; }
            public Builder WithDeviceDetails(DeviceDetails deviceDetails) { _deviceDetails = deviceDetails; return this; }
            public Builder WithClientRequestId(string clientRequestId) { _clientRequestId = clientRequestId; return this; }
            public Builder WithInternalRequestId(string internalRequestId) { _internalRequestId = internalRequestId; return this; }

            /// <summary>
            /// Builds the <see cref="PaymentRequest"/> instance.
            /// </summary>
            /// <returns>A configured instance of <see cref="PaymentRequest"/>.</returns>
            public PaymentRequest Build()
            {
                var request = new PaymentRequest(_merchantInfo, _sessionToken, _currency, _amount, _paymentOption)
                {
                    // Properties from PaymentRequest itself
                    OrderId = _orderId,
                    IsMoto = _isMoto,
                    SubMethodDetails = _subMethodDetails,
                    IsPartialApproval = _isPartialApproval,
                    CurrencyConversion = _currencyConversion,

                    // Properties from Authorize3dAndPaymentRequest
                    IsRebilling = _isRebilling,
                    AutoPayment3D = _autoPayment3D,

                    // Properties from SafechargePaymentRequest
                    TransactionType = _transactionType,
                    CustomSiteName = _customSiteName,
                    ProductId = _productId,
                    CustomData = _customData,
                    RelatedTransactionId = _relatedTransactionId,

                    // Properties from SafechargeOrderDetailsRequest
                    Items = _items,
                    UserDetails = _userDetails,
                    ShippingAddress = _shippingAddress,
                    BillingAddress = _billingAddress,
                    DynamicDescriptor = _dynamicDescriptor,
                    MerchantDetails = _merchantDetails,
                    UrlDetails = _urlDetails,
                    UserTokenId = _userTokenId,
                    ClientUniqueId = _clientUniqueId,
                    AmountDetails = _amountDetails,

                    // Properties from SafechargeRequest
                    RebillingType = _rebillingType,
                    AuthenticationTypeOnly = _authenticationTypeOnly,
                    SubMerchant = _subMerchant,
                    Addendums = _addendums,
                    UserId = _userId,
                    DeviceDetails = _deviceDetails,

                    // Properties from SafechargeBaseRequest
                    ClientRequestId = _clientRequestId,
                    InternalRequestId = _internalRequestId
                };
                // RequestUri is set in the PaymentRequest constructor.
                // TimeStamp and Checksum are handled by SafechargeBaseRequest.
                return request;
            }
        }
    }
}
