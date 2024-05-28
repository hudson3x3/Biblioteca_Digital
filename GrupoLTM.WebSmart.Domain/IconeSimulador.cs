using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class IconeSimulador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public short Tipo { get; set; }
        public string CaminhoImagem { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<MecanicaSimulador> MecanicaSimulador { get; set; }
    }
}
