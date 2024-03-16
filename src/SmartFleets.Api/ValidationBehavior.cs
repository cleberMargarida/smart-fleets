using FluentValidation;
using MediatR;
using ValidationException = SmartFleets.Application.Exceptions.ValidationException;

namespace SmartFleets.Api
{
    /// <summary>
    /// Represents a behavior in the MediatR pipeline that performs validation on requests using FluentValidation.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response produced by the handler.</typeparam>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">A collection of validators for the request type.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        /// <summary>
        /// Handles the request by validating it before passing it to the next delegate in the pipeline.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="next">The next delegate in the pipeline.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the response from the handler.</returns>
        /// <exception cref="ValidationException">Thrown when the request fails validation.</exception>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);

            if (errors.Count != 0)
            {
                throw new ValidationException(errors);
            }

            return await next();
        }
    }
}
