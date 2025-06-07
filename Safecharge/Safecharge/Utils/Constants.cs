using System.Collections.Generic;
using Safecharge.Utils.Enum;

namespace Safecharge.Utils
{
    /// <summary>
    /// Defines various constant values used across the SDK, such as date formats, checksum parameter orders,
    /// string length constraints, and validation patterns.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The standard timestamp format used in Safecharge API requests ("yyyyMMddHHmmss").
        /// </summary>
        public const string TimeStampFormat = "yyyyMMddHHmmss";

        /// <summary>
        /// Defines the order of parameters for checksum calculation based on the <see cref="ChecksumOrderMapping"/> type.
        /// The dictionary key is the mapping type, and the value is a list of parameter names in the order they should be concatenated.
        /// </summary>
        public static Dictionary<ChecksumOrderMapping, List<string>> RequestChecksumParameters = new Dictionary<ChecksumOrderMapping, List<string>>()
        {
            { ChecksumOrderMapping.ApiGenericChecksumMapping, new List<string>{ "MerchantId", "MerchantSiteId", "ClientRequestId", "Amount", "Currency", "TimeStamp" } },
            { ChecksumOrderMapping.ApiBasicChecksumMapping, new List<string>{ "MerchantId", "MerchantSiteId", "ClientRequestId", "TimeStamp" } },
            { 
                ChecksumOrderMapping.SettleGwTransactionChecksumMapping, 
                new List<string>
                { 
                    "MerchantId", "MerchantSiteId", "ClientRequestId", "ClientUniqueId", "Amount", "Currency", "RelatedTransactionId", "AuthCode", "DescriptorMerchantName", "DescriptorMerchantPhone", "Comment", "UrlDetails", "TimeStamp" 
                } 
            },
            {
                ChecksumOrderMapping.VoidGwTransactionChecksumMapping,
                new List<string>
                {
                    "MerchantId", "MerchantSiteId", "ClientRequestId", "ClientUniqueId", "Amount", "Currency", "RelatedTransactionId", "AuthCode", "Comment", "UrlDetails", "TimeStamp"
                }
            },
            {
                ChecksumOrderMapping.RefundGwTransactionChecksumMapping,
                new List<string>
                {
                    "MerchantId", "MerchantSiteId", "ClientRequestId", "ClientUniqueId", "Amount", "Currency", "RelatedTransactionId", "AuthCode", "Comment", "UrlDetails", "TimeStamp"
                }
            },
            { ChecksumOrderMapping.NoChecksumMapping, new List<string> { } },
        };

        // Maximum length constants for various fields
        /// <summary>Maximum length for string identifiers (e.g., ClientUniqueId, UserTokenId).</summary>
        public const int MaxLengthStringId = 45;
        /// <summary>Default maximum length for general string fields.</summary>
        public const int MaxLengthStringDefault = 255;
        /// <summary>Maximum length for card numbers.</summary>
        public const int MaxLengthCardNumber = 20;
        /// <summary>Maximum length for cardholder names.</summary>
        public const int MaxLengthCardHolderName = 70;
        /// <summary>Maximum length for first names.</summary>
        public const int MaxLengthFirstName = 30;
        /// <summary>Maximum length for last names.</summary>
        public const int MaxLengthLastName = 40;
        /// <summary>Maximum length for email addresses.</summary>
        public const int MaxLengthEmail = 100;
        /// <summary>Maximum length for phone numbers.</summary>
        public const int MaxLengthPhone = 18;
        /// <summary>Maximum length for address lines.</summary>
        public const int MaxLengthAddress = 60;
        /// <summary>Maximum length for city names.</summary>
        public const int MaxLengthCity = 30;
        /// <summary>Maximum length for country codes (ISO 3166-1 alpha-2).</summary>
        public const int MaxLengthCountry = 2;
        /// <summary>Maximum length for state or province codes.</summary>
        public const int MaxLengthState = 2; // Usually 2 for US/CA, but can vary. API might enforce specific formats.
        /// <summary>Maximum length for postal/ZIP codes.</summary>
        public const int MaxLengthZip = 10;
        /// <summary>Maximum length for county names.</summary>
        public const int MaxLengthCounty = 255;
        /// <summary>Default maximum length for date strings (e.g., YYYYMMDD).</summary>
        public const int MaxLengthDateDefault = 8;
        /// <summary>Maximum length for merchant descriptor name.</summary>
        public const int MaxLengthMerchantDescriptorName = 25;
        /// <summary>Maximum length for merchant descriptor phone.</summary>
        public const int MaxLengthMerchantDescriptorPhone = 13;
        /// <summary>Maximum length for URLs.</summary>
        public const int MaxLengthUrl = 1000;

        // Minimum length constants
        /// <summary>Default minimum length for general string fields that are not empty.</summary>
        public const int MinLengthStringDefault = 1;

        // Regex patterns
        /// <summary>Expected date of birth format pattern ("yyyy-MM-dd").</summary>
        public const string PatternDateOfBirth = "yyyy-MM-dd";
        /// <summary>Regular expression pattern for validating IPv4 addresses.</summary>
        public const string PatternIpAddress = "^(25[0-5]|2[0-4]\\d|[0-1]?\\d?\\d)(\\.(25[0-5]|2[0-4]\\d|[0-1]?\\d?\\d)){3}$";
    }
}
