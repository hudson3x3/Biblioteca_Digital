using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaFaixaAtingimentoGrupoItem
    {
        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public int CampanhaEstruturaId { get; set; }
        public int CampanhaPerfilId { get; set; }
        public int GrupoItemId { get; set; }
        public double ValorDe { get; set; }
        public double ValorAte { get; set; }
        public double Pontos { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo1 { get; set; }
        public virtual GrupoItem GrupoItem { get; set; }
    }
}
