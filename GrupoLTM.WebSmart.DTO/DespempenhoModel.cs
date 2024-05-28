using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class DesempenhoModel
    {
        private List<PontuacaoModel> _pontuacao = new List<PontuacaoModel>();

        public int TipoPontuacaoId { get; set; }
        public string TipoPontuacao { get; set; }
        public List<PontuacaoModel> Pontuacao { get { return _pontuacao; } set { _pontuacao = value; } }
        public double TotalPontos { get; set; }
    }
}
