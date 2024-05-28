using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class ExtratoModel
    {
        public ICollection<ExtratoItemModel> Itens { get; set; }
        public double TotalDebito { get; set; }
        public double TotalCredito { get; set; }
        public double SaldoAtual { get; set; }
    }
}
