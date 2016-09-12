using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public static class IEnumerableExtensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                var acol = prop.Attributes[typeof(NotMappedAttribute)];
                if(acol==null)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }                                                                        
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    var acol = prop.Attributes[typeof(NotMappedAttribute)];
                    if (acol == null)
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;                        
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}