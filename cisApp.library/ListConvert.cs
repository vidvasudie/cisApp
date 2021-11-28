using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace cisApp.library
{
    public static class ListConvert
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static T Tomaplist<T>(this Object r) where T : new()
        {
            T item = new T();
            if (r != null)
            {
                IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), r.GetType().GetProperty(property.Name).GetValue(r, null).ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {
                        try
                        {
                            object value = r.GetType().GetProperty(property.Name).GetValue(r, null);
                            if (value == DBNull.Value)
                                value = null;
                            property.SetValue(item, value, null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return item;
        }
        public static List<T> ToList_list<T>(this IEnumerable<object> obj) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (object r in obj)
            {
                T item = new T();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(System.DayOfWeek))
                    {
                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), r.GetType().GetProperty(property.Name).GetValue(r, null).ToString());
                        property.SetValue(item, day, null);
                    }
                    else
                    {
                        if (r.GetType().GetProperty(property.Name) != null)
                        {
                            object value = r.GetType().GetProperty(property.Name).GetValue(r, null);
                            if (value == DBNull.Value)
                                value = null;
                            property.SetValue(item, value, null);
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                try
                {
                    var item = CreateItemFromRow<T>((DataRow)row, properties);
                    result.Add(item);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return result;
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        object value = row[property.Name];
                        if (value == DBNull.Value)
                            value = null;
                        if (value != null)
                        {
                            if (value.GetType() == typeof(System.TimeSpan))
                            {
                                property.SetValue(item, Convert.ToString(value), null);
                            }
                            else
                            {
                                property.SetValue(item, value, null);
                            }
                        }
                        else
                        {
                            property.SetValue(item, value, null);
                        }
                    }
                }
            }
            return item;
        }
        public static DataTable ToDataTable<T>(this IList<T> data, string tableName)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            table.TableName = tableName;
            return table;
        }
    }
}
