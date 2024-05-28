using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class FaixaAtingemento
    {
        public int Id { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public Nullable<int> CampanhaEstruturaId { get; set; }
        public Nullable<int> CampanhaPerfilId { get; set; }
        public double ValorDe { get; set; }
        public double ValorAte { get; set; }
        public double CalculoAtingimentoPercentual { get; set; }
        public double Pontos { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
    }
}
