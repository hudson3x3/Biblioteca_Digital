using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.DTO
{
    public class ErroImportacaoArquivoModel
    {

        public int ArquivoId { get; set; }
        public string Nome { get; set; }
        public int TipoArquivoId { get; set; }
        public string TipoArquivo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataInclusaoErro { get; set; }
        public string DescricaoErro { get; set; }
        public int QtdLinhasErro { get; set; }
        public string TipoRegistro { get; set; }
        public string CSV { get; set; }
    }
}
