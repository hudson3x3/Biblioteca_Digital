using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class HistoricoModel
    {
        public DateTime DataCriacaoUsuario { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalResgatado { get; set; }
        public decimal TotalExpirado { get; set; }
    }
}

