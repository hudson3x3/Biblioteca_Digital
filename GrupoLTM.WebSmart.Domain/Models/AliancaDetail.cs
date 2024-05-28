using System;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public class AliancaDetail
    {
        public int Id { get; set; }

        public int LoteId { get; set; }

        public string EmpresariaAccountNumber { get; set; }

        public string MadrinhaAccountNumber { get; set; }

        public string MadrinhaNome { get; set; }

        public string RepresentativeAccountNumber { get; set; }

        public string RepresentativeName { get; set; }

        public int CpPrimeiroPedido { get; set; }

        public int AnoCpPrimeiroPedido { get; set; }

        public bool PrimeiroPedidoPago { get; set; }

        public long PontosConquistadosPrimeiroPedido { get; set; }

        public bool AtingiuVendas { get; set; }

        public long PontosConquistados { get; set; }

        public long PontosConquistadosEmpresaria { get; set; }

        public long PontosConquistadosEquipe { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}
