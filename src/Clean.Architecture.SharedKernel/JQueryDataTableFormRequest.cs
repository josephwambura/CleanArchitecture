using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Ardalis.SmartEnum;

namespace Clean.Architecture.SharedKernel;

public class JQueryDataTablesFormRequest
{
  [JsonPropertyName("draw")]
  public int Draw { get; set; }

  [JsonPropertyName("columns")]
  public List<Column>? Columns { get; set; }

  [JsonPropertyName("order")]
  public List<Order>? Order { get; set; }

  [JsonPropertyName("start")]
  public int Start { get; set; }

  [JsonPropertyName("length")]
  public int Length { get; set; }

  [JsonPropertyName("search")]
  public Search? Search { get; set; }
}

public class Column
{
  [JsonPropertyName("data")]
  public string? Data { get; set; }

  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("searchable")]
  public bool? Searchable { get; set; }

  [JsonPropertyName("orderable")]
  public bool? Orderable { get; set; }

  [JsonPropertyName("search")]
  public Search? Search { get; set; }
}

public class Order
{
  [JsonPropertyName("column")]
  public int Column { get; set; }

  [JsonPropertyName("dir")]
  public string? Dir { get; set; } = "desc";
}

public class Search
{
  [JsonPropertyName("value")]
  public string? Value { get; set; }

  [JsonPropertyName("regex")]
  public bool? Regex { get; set; }
}

public class JqueryResponse<T> where T : class
{
  // Properties are capitalised as C# conventions
  [JsonPropertyName("draw")]
  public int Draw { get; set; } // Draw counter
  [JsonPropertyName("recordsTotal")]
  public int RecordsTotal { get; set; } // Total number of records int the data set
  [JsonPropertyName("recordsFiltered")]
  public int RecordsFiltered { get; set; } // Total number of records int the data set after filtering
  [JsonPropertyName("data")]
  public List<T> Data { get; set; } // The data to be displayed int the table
  [JsonPropertyName("error")]
  public string? Error { get; set; } // Optional error message

  // Default constructor that sets the properties to default values
  public JqueryResponse()
  {
    Draw = default!;
    RecordsTotal = default!;
    RecordsFiltered = default!;
    Data = new List<T>();
  }

  // Method that returns a JSON representation of the object
  // Using System.Text.Json
  public string ToJson()
  {
    return JsonSerializer.Serialize(this);
  }

  // Method that queries a subset of data for each page
  public JqueryResponse<T> GetData(int startRowIndex, int maximumRows)
  {
    // Your logic here to query only the data within the range
    // For example, using LINQ:
    Data = Data.Skip(startRowIndex).Take(maximumRows).ToList();
    return this;
  }
}


public class JsonPropertyNameSmartEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : SmartEnum<TEnum>
{
  public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    //deserialize JSON into a SmartEnum value
    var name = reader.GetString();
    return SmartEnum<TEnum>.FromName(name);
  }

  public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
  {
    //serialize a SmartEnum value into JSON
    var name = value.Name;
    var attr = value.GetType().GetField(name)?.GetCustomAttribute<JsonPropertyNameAttribute>();
    if (attr != null && attr.Name != null)
    {
      name = attr.Name;
    }
    writer.WriteStringValue(name);
  }
}

