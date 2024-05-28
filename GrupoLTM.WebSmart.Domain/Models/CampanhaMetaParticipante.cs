using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaMetaParticipante
    {
        public int Id { get; set; }
        public int ArquivoId { get; set; }
        public int ParticipanteId { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public double Valor { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
        public virtual Participante Participante { get; set; }
    }
}
