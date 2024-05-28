using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaVendeuGanhouRankingModel
    {
        public int Id { get; set; }
        public bool OptInPercentual { get; set; }
        public bool? ExibirRankingIndividual { get; set; }
    }
}