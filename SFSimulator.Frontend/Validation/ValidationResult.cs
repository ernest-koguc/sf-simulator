namespace SFSimulator.Frontend;

public record ValidationResult
{
    protected ValidationResult() { }
    public bool IsValid => this is not FailedValidationResult;
    public static ValidationResult Success => new ValidationResult();
    public static implicit operator ValidationResult(string message) => new FailedValidationResult(message);
}

public record FailedValidationResult(string ValidationMessage) : ValidationResult
{
}

