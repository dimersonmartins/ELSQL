using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Vendor.Data;

namespace Vendor.Vmodel
{
    public static class ModelSelectExtension 
    {
        public static List<ExpandoObject> Select<T>(this T model, string[] values) where T : class, new()
        {

            List<ExpandoObject> objList = new List<ExpandoObject>();
            DataSet dataSet = new VMySql().QueryBuilder(@$"SELECT {string.Join(',', values)} 
                                                           FROM {model.GetType().GetProperty("table").GetValue(model, null).ToString().ToLower()}");
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var table = dataSet.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    int countCol = 0;
                    dynamic exo = new ExpandoObject();
                    foreach (DataColumn col in table.Columns)
                    {
                        ((IDictionary<String, Object>)exo).Add(col.ColumnName, table.Rows[i][countCol]);
                        countCol++;
                    }
                    objList.Add(exo);
                }
            }
            return objList;
        }

        public static List<ExpandoObject> Select<T>(this T model, string[] values, string field, string value) where T : class, new()
        {
            List<ExpandoObject> objList = new List<ExpandoObject>();
            DataSet dataSet = new VMySql().QueryBuilder(@$"SELECT {string.Join(',', values)} 
                                                           FROM {model.GetType().GetProperty("table").GetValue(model, null).ToString().ToLower()} 
                                                           WHERE {field} = '{value}'");


            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var table = dataSet.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    int countCol = 0;
                    dynamic exo = new ExpandoObject();
                    foreach (DataColumn col in table.Columns)
                    {
                        ((IDictionary<String, Object>)exo).Add(col.ColumnName, table.Rows[i][countCol]);
                        countCol++;
                    }
                    objList.Add(exo);
                }
            }
            return objList;
        }

        public static List<ExpandoObject> Select<T>(this T model, string[] values, string innerJoinTable, string firstField, string SecondField) where T : class, new()
        {
            List<ExpandoObject> objList = new List<ExpandoObject>();
            DataSet dataSet = new VMySql()
                .QueryBuilder(@$"SELECT {string.Join(',', values)} 
                                  FROM {model.GetType().GetProperty("table").GetValue(model, null).ToString().ToLower()}
                                  INNER JOIN {innerJoinTable} ON {firstField} = {SecondField}");

            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var table = dataSet.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    int countCol = 0;
                    dynamic exo = new ExpandoObject();
                    foreach (DataColumn col in table.Columns)
                    {
                        ((IDictionary<String, Object>)exo).Add(col.ColumnName, table.Rows[i][countCol]);
                        countCol++;
                    }
                    objList.Add(exo);
                }
            }
            return objList;
        }
    }
}
