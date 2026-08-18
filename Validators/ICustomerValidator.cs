using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Validators
{
    /// <summary>
    /// Defines validation responsibilities for Customer entities.
    /// </summary>
    public interface ICustomerValidator
    {
        /// <summary>
        /// Validate the provided customer and return a ValidationResult.
        /// Does not check database-level uniqueness.
        /// </summary>
        ValidationResult Validate(Customer customer);
    }
}
