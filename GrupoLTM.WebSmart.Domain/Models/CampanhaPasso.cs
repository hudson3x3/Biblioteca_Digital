using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaPasso
    {
        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public int PassoId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAtualizacao { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual Passo Passo { get; set; }
    }
}
