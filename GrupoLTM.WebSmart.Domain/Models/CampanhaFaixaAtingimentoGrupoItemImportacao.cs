using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaFaixaAtingimentoGrupoItemImportacao
    {
        public string Estrutura { get; set; }
        public string Perfil { get; set; }
        public string ItemCodigo { get; set; }
        public Nullable<double> ValorDe { get; set; }
        public Nullable<double> ValorAte { get; set; }
        public Nullable<double> Pontos { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> ArquivoId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public int Id { get; set; }
        public Nullable<int> EstruturaId { get; set; }
        public Nullable<int> PerfilId { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public string Erro { get; set; }
    }
}
