using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class FatorConversaoPontosSimulador
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int ValorInicial { get; set; }
        public int ValorFinal { get; set; }
        public int Pontos { get; set; }
        public int IdFatorConversaoSimulador { get; set; }
        public virtual FatorConversaoSimulador FatorConversaoSimulador { get; set; }
    }
}
