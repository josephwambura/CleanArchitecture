using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Clean.Architecture.SharedKernel.Utils;

// Define an extension method to get the description of an enum value
public static class EnumExtensions
{
  // Use a concurrent dictionary to store the descriptions
  private static readonly ConcurrentDictionary<Enum, string> _cache = new ConcurrentDictionary<Enum, string>();

  public static string? GetDescription(this Enum value)
  {
    // Check if the description is already cached
    if (_cache.TryGetValue(value, out string? description))
    {
      return description;
    }

    // Use a lock to avoid race conditions
    lock (_cache)
    {
      // Check again in case another thread added the value
      if (_cache.TryGetValue(value, out description))
      {
        return description;
      }

      // Get the field info for the enum value
      FieldInfo? fi = value.GetType().GetField(nameof(value));

      // Get the Description attribute, if any
      DescriptionAttribute? attribute = fi?.GetCustomAttribute<DescriptionAttribute>();

      // Return the description or the enum value as a string
      description = attribute != null ? attribute.Description : value.ToString();

      // Add the description to the cache
      _cache.TryAdd(value, description);

      return description;
    }
  }

  // Use a concurrent dictionary to store the descriptions
  private static readonly ConcurrentDictionary<(Type, string), Enum> _cache2 = new ConcurrentDictionary<(Type, string), Enum>();

  // var panda = "Giant Panda".GetValueFromDescription<Animal>();
  public static T GetValueFromDescription<T>(this string description) where T : Enum
  {
    // Check if the description is already cached
    if (_cache2.TryGetValue((typeof(T), description), out var value))
    {
      return (T)value;
    }

    // Use a lock to avoid race conditions
    lock (_cache2)
    {
      // Check again in case another thread added the value
      if (_cache2.TryGetValue((typeof(T), description), out value))
      {
        return (T)value;
      }

      // Loop through the enum fields and find the matching description
      foreach (var field in typeof(T).GetFields())
      {
        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
        {
          if (attribute.Description == description)
          {
            value = (T?)field.GetValue(null);
            break;
          }
        }
        else
        {
          if (field.Name == description)
          {
            value = (T?)field.GetValue(null);
            break;
          }
        }
      }

      // Throw an exception or return a default value if not found
      if (value == null)
      {
        throw new ArgumentException("Not found.", nameof(description));
        // Or return default(T);
      }

      // Add the value to the cache
      _cache2.TryAdd((typeof(T), description), value);

      return (T)value;
    }
  }
}
