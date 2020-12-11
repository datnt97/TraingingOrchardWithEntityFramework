using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.Utilities.ValidatorExtensions
{
    /// <summary>
    /// This is a test for reusable property validators
    /// </summary>
    public static class RegisterRequestValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.SetValidator(new ListCountValidator<TElement>(num));
        }
    }
}