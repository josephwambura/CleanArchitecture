﻿using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.SharedKernel.Utils;

public static class Utility
{
  /// <summary>
  /// Get a property info object from Item class filtering by property name.
  /// </summary>
  /// <param name="name">name of the property</param>
  /// <returns>property info object</returns>
  public static PropertyInfo? GetProperty<T>(string name)
  {
    var properties = typeof(T).GetProperties();
    PropertyInfo? prop = null;
    foreach (var item in properties)
    {
      if (item.Name.ToLower().Equals(name.ToLower()))
      {
        prop = item;
        break;
      }
    }
    return prop;
  }

  public static string AddTypedValue(string actualValue, string typedValue, string format)
  {
    bool stringMode = false;

    decimal.TryParse(actualValue, out decimal amnt);
    if (actualValue.EndsWith("-") || amnt == 0) stringMode = true;
    else
    {
      decimal.TryParse(typedValue, out amnt);
      if (amnt == 0) stringMode = true;
    }

    string dc = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

    if (typedValue == "." || typedValue == ",")
    {
      actualValue += dc;
      return actualValue;
    }

    if (stringMode)
      return actualValue + typedValue;

    string fmt = "0";
    string rfmt = format;

    if (actualValue.Contains(dc))
    {
      int dCount = (actualValue.Length - actualValue.IndexOf(dc));

      fmt = "0.".PadRight(dCount + 2, '0');
      rfmt = format.PadRight(dCount + rfmt.Length, '0');
    }

    string amount = string.IsNullOrEmpty(actualValue) ? "0" :
        Convert.ToDouble(actualValue).ToString(fmt);
    if (amount.Contains(dc))
      amount = amount[..^1];

    var dbl = Convert.ToDouble(amount + typedValue);
    return (dbl).ToString(rfmt);
  }

  public static bool IsValidFile(string fileName)
  {
    fileName = fileName.Trim();
    if (fileName == "." || !fileName.Contains('.')) return false;
    var result = false;
    try
    {
      new FileInfo(fileName);
      result = true;
    }
    catch (ArgumentException)
    {
    }
    catch (PathTooLongException)
    {
    }
    catch (NotSupportedException)
    {
    }
    return result;
  }

  public static bool IsNumericType(Type type)
  {
    if (type == null)
    {
      return false;
    }

    switch (Type.GetTypeCode(type))
    {
      case TypeCode.Byte:
      case TypeCode.Decimal:
      case TypeCode.Double:
      case TypeCode.Int16:
      case TypeCode.Int32:
      case TypeCode.Int64:
      case TypeCode.SByte:
      case TypeCode.Single:
      case TypeCode.UInt16:
      case TypeCode.UInt32:
      case TypeCode.UInt64:
        return true;
      case TypeCode.Object:
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          return IsNumericType(Nullable.GetUnderlyingType(type)!);
        }
        return false;
    }
    return false;
  }

  public static int GenerateCheckDigit(string idWithoutCheckdigit)
  {
    const string validChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVYWXZ_";
    idWithoutCheckdigit = idWithoutCheckdigit.Trim().ToUpper();

    var sum = 0;

    for (var i = 0; i < idWithoutCheckdigit.Length; i++)
    {
      var ch = idWithoutCheckdigit[idWithoutCheckdigit.Length - i - 1];
      if (validChars.IndexOf(ch) == -1)
        throw new Exception(ch + " is an invalid character");
      var digit = ch - 48;
      int weight = i % 2 == 0 ? (2 * digit) - digit / 5 * 9 : digit;
      sum += weight;
    }
    sum = Math.Abs(sum) + 10;
    return (10 - (sum % 10)) % 10;
  }

  public static bool ValidateCheckDigit(string id)
  {
    if (id.Length < 2) return false;
    var cd = Convert.ToInt32(id.Last().ToString());
    return cd == GenerateCheckDigit(id.Remove(id.Length - 1));
  }

  public static string RandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
  {
    if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
    if (String.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

    const int byteSize = 0x100;
    var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
    if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowed Chars may contain no more than {0} characters.", byteSize));

    //using (var rng = new RNGCryptoServiceProvider())
    using (var rng = RandomNumberGenerator.Create())
    {
      var result = new StringBuilder();
      var buf = new byte[128];
      while (result.Length < length)
      {
        rng.GetBytes(buf);
        for (var i = 0; i < buf.Length && result.Length < length; ++i)
        {
          var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
          if (outOfRangeStart <= buf[i]) continue;
          result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
        }
      }
      return result.ToString();
    }
  }

  public static string GetDateBasedUniqueString()
  {
    var date = DateTime.Now;
    return string.Format("{0}{1:00}{2:00}{3:00}{4:00}{5:000}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Millisecond);

  }

  public static string GenerateOrderTransactionNumber(string requestId)
  {
    if (!string.IsNullOrWhiteSpace(requestId))
    {
      var random = new Random(requestId.GetHashCode());

      int recordId = random.Next();

      int next = Random.Shared.Next(99);

      return $"{recordId}{next}".PadRight(12, '0'); ;
    }
    return requestId;
  }

  public static string GetUniqueKey(string trailingText, int maxSize)
  {
    var chars = new char[62];

    var a = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    chars = a.ToCharArray();
    byte[] data = new byte[1];

    var crypto = RandomNumberGenerator.Create();
    crypto.GetNonZeroBytes(data);
    int size = maxSize;
    data = new byte[size];
    crypto.GetNonZeroBytes(data);

    var result = new StringBuilder(size);

    result.Append(trailingText);

    foreach (byte b in data)
    {
      result.Append(chars[b % (chars.Length - 1)]);
    }

    return result.ToString();
  }

  public static string GetUniqueNumberKey(string trailingText, int maxSize)
  {
    var chars = new char[62];

    var a = "1234567890";

    chars = a.ToCharArray();
    byte[] data = new byte[1];

    var crypto = RandomNumberGenerator.Create();
    crypto.GetNonZeroBytes(data);
    int size = maxSize;
    data = new byte[size];
    crypto.GetNonZeroBytes(data);

    var result = new StringBuilder(size);

    result.Append(trailingText);

    foreach (byte b in data)
    {
      result.Append(chars[b % (chars.Length - 1)]);
    }

    return result.ToString();
  }

  public static string DbTableName<T>() where T : class
  {
    if (typeof(T).Name == "ApplicationUser")
      return "AspNetUsers";

    return $"{DefaultSettings.Instance.TablePrefix}{typeof(T).Name.Pluralize()}";
  }
}