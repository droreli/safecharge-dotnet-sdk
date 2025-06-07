using System;
using NUnit.Framework; // Changed for NUnit
using Safecharge.Utils;
using Safecharge.Utils.Enum; // For HashAlgorithmType

namespace Safecharge.Test.Core
{
    [TestFixture] // Changed for NUnit
    public class GuardTests
    {
        // Tests for RequiresValidCurrencyCode
        [Test] // Changed for NUnit
        public void RequiresValidCurrencyCode_ValidCodes_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Guard.RequiresValidCurrencyCode("USD", "currency"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCurrencyCode("EUR", "currency"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCurrencyCode("GBP", "currency"));
            // Null or empty are considered acceptable by the guard (means optional)
            Assert.DoesNotThrow(() => Guard.RequiresValidCurrencyCode(null, "currency"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCurrencyCode("", "currency"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCurrencyCode("   ", "currency")); // Whitespace only
        }

        [Test] // Changed for NUnit
        [TestCase("us")] // Changed for NUnit
        [TestCase("USDD")] // Changed for NUnit
        [TestCase("123")] // Changed for NUnit
        [TestCase("U S D")] // Changed for NUnit
        [TestCase("US1")] // Changed for NUnit
        public void RequiresValidCurrencyCode_InvalidCodes_ShouldThrowArgumentException(string invalidCode)
        {
            Assert.Throws<ArgumentException>(() => Guard.RequiresValidCurrencyCode(invalidCode, "currency")); // Changed for NUnit
        }

        // Tests for RequiresValidCountryCode
        [Test] // Changed for NUnit
        public void RequiresValidCountryCode_ValidCodes_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Guard.RequiresValidCountryCode("US", "country"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCountryCode("GB", "country"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCountryCode("DE", "country"));
            // Null or empty are considered acceptable by the guard (means optional)
            Assert.DoesNotThrow(() => Guard.RequiresValidCountryCode(null, "country"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCountryCode("", "country"));
            Assert.DoesNotThrow(() => Guard.RequiresValidCountryCode("  ", "country")); // Whitespace only
        }

        [Test] // Changed for NUnit
        [TestCase("u")] // Changed for NUnit
        [TestCase("USS")] // Changed for NUnit
        [TestCase("12")] // Changed for NUnit
        [TestCase("U S")] // Changed for NUnit
        [TestCase("U1")] // Changed for NUnit
        public void RequiresValidCountryCode_InvalidCodes_ShouldThrowArgumentException(string invalidCode)
        {
            Assert.Throws<ArgumentException>(() => Guard.RequiresValidCountryCode(invalidCode, "country")); // Changed for NUnit
        }

        // Tests for RequiresValidEnum (string overload)
        [Test] // Changed for NUnit
        public void RequiresValidEnum_String_ValidValues_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>("SHA256", "hashAlgorithm"));
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>("sha256", "hashAlgorithm")); // Case-insensitive
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>("MD5", "hashAlgorithm"));
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>("md5", "hashAlgorithm"));
            // Null or empty are considered acceptable by the guard (means optional)
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>(null, "hashAlgorithm"));
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>("", "hashAlgorithm"));
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum<HashAlgorithmType>("   ", "hashAlgorithm"));
        }

        [Test] // Changed for NUnit
        [TestCase("SHA1")] // Changed for NUnit
        [TestCase("MD55")] // Changed for NUnit
        [TestCase("UNDEFINED_VALUE")] // Changed for NUnit
        public void RequiresValidEnum_String_InvalidValues_ShouldThrowArgumentException(string invalidValue)
        {
            Assert.Throws<ArgumentException>(() => Guard.RequiresValidEnum<HashAlgorithmType>(invalidValue, "hashAlgorithm")); // Changed for NUnit
        }

        // Tests for RequiresValidEnum (TEnum overload)
        [Test] // Changed for NUnit
        public void RequiresValidEnum_Enum_ValidValues_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum(HashAlgorithmType.SHA256, "hashAlgorithm"));
            Assert.DoesNotThrow(() => Guard.RequiresValidEnum(HashAlgorithmType.MD5, "hashAlgorithm"));
        }

        [Test] // Changed for NUnit
        public void RequiresValidEnum_Enum_InvalidValues_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Guard.RequiresValidEnum((HashAlgorithmType)77, "hashAlgorithm")); // Changed for NUnit
            Assert.Throws<ArgumentException>(() => Guard.RequiresValidEnum((HashAlgorithmType)99, "hashAlgorithm")); // Changed for NUnit
            Assert.Throws<ArgumentException>(() => Guard.RequiresValidEnum((HashAlgorithmType)(-1), "hashAlgorithm")); // Changed for NUnit
        }
    }
}
