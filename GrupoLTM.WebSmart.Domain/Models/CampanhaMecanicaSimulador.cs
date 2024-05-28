using System;
using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaMecanicaSimulador
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public int IdMecanicaSimulador { get; set; }
        public int IdCampanhaSimulador { get; set; }
        public virtual MecanicaSimulador MecanicaSimulador { get; set; }
        public virtual CampanhaSimulador CampanhaSimulador { get; set; }
    }
}
