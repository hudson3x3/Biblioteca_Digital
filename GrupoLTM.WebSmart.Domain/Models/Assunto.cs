using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Assunto
    {
        public Assunto()
        {
            this.FaleConoscoes = new List<FaleConosco>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<FaleConosco> FaleConoscoes { get; set; }
    }
}
