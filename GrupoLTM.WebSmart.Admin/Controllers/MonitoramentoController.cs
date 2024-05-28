using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class MonitoramentoController : BaseController
    {
        #region "Services"

        private readonly ConsultarApiService consultarApiService = new ConsultarApiService();

        #endregion

        #region "Actions"

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult DashBoardConsolidado()
        {            
            return View(getCatalogos());
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult ArquivosMonitoramento()
        {
            return View(getCatalogos());
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public JsonResult GetInfoDashboard(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                validaCampos(dtInicio, dtFim);

                DashboardModel data = ArquivoService.ObterConsolidadoPontuacaoDashboard(catalogoId, dtInicio, dtFim);

                if (data.Itens.Count > 0)
                    return Json(new { Sucesso = true, Retorno = data });
                else
                    return Json(new { Sucesso = false, Mensagem = "Nenhum dado encontrado" });
            }
            catch(Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult _PartialArquivosMonitoramento(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                validaCampos(dtInicio, dtFim);

                ArquivoMonitoramentoModel retorno = ArquivoService.ObterArquivosMonitoramento(catalogoId, dtInicio, dtFim);

                return PartialView(retorno);
            }
            catch(Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public FileResult DownloadArquivoResumido(int id)
        {
            Arquivo arquivo = ArquivoService.ObterArquivoPorId(id);

            byte[] bytes = geraArquivoResumido(arquivo);
            string fileName = string.Format("LOG_RESUMIDO_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public FileResult DownloadArquivoCreditoLive(int id, int qtdLinhas, int qtdPontos)
        {
            Arquivo arquivo = ArquivoService.ObterArquivoPorId(id);

            byte[] bytes = geraArquivoCreditoLive(arquivo, qtdLinhas, qtdPontos);
            string fileName = string.Format("LOG_ARQUIVO_PONTOS_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public ActionResult Apis()
        {
            return View("MonitoramentoApi");
        }

        [HttpPost]
        public Task<JsonResult> ConsultarApi(RequestApi request)
        {
            var result = consultarApiService.ConsultarApi(request);

            return Task.FromResult(Json(result));
        }

        #endregion

        #region "Métodos Privados"

        private void validaCampos(DateTime dtInicio, DateTime dtFim)
        {
            if (dtInicio == null || dtFim == null)
                throw new Exception("Preencha o período.");

            if (dtInicio > dtFim)
                throw new Exception("A data inicial deve ser inferior à data final.");

            if ((dtFim - dtInicio).TotalDays > 60)
                throw new Exception("O período selecionado não pode ultrapassar 60 dias");
        }

        private byte[] geraArquivoResumido(Arquivo arquivo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("NOME DO ARQUIVO: {0}\r\n", arquivo.Nome));
            sb.Append(string.Format("DATA DO PROCESSAMENTO: {0}\r\n", arquivo.DataTerminoProcessamento == null ? 
                "-" : ((DateTime)arquivo.DataTerminoProcessamento).ToString("dd/MM/yyyy")));
            sb.Append(string.Format("HORA DO PROCESSAMENTO: {0}\r\n", arquivo.DataTerminoProcessamento == null ?
                "-" : ((DateTime)arquivo.DataTerminoProcessamento).ToString("HH:mm:ss")));
            sb.Append(string.Format("TOTAL DE LINHAS GERADAS: {0}\r\n", arquivo.QuantidadeLinhas));
            sb.Append(string.Format("TOTAL DE REVENDEDORAS PROCESSADAS: {0}\r\n", arquivo.QuantidadeRevendedorasProcessadas));
            sb.Append(string.Format("TOTAL DE PONTOS CANCELADOS: {0}\r\n", arquivo.PontosCancelados));
            sb.Append(string.Format("TOTAL DE PONTOS PENDENTES: {0}\r\n", arquivo.PontosPendentes));
            sb.Append(string.Format("TOTAL DE PONTOS DISPONIVEIS: {0}\r\n", arquivo.PontosDisponiveis));

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        private byte[] geraArquivoCreditoLive(Arquivo arquivo, int qtdLinhas, double qtdPontos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("QUANTIDADE LINHAS PROCESSADAS: {0}\r\n", qtdLinhas));
            sb.Append(string.Format("VALOR CREDITADO EM PONTOS: {0}\r\n", qtdPontos));
            sb.Append(string.Format("NOME DO ARQUIVO: {0}\r\n", arquivo.Nome));
            sb.Append(string.Format("DATA DE PROCESSAMENTO: {0}\r\n", arquivo.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss")));

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        #endregion

    }
}
