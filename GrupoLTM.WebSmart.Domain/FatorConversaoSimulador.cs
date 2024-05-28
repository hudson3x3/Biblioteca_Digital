using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class FatorConversaoSimulador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int[] IdSubMecanicaSimulador { get; set; }
        public short TipoPonto { get; set; }
        public short TipoConversao { get; set; }


        public int? MultiplicadorValor { get; set; }
        public int? MultiplicadorPontos { get; set; }

        public string Mensagem { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual ICollection<FatorConversaoMecanicaSimulador> FatorConversaoMecanicaSimulador { get; set; }
        public virtual ICollection<FatorConversaoPontosSimulador> FatorConversaoPontosSimulador { get; set; }
    }
}
