using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace codeCleaner.Helpers {
    public static class ListExtensions {
        public static DataTable ToDataTable<T>(this IList<T> data) {
            var props = typeof(T).GetProperties().Where(pi => pi.GetCustomAttributes(typeof(SkipPropertyAttribute), true).Length == 0).ToList();
            DataTable table = new DataTable();

            foreach (var prop in props)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data) {
                DataRow row = table.NewRow();

                //foreach (var prop in props)
                //    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                foreach (DataColumn column in table.Columns)
                    row[column] = item.GetType().GetProperty(column.ColumnName).GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }
            return table;
        }
        public class SkipPropertyAttribute : Attribute
        {
        }
    }
}
