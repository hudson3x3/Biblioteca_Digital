using GrupoLTM.WebSmart.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class FatorConversaoPontosSimuladorModel
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int ValorInicial { get; set; }
        public int ValorFinal { get; set; }
        public int Pontos { get; set; }
        public int IdFatorConversaoSimulador { get; set; }
        public virtual FatorConversaoSimuladorModel FatorConversaoSimulador { get; set; }

    }
}