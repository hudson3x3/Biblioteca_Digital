using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models
{
    public partial class AliancaExtrato
    {
        public AliancaExtrato()
        {
        }
        public AliancaExtratoDB AliancaExtratoDB { get; set; }
        public AliancaExtratoExterno AliancaExtratoExterno { get; set; }
    }

    public class AliancaExtratoExterno
    {
        //public int recordSetTotal { get; set; }
        //public int recordSetCount { get; set; }
        public string RegistroEmpresaria { get; set; }
        //public string Premiacao { get; set; }
        //public decimal ValorTotal { get; set; }
        public List<AliancaExtratoMadrinhas> Madrinhas { get; set; }
    }

    public class AliancaExtratoMadrinhas
    {
        public string RegistroMadrinha { get; set; }
        public string NomeMadrinha { get; set; }
        public string NomeNovaRepresentante { get; set; }
        public List<AliancaExtratoCP> Campanhas { get; set; }
    }

    public class AliancaExtratoCP
    {
        public string CpPrimeiroPedido { get; set; }
        public bool EviouPagouPrimeiroPedido { get; set; }
        public decimal PontosConquistadosPrimeiroPedido { get; set; }
        public bool AtingiuVendas { get; set; }
        public decimal PontosConquistados { get; set; }
        public decimal PontosConquistadosEmpresaria { get; set; }
        public decimal PontosConquistadosEquipe { get; set; }
    }

    public class AliancaExtratoDB
    {
        public int Id { get; set; }
        public string EmpresariaAccountNumber { get; set; }
        public string MadrinhaAccountNumber { get; set; }
        public string MadrinhaNome { get; set; }
        public string RepresentativeName { get; set; }
        public int CpPrimeiroPedido { get; set; }
        public int AnoCpPrimeiroPedido { get; set; }
        public bool PrimeiroPedidoPago { get; set; }
        public decimal PontosConquistadosPrimeiroPedido { get; set; }
        public bool AtingiuVendas { get; set; }
        public decimal PontosConquistados { get; set; }
        public decimal PontosConquistadosEmpresaria { get; set; }
        public decimal PontosConquistadosEquipe { get; set; }
    }
}
