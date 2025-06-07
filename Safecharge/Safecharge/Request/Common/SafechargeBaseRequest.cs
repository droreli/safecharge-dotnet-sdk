using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Safecharge.Model.Common;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Request.Common
{
    /// <summary>
    /// Abstract base class for all request objects sent to the Safecharge API.
    /// It includes common properties like session token, timestamp, checksum, and merchant host information.
    /// </summary>
    public abstract class SafechargeBaseRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeBaseRequest"/> class.
        /// This empty constructor is often used for deserialization or direct instantiation.
        /// </summary>
        public SafechargeBaseRequest() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeBaseRequest"/> class with essential merchant and session information.
        /// </summary>
        /// <param name="merchantInfo">Merchant information which includes server host, secret key, and hashing algorithm. See <see cref="Model.Common.MerchantInfo"/>.</param>
        /// <param name="checksumOrderMapping">The order of fields to be used for checksum calculation. See <see cref="Utils.Enum.ChecksumOrderMapping"/>.</param>
        /// <param name="sessionToken">The session token obtained from Safecharge API. This parameter is optional and can be null if not yet obtained (e.g., for GetSessionTokenRequest itself).</param>
        public SafechargeBaseRequest(
            MerchantInfo merchantInfo,
            ChecksumOrderMapping checksumOrderMapping,
            string sessionToken)
        {            
            this.ServerHost = merchantInfo.ServerHost;
            this.TimeStamp = DateTime.Now.ToString(Constants.TimeStampFormat);
            this.SessionToken = sessionToken;
            this.MerchantKey = merchantInfo.MerchantKey;
            this.HashAlgorithmType = merchantInfo.HashAlgorithm;
            this.ChecksumOrderMapping = checksumOrderMapping;
        }

        /// <summary>
        /// Gets the merchant's secret key, used for checksum calculation.
        /// </summary>
        protected string MerchantKey { get; private set; }

        /// <summary>
        /// Gets the hashing algorithm type used for checksum calculation.
        /// </summary>
        protected HashAlgorithmType HashAlgorithmType { get; private set; }

        /// <summary>
        /// Gets or sets the mapping type that defines the order of parameters for checksum calculation.
        /// </summary>
        protected ChecksumOrderMapping ChecksumOrderMapping { get; set; }

        /// <summary>
        /// Gets or sets an internal request identifier. This can be used for tracing or logging.
        /// </summary>
        public string InternalRequestId { get; set; }

        /// <summary>
        /// Gets or sets a client-generated request identifier.
        /// This can be used to ensure idempotency if the same ID is submitted multiple times.
        /// </summary>
        public string ClientRequestId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the request in "yyyyMMddHHmmss" format.
        /// Automatically set during construction.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Gets the calculated checksum for the request.
        /// The checksum is generated based on the <see cref="ChecksumOrderMapping"/> and <see cref="MerchantKey"/>.
        /// </summary>
        public string Checksum => this.CalculateChecksum(this.ChecksumOrderMapping);

        /// <summary>
        /// Gets or sets the session token for the request.
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// Gets or sets the server host URL for the Safecharge API.
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Gets or sets the fully constructed request URI, including the server host and relative path.
        /// This is typically set by derived request classes.
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        /// Gets the WebMaster ID, which includes the SDK version.
        /// This is used for identifying the source of the request.
        /// </summary>
        public string WebMasterId 
        {
            get
            {
                var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly()?.Location);

                return fvi != null ? $"{ApiConstants.SdkVersion}_{fvi.ProductVersion}" : string.Empty; 
            }
        } 

        /// <summary>
        /// Helper method for derived classes to create the full request URI.
        /// </summary>
        /// <param name="relativePath">The relative path for the specific API endpoint (e.g., "payment.do").</param>
        /// <param name="queryString">Optional query string to append to the URI.</param>
        /// <returns>The fully constructed <see cref="Uri"/> for the request.</returns>
        protected Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri($"{this.ServerHost}{relativePath}");
            var uriBuilder = new UriBuilder(endpoint) { Query = queryString };
            return uriBuilder.Uri;
        }

        private string CalculateChecksum(ChecksumOrderMapping checksumOrderMapping = ChecksumOrderMapping.ApiGenericChecksumMapping)
        {
            var checksum = new StringBuilder();

            if (Constants.RequestChecksumParameters.ContainsKey(checksumOrderMapping))
            {
                List<string> paramsOrder = Constants.RequestChecksumParameters[checksumOrderMapping];

                foreach (string parameterName in paramsOrder)
                {
                    checksum.Append(this.GetPropValue(parameterName));
                }
            }

            checksum.Append(this.MerchantKey);
            return this.GetHash(this.HashAlgorithmType, checksum.ToString());
        }

        private string GetHash(HashAlgorithmType hashAlgorithmType, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithmType == HashAlgorithmType.MD5
                ? new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input))
                : hashAlgorithmType == HashAlgorithmType.SHA256
                    ? SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input))
                    : null;

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            if (data != null)
            {
                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private string GetPropValue(string propName)
        {
            return this.GetType()?.GetProperty(propName)?.GetValue(this, null)?.ToString();
        }
    }
}
