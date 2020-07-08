using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using System.Text;
using Vendor.Data;
using Vendor.VLog;

namespace Vendor.Vmodel
{
    public static class ModelUpdateExtension
    {
        public static T Update<T>(this T model, object parametersToUpdate, object parametersToWhere) where T : class, new()
        {

            PropertyInfo proptable = model.GetType().GetProperty("table", BindingFlags.NonPublic | BindingFlags.Instance);
            string table = proptable.GetValue(model, null).ToString().ToLower();

            try
            {

                StringBuilder queryParameters = new StringBuilder();
                StringBuilder whereParameters = new StringBuilder();
                MySqlCommand mySqlCommand = new MySqlCommand();

               

                //adicionando condições where
                foreach (var prop in parametersToWhere.GetType().GetProperties())
                {
                    if (prop.PropertyType.BaseType == Type.GetType(""))
                    {
                        mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", $"'{prop.GetValue(parametersToWhere, null)}'");
                    }
                    else
                    {
                        mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(parametersToWhere, null));
                    }

                    whereParameters.Append($"{prop.Name} = @{prop.Name} AND ");
                }


                //adicionando campos a serem atualizados
                foreach (var prop in parametersToUpdate.GetType().GetProperties())
                {
                    if (prop.PropertyType.BaseType == Type.GetType(""))
                    {
                        mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", $"'{prop.GetValue(parametersToUpdate, null)}'");
                    }
                    else
                    {
                        mySqlCommand.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(parametersToUpdate, null));
                    }

                    queryParameters.Append($"{prop.Name} = @{prop.Name}, ");
                }


                string values = queryParameters.ToString().TrimEnd(", ".ToCharArray());
                string where = whereParameters.ToString().TrimEnd("AND ".ToCharArray());
                new VMySql().QueryBuilder($"UPDATE {table} SET {values} WHERE {where}", mySqlCommand);

                return new T();
            }
            catch (Exception ex)
            {
                HttpLog.DatabaseGravarLog(ex.Message, table.ToLower());
                return new T();
            }
        }
    }
}
