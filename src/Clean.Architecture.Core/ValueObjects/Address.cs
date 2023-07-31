using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ValueObjects;

public class Address : ValueObject
{
  public String? PhoneNumber { get; private set; }
  public String? Email { get; private set; }
  public String? Street { get; private set; }
  public String? City { get; private set; }
  public String? State { get; private set; }
  public String? Postal { get; private set; }
  public String? Country { get; private set; }
  public String? ZipCode { get; private set; }

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
    yield return PhoneNumber ?? String.Empty;
    yield return Email ?? String.Empty;
    yield return Street ?? String.Empty;
    yield return City ?? String.Empty;
    yield return State ?? String.Empty;
    yield return Postal ?? String.Empty;
    yield return Country ?? String.Empty;
    yield return ZipCode ?? String.Empty;
  }

  public override string ToString()
  {
    return $"Address: {Street}, {City}, {Country}";
  }
}
