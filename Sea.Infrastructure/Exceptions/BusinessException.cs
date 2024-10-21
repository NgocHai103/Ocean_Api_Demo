using Microsoft.AspNetCore.Identity;

namespace Sea.Infrastructure.Exceptions;

public class BusinessException(string? code = null, string? message = null, string details = null, Exception innerException = null) : Exception(message, innerException)
{
    public string Code { get; set; } = code;

    public string Details { get; set; } = details;

    public BusinessException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
    public BusinessException WithData(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
        {
            Data[error.Code] = error.Description;
        }

        return this;
    }
}