using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class LogDetalhado
    {
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public string Registro { get; set; }
        public string TipoAcao { get; set; }
        public string Pontos { get; set; }
        public string StatusPontos { get; set; }
        public string TipoPrograma { get; set; }
        public string Campanha { get; set; }
        public int LoteId { get; set; }
        public virtual Lote Lote { get; set; }
        public int NumeroLinha { get; set; }
    }
}
