using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class MecanicaSimuladorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? IdIconeSimulador { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public virtual IconeSimuladorModel IconeSimulador { get; set; }
        public virtual ICollection<CampanhaMecanicaSimuladorModel> CampanhaMecanicaSimulador { get; set; }
        public virtual ICollection<FatorConversaoMecanicaSimuladorModel> FatorConversaoMecanicaSimulador { get; set; }
        public virtual ICollection<MecanicaSubMecanicaSimuladorModel> MecanicaSubMecanicaSimulador { get; set; }
    }
}
