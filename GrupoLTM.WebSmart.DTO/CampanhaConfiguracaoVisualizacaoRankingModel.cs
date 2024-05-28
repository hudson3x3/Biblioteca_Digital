using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaConfiguracaoVisualizacaoRankingModel
    {
        public int CampanhaId { get; set; }
        public string Nome { get; set; }
        public string TipoCampanha { get; set; }
        public bool Ativo { get; set; }
        public int?  ExibirRankingIndividual { get; set; }
        public int[] arrExibirRankingIndividual { get; set; }

    }    
}
