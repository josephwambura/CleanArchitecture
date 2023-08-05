using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.ValueObjects;

public class TextMessage : ValueObject
{
  public string Recipient { get; private set; } = default!;
  public string Body { get; private set; } = default!;
  public bool SecurityCritical { get; private set; }

  public TextMessage() { }

  public TextMessage(string recipient, string body, bool securityCritical)
  {
    Recipient = recipient;
    Body = body;
    SecurityCritical = securityCritical;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    // Using a yield return statement to return each element one at a time
    yield return Recipient ?? string.Empty;
    yield return Body ?? string.Empty;
    yield return SecurityCritical;
  }

  public override string ToString()
  {
    return !SecurityCritical ? $"TextMessage: {Body}" : "TextMessage";
  }
}
