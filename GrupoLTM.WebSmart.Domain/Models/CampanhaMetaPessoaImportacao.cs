using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaMetaPessoaImportacao
    {
        public string Login { get; set; }
        public Nullable<double> Meta { get; set; }
        public int ArquivoId { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> PeriodoId { get; set; }
        public int Id { get; set; }
        public Nullable<int> ParticipanteId { get; set; }
        public string Erro { get; set; }
        public Nullable<DateTime> DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual Participante Participante { get; set; }
    }
}
