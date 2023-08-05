using System.ComponentModel;
using System.Data;

using Ardalis.Specification;

using Clean.Architecture.Domain.ValueObjects;

using Microsoft.Data.SqlClient;

namespace Clean.Architecture.Infrastructure.Extensions;

public static class SQLServerExtensions
{
  public static DataTable GenerateDataTable<T>(IEnumerable<T> data, string tableName, SqlBulkCopy bulkCopy)
  {
    var valueObjectsNameSpace = typeof(TransactionEnvironment).Namespace ?? "Clean.Architecture.Domain.ValueObjects";

    bulkCopy.BulkCopyTimeout = 300;

    bulkCopy.BatchSize = 5000;

    bulkCopy.DestinationTableName = tableName;

    var table = new DataTable();

    var props = TypeDescriptor.GetProperties(typeof(T))
                               //Dirty hack to make sure we only have system data types 
                               //i.e. filter out the relationships/collections
                               .Cast<PropertyDescriptor>()
                               .Where(propertyInfo => propertyInfo.PropertyType.Namespace!.Equals("System") || propertyInfo.PropertyType.Namespace.Equals(valueObjectsNameSpace))
                               .ToArray();

    var propsList = new List<PropertyDescriptor>();

    var valueObjectPropsList = new List<PropertyDescriptor>();

    foreach (var propertyInfo in props)
    {
      if (propertyInfo.PropertyType.Namespace!.Equals(valueObjectsNameSpace))
      {
        valueObjectPropsList.Add(propertyInfo);

        var valueObjectProps = propertyInfo.GetChildProperties()
                                   //Dirty hack to make sure we only have system data types 
                                   //i.e. filter out the relationships/collections
                                   .Cast<PropertyDescriptor>()
                                   .Where(voPropertyInfo => voPropertyInfo.PropertyType.Namespace!.Equals("System"))
                                   .ToArray();

        propsList.AddRange(valueObjectProps.ToList());

        foreach (var valueObjectPropertyInfo in valueObjectProps)
        {
          bulkCopy.ColumnMappings.Add(propertyInfo.Name + "_" + valueObjectPropertyInfo.Name, propertyInfo.Name + "_" + valueObjectPropertyInfo.Name);

          table.Columns.Add(propertyInfo.Name + "_" + valueObjectPropertyInfo.Name, Nullable.GetUnderlyingType(valueObjectPropertyInfo.PropertyType) ?? valueObjectPropertyInfo.PropertyType);
        }
      }
      else
      {
        propsList.Add(propertyInfo);

        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);

        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
      }
    }

    props = propsList.ToArray();

    var values = new object[props.Length];

    foreach (var item in data)
    {
      for (var i = 0; i < values.Length; i++)
      {
        if (props[i].ComponentType.Namespace != null && props[i].ComponentType.Namespace!.Equals(valueObjectsNameSpace))
        {
          object? valueObject = valueObjectPropsList.Where(x => x.PropertyType.Name.Equals(props[i].ComponentType.Name))?.FirstOrDefault()?.GetValue(item);

          values[i] = props[i].GetValue(valueObject)!;
        }
        else
        {
          values[i] = props[i].GetValue(item)!;
        }
      }

      table.Rows.Add(values);
    }

    return table;
  }
}
