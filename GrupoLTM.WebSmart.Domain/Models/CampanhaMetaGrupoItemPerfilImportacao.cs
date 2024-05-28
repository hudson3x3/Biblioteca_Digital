using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaMetaGrupoItemPerfilImportacao
    {
        public string Estrutura { get; set; }
        public string Perfil { get; set; }
        public string ItemCodigo { get; set; }
        public Nullable<double> Valor { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> ArquivoId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public int Id { get; set; }
        public Nullable<int> CampanhaEstruturaId { get; set; }
        public Nullable<int> CampanhaPerfilId { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public string Erro { get; set; }
    }
}
