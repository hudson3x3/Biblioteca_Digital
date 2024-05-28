using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class LogAprovacaoArquivo
    {
        public int Id { get; set; }
        public string Acao { get; set; }
        public string Login { get; set; }
        public string IP { get; set; }
        public int QtdeRegistrosProcessados { get; set; }
        public int QtdePontosDisponiveis { get; set; }
        public int ArquivoId { get; set; }
        public Arquivo Arquivo { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
