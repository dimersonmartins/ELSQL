using System.Collections.Generic;

namespace Vendor.VMigrations.Struct
{
    public class Field
    {
        public string table { get; set; }
        public string options { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool nullable { get; set; }
        public string create_index { get; set; }
    }

    public class Schema
    {
        public List<Field> fields = new List<Field>();
        public void Add(Field field)
        {
            fields.Add(field);
        }
    }
}
