using System;
using System.Collections.Generic;

namespace SmartFleets.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs during validation.
    /// </summary>
    public sealed class ValidationException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="errors">The validation errors that occurred.</param>
        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets the validation errors that occurred.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; }
    }
}
