using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaResultadoDetalheModel
    {
        public string Estrutura { get; set; }
        public string NomePai { get; set; }
        public string Nome { get; set; }
        public string GupoItemPai { get; set; }
        public string GrupoItem { get; set; }
        public double Meta { get; set; }
        public double Efetivo { get; set; }
        public double Atingimento { get; set; }
        public int PosicaoRanking { get; set; }
        public double Pontos { get; set; }
    }
}
