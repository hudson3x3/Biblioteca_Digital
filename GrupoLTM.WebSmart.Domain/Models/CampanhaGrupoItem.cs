using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaGrupoItem
    {
        public CampanhaGrupoItem()
        {
            this.CampanhaResultadoParticipantes = new List<CampanhaResultadoParticipante>();
        }

        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public int GrupoItemId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual ICollection<CampanhaResultadoParticipante> CampanhaResultadoParticipantes { get; set; }
        public virtual GrupoItem GrupoItem { get; set; }
    }
}
