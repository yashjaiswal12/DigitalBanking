using FluentValidation;
using MediatR;

namespace DigitalBanking.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var result = await Task.WhenAll(_validator.Select(x => x.ValidateAsync(context, cancellationToken)));

                var failures = result.SelectMany(s => s.Errors).Where(f => f != null).ToList();
                if (failures.Any())
                    throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
