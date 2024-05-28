using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class VendaModel
    {
        public int MesId { get; set; }
        public int Ano { get; set; }
        public HttpPostedFileBase FileArquivo { get; set; }
    }
}