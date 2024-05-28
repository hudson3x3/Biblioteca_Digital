using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaPeriodoParticipantePerfil
    {
        public int id { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> ParticipanteId { get; set; }
        public Nullable<int> PerfilId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public Nullable<DateTime> DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual Campanha Campanha1 { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo1 { get; set; }
        public virtual Participante Participante { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Participante Participante1 { get; set; }
        public virtual Perfil Perfil1 { get; set; }
    }
}
