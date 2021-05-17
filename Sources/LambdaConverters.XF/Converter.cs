using System;
using Xamarin.Forms;

namespace LambdaConverters
{
    internal abstract class Converter
    {
        protected Converter(
            ConverterErrorStrategy errorStrategy,
            object? defaultInputTypeValue,
            object? defaultOutputTypeValue,
            Type inputType,
            Type outputType,
            bool isConvertFunctionAvailable,
            bool isConvertBackFunctionAvailable)
        {
            ErrorStrategy = errorStrategy;
            DefaultInputTypeValue = defaultInputTypeValue;
            DefaultOutputTypeValue = defaultOutputTypeValue;
            InputType = inputType;
            OutputType = outputType;
            IsConvertFunctionAvailable = isConvertFunctionAvailable;
            IsConvertBackFunctionAvailable = isConvertBackFunctionAvailable;
        }

        internal ConverterErrorStrategy ErrorStrategy { get; }

        internal object? DefaultInputTypeValue { get; }

        internal object? DefaultOutputTypeValue { get; }

        internal Type InputType { get; }

        internal Type OutputType { get; }

        internal bool IsConvertFunctionAvailable { get; }

        internal bool IsConvertBackFunctionAvailable { get; }

        internal object? GetErrorValue(object? defaultValue)
            => ErrorStrategy switch
            {
                ConverterErrorStrategy.ReturnDefaultValue => defaultValue,
                ConverterErrorStrategy.UseFallbackOrDefaultValue => null,
                ConverterErrorStrategy.DoNothing => Binding.DoNothing,
                _ => throw new NotSupportedException()
            };
    }
}