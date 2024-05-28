using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.DTO
{
    public class CancelamentoPontosModel
    {
        public int ArquivoId { get; set; }
        public string Nome { get; set; }
        public int TipoArquivoId { get; set; }
        public string TipoArquivo { get; set; }
        public float TotalPontos { get; set; }
        public float PontosDisponiveis { get; set; }
        public float PontosPendentes { get; set; }
        public float PontosCancelados { get; set; }
        public string CSV { get; set; }
        public DateTime? DataTerminoProcessamento { get; set; }
        public string DataProcessamentoTexto { get; set; }
        public string IncentiveProgramDescriptionHeader { get; set; }

    }
}
