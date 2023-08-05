using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.ValueObjects;

public class Address : ValueObject
{
  public string? PhoneNumber { get; private set; }
  public string? Email { get; private set; }
  public string? Street { get; private set; }
  public string? City { get; private set; }
  public string? State { get; private set; }
  public string? Postal { get; private set; }
  public string? Country { get; private set; }
  public string? ZipCode { get; private set; }

  public Address() { }

  public Address(string phoneNumber, string email, string postal, string street, string city, string state, string country, string zipcode)
  {
    PhoneNumber = phoneNumber;
    Email = email;
    Street = street;
    City = city;
    State = state;
    Postal = postal;
    Country = country;
    ZipCode = zipcode;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    // Using a yield return statement to return each element one at a time
    yield return PhoneNumber ?? string.Empty;
    yield return Email ?? string.Empty;
    yield return Street ?? string.Empty;
    yield return City ?? string.Empty;
    yield return State ?? string.Empty;
    yield return Postal ?? string.Empty;
    yield return Country ?? string.Empty;
    yield return ZipCode ?? string.Empty;
  }

  public override string ToString()
  {
    return $"Address: {Street}, {City}, {Country}";
  }
}
