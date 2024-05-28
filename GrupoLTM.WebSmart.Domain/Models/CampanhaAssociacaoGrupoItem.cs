using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaAssociacaoGrupoItem
    {
        public int id { get; set; }
        public Nullable<int> ArquivoId { get; set; }
        public Nullable<int> CampanhaId { get; set; }
        public Nullable<int> CampanhaPeriodoId { get; set; }
        public Nullable<int> GrupoItemId { get; set; }
        public Nullable<int> CampanhaPerfilId { get; set; }
        public Nullable<int> CampanhaEstruturaId { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public Nullable<DateTime> DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
        public virtual GrupoItem GrupoItem { get; set; }
    }
}
