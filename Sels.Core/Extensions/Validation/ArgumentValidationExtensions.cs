﻿using Sels.Core.Extensions.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sels.Core.Extensions
{
    [Obsolete]
    public static class ArgumentValidationExtensions
    {
        /// <summary>
        /// Validates argument to see if it passes the condition. Throws exception from validationFailedExceptionFunc when it fails validation.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="condition">Condition that argument needs to pass</param>
        /// <param name="validationFailedExceptionFunc">Func that creates exception when validation fails. First arg is supplied argument</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgument<T>(this T argument, Predicate<T> condition, Func<T, Exception> validationFailedExceptionFunc)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (validationFailedExceptionFunc == null) throw new ArgumentNullException(nameof(validationFailedExceptionFunc));

            if (!condition(argument))
            {
                throw validationFailedExceptionFunc(argument);
            }

            return argument;
        }

        /// <summary>
        /// Validates argument to see if it passes the condition. Throws ArgumentException with validationFailedMessage when it fails validation.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="condition">Condition that argument needs to pass</param>
        /// <param name="validationFailedMessage">Message for ArgumentException when validation fails</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgument<T>(this T argument, Predicate<T> condition, string validationFailedMessage)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (string.IsNullOrWhiteSpace(validationFailedMessage)) throw new ArgumentException($"{nameof(validationFailedMessage)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(condition, x => new ArgumentException(validationFailedMessage));
        }

        /// <summary>
        /// Validates if argument is not null. Throws ArgumentNullException when argument is null.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgument<T>(this T argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => x != null, x => throw new ArgumentNullException(parameterName));
        }

        #region String
        /// <summary>
        /// Validates if argument is not null, empty or whitespace. Throws ArgumentException when it is.
        /// </summary>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <returns><paramref name="argument"/></returns>
        public static string ValidateArgumentNotNullOrEmpty(this string argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => !string.IsNullOrEmpty(x), $"{parameterName} cannot be null or empty");
        }

        /// <summary>
        /// Validates if argument is not null or empty. Throws ArgumentException when it is.
        /// </summary>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <returns><paramref name="argument"/></returns>
        public static string ValidateArgumentNotNullOrWhitespace(this string argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => !string.IsNullOrWhiteSpace(x), $"{parameterName} cannot be null, empty or whitespace");
        }
        #endregion

        #region Comparable
        /// <summary>
        /// Validates if argument is larger than comparator. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <typeparam name="TCompare">Type of comparator</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="comparator">Value to compare argument to</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentLarger<T, TCompare>(this T argument, string parameterName, TCompare comparator)
            where T : IComparable<TCompare>
            where TCompare : T
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => argument.CompareTo(comparator) > 0, $"{nameof(parameterName)} must be larger than <{comparator}>. Was <{argument}>");
        }

        /// <summary>
        /// Validates if argument is larger or equal to comparator. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <typeparam name="TCompare">Type of comparator</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="comparator">Value to compare argument to</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentLargerOrEqual<T, TCompare>(this T argument, string parameterName, TCompare comparator)
            where T : IComparable<TCompare>
            where TCompare : T
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => argument.CompareTo(comparator) >= 0, $"{nameof(parameterName)} must be larger or equal to <{comparator}>. Was <{argument}>");
        }

        /// <summary>
        /// Validates if argument is smaller than comparator. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <typeparam name="TCompare">Type of comparator</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="comparator">Value to compare argument to</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentSmaller<T, TCompare>(this T argument, string parameterName, TCompare comparator)
            where T : IComparable<TCompare>
            where TCompare : T
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => argument.CompareTo(comparator) < 0, $"{nameof(parameterName)} must be larger or equal to <{comparator}>. Was <{argument}>");
        }

        /// <summary>
        /// Validates if argument is smaller or equal to comparator. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <typeparam name="TCompare">Type of comparator</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="comparator">Value to compare argument to</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentSmallerOrEqual<T, TCompare>(this T argument, string parameterName, TCompare comparator)
            where T : IComparable<TCompare>
            where TCompare : T
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => argument.CompareTo(comparator) <= 0, $"{nameof(parameterName)} must be larger or equal to <{comparator}>. Was <{argument}>");
        }

        /// <summary>
        /// Validates if argument in range of startRange and endRange. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <typeparam name="TCompare">Type of comparator</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="startRange">Start value of range</param>
        /// <param name="endRange">End value of range</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentInRange<T, TCompare>(this T argument, string parameterName, TCompare startRange, TCompare endRange)
            where T : IComparable<TCompare>
            where TCompare : T
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => argument.CompareTo(startRange) >= 0 && argument.CompareTo(endRange) <= 0, $"{nameof(parameterName)} must be in range of <{startRange}> and <{endRange}>. Was <{argument}>");
        }
        #endregion

        #region Collection
        /// <summary>
        /// Validates if argument is not null and contains at least 1 item. Throws ArgumentException when it does not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <typeparam name="TItem">Type of item in collection</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentNotNullOrEmpty<T, TItem>(this T argument, string parameterName) where T : IEnumerable<TItem>
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => x.HasValue(), $"{parameterName} cannot be null and must contain at least 1 item");
        }

        /// <summary>
        /// Validates if argument is not null and contains at least 1 item. Throws ArgumentException when it does not.
        /// </summary>
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <returns><paramref name="argument"/></returns>
        public static IEnumerable<T> ValidateArgumentNotNullOrEmpty<T>(this IEnumerable<T> argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => x.HasValue(), $"{parameterName} cannot be null and must contain at least 1 item");
        }

        /// <summary>
        /// Validates if argument is not null and contains at least 1 item. Throws ArgumentException when it does not.
        /// </summary>
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <returns><paramref name="argument"/></returns>
        public static T[] ValidateArgumentNotNullOrEmpty<T>(this T[] argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => x.HasValue(), $"{parameterName} cannot be null and must contain at least 1 item");
        }
        #endregion

        #region Reflection
        /// <summary>
        /// Validates if argument is not null and is assignable from assignableType. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="assignableType">Type to check against</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentAssignableFrom<T>(this T argument, string parameterName, Type assignableType)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");
            if (assignableType == null) throw new ArgumentException($"{nameof(assignableType)} cannot be null");

            return argument.ValidateArgument(x => argument != null && argument.GetType().IsAssignableFrom(assignableType), $"{parameterName} cannot be null && must be assignable from Type <{assignableType}>");
        }

        /// <summary>
        /// Validates if argument is not null and assignableType is assignable from the type of argument. Throws ArgumentException when it is not.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="assignableType">Type to check against</param>
        /// <returns><paramref name="argument"/></returns>
        public static T ValidateArgumentAssignableTo<T>(this T argument, string parameterName, Type assignableType)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");
            if (assignableType == null) throw new ArgumentException($"{nameof(assignableType)} cannot be null");

            return argument.ValidateArgument(x => argument != null && assignableType.IsAssignableFrom(argument.GetType()), $"{parameterName} cannot be null && Type <{assignableType}> must be assignable from the type of argument");
        }

        /// <summary>
        /// Validates if argument is not null and is assignable from assignableType. Throws ArgumentException when it is not.
        /// </summary>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="assignableType">Type to check against</param>
        /// <returns><paramref name="argument"/></returns>
        public static Type ValidateArgumentAssignableFrom(this Type argument, string parameterName, Type assignableType)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");
            if (assignableType == null) throw new ArgumentException($"{nameof(assignableType)} cannot be null");

            return argument.ValidateArgument(x => argument != null && argument.IsAssignableFrom(assignableType), $"{parameterName} cannot be null && must be assignable from Type <{assignableType}>");
        }

        /// <summary>
        /// Validates if argument is not null and assignableType is assignable from the argument. Throws ArgumentException when it is not.
        /// </summary>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="assignableType">Type to check against</param>
        /// <returns><paramref name="argument"/></returns>
        public static Type ValidateArgumentAssignableTo(this Type argument, string parameterName, Type assignableType)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");
            if (assignableType == null) throw new ArgumentException($"{nameof(assignableType)} cannot be null");

            return argument.ValidateArgument(x => argument != null && assignableType.IsAssignableFrom(argument), $"{parameterName} cannot be null && Type <{assignableType}> must be assignable from the type of argument");
        }
        #endregion

        #region Type
        /// <summary>
        /// Validates if an instance can be constructed from <paramref name="argument"/> using the supplied <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="argument">Method/Constructor argument</param>
        /// <param name="parameterName">Method/Constructor parameter name</param>
        /// <param name="parameterTypes">Contructor argument types in order</param>
        /// <returns><paramref name="argument"/></returns>
        public static Type ValidateArgumentCanBeContructedWith(this Type argument, string parameterName, params Type[] parameterTypes)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException($"{nameof(parameterName)} cannot be null, empty or whitespace");

            return argument.ValidateArgument(x => argument != null && argument.CanConstructWith(), $"{parameterName} cannot be null && must contain {(parameterTypes.HasValue() ? "a constructor that has the following parameters: " + parameterTypes.JoinString(", ") : "a no-arg constructor")}");
        }
        #endregion
    }
}
