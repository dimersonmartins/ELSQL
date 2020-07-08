using System;
using Vendor.VDataAnnotations;

namespace LaraNetCore.Models
{
    public class Work
    {
        /// <summary>
        /// Propriedade obrigatoria em todas as Models
        /// </summary>
        protected string table
        {
            get
            {
                //Nome da tabela
                return "tb_working";
            }

            set { }
        }

        [IDENTITY_PRIMARY_KEY]
        [FIELD(VALUE = "BIGINT")]
        [INDEX(NAME = "id_idx")]
        public int id { get; set; }

        [NOT_NULL]
        [FIELD(VALUE = "VARCHAR(255)")]
        public string nome_trampo { get; set; }

        [NOT_NULL]
        [FIELD(VALUE = "DATETIME")]
        public DateTime data_trampo { get; set; }

    }


}
