using System;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class EstornoController : BaseController
    {
        private readonly EstornoService _estornoService = new EstornoService();

        [HttpGet]
        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult EstornoAprovacao()
        {
            return View();
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult EstornoErro()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public JsonResult ListarEstornosAprovacao()
        {
            try
            {
                inicializaDadosDatatable();

                var total = 0;

                var status = new EnumDomain.EstornoStatus[]
                {
                    EnumDomain.EstornoStatus.Processado,
                    EnumDomain.EstornoStatus.Aprovado,
                    EnumDomain.EstornoStatus.EmProcessoLive,
                    EnumDomain.EstornoStatus.EstornoRealizadoSucesso,
                    EnumDomain.EstornoStatus.ComunicacaoEnviada,
                };

                var estornos = _estornoService.ListarEstornos(sSearch, startExibir, regExibir, out total, status);

                return Json(new
                {
                    aaData = estornos,
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public JsonResult ListarEstornosInconsistentes()
        {
            try
            {
                inicializaDadosDatatable();

                var total = 0;

                var status = new EnumDomain.EstornoStatus[]
                {
                    EnumDomain.EstornoStatus.Inconsistente,
                    EnumDomain.EstornoStatus.EstornoRealizadoErro,
                    EnumDomain.EstornoStatus.EstornoErro,
                    EnumDomain.EstornoStatus.ComunicacaoErro,
                };

                var estornos = _estornoService.ListarEstornos(sSearch, startExibir, regExibir, out total, status);

                return Json(new
                {
                    aaData = estornos,
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public JsonResult Aprovar(int estornoId)
        {
            try
            {
                EstornoService.AtualizarStatusEstorno(estornoId, EnumDomain.EstornoStatus.Aprovado);

                GravaLogAprovacaoArquivo(estornoId, "AprovarEstorno");

                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
               // LogProcessamento.LogErro("Não foi possivel aprovar o estorno id: " + estornoId, "EstornoController", "Aprovar", ex);
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public JsonResult Reprovar(int estornoId)
        {
            try
            {
                EstornoService.AtualizarStatusEstorno(estornoId, EnumDomain.EstornoStatus.Reprovado);
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                //LogProcessamento.LogErro("Não foi possivel reprovar o estorno id: " + estornoId, "EstornoController", "Reprovar", ex);
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        private void GravaLogAprovacaoArquivo(int estornoId, string acao)
        {
            var logAprovacaoArquivo = new LogAprovacaoArquivo()
            {
                Acao = acao,
                Login = LoginHelper.GetLoginModel().Login,
                IP = HttpContext.Request.UserHostAddress.ToString(),
                ArquivoId = estornoId,
                DataInclusao = DateTime.Now
            };

            new LogAprovacaoArquivoService().GravarLogAprovacaoArquivo(logAprovacaoArquivo);
        }
    }
}