using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Reflection;
using System.Text;
using Vendor.Data;
using Vendor.VLog;

namespace Vendor.Vmodel
{
    public static class ModelSaveExtension
    {
        public static T Save<T>(this T model) where T : class, new()
        {


            PropertyInfo proptable = model.GetType().GetProperty("table", BindingFlags.NonPublic | BindingFlags.Instance);
            string table = proptable.GetValue(model, null).ToString().ToLower();

            try
            {

                T obj = new T();
                StringBuilder queryParameters = new StringBuilder();
                StringBuilder queryValues = new StringBuilder();
                MySqlCommand mySqlCommand = new MySqlCommand();
                PropertyInfo propertyInfo = null;

               
              

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo mds = model.GetType().GetProperty(prop.Name);
                        propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(mds.GetValue(model, null), propertyInfo.PropertyType), null);


                        if (prop.Name == "table")
                        {
                            table = mds.GetValue(model, null).ToString().ToLower();
                            continue;
                        }

                        if (prop.Name == "id")
                        {
                            continue;
                        }

                        if (propertyInfo.PropertyType.BaseType == Type.GetType(""))
                        {
                            mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", $"'{mds.GetValue(model, null)}'");
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", mds.GetValue(model, null));
                        }


                        queryParameters.Append($"{prop.Name},");
                        queryValues.Append($"@{prop.Name},");
                    }
                    catch
                    {
                        continue;
                    }

                }

                string pparams = queryParameters.ToString().TrimEnd(',');
                string values = queryValues.ToString().TrimEnd(',');
                DataSet ds = new VMySql().QueryBuilder(@$"INSERT INTO {table.ToLower()} 
                                                                        ({pparams}) VALUES({values}); 
                                                                        SELECT LAST_INSERT_ID()", mySqlCommand);

                propertyInfo = obj.GetType().GetProperty("id");
                propertyInfo.SetValue(obj, Convert.ChangeType(ds.Tables[0].Rows[0].Field<UInt64>("LAST_INSERT_ID()"), propertyInfo.PropertyType), null);

                return obj;
            }
            catch (Exception ex)
            {
                HttpLog.DatabaseGravarLog(ex.Message, table.ToLower());
                return null;
            }
        }



    }
}
