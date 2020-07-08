using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;
using Vendor.Data;
using Vendor.VLog;

namespace Vendor.Vmodel
{
    public static class ModelQueryBuilderExtension
    {
        public static List<ExpandoObject> QueryBuilderDynamicModel<T>(this T model, string pQuery, object pObject) where T : class, new()
        {

            MySqlCommand mySqlCommand = new MySqlCommand();

            foreach (var prop in pObject.GetType().GetProperties())
            {
                if (prop.PropertyType.BaseType == Type.GetType(""))
                {
                    mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", $"'{prop.GetValue(pObject, null)}'");
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(pObject, null));
                }

            }

            DataSet dataSet = new VMySql().QueryBuilder(pQuery, mySqlCommand);

            DynamicModel dynamicModel = new DynamicModel(dataSet);

            return dynamicModel.Monthed();
        }

        public static List<T> QueryBuilderToModel<T>(this T model, string pQuery, object pObject) where T : class, new()
        {

            MySqlCommand mySqlCommand = new MySqlCommand();

            foreach (var prop in pObject.GetType().GetProperties())
            {
                if (prop.PropertyType.BaseType == Type.GetType(""))
                {
                    mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", $"'{prop.GetValue(pObject, null)}'");
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(pObject, null));
                }

            }

            DataSet dataSet = new VMySql().QueryBuilder(pQuery, mySqlCommand);

            if (dataSet.Tables == null || dataSet.Tables.Count == 0) return null;

            DataTable dataTable = dataSet.Tables[0];
            try
            {
                List<T> list = new List<T>();

                foreach (var row in dataTable.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in model.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch(Exception ex)
                        {
                            HttpLog.DatabaseGravarLog(ex.Message, "Instacia => QueryBuilderToModel");
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                HttpLog.DatabaseGravarLog(ex.Message, "Agro1");
                return null;
            }
        }

    }
}
