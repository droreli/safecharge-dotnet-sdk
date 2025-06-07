using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Safecharge.Request;
using Safecharge.Request.Common;
using Safecharge.Response;
using Safecharge.Response.Payment;
using Safecharge.Response.Transaction;
using Safecharge.Utils.Exceptions;
using Safecharge.Utils.Serialization;

namespace Safecharge
{
    /// <summary>
    /// This class provides functionality to execute SafechargeRequests directly to the SafeCharge's REST API
    /// </summary>
    /// <inheritdoc/>
    public class SafechargeRequestExecutor : ISafechargeRequestExecutor
    {
        private readonly ILogger<SafechargeRequestExecutor> _logger;
        private static readonly TimeSpan DefaultTimeoutTimeSpan = new TimeSpan(0, 0, 30);

        private static JsonSerializerSettings SerializerSettings =>
            new JsonSerializerSettings { 
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, 
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                {
                    NamingStrategy = new CustomNamingStrategy()
                },
            };

        protected static HttpClient HttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeRequestExecutor"/> class with a default HttpClient.
        /// </summary>
        /// <param name="logger">Optional logger instance for this executor.</param>
        public SafechargeRequestExecutor(ILogger<SafechargeRequestExecutor> logger = null)
        {
            _logger = logger ?? NullLogger<SafechargeRequestExecutor>.Instance;
            HttpClient = new HttpClient { Timeout = DefaultTimeoutTimeSpan };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafechargeRequestExecutor"/> class with a provided HttpClient.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance to use for making requests.</param>
        /// <param name="logger">Optional logger instance for this executor.</param>
        public SafechargeRequestExecutor(HttpClient httpClient, ILogger<SafechargeRequestExecutor> logger = null)
        {
            _logger = logger ?? NullLogger<SafechargeRequestExecutor>.Instance;
            HttpClient = httpClient;
        }

        public async Task<GetSessionTokenResponse> GetSessionToken(GetSessionTokenRequest getSessionTokenRequest)
        {
            return await this.PostAsync<GetSessionTokenResponse, GetSessionTokenRequest>(getSessionTokenRequest).ConfigureAwait(false);
        }

        public async Task<PaymentResponse> Payment(PaymentRequest paymentRequest)
        {
            return await this.PostAsync<PaymentResponse, PaymentRequest>(paymentRequest).ConfigureAwait(false);
        }

        public async Task<SettleTransactionResponse> SettleTransaction(SettleTransactionRequest settleTransactionRequest)
        {
            return await this.PostAsync<SettleTransactionResponse, SettleTransactionRequest>(settleTransactionRequest).ConfigureAwait(false);
        }

        public async Task<VoidTransactionResponse> VoidTransaction(VoidTransactionRequest voidTransactionRequest)
        {
            return await this.PostAsync<VoidTransactionResponse, VoidTransactionRequest>(voidTransactionRequest).ConfigureAwait(false);
        }

        public async Task<RefundTransactionResponse> RefundTransaction(RefundTransactionRequest refundTransactionRequest)
        {
            return await this.PostAsync<RefundTransactionResponse, RefundTransactionRequest>(refundTransactionRequest).ConfigureAwait(false);
        }

        public async Task<GetPaymentStatusResponse> GetPaymentStatus(GetPaymentStatusRequest getPaymentStatusRequest)
        {
            return await this.PostAsync<GetPaymentStatusResponse, GetPaymentStatusRequest>(getPaymentStatusRequest).ConfigureAwait(false);
        }

        public async Task<OpenOrderResponse> OpenOrder(OpenOrderRequest openOrderRequest)
        {
            return await this.PostAsync<OpenOrderResponse, OpenOrderRequest>(openOrderRequest).ConfigureAwait(false);
        }

        public async Task<InitPaymentResponse> InitPayment(InitPaymentRequest initPaymentRequest)
        {
            return await this.PostAsync<InitPaymentResponse, InitPaymentRequest>(initPaymentRequest).ConfigureAwait(false);
        }

        public async Task<Authorize3dResponse> Authorize3d(Authorize3dRequest authorize3dRequest)
        {
            return await this.PostAsync<Authorize3dResponse, Authorize3dRequest>(authorize3dRequest).ConfigureAwait(false);
        }

        public async Task<Verify3dResponse> Verify3d(Verify3dRequest verify3dRequest)
        {
            return await this.PostAsync<Verify3dResponse, Verify3dRequest>(verify3dRequest).ConfigureAwait(false);
        }

        public async Task<PayoutResponse> Payout(PayoutRequest payoutRequest)
        {
            return await this.PostAsync<PayoutResponse, PayoutRequest>(payoutRequest).ConfigureAwait(false);
        }

        public async Task<GetCardDetailsResponse> GetCardDetails(GetCardDetailsRequest request)
        {
            return await this.PostAsync<GetCardDetailsResponse, GetCardDetailsRequest>(request).ConfigureAwait(false);
        }

        public async Task<GetDCCResponse> GetDCCDetails(GetDCCRequest request)
        {
            return await this.PostAsync<GetDCCResponse, GetDCCRequest>(request).ConfigureAwait(false);
        }

        public async Task<GetMerchantPaymentMethodsResponse> GetMerchantPaymentMethods(GetMerchantPaymentMethodsRequest request)
        {
            return await this.PostAsync<GetMerchantPaymentMethodsResponse, GetMerchantPaymentMethodsRequest>(request).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a POST request to the Safecharge API with the given request object.
        /// </summary>
        /// <typeparam name="T1">The expected response type, which must derive from <see cref="SafechargeResponse"/>.</typeparam>
        /// <typeparam name="T2">The request type, which must derive from <see cref="SafechargeBaseRequest"/>.</typeparam>
        /// <param name="request">The request object to serialize and post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type <typeparamref name="T1"/>.</returns>
        /// <exception cref="SafechargeException">If an error occurs during the HTTP request, response processing, or JSON deserialization.</exception>
        public async Task<T1> PostAsync<T1, T2>(SafechargeBaseRequest request)
        {
            this._logger?.LogInformation("Sending {RequestType} to {RequestUri}", typeof(T2).Name, request.RequestUri);
            // In a real scenario, consider if request content should be logged, being mindful of sensitive data.
            // For this example, we are just logging the type and URI.

            var response = await HttpClient.PostAsync(request.RequestUri.ToString(), CreateHttpContent(request)).ConfigureAwait(false);
            return await this.GetResponseData<T1>(response).ConfigureAwait(false); // Added 'this.'
        }

        private static HttpContent CreateHttpContent<T>(T content) // This can remain static as it doesn't use instance members
        {
            var json = JsonConvert.SerializeObject(content, SerializerSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<T> GetResponseData<T>(HttpResponseMessage response) // Removed 'static'
        {
            try
            {
                this._logger?.LogInformation("Received HTTP {StatusCode} for {RequestUri}", response.StatusCode, response.RequestMessage.RequestUri); // Added 'this.'
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var responseObject = JsonConvert.DeserializeObject<T>(data);
                this._logger?.LogDebug("Successfully deserialized response to {ResponseType} for {RequestUri}", typeof(T).Name, response.RequestMessage.RequestUri); // Added 'this.'
                return responseObject;
            }
            catch (HttpRequestException ex)
            {
                this._logger?.LogError(ex, "Error processing HTTP request to {RequestUri}: {ErrorMessage}", response?.RequestMessage?.RequestUri, ex.Message); // Added 'this.'
                throw new SafechargeException(ex.Message);
            }
            catch (JsonException ex)
            {
                this._logger?.LogError(ex, "Error deserializing response from {RequestUri} to {ResponseType}: {ErrorMessage}", response?.RequestMessage?.RequestUri, typeof(T).Name, ex.Message); // Added 'this.'
                // Optionally, include response body in log if it's helpful and not too large/sensitive
                // var responseBodyForLog = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                // this._logger?.LogDebug("Response body for deserialization error: {ResponseBody}", responseBodyForLog); // Added 'this.'
                throw new SafechargeException($"Failed to deserialize response to {typeof(T).Name}.", ex);
            }
        }
    }
}
