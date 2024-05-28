using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class SaldoModel
    {
        public double SaldoAtual { get; set; }
        public double SaldoBloqueado { get; set; }
        public double SaldoTotal { get; set; }
        public double TotalDebito { get; set; }
        public double TotalCredito { get; set; }
    }
}

