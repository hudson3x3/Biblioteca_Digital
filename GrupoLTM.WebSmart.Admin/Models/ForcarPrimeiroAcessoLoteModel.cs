using GrupoLTM.WebSmart.Admin.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class ForcarPrimeiroAcessoLoteModel
    {
        public ForcarPrimeiroAcessoLoteModel()
        {

        }

        public int? ProjectId { get; set; }
        public HttpPostedFileBase ArquivoUploadBaseRA { get; set; }
        public SelectList ddlCatalogo { get; set; }
        public string PageName { get; set; }
        public int IdAdmin { get; set; }

    }
}