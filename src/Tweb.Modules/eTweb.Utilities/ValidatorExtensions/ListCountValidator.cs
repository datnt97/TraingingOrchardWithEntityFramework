using System.Collections.Generic;
using FluentValidation.Validators;

public class ListCountValidator<T> : PropertyValidator
{
    private int _max;

    public ListCountValidator(int max)
        : base("{PropertyName} must contain fewer than {MaxElements} items.")
    {
        _max = max;
    }

    protected override bool IsValid(PropertyValidatorContext context)
    {
        var list = context.PropertyValue as IList<T>;

        if (list != null && list.Count >= _max)
        {
            context.MessageFormatter.AppendArgument("MaxElements", _max);
            return false;
        }

        return true;
    }
}