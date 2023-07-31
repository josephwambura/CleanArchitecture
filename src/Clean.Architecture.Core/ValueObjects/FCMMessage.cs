using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ValueObjects;

public class FCMMessage : ValueObject
{
  public string? Subject { get; private set; }
  public string? Body { get; private set; }
  public bool SecurityCritical { get; private set; }

  public FCMMessage() { }

  public FCMMessage(string subject, string body, bool securityCritical)
  {
    Subject = subject;
    Body = body;
    SecurityCritical = securityCritical;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    // Using a yield return statement to return each element one at a time
    yield return Subject ?? string.Empty;
    yield return Body ?? string.Empty;
    yield return SecurityCritical;
  }

  public override string ToString()
  {
    return !SecurityCritical ? $"FCMMessage: {Body}" : "FCMMessage";
  }
}
