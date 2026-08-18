using System;

namespace InfoMasterKonsole.Validators
{
    /// <summary>
    /// Generic validator interface. Concrete validators will be added in future phases.
    /// </summary>
    public interface IValidator<T>
    {
        ValidationResult Validate(T item);
    }
}
