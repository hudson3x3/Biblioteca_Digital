using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaMecanicaSimuladorModel
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int IdMecanicaSimulador { get; set; }
        public int IdCampanhaSimulador { get; set; }
        public virtual MecanicaSimuladorModel MecanicaSimulador { get; set; }
        public virtual CampanhaSimuladorModel CampanhaSimulador { get; set; }
    }
}
