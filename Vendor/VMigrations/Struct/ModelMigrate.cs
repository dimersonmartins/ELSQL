using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vendor.VMigrations.Struct
{
    public class ModelMigrate
    {
        public List<object> ModelsToMigrate = new List<object>();
        private List<Schema> SchemasRegisters = new List<Schema>();
        private string TableName { get; set; }
        private string Procedure { get; set; }
        private string Columns { get; set; }
        private string Create_Indexs { get; set; }
        private string ColumnName { get; set; }
        private string Properties { get; set; }
        private bool IsUpdate { get; set; }

        public void GenerateSchema()
        {
            try
            {
                foreach (var model in ModelsToMigrate)
                {
                    PropertyInfo proptable = model.GetType().GetProperty("table", BindingFlags.NonPublic | BindingFlags.Instance);
                    string table = proptable.GetValue(model, null).ToString().ToLower();

                    Schema schema = new Schema();
                    foreach (var pModel in model.GetType().GetProperties())
                    {
                        Field field = new Field();
                        field.table = table;
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
                    SchemasRegisters.Add(schema);
                }
            }
            catch { }
        }

        /// <summary>
        /// Cria as tabelas na base de dados
        /// </summary>
        public void Create()
        {
            int countTablesCreate = 0;
            IsUpdate = false;
            for (int i = 0; i < SchemasRegisters.Count; i++)
            {
                try
                {
                    TableName = SchemasRegisters[i].fields[0].table;

                    Properties = string.Empty;

                    Properties += MysqlConstants.CREATE_TABLE + TableName + MysqlConstants.START_PARENTHESIS;

                    MouthedLine(SchemasRegisters[i].fields);

                    Properties += Columns;

                    //base.QueryBuilder(Properties);
                    InsertMigrate();

                    if (!string.IsNullOrWhiteSpace(Create_Indexs))
                    {
                        CreateIndexs(SchemasRegisters[i].fields);
                    }              
                    countTablesCreate++;
                }
                catch
                {
                    continue;
                }
            }
        }

        private void CreateIndexs(List<Field> schemas)
        {
            foreach (var reg in schemas)
            {
                string NULLABLE = string.Empty;

                if (!string.IsNullOrWhiteSpace(reg.create_index))
                {
                   // base.QueryBuilder(@"CREATE INDEX " + reg.create_index.ToLower() + " ON " + TableName + " (" + reg.name.ToLower() + ")");
                }
            }
        }

        private void InsertMigrate()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (!IsUpdate)
            {
                if (TableName != MysqlConstants.MIGRATIONS)
                {
                  //  base.QueryBuilder(MysqlConstants.INSERT_MIGRATIONS
                  //+ MysqlConstants.SINGLE_QUOTES
                  //+ TableName
                  //+ MysqlConstants.SINGLE_QUOTES
                  //+ MysqlConstants.COMMA
                  //+ MysqlConstants.SPACE
                  //+ MysqlConstants.SINGLE_QUOTES
                  //+ date
                  //+ MysqlConstants.SINGLE_QUOTES
                  //+ MysqlConstants.COMMA
                  //+ MysqlConstants.SPACE
                  //+ MysqlConstants.SINGLE_QUOTES
                  //+ date
                  //+ MysqlConstants.SINGLE_QUOTES
                  //+ MysqlConstants.END_PARENTHESIS);
                }
            }
            else
            {
                if (TableName != MysqlConstants.MIGRATIONS)
                {
                    //base.QueryBuilder(MysqlConstants.START_UPDATE_MIGRATIONS
                    //     + MysqlConstants.SET
                    //     + MysqlConstants.SPACE
                    //     + MysqlConstants.UPDATED_AT
                    //     + MysqlConstants.SPACE
                    //     + MysqlConstants.EQUALS
                    //     + MysqlConstants.SPACE
                    //     + MysqlConstants.SINGLE_QUOTES
                    //     + date
                    //     + MysqlConstants.SINGLE_QUOTES
                    //     + MysqlConstants.SPACE
                    //     + MysqlConstants.END_UPDATE_MIGRATIONS
                    //     + MysqlConstants.SINGLE_QUOTES
                    //     + TableName
                    //     + MysqlConstants.SINGLE_QUOTES);
                }
            }
        }


        private void MouthedLine(List<Field> schemas)
        {
            Columns = string.Empty;

            foreach (var reg in schemas)
            {
                ColumnName = reg.name.ToLower();

                //if (HasColumn())
                //{
                //    continue;
                //}

                string NULLABLE = string.Empty;

                if (!reg.nullable)
                {
                    NULLABLE = MysqlConstants.NOTNULL;
                }


                if (!string.IsNullOrWhiteSpace(reg.create_index))
                {
                    Create_Indexs = reg.create_index + MysqlConstants.COMMA;
                }

                if (ColumnName == MysqlConstants.ID)
                {
                    Columns += ColumnName = ColumnName
                        + MysqlConstants.SPACE
                        + reg.type
                        + NULLABLE
                        + MysqlConstants.IDENTITY_PRIMARY_KEY
                        + MysqlConstants.COMMA;
                }
                else
                {
                    Columns += ColumnName
                        + MysqlConstants.SPACE
                        + reg.type
                        + NULLABLE
                        + MysqlConstants.COMMA
                        + MysqlConstants.SPACE;
                }
            }
            if (!IsUpdate)
            {
                Columns = Columns.TrimEnd(MysqlConstants.SPACE).TrimEnd(MysqlConstants.COMMA) + MysqlConstants.END_PARENTHESIS + Environment.NewLine;
            }
            else
            {
                Columns = Columns.TrimEnd(MysqlConstants.SPACE).TrimEnd(MysqlConstants.COMMA);
            }

        }

    }
}
