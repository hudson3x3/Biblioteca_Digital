using GrupoLTM.WebSmart.Infrastructure.Excel;
using IronPdf;
using System;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class TesteController : Controller
    {

        public ActionResult Index()
        {
            IronPdf.HtmlToPdf htmlToPdf = new IronPdf.HtmlToPdf();
            PdfResource pdf = htmlToPdf.RenderUrlAsPdf(new Uri(@"https://scmanager.com.br/"));
            ExcelExport.ToPdf(pdf.Stream);

            return View();
        }

    }
}
