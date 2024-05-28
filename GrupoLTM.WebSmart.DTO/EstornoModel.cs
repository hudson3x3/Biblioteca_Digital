using System;

namespace GrupoLTM.WebSmart.DTO
{
    public class EstornoModel
    {
        public int EstornoId { get; set; }

        public string Nome { get; set; }

        public string Status { get; set; }

        public int? TotalPedido { get; set; }

        public long? TotalPontos { get; set; }

        public string DataInclusao { get; set; }

        public bool Processado { get; set; }

        public bool Inconsistente { get; set; }

        public string Csv { get; set; }

        public string DataAprovacao { get; set; }

        public string LoginAprovacao { get; set; }
    }
}