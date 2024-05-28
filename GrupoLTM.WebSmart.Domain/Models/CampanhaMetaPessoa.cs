using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaMetaPessoa
    {
        public int Id { get; set; }
        public Nullable<int> ArquivoId { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public Nullable<int> ParticipanteId { get; set; }
        public Nullable<double> Meta { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public Nullable<DateTime> DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
    }
}
