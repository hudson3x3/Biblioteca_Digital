using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaVendeuGanhouCalculadoModel
    {
        public Domain.Enums.EnumDomain.TipoCampanha ETipoCampanha { get; set; }

        public int TipoCampanhaId { get; set; }

        public int CampanhaId { get; set; }

        public bool? ExibirRankingIndividual { get; set; }
    }
}