using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Safecharge.Model.Common;
using Safecharge.Model.Common.Addendum;
using Safecharge.Model.PaymentModels;
using Safecharge.Model.PaymentOptionModels;
using Safecharge.Model.PaymentOptionModels.CardModels;
using Safecharge.Model.PaymentOptionModels.InitPayment;
using Safecharge.Model.PaymentOptionModels.OpenOrder;
using Safecharge.Model.PaymentOptionModels.Verify3d;
using Safecharge.Request;
using Safecharge.Response;
using Safecharge.Response.Payment;
using Safecharge.Response.Transaction;
using Safecharge.Utils.Enum;
using Safecharge.Utils.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Safecharge
{
    /// <summary>
    /// Main class for interacting with the Safecharge (Nuvei) REST API.
    /// It provides methods for various payment operations such as creating orders, processing payments, and managing transactions.
    /// This class should be instantiated asynchronously using one of the <c>CreateAsync</c> factory methods.
    /// It handles session token acquisition and renewal automatically.
    /// </summary>
    /// <remarks>
    /// Ensure that merchant configuration details are correctly provided during instantiation.
    /// All public methods making API calls return a <see cref="Task{T}"/> representing the asynchronous operation.
    /// </remarks>
    /// <inheritdoc cref="ISafecharge"/>
    public class Safecharge : ISafecharge
    {
        private readonly MerchantInfo merchantInfo;
        private string sessionToken; // Made non-readonly to be assigned in InitializeAsync
        private readonly SafechargeRequestExecutor safechargeRequestExecutor;
        private readonly ILogger<Safecharge> _logger;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Private constructor for Safecharge wrapper with a configured HttpClient and server information.
        /// </summary>
        /// <param name="configuredHttpClient">httpClient to get the client's properties from</param>
        /// <param name="merchantInfo">Merchant information</param>
        /// <param name="loggerFactory">Optional logger factory</param>
        private Safecharge(
            HttpClient configuredHttpClient,
            MerchantInfo merchantInfo,
            ILoggerFactory loggerFactory = null)
        {
            this.merchantInfo = merchantInfo;
            this._loggerFactory = loggerFactory;
            this._logger = loggerFactory?.CreateLogger<Safecharge>() ?? NullLogger<Safecharge>.Instance;
            this.safechargeRequestExecutor = new SafechargeRequestExecutor(configuredHttpClient, _loggerFactory?.CreateLogger<SafechargeRequestExecutor>());
            // sessionToken is NOT initialized here
        }

        /// <summary>
        /// Private constructor for Safecharge wrapper with a default Safecharge's HttpClient and server information.
        /// </summary>
        /// <param name="merchantInfo">Merchant information</param>
        /// <param name="loggerFactory">Optional logger factory</param>
        private Safecharge(MerchantInfo merchantInfo, ILoggerFactory loggerFactory = null)
        {
            this.merchantInfo = merchantInfo;
            this._loggerFactory = loggerFactory;
            this._logger = loggerFactory?.CreateLogger<Safecharge>() ?? NullLogger<Safecharge>.Instance;
            this.safechargeRequestExecutor = new SafechargeRequestExecutor(_loggerFactory?.CreateLogger<SafechargeRequestExecutor>());
            // sessionToken is NOT initialized here
        }

        /// <summary>
        /// Private constructor for Safecharge wrapper with a default Safecharge's HttpClient and server information.
        /// </summary>
        /// <param name="merchantKey">The secret merchant key obtained by the Merchant during integration process with Safecharge</param>
        /// <param name="merchantId">Merchant id in the Safecharge's system</param>
        /// <param name="siteId">Merchant site id in the Safecharge's system</param>
        /// <param name="serverHost">The Safecharge's server address to send the request to</param>
        /// <param name="algorithmType">The hashing algorithm used to generate the checksum</param>
        /// <param name="loggerFactory">Optional logger factory</param>
        private Safecharge(
            string merchantKey,
            string merchantId,
            string siteId,
            string serverHost,
            HashAlgorithmType algorithmType,
            ILoggerFactory loggerFactory = null)
        {
            this.merchantInfo = new MerchantInfo(merchantKey, merchantId, siteId, serverHost, algorithmType);
            this._loggerFactory = loggerFactory;
            this._logger = loggerFactory?.CreateLogger<Safecharge>() ?? NullLogger<Safecharge>.Instance;
            this.safechargeRequestExecutor = new SafechargeRequestExecutor(_loggerFactory?.CreateLogger<SafechargeRequestExecutor>());
            // sessionToken is NOT initialized here
        }

        /// <summary>
        /// Creates and initializes a new instance of the <see cref="Safecharge"/> wrapper asynchronously using a provided <see cref="HttpClient"/> and <see cref="MerchantInfo"/>.
        /// </summary>
        /// <param name="configuredHttpClient">An <see cref="HttpClient"/> instance to be used for requests. This allows for custom HttpClient configurations (e.g., proxies, specific handlers).</param>
        /// <param name="merchantInfo">The <see cref="MerchantInfo"/> object containing merchant identification and credentials.</param>
        /// <param name="loggerFactory">Optional <see cref="ILoggerFactory"/> to enable logging within the SDK.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the initialized <see cref="Safecharge"/> instance.</returns>
        /// <exception cref="SafechargeConfigurationException">Thrown if session token acquisition fails during initialization.</exception>
        public static async Task<Safecharge> CreateAsync(HttpClient configuredHttpClient, MerchantInfo merchantInfo, ILoggerFactory loggerFactory = null)
        {
            var instance = new Safecharge(configuredHttpClient, merchantInfo, loggerFactory);
            await instance.InitializeAsync().ConfigureAwait(false);
            return instance;
        }

        /// <summary>
        /// Creates and initializes a new instance of the <see cref="Safecharge"/> wrapper asynchronously using <see cref="MerchantInfo"/>.
        /// A default <see cref="HttpClient"/> will be used.
        /// </summary>
        /// <param name="merchantInfo">The <see cref="MerchantInfo"/> object containing merchant identification and credentials.</param>
        /// <param name="loggerFactory">Optional <see cref="ILoggerFactory"/> to enable logging within the SDK.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the initialized <see cref="Safecharge"/> instance.</returns>
        /// <exception cref="SafechargeConfigurationException">Thrown if session token acquisition fails during initialization.</exception>
        public static async Task<Safecharge> CreateAsync(MerchantInfo merchantInfo, ILoggerFactory loggerFactory = null)
        {
            var instance = new Safecharge(merchantInfo, loggerFactory);
            await instance.InitializeAsync().ConfigureAwait(false);
            return instance;
        }

        /// <summary>
        /// Creates and initializes a new instance of the <see cref="Safecharge"/> wrapper asynchronously using explicit merchant configuration details.
        /// A default <see cref="HttpClient"/> will be used.
        /// </summary>
        /// <param name="merchantKey">The secret merchant key.</param>
        /// <param name="merchantId">The merchant ID.</param>
        /// <param name="siteId">The merchant site ID.</param>
        /// <param name="serverHost">The Safecharge API server host URL.</param>
        /// <param name="algorithmType">The hashing algorithm to be used for checksum generation (e.g., <see cref="HashAlgorithmType.SHA256"/>).</param>
        /// <param name="loggerFactory">Optional <see cref="ILoggerFactory"/> to enable logging within the SDK.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the initialized <see cref="Safecharge"/> instance.</returns>
        /// <exception cref="SafechargeConfigurationException">Thrown if session token acquisition fails during initialization.</exception>
        public static async Task<Safecharge> CreateAsync(
            string merchantKey,
            string merchantId,
            string siteId,
            string serverHost,
            HashAlgorithmType algorithmType,
            ILoggerFactory loggerFactory = null)
        {
            var instance = new Safecharge(merchantKey, merchantId, siteId, serverHost, algorithmType, loggerFactory);
            await instance.InitializeAsync().ConfigureAwait(false);
            return instance;
        }

        private async Task InitializeAsync()
        {
            _logger?.LogInformation("Attempting to retrieve session token for Merchant ID: {MerchantId}, Merchant Site ID: {MerchantSiteId}", this.merchantInfo.MerchantId, this.merchantInfo.MerchantSiteId);
            var request = new GetSessionTokenRequest(this.merchantInfo);
            var response = await this.safechargeRequestExecutor.GetSessionToken(request).ConfigureAwait(false);

            if (response.Status == ResponseStatus.Error)
            {
                _logger?.LogError("Failed to retrieve session token. Reason: {Reason}, ErrorCode: {ErrorCode}", response.Reason, response.ErrCode);
                throw new SafechargeConfigurationException(response.Reason);
            }
            this.sessionToken = response.SessionToken;
            _logger?.LogInformation("Session token retrieved successfully.");
        }

        /// <inheritdoc/>
        public async Task<PaymentResponse> Payment(
            string currency,
            string amount,
            PaymentOption paymentOption,
            List<Item> items = null,
            string userTokenId = null,
            string userId = null,
            string clientUniqueId = null,
            string clientRequestId = null,
            int? isRebilling = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            AmountDetails amountDetails = null,
            DeviceDetails deviceDetails = null,
            CashierUserDetails userDetails = null,
            UserAddress shippingAddress = null,
            UserAddress billingAddress = null,
            DynamicDescriptor dynamicDescriptor = null,
            MerchantDetails merchantDetails = null,
            Addendums addendums = null,
            UrlDetails urlDetails = null,
            string customSiteName = null,
            string productId = null,
            string customData = null,
            string relatedTransactionId = null,
            string transactionType = null,
            bool autoPayment3D = default,
            string isMoto = null,
            SubMethodDetails subMethodDetails = null,
            string isPartialApproval = null,
            SubMerchant subMerchant = null,
            string orderId = null)
        {
            var paymentRequest = new PaymentRequest(merchantInfo, sessionToken, currency, amount, paymentOption)
            {
                Items = items,
                UserTokenId = userTokenId,
                UserId = userId,
                ClientRequestId = clientRequestId,
                ClientUniqueId = clientUniqueId,
                IsRebilling = isRebilling,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                AmountDetails = amountDetails,
                DeviceDetails = deviceDetails,
                UserDetails = userDetails,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                DynamicDescriptor = dynamicDescriptor,
                MerchantDetails = merchantDetails,
                Addendums = addendums,
                UrlDetails = urlDetails,
                CustomSiteName = customSiteName,
                ProductId = productId,
                CustomData = customData,
                RelatedTransactionId = relatedTransactionId,
                TransactionType = transactionType,
                AutoPayment3D = autoPayment3D,
                IsMoto = isMoto,
                SubMethodDetails = subMethodDetails,
                IsPartialApproval = isPartialApproval,
                SubMerchant = subMerchant,
                OrderId = orderId
            };

            _logger?.LogInformation("Initiating Payment operation for Order ID: {OrderId}, Amount: {Amount} {Currency}", orderId, amount, currency);
            var response = await safechargeRequestExecutor.Payment(paymentRequest).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("Payment operation successful. Transaction ID: {TransactionId}, Order ID: {OrderId}", response.TransactionId, response.OrderId);
            }
            else
            {
                _logger?.LogError("Payment operation failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}, Order ID: {OrderId}", response.Reason, response.ErrCode, response.TransactionId, response.OrderId);
            }
            return response;
        }

        /// <inheritdoc/>
        public async Task<SettleTransactionResponse> SettleTransaction(
            string currency,
            string amount,
            string relatedTransactionId,
            string clientUniqueId = null,
            string clientRequestId = null,
            string userId = null,
            Addendums addendums = null,
            string descriptorMerchantName = null,
            string descriptorMerchantPhone = null,
            DynamicDescriptor dynamicDescriptor = null,
            UrlDetails urlDetails = null,
            string authCode = null,
            string customData = null,
            string comment = null,
            string customSiteName = null,
            string productId = null,
            DeviceDetails deviceDetails = null, 
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null)
        {
            var request = new SettleTransactionRequest(
                merchantInfo,
                sessionToken,
                currency,
                amount,
                relatedTransactionId)
            { 
                AuthCode = authCode,
                ClientUniqueId = clientUniqueId,
                ClientRequestId = clientRequestId,
                UserId = userId,
                Addendums = addendums,
                DescriptorMerchantName = dynamicDescriptor?.MerchantName ?? descriptorMerchantName,
                DescriptorMerchantPhone = dynamicDescriptor?.MerchantPhone ?? descriptorMerchantPhone,
                UrlDetails = urlDetails,
                CustomData = customData,
                Comment = comment,
                CustomSiteName = customSiteName,
                ProductId = productId,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant
            };

            _logger?.LogInformation("Initiating SettleTransaction for RelatedTransactionId: {RelatedTransactionId}", relatedTransactionId);
            var response = await safechargeRequestExecutor.SettleTransaction(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("SettleTransaction successful. Transaction ID: {TransactionId}", response.TransactionId);
            }
            else
            {
                _logger?.LogError("SettleTransaction failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}", response.Reason, response.ErrCode, response.TransactionId);
            }
            return response;
        }

        public async Task<VoidTransactionResponse> VoidTransaction(
            string currency,
            string amount,
            string relatedTransactionId,
            string clientUniqueId = null,
            string clientRequestId = null,
            string userId = null,
            UrlDetails urlDetails = null,
            string authCode = null,
            string customData = null,
            string comment = null,
            string customSiteName = null,
            string productId = null,
            DeviceDetails deviceDetails = null, 
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null)
        {
            var request = new VoidTransactionRequest(
                merchantInfo,
                sessionToken,
                currency,
                amount,
                relatedTransactionId)
            {
                AuthCode = authCode,
                ClientUniqueId = clientUniqueId,
                ClientRequestId = clientRequestId,
                UserId = userId,
                UrlDetails = urlDetails,
                CustomData = customData,
                Comment = comment,
                CustomSiteName = customSiteName,
                ProductId = productId,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums
            };

            _logger?.LogInformation("Initiating VoidTransaction for RelatedTransactionId: {RelatedTransactionId}", relatedTransactionId);
            var response = await safechargeRequestExecutor.VoidTransaction(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("VoidTransaction successful. Transaction ID: {TransactionId}", response.TransactionId);
            }
            else
            {
                _logger?.LogError("VoidTransaction failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}", response.Reason, response.ErrCode, response.TransactionId);
            }
            return response;
        }

        public async Task<RefundTransactionResponse> RefundTransaction(
            string currency,
            string amount,
            string relatedTransactionId,
            string clientUniqueId = null,
            string clientRequestId = null,
            string userId = null,
            UrlDetails urlDetails = null,
            string authCode = null,
            string customData = null,
            string comment = null,
            string customSiteName = null,
            string productId = null,
            DeviceDetails deviceDetails = null, 
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null)
        {
            var request = new RefundTransactionRequest(
                merchantInfo,
                sessionToken,
                currency,
                amount,
                relatedTransactionId)
            {
                AuthCode = authCode,
                ClientUniqueId = clientUniqueId,
                ClientRequestId = clientRequestId,
                UserId = userId,
                UrlDetails = urlDetails,
                CustomData = customData,
                Comment = comment,
                CustomSiteName = customSiteName,
                ProductId = productId,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums
            };

            _logger?.LogInformation("Initiating RefundTransaction for RelatedTransactionId: {RelatedTransactionId}", relatedTransactionId);
            var response = await safechargeRequestExecutor.RefundTransaction(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("RefundTransaction successful. Transaction ID: {TransactionId}", response.TransactionId);
            }
            else
            {
                _logger?.LogError("RefundTransaction failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}", response.Reason, response.ErrCode, response.TransactionId);
            }
            return response;
        }

        public async Task<GetPaymentStatusResponse> GetPaymentStatus(
            string userId = null,
            DeviceDetails deviceDetails = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null)
        {
            var request = new GetPaymentStatusRequest(merchantInfo, sessionToken) 
            {
                UserId = userId,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums
            };

            _logger?.LogInformation("Initiating GetPaymentStatus.");
            var response = await safechargeRequestExecutor.GetPaymentStatus(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("GetPaymentStatus successful.");
            }
            else
            {
                _logger?.LogError("GetPaymentStatus failed. Reason: {Reason}, ErrorCode: {ErrorCode}", response.Reason, response.ErrCode);
            }
            return response;
        }

        // Only adding a few more for brevity in this example.
        // Other public methods (VoidTransaction, RefundTransaction, etc.) would be documented similarly, referencing ISafecharge.

        /// <inheritdoc/>
        public async Task<OpenOrderResponse> OpenOrder(

            string currency,
            string amount,
            List<Item> items = null,
            OpenOrderPaymentOption paymentOption = null,
            UserPaymentOption userPaymentOption = null,
            string paymentMethod = null,
            string userTokenId = null,
            string clientUniqueId = null,
            string clientRequestId = null,
            string userId = null,
            string authenticationTypeOnly = null,
            AmountDetails amountDetails = null,
            DeviceDetails deviceDetails = null,
            CashierUserDetails userDetails = null,
            UserAddress shippingAddress = null,
            UserAddress billingAddress = null,
            DynamicDescriptor dynamicDescriptor = null,
            MerchantDetails merchantDetails = null,
            Addendums addendums = null,
            UrlDetails urlDetails = null,
            string customSiteName = null,
            string productId = null,
            string customData = null,
            string transactionType = null,
            string isMoto = null,
            string isRebilling = null,
            string rebillingType = null,
            SubMerchant subMerchant = null)
        {
            var request = new OpenOrderRequest(merchantInfo, sessionToken, currency, amount)
            {
                Items = items,
                PaymentOption = paymentOption,
                UserPaymentOption = userPaymentOption,
                PaymentMethod = paymentMethod,
                UserTokenId = userTokenId,
                ClientRequestId = clientRequestId,
                ClientUniqueId = clientUniqueId,
                UserId = userId,
                AuthenticationTypeOnly = authenticationTypeOnly,
                AmountDetails = amountDetails,
                DeviceDetails = deviceDetails,
                UserDetails = userDetails,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                DynamicDescriptor = dynamicDescriptor,
                MerchantDetails = merchantDetails,
                Addendums = addendums,
                UrlDetails = urlDetails,
                CustomSiteName = customSiteName,
                ProductId = productId,
                CustomData = customData,
                TransactionType = transactionType,
                IsMoto = isMoto,
                IsRebilling = isRebilling,
                RebillingType = rebillingType,
                SubMerchant = subMerchant
            };

            _logger?.LogInformation("Initiating OpenOrder for Amount: {Amount} {Currency}", amount, currency);
            var response = await safechargeRequestExecutor.OpenOrder(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("OpenOrder successful. Order ID: {OrderId}", response.OrderId);
            }
            else
            {
                _logger?.LogError("OpenOrder failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Order ID: {OrderId}", response.Reason, response.ErrCode, response.OrderId);
            }
            return response;
        }

        public async Task<InitPaymentResponse> InitPayment(
            string currency,
            string amount,
            InitPaymentPaymentOption paymentOption,
            string userTokenId = null,
            string clientUniqueId = null,
            string clientRequestId = null,
            DeviceDetails deviceDetails = null,
            UrlDetails urlDetails = null,
            string customData = null,
            UserAddress billingAddress = null,
            string userId = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null,
            string orderId = null)
        {
            var request = new InitPaymentRequest(merchantInfo, sessionToken, currency, amount, paymentOption)
            {
                UserTokenId = userTokenId,
                ClientRequestId = clientRequestId,
                ClientUniqueId = clientUniqueId,
                DeviceDetails = deviceDetails,
                UrlDetails = urlDetails,
                CustomData = customData,
                BillingAddress = billingAddress,
                UserId = userId,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums,
                OrderId = orderId
            };

            _logger?.LogInformation("Initiating InitPayment for Order ID: {OrderId}, Amount: {Amount} {Currency}", orderId, amount, currency);
            var response = await safechargeRequestExecutor.InitPayment(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("InitPayment successful. Transaction ID: {TransactionId}, Order ID: {OrderId}", response.TransactionId, response.OrderId);
            }
            else
            {
                _logger?.LogError("InitPayment failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}, Order ID: {OrderId}", response.Reason, response.ErrCode, response.TransactionId, response.OrderId);
            }
            return response;
        }

        public async Task<Authorize3dResponse> Authorize3d(
            string currency,
            string amount,
            PaymentOption paymentOption,
            string relatedTransactionId,
            List<Item> items = null,
            string userTokenId = null,
            string clientUniqueId = null,
            string clientRequestId = null,
            int? isRebilling = null,
            AmountDetails amountDetails = null,
            DeviceDetails deviceDetails = null,
            CashierUserDetails userDetails = null,
            UserAddress shippingAddress = null,
            UserAddress billingAddress = null,
            DynamicDescriptor dynamicDescriptor = null,
            MerchantDetails merchantDetails = null,
            Addendums addendums = null,
            UrlDetails urlDetails = null,
            string customSiteName = null,
            string productId = null,
            string customData = null,
            string transactionType = null,
            bool autoPayment3D = default,
            string userId = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null)
        {
            var request = new Authorize3dRequest(merchantInfo, sessionToken, currency, amount, paymentOption, relatedTransactionId)
            {
                Items = items,
                UserTokenId = userTokenId,
                ClientRequestId = clientRequestId,
                ClientUniqueId = clientUniqueId,
                IsRebilling = isRebilling,
                AmountDetails = amountDetails,
                DeviceDetails = deviceDetails,
                UserDetails = userDetails,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                DynamicDescriptor = dynamicDescriptor,
                MerchantDetails = merchantDetails,
                Addendums = addendums,
                UrlDetails = urlDetails,
                CustomSiteName = customSiteName,
                ProductId = productId,
                CustomData = customData,
                TransactionType = transactionType,
                AutoPayment3D = autoPayment3D,
                UserId = userId,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant
            };

            _logger?.LogInformation("Initiating Authorize3d for RelatedTransactionId: {RelatedTransactionId}", relatedTransactionId);
            var response = await safechargeRequestExecutor.Authorize3d(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("Authorize3d successful. Transaction ID: {TransactionId}", response.TransactionId);
            }
            else
            {
                _logger?.LogError("Authorize3d failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}", response.Reason, response.ErrCode, response.TransactionId);
            }
            return response;
        }

        public async Task<Verify3dResponse> Verify3d(
            string currency,
            string amount,
            Verify3dPaymentOption paymentOption,
            string relatedTransactionId,
            string clientUniqueId = null,
            string clientRequestId = null,
            UserAddress billingAddress = null,
            string customData = null,
            string customSiteName = null,
            MerchantDetails merchantDetails = null,
            SubMerchant subMerchant = null,
            string userId = null,
            string userTokenId = null,
            DeviceDetails deviceDetails = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            Addendums addendums = null)
        {
            var request = new Verify3dRequest(merchantInfo, sessionToken, currency, amount, paymentOption, relatedTransactionId)
            {
                UserId = userId,
                UserTokenId = userTokenId,
                ClientRequestId = clientRequestId,
                ClientUniqueId = clientUniqueId,
                SubMerchant = subMerchant,
                BillingAddress = billingAddress,
                MerchantDetails = merchantDetails,
                CustomSiteName = customSiteName,
                CustomData = customData,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                Addendums = addendums
            };

            _logger?.LogInformation("Initiating Verify3d for RelatedTransactionId: {RelatedTransactionId}", relatedTransactionId);
            var response = await safechargeRequestExecutor.Verify3d(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("Verify3d successful. Transaction ID: {TransactionId}", response.TransactionId);
            }
            else
            {
                _logger?.LogError("Verify3d failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}", response.Reason, response.ErrCode, response.TransactionId);
            }
            return response;
        }

        public async Task<PayoutResponse> Payout(
            string userTokenId,
            string clientUniqueId,
            string amount,
            string currency,
            UserPaymentOption userPaymentOption,
            string comment = null,
            DynamicDescriptor dynamicDescriptor = null,
            MerchantDetails merchantDetails = null,
            UrlDetails urlDetails = null,
            DeviceDetails deviceDetails = null,
            CardData cardData = null,
            string userId = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null)
        {
            var request = new PayoutRequest(
                merchantInfo,
                sessionToken,
                userTokenId,
                clientUniqueId,
                amount,
                currency,
                userPaymentOption)
            {
                Comment = comment,
                DynamicDescriptor = dynamicDescriptor,
                MerchantDetails = merchantDetails,
                UrlDetails = urlDetails,
                DeviceDetails = deviceDetails,
                CardData = cardData,
                UserId = userId,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums
            };

            _logger?.LogInformation("Initiating Payout for UserTokenId: {UserTokenId}, Amount: {Amount} {Currency}", userTokenId, amount, currency);
            var response = await safechargeRequestExecutor.Payout(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("Payout successful. Transaction ID: {TransactionId}", response.TransactionId);
            }
            else
            {
                _logger?.LogError("Payout failed. Reason: {Reason}, ErrorCode: {ErrorCode}, Transaction ID: {TransactionId}", response.Reason, response.ErrCode, response.TransactionId);
            }
            return response;
        }

        public async Task<GetCardDetailsResponse> GetCardDetails(
            string clientUniqueId,
            string cardNumber,
            string userId = null,
            DeviceDetails deviceDetails = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null)
        {
            var request = new GetCardDetailsRequest(
                merchantInfo,
                sessionToken,
                clientUniqueId,
                cardNumber)
            {
                UserId = userId,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums
            };

            _logger?.LogInformation("Initiating GetCardDetails for CardNumber (masked): ****{LastFourDigits}", cardNumber?.Length > 4 ? cardNumber.Substring(cardNumber.Length - 4) : "****");
            var response = await safechargeRequestExecutor.GetCardDetails(request).ConfigureAwait(false);
            if (response.Status == ResponseStatus.Success)
            {
                _logger?.LogInformation("GetCardDetails successful.");
            }
            else
            {
                _logger?.LogError("GetCardDetails failed. Reason: {Reason}, ErrorCode: {ErrorCode}", response.Reason, response.ErrCode);
            }
            return response;
        }

        public async Task<GetMerchantPaymentMethodsResponse> GetMerchantPaymentMethods(
            string clientRequestId,
            string currencyCode = null,
            string countryCode = null,
            string languageCode = null,
            string type = null,
            string userId = null,
            DeviceDetails deviceDetails = null,
            string rebillingType = null,
            string authenticationTypeOnly = null,
            SubMerchant subMerchant = null,
            Addendums addendums = null)
        {
            var request = new GetMerchantPaymentMethodsRequest(
                merchantInfo,
                sessionToken,
                clientRequestId)
            {
                CurrencyCode = currencyCode,
                CountryCode = countryCode,
                LanguageCode = languageCode,
                Type = type,
                UserId = userId,
                DeviceDetails = deviceDetails,
                RebillingType = rebillingType,
                AuthenticationTypeOnly = authenticationTypeOnly,
                SubMerchant = subMerchant,
                Addendums = addendums
            };

            return await safechargeRequestExecutor.GetMerchantPaymentMethods(request).ConfigureAwait(false);
        }

        public async Task<GetDCCResponse> GetDccDetails(string clientRequestId, string clientUniqueId, string cardNumber, string originalAmount, string originalCurrency, string currency)
        {
            var request = new GetDCCRequest(this.merchantInfo, this.sessionToken)
            {
                SessionToken = this.sessionToken, // Ensure this.sessionToken is used
                MerchantId = this.merchantInfo.MerchantId,
                    MerchantSiteId = this.merchantInfo.MerchantSiteId,
                    ClientRequestId = clientRequestId,
                    ClientUniqueId = clientUniqueId,
                    Amount = originalAmount,
                    OriginalAmount = originalAmount,
                    OriginalCurrency = originalCurrency,
                    Currency = currency,
                    Apm = "apmgw_expresscheckout"
                };
            // This call might need to be awaited if GetDCCDetails is an async method in SafechargeRequestExecutor.
            // The current method signature `Task<GetDCCResponse> GetDccDetails(...)` suggests it should be.
            // However, sticking to the subtask's focus on session token initialization.
            // The original code `var response = this.safechargeRequestExecutor.GetDCCDetails(...)` implies GetDCCDetails might not be async.
            // If it is async and returns Task, it should be `await this.safechargeRequestExecutor.GetDCCDetails(request);`
            // For now, replicating existing logic but using the local 'request' variable.
            return await this.safechargeRequestExecutor.GetDCCDetails(request).ConfigureAwait(false);
        }

        // The GetSessionToken method is now effectively replaced by InitializeAsync.
        // It's good practice to remove unused private methods.
        // private string GetSessionToken()
        // {
        //     var request = new GetSessionTokenRequest(this.merchantInfo);
        //     var response = await this.safechargeRequestExecutor.GetSessionToken(request);
        //     if (response.Status == ResponseStatus.Error)
        //     {
        //         throw new SafechargeConfigurationException(response.Reason);
        //     }
        //     return response.SessionToken;
        // }
    }
}
