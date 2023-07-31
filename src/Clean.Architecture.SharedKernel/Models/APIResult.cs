namespace Clean.Architecture.SharedKernel.Models;

/// <summary>
/// Represents the result of an API operation.
/// </summary>
/// <param name="Succeeded">
/// Summary: Returns an Clean.Architecture.SharedKernel.Models.APIResult indicating a successful identity operation.
/// Returns: An Clean.Architecture.SharedKernel.Models.APIResult indicating a successful operation.
/// </param>
/// <param name="Errors">
/// Summary: An System.Collections.Generic.IEnumerable`1 of Clean.Architecture.SharedKernel.Models.APIError instances containing errors that occurred during the identity operation.
/// Value: An System.Collections.Generic.IEnumerable`1 of Clean.Architecture.SharedKernel.Models.APIError instances.
/// </param>
public record APIResult(bool Succeeded, object @object, IEnumerable<APIError>? Errors = default)
{
  public static APIResult Success { get; } = default!;

  //public static APIResult Failed(params APIError[] errors);

  // Summary:
  //     Converts the value of the current Clean.Architecture.SharedKernel.Models.APIResult
  //     object to its equivalent string representation.
  //
  // Returns:
  //     A string representation of the current Clean.Architecture.SharedKernel.Models.APIResult
  //     object.
  //
  // Remarks:
  //     If the operation was successful the ToString() will return "Succeeded" otherwise
  //     it returned "Failed : " followed by a comma delimited list of error codes from
  //     its Clean.Architecture.SharedKernel.Models.APIResult.Errors collection, if any.
  public override string ToString()
  {
    if (Succeeded)
    {
      return $"{Succeeded}";
    }

    if (Errors != null && Errors.Any())
    {
      return $"Failed: {string.Join(',', Errors.Select(e => e.Code))}";
    }

    return string.Empty;
  }
}
