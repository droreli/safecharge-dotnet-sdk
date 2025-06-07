namespace Safecharge.Utils
{
    /// <summary>
    /// Defines constant values used throughout the Safecharge API integration,
    /// such as server hosts, API endpoint paths, transaction statuses, and types.
    /// </summary>
    public class ApiConstants
    {
        // Pre-configured hosts:
        /// <summary>
        /// The base URL for the Safecharge PPP (Payment Page Platform) integration/testing environment.
        /// </summary>
        public const string IntegrationHost = "https://ppp-test.safecharge.com/ppp/";

        // API enpoints:
        /// <summary>Relative path for the GetSessionToken API endpoint.</summary>
        public const string GetSessionTokenUrl = "api/v1/getSessionToken.do";
        /// <summary>Relative path for the Payment API endpoint.</summary>
        public const string PaymentUrl = "api/v1/payment.do";
        /// <summary>Relative path for the SettleTransaction API endpoint.</summary>
        public const string SettleTransactionUrl = "api/v1/settleTransaction.do";
        public const string VoidTransactionUrl = "api/v1/voidTransaction.do";
        public const string RefundTransactionUrl = "api/v1/refundTransaction.do";
        public const string GetPaymentStatusUrl = "api/v1/getPaymentStatus.do";
        public const string OpenOrderUrl = "api/v1/openOrder.do";
        public const string InitPaymentUrl = "api/v1/initPayment.do";
        public const string Authorize3dUrl = "api/v1/authorize3d.do";
        public const string Verify3dUrl = "api/v1/verify3d.do";
        public const string PayoutUrl = "api/v1/payout.do";
        public const string GetCardDetailsUrl = "api/v1/getCardDetails.do";
        public const string GetMerchantPaymentMethodsUrl = "api/v1/getMerchantPaymentMethods.do";
        public const string GetDcc = "api/v1/getDccDetails";

        /// <summary>Transaction status indicating approval.</summary>
        public const string TransactionStatusApproved = "APPROVED";
        /// <summary>Transaction status indicating decline.</summary>
        public const string TransactionStatusDeclined = "DECLINED";
        /// <summary>Transaction status indicating an error.</summary>
        public const string TransactionStatusError = "ERROR";
        /// <summary>Transaction status indicating a redirect is required (e.g., for 3D Secure).</summary>
        public const string TransactionStatusRedirect = "REDIRECT";

        /// <summary>Transaction type for Authorization.</summary>
        public const string TransactionTypeAuth = "Auth";
        /// <summary>Transaction type for Sale (direct capture).</summary>
        public const string TransactionTypeSale = "Sale";
        /// <summary>Transaction type for Pre-Authorization.</summary>
        public const string TransactionTypePreAuth = "PreAuth";
        /// <summary>Transaction type for Initializing 3D Secure Authorization.</summary>
        public const string TransactionTypeInitAuth3D = "InitAuth3D";

        /// <summary>Payment method type for deposits.</summary>
        public const string PaymentMethodTypeDeposit = "DEPOSIT";
        /// <summary>Payment method type for withdrawals.</summary>
        public const string PaymentMethodTypeWithdrawal = "WITHDRAWAL";

        /// <summary>
        /// Identifier for the source application, indicating requests originate from this .NET SDK.
        /// </summary>
        public const string SourceApplication = "NET_SDK";

        /// <summary>
        /// Prefix used for identifying the SDK version in requests (e.g., as part of WebMasterId).
        /// </summary>
        public const string SdkVersion = "sdk_dotnet_ver";
    }
}
