using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaLogArquivoModel
    {
        public int Id { get; set; }
        public int CampanhaPeriodoId { get; set; }
        public int CampanhaId { get; set; }
        public int ArquivoId { get; set; }
        public string Nome { get; set; }
        public string NomeGerado { get; set; }
        public string CampanhaPeriodo { get; set; }
        public DateTime DataInclusao { get; set; }
        public string CaminhoArquivo { get; set; }
        public DateTime PeriodoDe { get; set; }
        public DateTime PeriodoAte { get; set; }
        public DateTime DataFechamento { get; set; }
        public string UrlAccess { get; set; }
    }
}
