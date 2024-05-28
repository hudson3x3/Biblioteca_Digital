using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class FatorConversaoSimuladorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int[] IdSubMecanicaSimulador { get; set; }
        public string PontosSimulador { get; set; }
        public short TipoPonto { get; set; }
        public short TipoConversao { get; set; }

        public int? MultiplicadorValor { get; set; }
        public int? MultiplicadorPontos { get; set; }

        public string Mensagem { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public ArrayList ArrSubMecanicaSimuladorId { get; set; }
        public virtual List<FatorConversaoPontosSimuladorModel> FatorConversaoPontosSimulador { get; set; }

    }
}