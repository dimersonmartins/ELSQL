using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Vendor.Vmodel
{
    public class DynamicModel
    {
        private DataSet _dataSet;

        public DynamicModel(DataSet dataSet)
        {
            _dataSet = dataSet;
        }

        public List<ExpandoObject> Monthed()
        {
            List<ExpandoObject> objList = new List<ExpandoObject>();
            if (_dataSet != null && _dataSet.Tables != null && _dataSet.Tables.Count > 0 && _dataSet.Tables[0].Rows.Count > 0)
            {
                var table = _dataSet.Tables[0];
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
