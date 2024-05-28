using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class FatorConversaoMecanicaSimuladorModel
    {

        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int? IdFatorConversaoSimulador { get; set; }
        public int? IdSubMecanicaSimulador { get; set; }
        public int? IdMecanicaSimulador { get; set; }
        public virtual FatorConversaoSimuladorModel FatorConversaoSimulador { get; set; }
        public virtual SubMecanicaSimuladorModel SubMecanicaSimulador { get; set; }
        public virtual MecanicaSimuladorModel MecanicaSimulador { get; set; }
    }
}
