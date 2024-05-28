using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class CampanhaPasso2Model
    {

        public int CampanhaId { get; set; }
        public int TipoEstruturaId { get; set; }
        public int[] EstruturaId { get; set; }

        public int[] EstruturaViewId { get; set; }
    }
}
