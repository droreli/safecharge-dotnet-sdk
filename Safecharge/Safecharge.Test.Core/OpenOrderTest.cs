using System; // Added for ArgumentException
using NUnit.Framework;
using Safecharge.Model.Common;
using Safecharge.Request;
using Safecharge.Test.Core.Common;
using Safecharge.Utils.Enum;

namespace Safecharge.Test.Core
{
    public class OpenOrderTest : SafechargeRequestExecutorBaseTest
    {
        [Test]
        public void TestOpenOrderSuccess()
        {
            var request = new OpenOrderRequest(merchantInfo,
                sessionToken, currency, amount)
            {
                MerchantDetails = new MerchantDetails
                {
                    CustomField1 = "test",
                    CustomField2 = "test",
                    CustomField3 = "test",
                    CustomField4 = "",
                    CustomField5 = "",
                    CustomField6 = "",
                    CustomField7 = "",
                    CustomField8 = "",
                    CustomField9 = "",
                    CustomField10 = "",
                    CustomField11 = "",
                    CustomField12 = "",
                    CustomField13 = "",
                    CustomField14 = "",
                    CustomField15 = ""
                }

            };

            var response = requestExecutor.OpenOrder(request).GetAwaiter().GetResult();

            Assert.IsNotNull(response);
            Assert.IsEmpty(response.Reason);
            Assert.AreEqual(ResponseStatus.Success, response.Status);
        }

        [Test]
        public void OpenOrderRequest_Constructor_ValidCurrency_ShouldNotThrow()
        {
            // Assuming merchantInfo, sessionToken, amount are available and valid from base or setup
            Assert.DoesNotThrow(() => new OpenOrderRequest(
                merchantInfo,
                sessionToken,
                "USD", // Valid currency
                amount
            ), "Constructing OpenOrderRequest with valid currency 'USD' should not throw.");

            Assert.DoesNotThrow(() => new OpenOrderRequest(
                merchantInfo,
                sessionToken,
                "EUR", // Valid currency
                amount
            ), "Constructing OpenOrderRequest with valid currency 'EUR' should not throw.");

            // Test with null or empty currency, as Guard allows it (optional)
            // The Guard.RequiresValidCurrencyCode currently returns if string.IsNullOrWhiteSpace(currencyCode) is true.
            Assert.DoesNotThrow(() => new OpenOrderRequest(
                merchantInfo,
                sessionToken,
                null, // Null currency
                amount
            ), "Constructing OpenOrderRequest with null currency should not throw.");

            Assert.DoesNotThrow(() => new OpenOrderRequest(
                merchantInfo,
                sessionToken,
                "", // Empty currency
                amount
            ), "Constructing OpenOrderRequest with empty currency should not throw.");

            Assert.DoesNotThrow(() => new OpenOrderRequest(
                merchantInfo,
                sessionToken,
                "   ", // Whitespace currency
                amount
            ), "Constructing OpenOrderRequest with whitespace currency should not throw.");
        }

        [TestCase("US")]
        [TestCase("EURO")]
        [TestCase("123")]
        [TestCase("usD")] // Regex is case-sensitive for [A-Z]
        [TestCase(" EUR")] // Leading space
        public void OpenOrderRequest_Constructor_InvalidCurrency_ShouldThrowArgumentException(string invalidCurrency)
        {
            Assert.Throws<ArgumentException>(() => new OpenOrderRequest(
                merchantInfo,
                sessionToken,
                invalidCurrency, // Invalid currency
                amount
            ), $"Constructing OpenOrderRequest with invalid currency '{invalidCurrency}' should throw ArgumentException.");
        }
    }
}
