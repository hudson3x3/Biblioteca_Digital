using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class Faixa
    {
        public int Id { get; set; }
        public int CampanhaPerfilId { get; set; }
        public int CampanhaEstruturaId { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public double PosicaoDe { get; set; }
        public double PosicaoAte { get; set; }
        public double Pontos { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
    }
}
