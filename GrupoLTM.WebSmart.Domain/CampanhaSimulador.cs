using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class CampanhaSimulador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int NumeroCampanha { get; set; }
        public int AnoCampanha { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public string LinkDownload { get; set; }
        public virtual ICollection<CampanhaMecanicaSimulador> CampanhaMecanicaSimulador { get; set; }
        public virtual ICollection<MecanicaSubMecanicaSimulador> MecanicaSubMecanicaSimulador { get; set; }
    }
}
