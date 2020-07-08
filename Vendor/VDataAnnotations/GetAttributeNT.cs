using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vendor.VMigrations.Struct;

namespace Vendor.VDataAnnotations
{
    public static class GetAttributeNT
    {
        private static List<Attribute> attributes = new List<Attribute>();
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            try
            {
                var attrType = typeof(T);
                var property = instance.GetType().GetProperty(propertyName);
                return (T)property.GetCustomAttributes(attrType, false).First();
            }
            catch
            {
                return null;
            }
           
        }


        public static List<Field> GetAttributeValue<T>(this T model) where T : class, new()
        {
            try
            {
                PropertyInfo proptable = model.GetType().GetProperty("table", BindingFlags.NonPublic | BindingFlags.Instance);
                string table = proptable.GetValue(model, null).ToString().ToLower();
                Schema schema = new Schema();
                var attrs = model.GetType().GetCustomAttributes();

                foreach (var pModel in model.GetType().GetProperties())
                {
                    Field field = new Field();
                    field.name = pModel.Name;

                    foreach (var item in pModel.GetCustomAttributes())
                    {
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            if (prop.Name == "TypeId")
                            {
                                continue;
                            }

                            if (!item.GetType().Name.Contains("IDENTITY_PRIMARY_KEY")
                              && !item.GetType().Name.Contains("MY_CONNECTION")
                              && !item.GetType().Name.Contains("AUTOINCREMENT")
                              && !item.GetType().Name.Contains("NOT_NULL")
                              && !item.GetType().Name.Contains("INDEX"))
                            {
                                field.type = prop.GetValue(item, null).ToString();
                            }

                            if (item.GetType().Name.Contains("IDENTITY_PRIMARY_KEY") || item.GetType().Name.Contains("AUTOINCREMENT"))
                            {
                                field.options = prop.GetValue(item, null).ToString();
                            }

                            if (item.GetType().Name.Replace("Attribute", "") == "NOT_NULL")
                            {
                                field.nullable = true;
                            }

                            if (item.GetType().Name.Replace("Attribute", "") == "INDEX")
                            {
                                field.create_index = prop.GetValue(item, null).ToString();
                            }
                        }
                    }

                    schema.Add(field);

                }

                return schema.fields;
            }
            catch
            {
                return null;
            }

        }
    }
}
