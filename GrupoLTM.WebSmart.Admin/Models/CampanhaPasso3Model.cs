using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaPasso3Model
    {

        public int CampanhaId { get; set; }
        public int[] PerfilId { get; set; }
        public int[] PerfilViewId { get; set; }
    }
}
