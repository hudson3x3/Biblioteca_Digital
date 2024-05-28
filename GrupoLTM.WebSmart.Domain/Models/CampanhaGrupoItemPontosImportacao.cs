using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaGrupoItemPontosImportacao
    {
        public string Estrutura { get; set; }
        public string Perfil { get; set; }
        public string ItemCodigo { get; set; }
        public string Multiplicador { get; set; }
        public string ValorPontos { get; set; }
        public Nullable<int> ArquivoId { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public Nullable<int> EstruturaId { get; set; }
        public Nullable<int> PerfilId { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public Nullable<int> CampanhaEstruturaId { get; set; }
        public Nullable<int> CampanhaPerfilId { get; set; }
        public Nullable<int> CampanhaGrupoItemId { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public string Erro { get; set; }
        public int Id { get; set; }
    }
}
