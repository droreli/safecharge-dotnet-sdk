using System; // Added for ArgumentException
using NUnit.Framework;
using Safecharge.Model.Common;
using Safecharge.Model.PaymentOptionModels;
using Safecharge.Request;
using Safecharge.Test.Core.Common;
using Safecharge.Utils;
using Safecharge.Utils.Enum;

namespace Safecharge.Test.Core
{
    public class PaymentWorkflowTest : SafechargeRequestExecutorBaseTest
    {
        [Test]
        public void TestSimplePaymentSuccess()
        {
            var paymentRequest = new PaymentRequest(merchantInfo, sessionToken, currency, amount, paymentOptionCard);

            var paymentResponse = requestExecutor.Payment(paymentRequest).GetAwaiter().GetResult();

            Assert.IsNotNull(paymentResponse);
            Assert.IsEmpty(paymentResponse.Reason);
            Assert.AreEqual(ResponseStatus.Success, paymentResponse.Status);
            Assert.IsNull(paymentResponse.GwErrorReason);
            Assert.IsNull(paymentResponse.PaymentMethodErrorReason);
            Assert.AreNotEqual(ApiConstants.TransactionStatusError, paymentResponse.TransactionStatus);
        }

        [Test]
        public void TestPaymentSuccess()
        {
            var paymentRequest = new PaymentRequest(merchantInfo, sessionToken, currency, amount, paymentOptionCard)
            {
                BillingAddress = userAddress,
                DeviceDetails = new DeviceDetails { IpAddress = "93.146.254.172" },
                AmountDetails = new AmountDetails { TotalDiscount = "0", TotalHandling = "0", TotalShipping = "0", TotalTax = "0" },
                Items = items
            };

            var paymentResponse = requestExecutor.Payment(paymentRequest).GetAwaiter().GetResult();

            Assert.IsNotNull(paymentResponse);
            Assert.IsEmpty(paymentResponse.Reason);
            Assert.AreEqual(ResponseStatus.Success, paymentResponse.Status);
            Assert.IsNull(paymentResponse.GwErrorReason);
            Assert.IsNull(paymentResponse.PaymentMethodErrorReason);
            Assert.AreNotEqual(ApiConstants.TransactionStatusError, paymentResponse.TransactionStatus);
        }

        [Test]
        public void TestSimpleUPORequestPaymentSuccess()
        {
            var paymentRequest = new PaymentRequest(merchantInfo, sessionToken, currency, amount, paymentOptionCard)
            {
                UserTokenId = "MusalaTestUser",
                BillingAddress = userAddress,
                DeviceDetails = new DeviceDetails { IpAddress = "93.146.254.172" },
                AmountDetails = new AmountDetails { TotalDiscount = "0", TotalHandling = "0", TotalShipping = "0", TotalTax = "0" },
                Items = items
            };

            var paymentResponse = requestExecutor.Payment(paymentRequest).GetAwaiter().GetResult();

            var getSessionTokenRequest = new GetSessionTokenRequest(merchantInfo);

            var response = requestExecutor.GetSessionToken(getSessionTokenRequest).GetAwaiter().GetResult();
            sessionToken = response.SessionToken;
            var paymentOptionUpo = new PaymentOption
            {
                UserPaymentOptionId = paymentResponse.PaymentOption.UserPaymentOptionId
            };

            var paymentUPORequest = new PaymentRequest(merchantInfo, sessionToken, currency, amount, paymentOptionUpo)
            {
                UserTokenId = "MusalaTestUser",
                Items = items
            };

            var paymentUPOResponse = requestExecutor.Payment(paymentUPORequest).GetAwaiter().GetResult();

            Assert.IsNotNull(paymentUPOResponse);
            Assert.IsEmpty(paymentUPOResponse.Reason);
            Assert.AreEqual(ResponseStatus.Success, paymentUPOResponse.Status);
            Assert.IsNull(paymentUPOResponse.GwErrorReason);
            Assert.IsNull(paymentUPOResponse.PaymentMethodErrorReason);
            Assert.AreNotEqual(ApiConstants.TransactionStatusError, paymentUPOResponse.TransactionStatus);
        }

        [Test]
        public void PaymentRequest_Constructor_ValidCurrency_ShouldNotThrow()
        {
            // Assuming merchantInfo, sessionToken, amount, paymentOptionCard are available from base or setup
            Assert.DoesNotThrow(() => new PaymentRequest(
                merchantInfo,
                sessionToken,
                "USD", // Valid currency
                amount,
                paymentOptionCard
            ), "Constructing PaymentRequest with valid currency 'USD' should not throw.");

            Assert.DoesNotThrow(() => new PaymentRequest(
                merchantInfo,
                sessionToken,
                "EUR", // Valid currency
                amount,
                paymentOptionCard
            ), "Constructing PaymentRequest with valid currency 'EUR' should not throw.");

            // Test with null or empty currency, as Guard allows it (optional)
            // The Guard.RequiresValidCurrencyCode currently returns if string.IsNullOrWhiteSpace(currencyCode) is true.
            Assert.DoesNotThrow(() => new PaymentRequest(
                merchantInfo,
                sessionToken,
                null, // Null currency
                amount,
                paymentOptionCard
            ), "Constructing PaymentRequest with null currency should not throw.");

            Assert.DoesNotThrow(() => new PaymentRequest(
                merchantInfo,
                sessionToken,
                "", // Empty currency
                amount,
                paymentOptionCard
            ), "Constructing PaymentRequest with empty currency should not throw.");

            Assert.DoesNotThrow(() => new PaymentRequest(
                merchantInfo,
                sessionToken,
                "   ", // Whitespace currency
                amount,
                paymentOptionCard
            ), "Constructing PaymentRequest with whitespace currency should not throw.");
        }

        [TestCase("US")]
        [TestCase("EURO")]
        [TestCase("123")]
        [TestCase("usD")] // Regex is case-sensitive for [A-Z]
        [TestCase(" EUR")] // Leading space
        public void PaymentRequest_Constructor_InvalidCurrency_ShouldThrowArgumentException(string invalidCurrency)
        {
            Assert.Throws<ArgumentException>(() => new PaymentRequest(
                merchantInfo,
                sessionToken,
                invalidCurrency, // Invalid currency
                amount,
                paymentOptionCard
            ), $"Constructing PaymentRequest with invalid currency '{invalidCurrency}' should throw ArgumentException.");
        }
    }
}
