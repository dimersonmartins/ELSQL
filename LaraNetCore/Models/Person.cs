using System;
using Vendor.VDataAnnotations;

namespace LaraNetCore.Models
{
    public class Person
    {
        /// <summary>
        /// Propriedade obrigatoria em todas as Models
        /// </summary>
        protected string table
        {
            get
            {
                //Nome da tabela
                return "tb_person";
            }

            set { }
        }

        [IDENTITY_PRIMARY_KEY]
        [FIELD(VALUE = "BIGINT")]
        [INDEX(NAME = "id_idx")]
        public int id { get; set; }

        [NOT_NULL]
        [FIELD(VALUE = "VARCHAR(255)")]
        public string nome { get; set; }

        [NOT_NULL]
        [FIELD(VALUE = "DATETIME")]
        public DateTime data { get; set; }

    }


}
