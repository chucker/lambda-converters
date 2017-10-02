﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace LambdaConverters
{
    /// <summary>
    /// A factory class used to create lambda-based instances of the <see cref="ValidationRule"/> class.
    /// </summary>
    public static class Validator
    {
        sealed class Rule<I> : System.Windows.Controls.ValidationRule
        {
            readonly Func<ValidationRuleArgs<I>, ValidationResult> ruleFunction;

            internal Rule(
                Func<ValidationRuleArgs<I>, ValidationResult> ruleFunction,
                RuleErrorStrategy errorStrategy)
            {
                ErrorStrategy = errorStrategy;
                this.ruleFunction = ruleFunction;
            }

            RuleErrorStrategy ErrorStrategy { get; }

            [Pure]
            ValidationResult GetErrorValue()
            {
                switch (ErrorStrategy)
                {
                    case RuleErrorStrategy.ReturnNull:
                        return null;

                    case RuleErrorStrategy.ReturnInvalid:
                        return new ValidationResult(false, null);

                    case RuleErrorStrategy.ReturnValid:
                        return new ValidationResult(true, null);

                    default:
                        throw new NotSupportedException();
                }
            }

            ValidationResult ValidateInternal(object item, CultureInfo cultureInfo)
            {
                I inputValue;
                try
                {
                    inputValue = (I)item;
                }
                catch (SystemException e) when (e is InvalidCastException || e is NullReferenceException)
                {
                    EventSource.Log.UnableToCastToRuleInputType(item?.GetType().Name ?? "null", typeof(I).Name, ErrorStrategy.ToString());

                    return GetErrorValue();
                }

                Debug.Assert(ruleFunction != null);

                return ruleFunction(new ValidationRuleArgs<I>(inputValue, cultureInfo));
            }

            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                if (this.ruleFunction == null)
                {
                    EventSource.Log.MissingRuleFunction("ruleFunction", ErrorStrategy.ToString());

                    return GetErrorValue();
                }

                return ValidateInternal(value, cultureInfo);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRule" /> class.
        /// </summary>
        /// <typeparam name="I">The value type.</typeparam>
        /// <param name="ruleFunction">The <see cref="ValidationRule.Validate(object, CultureInfo)"/> method.</param>
        /// <param name="errorStrategy">The error strategy.</param>
        /// <returns>An <see cref="ValidationRule" /> object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="errorStrategy"/> is not a valid <see cref="RuleErrorStrategy"/> value.
        /// </exception>
        [Pure]
        [NotNull]
        public static ValidationRule Create<I>(
            Func<ValidationRuleArgs<I>, ValidationResult> ruleFunction = null,
            RuleErrorStrategy errorStrategy = RuleErrorStrategy.ReturnNull)
        {
            switch (errorStrategy)
            {
                case RuleErrorStrategy.ReturnNull:
                case RuleErrorStrategy.ReturnInvalid:
                case RuleErrorStrategy.ReturnValid:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(errorStrategy));
            }

            return new Rule<I>(ruleFunction, errorStrategy);
        }
    }
}