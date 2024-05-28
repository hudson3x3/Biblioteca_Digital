using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class ForcarPrimeiroAcessoCatalogo
    {
        public ForcarPrimeiroAcessoCatalogo()
        {
        }
        public int Id { get; set; }
        public int ArquivoUploadId { get; set; }
        public string PageName { get; set; }
        public int IdAdmin { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        
    }
}
