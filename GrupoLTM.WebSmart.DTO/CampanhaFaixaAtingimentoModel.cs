using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaFaixaAtingimentoModel
    {
        public int Id { get; set; }
        public int CampanhaId { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public int[] CampanhaEstruturaIdArr { get; set; }
        public int CampanhaEstruturaId { get; set; }
        public int CampanhaPerfilId { get; set; }
        public double ValorDe { get; set; }
        public double ValorAte { get; set; }
        public double CalculaAtingimentoPercentual { get; set; }
        public double Pontos { get; set; }
        public Boolean? Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Estrutura { get; set; }
        public string Perfil { get; set; }
        public string CampanhaPeriodo { get; set; }
        public int EstruturaId { get; set; }
        public int PerfilId { get; set; }

    }
}
