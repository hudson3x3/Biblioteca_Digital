using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaMetaGrupoItemPerfil
    {
        public int Id { get; set; }
        public int ArquivoId { get; set; }
        public int CampanhaPerfilId { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public int CampanhaEstruturaId { get; set; }
        public int CampanhaId { get; set; }
        public int GrupoItemId { get; set; }
        public double Valor { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual Campanha Campanha { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
        public virtual CampanhaMetaGrupoItemPerfil CampanhaMetaGrupoItemPerfil1 { get; set; }
        public virtual CampanhaMetaGrupoItemPerfil CampanhaMetaGrupoItemPerfil2 { get; set; }
        public virtual GrupoItem GrupoItem { get; set; }
    }
}
