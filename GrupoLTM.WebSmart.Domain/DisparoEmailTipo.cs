using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class DisparoEmailTipo
    {
        public DisparoEmailTipo()
        {
            this.DisparoEmails = new List<DisparoEmail>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string KeyTemplate { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual ICollection<DisparoEmail> DisparoEmails { get; set; }
    }
}
