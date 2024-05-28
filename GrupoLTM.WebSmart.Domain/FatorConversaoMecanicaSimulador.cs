using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class FatorConversaoMecanicaSimulador
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int? IdFatorConversaoSimulador { get; set; }
        public int? IdSubMecanicaSimulador { get; set; }
        public int? IdMecanicaSimulador { get; set; }
        public virtual FatorConversaoSimulador FatorConversaoSimulador { get; set; }
        public virtual SubMecanicaSimulador SubMecanicaSimulador { get; set; }
        public virtual MecanicaSimulador MecanicaSimulador { get; set; }
    }
}
