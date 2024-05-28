using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class ArquivoCreditoLote
    {
        public int Id { get; set; }
        public int ArquivoId { get; set; }
        public string Nome { get; set; }
        public bool ArquivoPai { get; set; }
        public int? ArquivoCreditoLotePaiId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual Arquivo Arquivo { get; set; }
    }
}
