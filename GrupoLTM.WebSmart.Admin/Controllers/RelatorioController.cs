using GrupoLTM.WebSmart.Domain.DTO;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class RelatorioController : BaseController
    {
        private LogAccountService _logAccountService;

        public RelatorioController()
        {
            _logAccountService = new LogAccountService();
        }

        #region "Actions"

        public ActionResult RelParticipante()
        {
            return View();
        }

        public ActionResult RelParticipanteQuiz()
        {
            return View();
        }

        public ActionResult RelLog()
        {
            return View();
        }

        public ActionResult RelParticipantePesquisa()
        {
            return View();
        }

        public ActionResult RelPontuacao()
        {
            return View();
        }
        //public ActionResult RelArquivoResgateOffLine()
        //{
        //    return View(getCatalogos());
        //}

        public ActionResult AcessoPorParticipantes()
        {
            return View(getCatalogos(true));
        }

        public ActionResult AcessoPorParticipantesExterno()
        {
            return View(getCatalogos(true));
        }

        public ActionResult RelatorioPermissao()
        {
            return View();
        }

        #endregion
        #region "Actions Posts"

        [HttpPost]
        public ActionResult ExportarRelParticipante()
        {
            try
            {
                string strDownloadArquivo = "";

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                if (LoginHelper.GetLoginModel().TipoAcessoId == Convert.ToInt32(Domain.Enums.EnumDomain.TipoAcesso.PF))
                {
                    dt = GrupoLTM.WebSmart.Services.RelatorioService.RelParticipantePF();
                }
                else
                {
                    dt = GrupoLTM.WebSmart.Services.RelatorioService.RelParticipantePJ();
                }

                ds.Tables.Add(dt);

                string strArquivoGerado = "";
                ExcelExport.WriteXLSFile(
                    ds,
                    "relatorio/",
                    out strArquivoGerado);

                strDownloadArquivo = Settings.Caminho.StoragePath + "relatorio/" + strArquivoGerado + Settings.Caminho.StorageToken;

                var data = new { ok = true, msg = "Exportado com sucesso.", linkDownload = strDownloadArquivo };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível exportar os dados. " + exc.ToString(), linkDownload = "" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ExportarRelPontuacao()
        {
            try
            {
                string strDownloadArquivoErro = "";

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                dt = Services.RelatorioService.RelPontuacao();

                ds.Tables.Add(dt);

                string strArquivoGerado = "";
                ExcelExport.WriteXLSFile(
                    ds,
                    "relatorio/",
                    out strArquivoGerado);

                strDownloadArquivoErro = Settings.Caminho.StoragePath + "relatorio/" + strArquivoGerado + Settings.Caminho.StorageToken;

                var data = new { ok = true, msg = "Exportado com sucesso.", linkDownload = strDownloadArquivoErro };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível exportar os dados. " + exc.ToString(), linkDownload = "" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ExportarRelPesquisa()
        {
            try
            {
                string strDownloadArquivoErro = "";

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                if (LoginHelper.GetLoginModel().TipoAcessoId == Convert.ToInt32(Domain.Enums.EnumDomain.TipoAcesso.PF))
                {
                    dt = GrupoLTM.WebSmart.Services.RelatorioService.RelParticipantePesquisaPF();
                }
                else
                {
                    dt = GrupoLTM.WebSmart.Services.RelatorioService.RelParticipantePesquisaPJ();
                }

                ds.Tables.Add(dt);

                string strArquivoGerado = "";
                ExcelExport.WriteXLSFile(
                    ds,
                    "relatorio/",
                    out strArquivoGerado);

                strDownloadArquivoErro = Settings.Caminho.StoragePath + "relatorio/" + strArquivoGerado + Settings.Caminho.StorageToken;

                var data = new { ok = true, msg = "Exportado com sucesso.", linkDownload = strDownloadArquivoErro };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível exportar os dados. " + exc.ToString(), linkDownload = "" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ExportarRelQuiz()
        {
            try
            {
                string strDownloadArquivoErro = "";

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                if (LoginHelper.GetLoginModel().TipoAcessoId == Convert.ToInt32(Domain.Enums.EnumDomain.TipoAcesso.PF))
                {
                    dt = GrupoLTM.WebSmart.Services.RelatorioService.RelParticipanteQuizPF();
                }
                else
                {
                    dt = GrupoLTM.WebSmart.Services.RelatorioService.RelParticipanteQuizPJ();
                }

                ds.Tables.Add(dt);

                string strArquivoGerado = "";
                ExcelExport.WriteXLSFile(
                    ds,
                    "relatorio/",
                    out strArquivoGerado);

                strDownloadArquivoErro = Settings.Caminho.StoragePath + "relatorio/" + strArquivoGerado + Settings.Caminho.StorageToken;

                var data = new { ok = true, msg = "Exportado com sucesso.", linkDownload = strDownloadArquivoErro };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível exportar os dados. " + exc.ToString(), linkDownload = "" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ExportarRelLog(DateTime DataInicio, DateTime DataFim)
        {
            try
            {
                string strDownloadArquivoErro = "";
                var data = new object();

                if (DataFim < DataInicio)
                {
                    data = new { ok = false, msg = "Data fim precisa ser maior que a data de início.", linkDownload = "" };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt = GrupoLTM.WebSmart.Services.RelatorioService.RelLog(DataInicio, DataFim, null);
                ds.Tables.Add(dt);

                string strArquivoGerado = "";
                ExcelExport.WriteXLSFile(
                    ds,
                    "relatorio/",
                    out strArquivoGerado);

                strDownloadArquivoErro = Settings.Caminho.StoragePath + "relatorio/" + strArquivoGerado + Settings.Caminho.StorageToken;

                data = new { ok = true, msg = "Exportado com sucesso.", linkDownload = strDownloadArquivoErro };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível exportar os dados. " + exc.ToString(), linkDownload = "" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        #region Acesso POr Participante Admin
        [HttpGet]
        public JsonResult ListarAcessos(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                this.inicializaDadosDatatable();

                int total = 0;

                var lstAcessos = _logAccountService.ListarRelatorio(catalogoId, dtInicio, dtFim, startExibir, regExibir, out total);

                return Json(new
                {
                    aaData = lstAcessos.Select(x => new
                    {
                        x.Catalogo,
                        x.LoginAdmin,
                        x.AccountNumber,
                        x.NameAccountNumber,
                        DataInclusao = x.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss")
                    }).ToList(),
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        public FileResult ExportarRelatorioAcessos(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            var lstRetorno = _logAccountService.ListarRelatorio(catalogoId, dtInicio, dtFim);

            string filename = string.Format("ListagemAcessos_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(geraCSVAcessosParticipantes(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet,
                filename);
        }

        public FileResult ExportarRelatorioPermissao()
        {
            var lstRetorno = _logAccountService.ObterPontosCancelamento();

            string filename = string.Format("RelatorioPermissao_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(GeraCsvRelatorioPermissao(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet,
                filename);
        }
        #endregion

        #region Acesso POr Participante SSO
        [HttpGet]
        public JsonResult ListarAcessosExterno(int catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                this.inicializaDadosDatatable();

                int total = 0;

                var lstAcessos = _logAccountService.ListarRelatorio(catalogoId, dtInicio, dtFim, startExibir, regExibir, out total);

                return Json(new
                {
                    aaData = lstAcessos.Select(x => new
                    {
                        LoginAdmin = x.LoginAdmin,
                        AccountNumber = x.AccountNumber,
                        NameAccountNumber = x.NameAccountNumber,
                        DataInclusao = x.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss")
                    }).ToList(),
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        public FileResult ExportarRelatorioAcessosExterno(int catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            var lstRetorno = _logAccountService.ListarRelatorio(catalogoId, dtInicio, dtFim);

            string filename = string.Format("ListagemAcessos_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(geraCSVAcessosParticipantes(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet,
                filename);
        }
        #endregion

        private byte[] geraCSVAcessosParticipantes(List<LogAccountDTO> lstAcessos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Login Adm;Account Number;Usuário Acessado;Data Inclusão;Catálogo\r\t");

            foreach (var acesso in lstAcessos)
                sb.Append(string.Format("{0};{1};{2};{3};{4}\r\t", acesso.LoginAdmin, acesso.AccountNumber, acesso.NameAccountNumber, acesso.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss"), acesso.Catalogo));

            return Encoding.Default.GetBytes(sb.ToString());

        }

        private byte[] GeraCsvRelatorioPermissao(List<RelatorioPermissaoModel> model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Login Adm;Nome Usuário;Perfil;Menu\r\t");

            foreach (var item in model)
                sb.Append(string.Format("{0};{1};{2};{3}\r\t", item.Login, item.Usuario, item.Perfil, item.Menu));

            return Encoding.Default.GetBytes(sb.ToString());
        }

        #endregion

        #region "static metods"
        static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "Relatorio",
                Pagina = string.Empty,
                Codigo = codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
        #endregion
    }
}
