using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Model.Common
{
    /// <summary>
    /// Represents essential merchant information required for API requests.
    /// This includes identification details, secret key, server host, and hashing algorithm.
    /// </summary>
    public class MerchantInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantInfo"/> class.
        /// This constructor is often used for deserialization or direct instantiation.
        /// </summary>
        public MerchantInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantInfo"/> class with provided merchant details.
        /// </summary>
        /// <param name="merchantKey">The secret merchant key provided by Safecharge during integration.</param>
        /// <param name="merchantId">The merchant's unique identifier in the Safecharge system.</param>
        /// <param name="merchantSiteId">The merchant's site identifier in the Safecharge system.</param>
        /// <param name="serverHost">The Safecharge API server host URL (e.g., "https://ppp-test.safecharge.com/ppp/api/v1/").</param>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithmType"/> used for generating the request checksum.</param>
        public MerchantInfo(string merchantKey, string merchantId, string merchantSiteId, string serverHost, HashAlgorithmType hashAlgorithm)
        {
            Guard.RequiresNotNull(merchantKey, nameof(merchantKey));
            Guard.RequiresNotNull(merchantId, nameof(merchantId));
            Guard.RequiresNotNull(merchantSiteId, nameof(merchantSiteId));
            Guard.RequiresNotNull(serverHost, nameof(serverHost));
            Guard.RequiresValidEnum<HashAlgorithmType>(hashAlgorithm, nameof(hashAlgorithm));

            this.MerchantKey = merchantKey;
            this.MerchantId = merchantId;
            this.MerchantSiteId = merchantSiteId;
            this.ServerHost = serverHost;
            this.HashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Gets or sets the secret merchant key.
        /// </summary>
        public string MerchantKey { get; set; }

        /// <summary>
        /// Gets or sets the merchant ID.
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the merchant site ID.
        /// </summary>
        public string MerchantSiteId { get; set; }

        /// <summary>
        /// Gets or sets the Safecharge API server host URL.
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Gets or sets the hashing algorithm used for checksum generation.
        /// </summary>
        public HashAlgorithmType HashAlgorithm { get; set; }
    }
}
