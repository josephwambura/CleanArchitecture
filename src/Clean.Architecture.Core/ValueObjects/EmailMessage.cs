using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ValueObjects;

public class EmailMessage : ValueObject
{
    public string To { get; private set; } = default!;
    public string? CC { get; private set; }
    public string? Subject { get; private set; }
    public string Body { get; private set; } = default!;
    public string? Attachments { get; private set; }
    public bool IsBodyHtml { get; private set; }
    public bool SecurityCritical { get; private set; }

    public EmailMessage() { }

    public EmailMessage(string to, string? cc, string subject, string body, string? attachments, bool isBodyHtml, bool securityCritical)
    {
        To = to;
        CC = cc;
        Subject = subject;
        Body = body;
        Attachments = attachments;
        IsBodyHtml = isBodyHtml;
        SecurityCritical = securityCritical;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return To;
        yield return CC ?? string.Empty;
        yield return Subject ?? string.Empty;
        yield return Body;
        yield return Attachments ?? string.Empty;
        yield return IsBodyHtml;
        yield return SecurityCritical;
    }

    public override string ToString()
    {
        return !SecurityCritical && !IsBodyHtml ? $"EmailMessage: {Body}" : $"EmailMessage: {Subject}";
    }
}
