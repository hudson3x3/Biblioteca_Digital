using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaGrupoItemPonto
    {
        public int Id { get; set; }
        public int ArquivoId { get; set; }
        public int CampanhaId { get; set; }
        public int CampanhaEstruturaId { get; set; }
        public int CampanhaPerfilId { get; set; }
        public int GrupoItemId { get; set; }
        public double Multiplicador { get; set; }
        public double ValorPontos { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public Nullable<DateTime> DataInativacao { get; set; }
        public bool Ativo { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual CampanhaEstrutura CampanhaEstrutura { get; set; }
        public virtual CampanhaPerfil CampanhaPerfil { get; set; }
        public virtual CampanhaPeriodo CampanhaPeriodo { get; set; }
    }
}
