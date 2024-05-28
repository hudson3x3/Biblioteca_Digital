using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaPasso5Model
    {
        public int Id { get; set; }
        public string Campanha { get; set; }
        public int? TipoCampanhaId { get; set; }
    }
}