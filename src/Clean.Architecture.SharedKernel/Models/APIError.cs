namespace Clean.Architecture.SharedKernel.Models;
/// <summary>
/// Encapsulates an error from the API.
/// </summary>
/// <param name="Code">
/// Summary: Gets or sets the code for this error.
/// Value: The code for this error.
/// </param>
/// <param name="Description">
/// Summary: Gets or sets the description for this error.
///  Value: The description for this error.
///  </param>
public record APIError(string Code, string Description);
