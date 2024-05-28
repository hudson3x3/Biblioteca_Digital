using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaCalculadaVendeuGanhouPessoaImportacao
    {
        public string Login { get; set; }
        public string Efetivo { get; set; }
        public string Estrutura { get; set; }
        public string Perfil { get; set; }
        public string Pontos { get; set; }
        public Nullable<int> ArquivoId { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public Nullable<int> CampanhaEstruturaId { get; set; }
        public Nullable<int> CampanhaPerfilId { get; set; }
        public Nullable<int> EstruturaId { get; set; }
        public Nullable<int> PerfilId { get; set; }
        public Nullable<int> ParticipanteId { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public string Erro { get; set; }
        public Nullable<int> ParticipanteEstruturaId { get; set; }
        public Nullable<int> ParticipantePerfilId { get; set; }
        public int Id { get; set; }
    }
}
