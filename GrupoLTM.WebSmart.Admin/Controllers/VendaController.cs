using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class VendaController : Controller
    {
        #region "Actions"

        [HttpGet]
        public ActionResult Venda()
        {
            return View(new VendaModel());
        }

        public ActionResult ListaVendaLogArquivo()
        {
            var list = VendaService.ListaVendaArquivoLog().Select(l => new
            {
                l.Id,
                l.Nome,
                l.Mes,
                l.Ano,
                l.NomeGerado,
                l.DataInclusao,
                linkDownload = Settings.Caminho.StoragePath + "venda/" + l.NomeGerado + Settings.Caminho.StorageToken
            }).ToList();


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region "Internal Functions"

        #endregion

    }
}
