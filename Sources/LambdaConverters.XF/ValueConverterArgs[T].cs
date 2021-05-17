﻿using System.Globalization;

namespace LambdaConverters
{
    /// <summary>
    /// Provides data for conversion functions.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public partial struct ValueConverterArgs<T>
    {
        internal ValueConverterArgs(T value, CultureInfo? culture)
        {
            Value = value;
            Culture = culture;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        public CultureInfo? Culture { get; }
    }
}