using Safecharge.Model.Common;
using Safecharge.Model.Common.Addendum;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Request.Common
{
    /// <summary>
    /// Abstract base class for all requests made to the Safecharge API.
    /// It extends <see cref="SafechargeBaseRequest"/> and includes common parameters like Merchant ID, Merchant Site ID, and session token.
    /// </summary>
    public abstract class SafechargeRequest : SafechargeBaseRequest
    {
        private string userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeRequest"/> class (empty constructor, often used for deserialization).
        /// </summary>
        public SafechargeRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeRequest"/> class with essential merchant and session information.
        /// </summary>
        /// <param name="merchantInfo">Merchant information. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="checksumOrderMapping">The order of fields used for checksum calculation. See <see cref="Utils.Enum.ChecksumOrderMapping"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API. This parameter is optional.</param>
        public SafechargeRequest(
            MerchantInfo merchantInfo,
            ChecksumOrderMapping checksumOrderMapping,
            string sessionToken = null)
            : base(merchantInfo, checksumOrderMapping, sessionToken)
        {
            this.MerchantId = merchantInfo.MerchantId;
            this.MerchantSiteId = merchantInfo.MerchantSiteId;
        }

        /// <summary>
        /// Gets or sets the Merchant ID provided by Safecharge.
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the Merchant Site ID provided by Safecharge.
        /// </summary>
        public string MerchantSiteId { get; set; }

        /// <summary>
        /// Gets the source application identifier, typically a constant value indicating the SDK.
        /// See <see cref="ApiConstants.SourceApplication"/>.
        /// </summary>
        public string SourceApplication { get { return ApiConstants.SourceApplication;  } }

        /// <summary>
        /// Gets or sets the type of rebilling (recurring) transaction.
        /// Examples: "RECURRING", "MIT" (Merchant Initiated Transaction).
        /// </summary>
        public string RebillingType { get; set; }

        /// <summary>
        /// Gets or sets the type of authentication to be performed, if only authentication is required.
        /// </summary>
        public string AuthenticationTypeOnly { get; set; }

        /// <summary>
        /// Gets or sets sub-merchant information, if applicable (e.g., for payment facilitators).
        /// </summary>
        public SubMerchant SubMerchant { get; set; }

        /// <summary>
        /// Gets or sets industry-specific addendums (e.g., for local payments, hotel, airline sectors).
        /// </summary>
        public Addendums Addendums { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user in the merchant's system.
        /// </summary>
        /// <remarks>Max length is defined by <see cref="Constants.MaxLengthStringDefault"/>.</remarks>
        public string UserId
        {
            get { return this.userId; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthStringDefault, nameof(this.UserId));
                this.userId = value;
            }
        }

        /// <summary>
        /// Gets or sets the details of the device from which the transaction is initiated.
        /// </summary>
        public DeviceDetails DeviceDetails { get; set; }
    }
}
