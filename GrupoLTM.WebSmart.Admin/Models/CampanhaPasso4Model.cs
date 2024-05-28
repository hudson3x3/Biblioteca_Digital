using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaPasso4Model
    {
        public int CampanhaId { get; set; }
        public string LinkBannerHome { get; set; }
        public string LinkImagemMecanica { get; set; }
        public string LinkImagemMecanicaMobile { get; set; }
        public string Regulamento { get; set; }
        public HttpPostedFileBase FileBannerHome { get; set; }
        public HttpPostedFileBase FileImagemMecanica { get; set; }
        public HttpPostedFileBase FileImagemMecanicaMobile { get; set; }
    }
}