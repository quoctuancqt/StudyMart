using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;

namespace StudyMart.Libs.CQRS;

internal sealed class ValidationCommandHandler<TCommand, TResponse>(
    ICommandHandler<TCommand, TResponse> innerHandler,
    IEnumerable<IValidator<TCommand>> validators)
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        // Validate the command using all registered validators
        ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

        if (validationFailures.Length == 0)
        {
            return await innerHandler.Handle(command, cancellationToken);
        }

        // If validation fails, return a failure result with the errors
        return Result.Failure<TResponse>(JsonSerializer.Serialize(validationFailures.Select(e => new ValidationError
        {
            Code = e.ErrorCode,
            Message = e.ErrorMessage
        }).ToArray()));
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TCommand>(command);

        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    // private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
    //     new(validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray());
}

internal class ValidationError
{
    public string Code { get; set; }

    public string Message { get; set; }
}