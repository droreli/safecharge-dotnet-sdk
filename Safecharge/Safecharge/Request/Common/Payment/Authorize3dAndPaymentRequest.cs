using Safecharge.Model.Common;
using Safecharge.Model.PaymentOptionModels;
using Safecharge.Utils.Enum;

namespace Safecharge.Request.Common.Payment
{
    /// <summary>
    /// Abstract base class for requests that involve payment authorization, such as Payment and Authorize3D requests.
    /// It extends <see cref="SafechargePaymentRequest"/> by adding payment option details.
    /// </summary>
    public abstract class Authorize3dAndPaymentRequest : SafechargePaymentRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Authorize3dAndPaymentRequest"/> class (empty constructor, often used for deserialization).
        /// </summary>
        public Authorize3dAndPaymentRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorize3dAndPaymentRequest"/> class with essential parameters.
        /// </summary>
        /// <param name="merchantInfo">Merchant information. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="checksumOrderMapping">The order of fields used for checksum calculation. See <see cref="Utils.Enum.ChecksumOrderMapping"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API.</param>
        /// <param name="currency">The three-letter ISO currency code.</param>
        /// <param name="amount">The transaction amount as a string.</param>
        /// <param name="paymentOption">The payment option details. See <see cref="Model.PaymentOptionModels.PaymentOption"/>.</param>
        public Authorize3dAndPaymentRequest(
            MerchantInfo merchantInfo,
            ChecksumOrderMapping checksumOrderMapping,
            string sessionToken,
            string currency,
            string amount,
            PaymentOption paymentOption)
            : base(merchantInfo, checksumOrderMapping, sessionToken, currency, amount)
        {
            this.PaymentOption = paymentOption;
        }

        /// <summary>
        /// Gets or sets the details about the payment method (e.g., card details, APM details).
        /// </summary>
        public PaymentOption PaymentOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a rebilling (recurring) transaction.
        /// Expected values: 0 (No) or 1 (Yes).
        /// </summary>
        public int? IsRebilling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically trigger 3D Secure authentication if required.
        /// </summary>
        public bool AutoPayment3D { get; set; }
    }
}
