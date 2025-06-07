using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Safecharge.Utils
{
    /// <summary>
    /// Provides static methods for performing common input validation checks (guards).
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures that the specified object parameter is not null.
        /// </summary>
        /// <param name="parameter">The object to check.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameter"/> is null.</exception>
        public static void RequiresNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Ensures that the length of a value (if it has one) matches an exact restriction value.
        /// </summary>
        /// <param name="length">The length of the value to check. Can be null if the value itself is optional or its length is not applicable.</param>
        /// <param name="lenghtRestrictionValue">The exact length the value must be.</param>
        /// <param name="parameterName">The name of the parameter whose length is being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="length"/> has a value and it does not equal <paramref name="lenghtRestrictionValue"/>.</exception>
        public static void RequiresLength(int? length, int lenghtRestrictionValue, string parameterName)
        {
            if (length.HasValue && length != lenghtRestrictionValue)
            {
                throw new ArgumentException($"{parameterName} should be exactly {lenghtRestrictionValue} characters long.");
            }

        }

        /// <summary>
        /// Ensures that the length of a value (if it has one) does not exceed a specified maximum.
        /// </summary>
        /// <param name="length">The length of the value to check. Can be null.</param>
        /// <param name="max">The maximum allowed length.</param>
        /// <param name="parameterName">The name of the parameter whose length is being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="length"/> has a value and it is greater than <paramref name="max"/>.</exception>
        public static void RequiresMaxLength(int? length, int max, string parameterName)
        {
            if (length.HasValue && length > max)
            {
                throw new ArgumentException($"{parameterName} should be up to {max} characters long.");
            }
        }

        /// <summary>
        /// Ensures that the length of a value (if it has one) falls within a specified minimum and maximum range.
        /// </summary>
        /// <param name="length">The length of the value to check. Can be null.</param>
        /// <param name="min">The minimum allowed length.</param>
        /// <param name="max">The maximum allowed length.</param>
        /// <param name="parameterName">The name of the parameter whose length is being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="length"/> has a value and it is less than <paramref name="min"/> or greater than <paramref name="max"/>.</exception>
        public static void RequiresLengthBetween(int? length, int min, int max, string parameterName)
        {
            if (length.HasValue && (length < min || length > max))
            {
                throw new ArgumentException($"The length of the {parameterName} should be between {min} and {max} symbols.");
            }
        }

        /// <summary>
        /// Ensures that a string parameter matches a specified regular expression pattern.
        /// </summary>
        /// <param name="parameterValue">The string value to check. If null, the check is bypassed.</param>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="parameterValue"/> is not null and does not match the <paramref name="pattern"/>.</exception>
        public static void RequiresPattern(string parameterValue, string pattern, string parameterName)
        {
            if (parameterValue != null && !Regex.IsMatch(parameterValue, pattern))
            {
                throw new ArgumentException($"{parameterName} should match the pattern {pattern}.");
            }
        }

        /// <summary>
        /// Ensures that a string parameter represents a boolean value as "0" (false) or "1" (true).
        /// </summary>
        /// <param name="parameterValue">The string value to check.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="parameterValue"/> is not "0" or "1".</exception>
        public static void RequiresBool(string parameterValue, string parameterName)
        {
            if (parameterValue != "0" && parameterValue != "1")
            {
                throw new ArgumentException($"{parameterName} should be either 0 or 1.");
            }
        }

        /// <summary>
        /// Ensures that a string parameter, if not null, can be parsed as a date in the specified format.
        /// </summary>
        /// <param name="parameterValue">The string value to check. If null, the check is bypassed.</param>
        /// <param name="format">The expected date format string.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="parameterValue"/> is not null and cannot be parsed into a date with the specified <paramref name="format"/>.</exception>
        public static void RequiresDateInFormat(string parameterValue, string format, string parameterName)
        {
            if (parameterValue != null && !DateTime.TryParseExact(
                    parameterValue,
                    format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out _))
            {
                throw new ArgumentException($"{parameterName} should be in the format {format}.");
            }
        }

        /// <summary>
        /// Ensures that a parameter's value is one of a specified list of allowed values.
        /// </summary>
        /// <typeparam name="T">The type of the parameter and allowed values. Must be a class.</typeparam>
        /// <param name="parameter">The parameter value to check. If null, the check is bypassed.</param>
        /// <param name="allowedValues">A list of allowed values.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="parameter"/> is not null and not present in the <paramref name="allowedValues"/> list.</exception>
        public static void RequiresAllowedValues<T>(T parameter, List<T> allowedValues, string parameterName)
            where T : class
        {
            if (parameter != null && !allowedValues.Contains(parameter))
            {
                throw new ArgumentException($"Allowed values for {parameterName} are: {string.Join(", ", allowedValues)}.");
            }
        }

        /// <summary>
        /// Ensures that a string parameter, if not null or whitespace, is a valid 3-letter ISO 4217 currency code (all uppercase).
        /// </summary>
        /// <param name="currencyCode">The currency code string to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="currencyCode"/> is provided and is not a valid 3-letter uppercase currency code.</exception>
        public static void RequiresValidCurrencyCode(string currencyCode, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                // Assuming currency code can be optional, but if provided, must be valid.
                // If it's mandatory, RequiresNotNullOrWhiteSpace should be used separately.
                return;
            }

            // ISO 4217: 3 uppercase letters
            if (!Regex.IsMatch(currencyCode, @"^[A-Z]{3}$"))
            {
                throw new ArgumentException($"Parameter {parameterName} ('{currencyCode}') must be a valid 3-letter ISO 4217 currency code (all uppercase).");
            }
        }

        /// <summary>
        /// Ensures that a string parameter, if not null or whitespace, is a valid 2-letter ISO 3166-1 alpha-2 country code (all uppercase).
        /// </summary>
        /// <param name="countryCode">The country code string to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="countryCode"/> is provided and is not a valid 2-letter uppercase country code.</exception>
        public static void RequiresValidCountryCode(string countryCode, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                // Assuming country code can be optional.
                return;
            }

            // ISO 3166-1 alpha-2: 2 uppercase letters
            if (!Regex.IsMatch(countryCode, @"^[A-Z]{2}$"))
            {
                throw new ArgumentException($"Parameter {parameterName} ('{countryCode}') must be a valid 2-letter ISO 3166-1 alpha-2 country code (all uppercase).");
            }
        }

        /// <summary>
        /// Ensures that a string parameter, if not null or whitespace, is a valid value for the specified enum type (case-insensitive).
        /// </summary>
        /// <typeparam name="TEnum">The enum type to validate against. Must be a struct and inherit from <see cref="System.Enum"/>.</typeparam>
        /// <param name="enumValue">The string representation of the enum value.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="enumValue"/> is provided and is not a valid name for <typeparamref name="TEnum"/>.</exception>
        public static void RequiresValidEnum<TEnum>(string enumValue, string parameterName) where TEnum : struct, System.Enum
        {
            if (string.IsNullOrWhiteSpace(enumValue))
            {
                // Assuming enum value can be optional.
                return;
            }

            // Using System.Enum.TryParse for case-insensitive check
            if (!System.Enum.TryParse<TEnum>(enumValue, true, out _))
            {
                throw new ArgumentException($"Parameter {parameterName} ('{enumValue}') is not a valid value for enum {typeof(TEnum).Name}. Valid values (case-insensitive) are: {string.Join(", ", System.Enum.GetNames(typeof(TEnum)))}.");
            }
        }

        /// <summary>
        /// Ensures that a given enum value is a defined member of its enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type. Must be a struct and inherit from <see cref="System.Enum"/>.</typeparam>
        /// <param name="enumValue">The enum value to check.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="enumValue"/> is not a defined member of <typeparamref name="TEnum"/>.</exception>
        public static void RequiresValidEnum<TEnum>(TEnum enumValue, string parameterName) where TEnum : struct, System.Enum
        {
            // This check is for enum values that are already of the enum type.
            // It ensures the underlying integral value is defined in the enum.
            if (!System.Enum.IsDefined(typeof(TEnum), enumValue))
            {
                throw new ArgumentException($"Parameter {parameterName} ('{enumValue}') is not a defined value for enum {typeof(TEnum).Name}. Valid values are: {string.Join(", ", System.Enum.GetNames(typeof(TEnum)))}.");
            }
        }
    }
}