using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaUploadMetaPessoaModel
    {
        public int CampanhaId { get; set; }
        public int TipoCampanhaId { get; set; }
        public bool? ExibirRankingIndividual { get; set; }
    }
}