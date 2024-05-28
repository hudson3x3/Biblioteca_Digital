using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class PerguntaModel
    {
        public int id { get; set; }
        public int questionarioID { get; set; }
        public int tipoRespostaID { get; set; }
        public string nome { get; set; }
        public bool Ativo { get; set; }
        public bool AtivoPontos { get; set; }
        public double ValorPontos { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
