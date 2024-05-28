using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Estado
    {
        public Estado()
        {
            this.Participantes = new List<Participante>();
        }

        public int EstadoId { get; set; }
        public string Nome { get; set; }
        public string Regiao { get; set; }
        public string Capital { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual ICollection<Participante> Participantes { get; set; }
    }
}
