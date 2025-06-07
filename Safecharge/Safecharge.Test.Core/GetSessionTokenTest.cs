using System;
using System.Net.Http;
using System.Threading.Tasks; // Added for Task
using Microsoft.Extensions.Logging.Abstractions; // Added for NullLoggerFactory
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Safecharge.Model.Common;
using Safecharge.Request;
using Safecharge.Test.Core.Common;
using Safecharge.Utils;
using Safecharge.Utils.Enum;
using Safecharge.Utils.Exceptions;

namespace Safecharge.Test.Core
{
    public class GetSessionTokenTest : SafechargeRequestExecutorBaseTest
    {
        private readonly MerchantInfo validMerchantInfoShaAlgo = new MerchantInfo(
            MerchantKeyValue,
            MerchantIdValue,
            MerchantSiteIdValue,
            ServerHostValue,
            HashAlgorithmType.SHA256);

        private readonly MerchantInfo validMerchantInfoMd5Algo = new MerchantInfo(
            MerchantKeyValue,
            MerchantIdValue,
            MerchantSiteIdValue,
            ServerHostValue,
            HashAlgorithmType.MD5);

        [Test]
        public void TestCreatingValidMerchant()
        {
            var getSessionTokenRequest = new GetSessionTokenRequest(validMerchantInfoMd5Algo);

            Assert.IsNotNull(getSessionTokenRequest);
        }

        [Test]
        public void TestCreatingNullMerchantKey()
        {
            ActualValueDelegate<object> getSessionTokenRequestDelegate = () => new MerchantInfo(
                null,
                MerchantIdValue,
                MerchantSiteIdValue,
                ServerHostValue,
                HashAlgorithmType.MD5);

            Assert.That(getSessionTokenRequestDelegate, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TestCreatingNullMerchantId()
        {
            ActualValueDelegate<object> getSessionTokenRequestDelegate = () => new MerchantInfo(
                MerchantKeyValue,
                null, 
                MerchantSiteIdValue,
                ServerHostValue,
                HashAlgorithmType.MD5);

            Assert.That(getSessionTokenRequestDelegate, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TestCreatingNullMerchantSiteId()
        {
            ActualValueDelegate<object> getSessionTokenRequestDelegate = () => new MerchantInfo(
                MerchantKeyValue,
                MerchantIdValue,
                null,
                ServerHostValue,
                HashAlgorithmType.MD5);

            Assert.That(getSessionTokenRequestDelegate, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TestCreatingNullServerHost()
        {
            ActualValueDelegate<object> getSessionTokenRequestDelegate = () => new MerchantInfo(
                MerchantKeyValue,
                MerchantIdValue,
                MerchantSiteIdValue,
                null,
                HashAlgorithmType.MD5);

            Assert.That(getSessionTokenRequestDelegate, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TestCreatingInvalidServerHost()
        {
            var merchantInfo = new MerchantInfo(
                MerchantKeyValue,
                MerchantIdValue,
                MerchantSiteIdValue,
                ApiConstants.IntegrationHost + "invalid",
                HashAlgorithmType.SHA256);
            var getSessionTokenRequest = new GetSessionTokenRequest(merchantInfo);

            ActualValueDelegate<object> getSessionTokenRequestDelegate = 
                () => requestExecutor.GetSessionToken(getSessionTokenRequest).GetAwaiter().GetResult();

            Assert.That(getSessionTokenRequestDelegate, Throws.TypeOf<SafechargeException>());
        }

        [Test]
        public void TestChecksumWithShaAlgorithm()
        {
            var getSessionTokenRequest = new GetSessionTokenRequest(validMerchantInfoShaAlgo);

            var response = requestExecutor.GetSessionToken(getSessionTokenRequest).GetAwaiter().GetResult();

            Assert.IsTrue(response.ErrorType != ErrorType.InvalidChecksum);
        }

        [Test]
        public void TestChecksumWithMd5Algorithm()
        {
            var getSessionTokenRequest = new GetSessionTokenRequest(validMerchantInfoMd5Algo);

            var response = requestExecutor.GetSessionToken(getSessionTokenRequest).GetAwaiter().GetResult();

            Assert.IsTrue(response.ErrorType != ErrorType.InvalidChecksum);
        }

        [Test]
        public void TestValidRequest()
        {
            var getSessionTokenRequest = new GetSessionTokenRequest(validMerchantInfoShaAlgo);

            var response = requestExecutor.GetSessionToken(getSessionTokenRequest).GetAwaiter().GetResult();

            Assert.IsTrue(response.ApiType == ApiType.Payment);
            Assert.IsNull(response.ClientUniqueId);
            Assert.IsTrue(response.ErrCode == default);
            Assert.IsNull(response.ErrorType);
            Assert.IsNull(response.Hint);
            Assert.IsNotNull(response.InternalRequestId);
            Assert.IsNotNull(response.MerchantId);
            Assert.IsNotNull(response.MerchantSiteId);
            Assert.IsEmpty(response.Reason);
            Assert.IsNotNull(response.SessionToken);
            Assert.IsNotEmpty(response.SessionToken);
            Assert.IsTrue(response.Status == ResponseStatus.Success);
            Assert.IsNotEmpty(response.Version);
        }

        [Test]
        public async Task TestInitializingSafecharge() // Changed to async Task
        {
            var safeCharge = await Safecharge.CreateAsync( // Changed to await CreateAsync
                MerchantKeyValue,
                MerchantIdValue,
                MerchantSiteIdValue,
                ServerHostValue,
                HashAlgorithmType.SHA256,
                NullLoggerFactory.Instance); // Added NullLoggerFactory

            Assert.IsNotNull(safeCharge);
        }

        [Test]
        public async Task TestInitializingSafechargeWithHttpClient() // Changed to async Task
        {
            var safeCharge = await Safecharge.CreateAsync( // Changed to await CreateAsync
                new HttpClient(),
                new MerchantInfo(
                    MerchantKeyValue,
                    MerchantIdValue,
                    MerchantSiteIdValue,
                    ServerHostValue,
                    HashAlgorithmType.SHA256),
                NullLoggerFactory.Instance); // Added NullLoggerFactory

            Assert.IsNotNull(safeCharge);
        }

        [Test]
        public async Task TestInitializingSafechargeWithMerchantInfo() // Changed to async Task
        {
            var safeCharge = await Safecharge.CreateAsync( // Changed to await CreateAsync
                new MerchantInfo(
                    MerchantKeyValue,
                    MerchantIdValue,
                    MerchantSiteIdValue,
                    ServerHostValue,
                    HashAlgorithmType.SHA256),
                NullLoggerFactory.Instance); // Added NullLoggerFactory

            Assert.IsNotNull(safeCharge);
        }

        [Test]
        public void TestInitializingSafechargeWithWrongMerchantId() // Stays sync for Assert.Throws
        {
            // NUnit's Assert.Throws can take an AsyncTestDelegate
            AsyncTestDelegate asyncTestDelegate = async () => await Safecharge.CreateAsync(
                MerchantKeyValue,
                "invalid", // This merchant ID might cause an error during GetSessionToken
                MerchantSiteIdValue,
                ServerHostValue,
                HashAlgorithmType.SHA256,
                NullLoggerFactory.Instance); // Added NullLoggerFactory

            Assert.That(asyncTestDelegate, Throws.TypeOf<SafechargeConfigurationException>());
        }
    }
}