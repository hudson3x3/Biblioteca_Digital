using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using System.Transactions;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Infrastructure.CSV;
using GrupoLTM.WebSmart.Infrastructure.Converter;
using System.Data;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Admin.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class PontuacaoController : BaseController
    {
        #region "Services"

        private ArquivoService _arquivoService;
        private CampanhaService _campanhaService;
        private ProgramaIncentivoService _programaIncentivoService;

        #endregion

        public PontuacaoController()
        {
            this._arquivoService = new ArquivoService();
            this._campanhaService = new CampanhaService();
        }

        #region "Actions"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult Index()
        {
            return View(getCatalogos());
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarPontuacaoAprovacao(int? id)
        {
            try
            {
                this.inicializaDadosDatatable();
                int total = 0;

                List<AprovacaoPontosModel> lstRetorno = _arquivoService.ObterPontosAprovacao(id, startExibir, regExibir, out total);

                return Json(new
                {
                    aaData = lstRetorno,
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

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarPontuacaoCancelamento(int? id)
        {
            try
            {
                this.inicializaDadosDatatable();
                int total = 0;

                List<CancelamentoPontosModel> lstRetorno = _arquivoService.ObterPontosCancelamento(id, out total).Skip(startExibir).Take(regExibir).ToList();

                return Json(new
                {
                    aaData = lstRetorno,
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

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarPontuacaoArquivos(int? catalogoId, DateTime dtInicio, DateTime dtFim, int? campanhaId, int? tipoArquivoId, string statusArquivoId, int? anoCampanha)
        {
            try
            {
                var total = 0;

                if (campanhaId == 0)
                    campanhaId = null;

                if (tipoArquivoId == 0)
                    tipoArquivoId = null;

                var lstRetorno = _arquivoService.ObterArquivoPontos(catalogoId, dtInicio, dtFim, campanhaId, tipoArquivoId, statusArquivoId, anoCampanha, out total);

                return Json(new
                {
                    success = true,
                    data = lstRetorno,
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao Listar Pontuacao Arquivo", ex.Message, "GrupoLTM.WebSmart.Admin", "ListarPontuacaoArquivos", "jobCatalog");
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarLogErroImportacaoPontuacao(DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                this.inicializaDadosDatatable();
                int total = 0;

                List<ErroImportacaoArquivoModel> lstRetorno = _arquivoService.ObterLogImportacaoErro(dtInicio, dtFim, startExibir, regExibir, out total);

                return Json(new
                {
                    aaData = lstRetorno,
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao Listar os Erros de Importação Pontuacao Arquivo", ex.Message, "GrupoLTM.WebSmart.Admin", "ListarLogErroImportacaoPontuacao", "jobCatalog");
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public JsonResult AprovarPontucoes(List<int> ids)
        {
            try
            {
                string idsConvertidos = string.Empty;

                ids.ForEach(id =>
                {
                    idsConvertidos += string.Format("{0},", id);
                    gravaLogAprovacaoArquivo(id);
                });

                _arquivoService.AprovarPontuacoes(idsConvertidos.Substring(0, idsConvertidos.Length - 1));
                _arquivoService.LimparExtratoRedisPorLogin(ids);

                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao aprovar arquivos", ex.Message, "GrupoLTM.WebSmart.Admin", string.Format("AprovarPontuacoes(ArquivosId:{0})", ids), "jobCatalog");

                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "Pontuacao",
                Pagina = string.Empty,
                Codigo = codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }

        internal void gravaLogAprovacaoArquivo(int ArquivoId)
        {

            LogAprovacaoArquivo logAprovacaoArquivo = new LogAprovacaoArquivo()
            {
                Acao = "AprovarPontuacao",
                Login = LoginHelper.GetLoginModel().Login,
                IP = HttpContext.Request.UserHostAddress.ToString(),
                //QtdeRegistrosProcessados = , 
                //QtdePontosDisponiveis
                ArquivoId = ArquivoId,
                //Arquivo
                DataInclusao = DateTime.Now
            };

            new LogAprovacaoArquivoService().GravarLogAprovacaoArquivo(logAprovacaoArquivo);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public JsonResult ReprovarPontucoes(List<int> ids)
        {
            try
            {
                if (ids == null)
                {
                    return Json(new { Sucesso = false, Mensagem = "Selecione um arquivo para Reprovação." });
                }
                string idsConvertidos = string.Empty;

                ids.ForEach(id =>
                {
                    idsConvertidos += string.Format("{0},", id);
                });

                ArquivoService.ReprovarPontuacoes(idsConvertidos.Substring(0, idsConvertidos.Length - 1));

                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public JsonResult CancelarPontucoes(List<int> ids)
        {
            try
            {
                if (ids == null)
                {
                    return Json(new { Sucesso = false, Mensagem = "Selecione um arquivo para Reprovação." });
                }
                string idsConvertidos = string.Empty;

                ids.ForEach(id =>
                {
                    idsConvertidos += string.Format("{0},", id);
                });

                _arquivoService.CancelarPontuacoes(idsConvertidos.Substring(0, idsConvertidos.Length - 1));

                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public FileResult DownloadCSV(int id)
        {
            List<AprovacaoPontosModel> lstRetorno = _arquivoService.ObterPontosAprovacao(id);
            string filename = string.Format("PONTUACOES_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(geraCSVAprovacaoPontuacao(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public FileResult DownloadCSVArquivoPontos(int? catalogoId, DateTime dtInicio, DateTime dtFim, int? campanhaId, int? tipoArquivoId, string statusArquivoId, int? anoCampanha)
        {
            int total = 0;

            if (campanhaId == 0)
                campanhaId = null;

            List<ArquivoPontosModel> lstRetorno = _arquivoService.ObterArquivoPontos(catalogoId, dtInicio, dtFim, campanhaId, tipoArquivoId, statusArquivoId, anoCampanha, out total);

            string filename = string.Format("ARQUIVOPONTOS_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(geraCSVArquivoPontos(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public FileResult DownloadCSVCancelamento(int id)
        {
            int total = 0;

            List<CancelamentoPontosModel> lstRetorno = _arquivoService.ObterPontosCancelamento(id, out total);

            string filename = string.Format("CANCELAMENTO_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(geraCSVCancelamento(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public FileResult DownloadImportacaoErroCSV(int id)
        {
            List<ImportacaoErroDetalheModel> lstRetorno = _arquivoService.ObterImportacaoErroCSV(id);
            string filename = string.Format("IMPORTACAO_ERRO_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(geraCSVImportacaoErro(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public ActionResult Cancelamento()
        {
            return View(getCatalogos());
        }

        public ActionResult RelatorioArquivo(int? idcatalogo, int? idTipoArquivo, int? anoCampanha)
        {
            if (idcatalogo > 0)
                ViewBag.TipoArquivo = getTipoArquivoCatalogo((int)idcatalogo);
            else
                ViewBag.TipoArquivo = new List<TipoArquivo>();

            ViewBag.AnoCampanha = GetAnoCampanhas();

            if (idTipoArquivo > 0)
                ViewBag.Campanha = getCampanha(idcatalogo, idTipoArquivo, anoCampanha);
            else
                ViewBag.Campanha = new List<DTO.CampanhaModel>();

            ViewBag.StatusArquivo = getStatusArquivos();

            return View(getCatalogos());

        }

        [HttpPost]
        public JsonResult ObterCampanha(int? idcatalogo, int? idTipoArquivo, int? anoCampanha)
        {
            var list = getCampanha(idcatalogo, idTipoArquivo, anoCampanha);
            var data = new object();

            if (idTipoArquivo < 0)
            {
                data = new { ok = false };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            if (list.Count > 0)
            {
                data = new { ok = true, campanha = list };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = new { ok = false };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObterTipoArquivo(int? idcatalogo)
        {
            var list = getTipoArquivoCatalogo(idcatalogo);
            var data = new object();

            if (list.Count > 0)
            {
                data = new { ok = true, tipoArquivo = list };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = new { ok = false };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LogErroImportacaoPontuacao()
        {
            return View();
        }


        public ActionResult Lote()
        {
            return View(new GrupoLTM.WebSmart.Admin.Models.PontuacaoModel());
        }

        public ActionResult Campanha()
        {
            return View(new List<CampanhaPontuacaoModel>());
        }

        [HttpGet]
        public JsonResult ListarPontosCampanha(int? campanhaId, int? campanhaPeriodoId)
        {

            var listPontuacao = CarregaResultadoCalculadoAprovao(campanhaId.Value, campanhaPeriodoId.Value);
            return Json(new DataTableRetorno<AprovacaoResultadoCalculadoModel>()
            {
                aaData = listPontuacao,
                sEcho = "1"
            }, "application/json"
            , System.Text.UTF8Encoding.UTF8
            , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAprovacaoPonto(int? tipoPontuacaoId)
        {

            var listPontuacao = CarregaAprovacaoPontuacao(tipoPontuacaoId);
            return Json(new DataTableRetorno<GrupoLTM.WebSmart.Admin.Models.PontuacaoModel>()
            {
                aaData = listPontuacao,
                sEcho = "1"
            }, "application/json"
            , System.Text.UTF8Encoding.UTF8
            , JsonRequestBehavior.AllowGet);

            //return Json(new DataTableRetorno<PontuacaoModel>()
            //{
            //    aaData = listPontuacao,
            //    sEcho = "1",
            //    MaxJsonLenght,
            //}, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }


        [HttpGet]
        public ActionResult AprovacaoTipo()
        {

            return View(new List<GrupoLTM.WebSmart.Admin.Models.PontuacaoModel>());
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult AprovarPontuacao(string arrP)
        {
            var data = new object();

            try
            {
                string arr = arrP.Replace("\\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
                List<int> listId = (
                  from string s in arr.Split(',')
                  select Convert.ToInt32(s)
                ).ToList<int>();

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //inicializando repositórios
                    IRepository repPontuacao = context.CreateRepository<Pontuacao>();
                    IRepository repConfiguracao = context.CreateRepository<ConfiguracaoCampanha>();

                    //efetuando a consulta das pontuações selecionadas:
                    var pontuacao = repPontuacao.Filter<Pontuacao>(x => listId.Contains(x.Id) && x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.PendenteEnvio);

                    //Verifica se Lista contém mais de um Registro
                    if (pontuacao.Count() <= 0)
                    {
                        data = new { ok = false, msg = "Nenhuma Linha encontrada para crédito de pontos" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }


                    var creditopontos = pontuacao.Select(l => new
                    {
                        l.Id,
                        l.Participante.Login,
                        l.Participante.Nome,
                        l.Descricao,
                        Valor = l.Pontos
                    }).ToList();

                    //Efetua o processo de geração do Excel
                    string dir = "pontuacao/credito/";
                    string nomeArquivo;
                    var dtPontos = DataConverter.ToDataTable(creditopontos);

                    //verifica se o arquivo de crédito foi gerado com sucesso
                    if (!CsvExport.WriteCSVFile(dtPontos, dir, null, out nomeArquivo))
                    {
                        data = new { ok = false, msg = "Erro ao gerar o arquivo de crédito, tente novamente mais tarde." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                    //Se tudo estiver Ok, efetua a atualização do status da Pontuação
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(1, 0, 0, 0)))
                    {
                        //Atualiza Status da pontuação
                        var resultado = AprovaPontuacao(arr);

                        if (resultado)
                        {

                            //INICIO DO PROCEDIMENTO DE DISPARO DE EMAIL.

                            //Total de pontos do arquivo gerado
                            double valorCredito;
                            valorCredito = 0;
                            foreach (var item in creditopontos)
                            {
                                valorCredito = valorCredito + (double)item.Valor;
                            }


                            //Coleta os dados da configuração para o envio de email
                            var configuracaoDisparo = repConfiguracao.All<ConfiguracaoCampanha>().Select(x => new
                            {
                                x.EMAILCREDITOPONTOS,
                            }).ToList().FirstOrDefault();



                            //Gera Configuração do corpo do email de disparos de Emails
                            StringBuilder emailMessage = new StringBuilder();
                            emailMessage = emailMessage.Append("<Center><H3><b>Crédito de Pontos Campanha</Center></H3></b></br>");
                            emailMessage = emailMessage.Append("Nome do Arquivo creditado:" + nomeArquivo + "</br>");
                            emailMessage = emailMessage.Append("Total de créditos do Arquivo: " + Convert.ToString(valorCredito) + "</br>" + "</br>");
                            emailMessage = emailMessage.Append("Faça o upload do arquivo em anexo na respectiva campanha da plataforma live para que os créditos sejam feito nas contas dos participantes.");

                            //Efetua disparo de Email com o arquivo de crédito:
                            string anexoEmail = Settings.Caminho.StoragePath + dir + nomeArquivo + Settings.Caminho.StorageToken;
                            GrupoLTM.WebSmart.Infrastructure.Mail.Email email = new GrupoLTM.WebSmart.Infrastructure.Mail.Email();
                            var envioResposta = email.EnviarEmailAutenticado
                                                (Settings.EmailConfiguracao.CreditoDisplay,
                                                 Settings.EmailConfiguracao.CreditoFrom,
                                                 configuracaoDisparo.EMAILCREDITOPONTOS,
                                                 Settings.EmailConfiguracao.CreditoEmailSubject,
                                                 emailMessage.ToString(),
                                                 Settings.EmailConfiguracao.CreditoSMTP,
                                                 Settings.EmailConfiguracao.CreditoPortaSMTP,
                                                 Settings.EmailConfiguracao.CreditoEmailSMTP,
                                                 Settings.EmailConfiguracao.CreditoSenhaSMTP,
                                                 anexoEmail);

                            if (!envioResposta.emailEnviado)
                            {
                                data = new { ok = false, msg = "Falha ao Enviar o Email do arquivo de crédito, tente novamente mais tarde." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            ts.Complete();
                        }
                        else
                        {
                            data = new { ok = true, msg = "Ocorreu um erro ao efetuar o processo de Aprovação, tente novamente mais tarde." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                    }

                }

                data = new { ok = true, msg = "Pontuação Aprovada com sucesso! Um Arquivo de crédito foi gerado para que o responsável efetue o crédito na plataforma." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                data = new { ok = false, msg = "Ocorreu um erro ao efetuar o processo de Aprovação, tente novamente mais tarde." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult AprovarPontuacaoCampanha(int? campanhaId, int? campanhaPeriodoId)
        {
            var data = new object();

            try
            {
                var login = LoginHelper.GetLoginModel();

                var pontuacoes = CampanhaService.AprovaPontuacaoCampanha(
                                        campanhaId.Value,
                                        campanhaPeriodoId.Value,
                                        login.Id,
                                        (int)EnumDomain.TipoPontuacao.Campanha,
                                        (int)EnumDomain.StatusPontuacao.Enviado);

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repCampanha = context.CreateRepository<Campanha>();
                    IRepository repCampanhaPeriodo = context.CreateRepository<CampanhaPeriodo>();
                    IRepository repConfiguracao = context.CreateRepository<ConfiguracaoCampanha>();

                    //Efetua o processo de geração do Excel
                    string dir = "pontuacao/credito/";
                    string nomeArquivo;

                    //verifica se o arquivo de crédito foi gerado com sucesso
                    if (!CsvExport.WriteCSVFile(pontuacoes, dir, null, out nomeArquivo))
                    {
                        data = new { ok = false, msg = "Erro ao gerar o arquivo de crédito, tente novamente mais tarde." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                    //Total de pontos do arquivo gerado
                    double valorCredito = pontuacoes.AsEnumerable().Sum(r => r.Field<double>("Pontos"));

                    //Coleta os dados da configuração para o envio de email
                    var configuracaoDisparo = repConfiguracao.All<ConfiguracaoCampanha>().Select(x => new
                    {
                        x.EMAILCREDITOPONTOS,
                    }).ToList().FirstOrDefault();

                    var nomeCampanha = repCampanha.Filter<Campanha>(x => x.Id == campanhaId.Value).FirstOrDefault().Nome;
                    var nomePeriodo = repCampanha.Filter<CampanhaPeriodo>(x => x.Id == campanhaPeriodoId.Value).FirstOrDefault().Nome;

                    //Gera Configuração do corpo do email de disparos de Emails
                    StringBuilder emailMessage = new StringBuilder();
                    emailMessage = emailMessage.Append("<Center><H3><b>Crédito de Pontos Campanha - " + nomeCampanha + " - " + nomePeriodo + "</Center></H3></b></br>");
                    emailMessage = emailMessage.Append("Nome do Arquivo creditado:" + nomeArquivo + "</br>");
                    emailMessage = emailMessage.Append("Total de créditos do Arquivo: " + Convert.ToString(valorCredito) + "</br>" + "</br>");
                    emailMessage = emailMessage.Append("Faça o upload do arquivo em anexo na respectiva campanha da plataforma live para que os créditos sejam feito nas contas dos participantes.");

                    //Efetua disparo de Email com o arquivo de crédito:
                    string anexoEmail = Settings.Caminho.StoragePath + dir + nomeArquivo + Settings.Caminho.StorageToken;
                    GrupoLTM.WebSmart.Infrastructure.Mail.Email email = new GrupoLTM.WebSmart.Infrastructure.Mail.Email();
                    var envioResposta = email.EnviarEmailAutenticado
                                        (Settings.EmailConfiguracao.CreditoDisplay,
                                         Settings.EmailConfiguracao.CreditoFrom,
                                         configuracaoDisparo.EMAILCREDITOPONTOS,
                                         string.Concat(Settings.EmailConfiguracao.CreditoEmailSubject, " - ", nomeCampanha),
                                         emailMessage.ToString(),
                                         Settings.EmailConfiguracao.CreditoSMTP,
                                         Settings.EmailConfiguracao.CreditoPortaSMTP,
                                         Settings.EmailConfiguracao.CreditoEmailSMTP,
                                         Settings.EmailConfiguracao.CreditoSenhaSMTP,
                                         anexoEmail);

                    if (!envioResposta.emailEnviado)
                    {
                        data = new { ok = false, msg = "Falha ao Enviar o Email do arquivo de crédito, tente novamente mais tarde." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                data = new { ok = true, msg = "Pontuação Aprovada com sucesso! Um Arquivo de crédito foi gerado para que o responsável efetue o crédito na plataforma." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                data = new { ok = false, msg = "Ocorreu um erro ao efetuar o processo de Aprovação, tente novamente mais tarde." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult ReprovarPontuacaoCampanha(int? campanhaId, int? campanhaPeriodoId)
        {
            var data = new object();

            try
            {
                var login = LoginHelper.GetLoginModel();

                var pontuacoes = CampanhaService.AprovaPontuacaoCampanha(
                                        campanhaId.Value,
                                        campanhaPeriodoId.Value,
                                        login.Id,
                                        (int)EnumDomain.TipoPontuacao.Campanha,
                                        (int)EnumDomain.StatusPontuacao.Reprovada);

                data = new { ok = true, msg = "Pontuação reprovada com sucesso!" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                data = new { ok = false, msg = "Ocorreu um erro ao efetuar o processo de reprovação, tente novamente mais tarde." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult ReprovarPontuacao(string arrP)
        {
            var data = new object();

            try
            {
                string arr = arrP.Replace("\\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
                List<int> listId = (
                  from string s in arr.Split(',')
                  select Convert.ToInt32(s)
                ).ToList<int>();

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //inicializando repositórios
                    IRepository repPontuacao = context.CreateRepository<Pontuacao>();

                    //efetuando a consulta das pontuações selecionadas:
                    var pontuacao = repPontuacao.Filter<Pontuacao>(x => listId.Contains(x.Id) && x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.PendenteEnvio);

                    //Verifica se Lista contém mais de um Registro
                    if (pontuacao.Count() <= 0)
                    {
                        data = new { ok = false, msg = "Nenhuma Linha encontrada para crédito de pontos" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                    //Se tudo estiver Ok, efetua a atualização do status da Pontuação para reprovada
                    using (TransactionScope ts = new TransactionScope())
                    {
                        var resultado = ReprovaPontuacao(arr);

                        if (!resultado)
                        {
                            data = new { ok = true, msg = "Erro ao reprovar a pontuação, tente novamente mais tarde." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        ts.Complete();
                        data = new { ok = true, msg = "Pontuação reprovada com sucesso! As pontuações foram inativadas e não poderão ser selecionadas para aprovação no futuro" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch
            {
                data = new { ok = false, msg = "Erro ao reprovar a pontuação, tente novamente mais tarde." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Lote(HttpPostedFileBase FileArquivo, int PeriodoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();

                    if (FileArquivo != null)
                    {
                        //Faz o upload

                        var uploadFileResult = UploadFile.Upload(
                            FileArquivo,
                            Settings.Extensoes.ExtensoesPermitidasArquivos,
                            int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                            "pontuacao/");

                        //Não salvou, erro
                        if (!uploadFileResult.arquivoSalvo)
                        {
                            data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Carrega importação
                            string msg = "";
                            string urlDownloadArquivoErro = "";

                            //Carrega importacao, importa arquivo, processa e retorna arquivo de erro
                            if (!CarregaPontuacaoImportacao(
                                "pontuacao/" + uploadFileResult.nomeArquivoGerado,
                                FileArquivo.FileName,
                                uploadFileResult.nomeArquivoGerado,
                                PeriodoId,
                                out msg, out urlDownloadArquivoErro))
                            {
                                data = new { ok = false, msg = msg, arquivoErro = urlDownloadArquivoErro };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            else //Sucesso
                            {
                                data = new { ok = true, msg = "Arquivo gravado com sucesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Por favor, selecione o campo Arquivo." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Internal Functions"
        internal bool AprovaPontuacao(string pontuacao)
        {
            //Chama a o Método de atualização da pontuacao.
            var login = LoginHelper.GetLoginModel();
            var resultado = PontuacaoService.AprovarPontuacao(pontuacao, login.Id);
            if (resultado)
            {
                return true;
            }
            return false;
        }

        internal bool ReprovaPontuacao(string pontuacao)
        {
            //Chama a o Método de atualização da pontuacao.
            var login = LoginHelper.GetLoginModel();
            var resultado = PontuacaoService.ReprovarPontuacao(pontuacao, login.Id);
            if (resultado)
            {
                return true;
            }
            return false;
        }

        internal bool CarregaPontuacaoImportacao(string filePath, string nomeArquivo, string nomeArquivoGerado, int periodoId, out string msg, out string urlDownloadArquivoErro)
        {
            //Grava o arquivo no banco de dados
            var arquivo = ArquivoService.CadastrarArquivo(filePath, nomeArquivo, nomeArquivoGerado, EnumDomain.TipoArquivo.CreditoPontos);
            bool blnSucesso = false;
            msg = "";
            urlDownloadArquivoErro = "";

            try
            {
                //Arquivo para dataset
                var dsArquivo = ExcelDataReader.OpenExcel(filePath);

                //Verifica se existem linhas
                if (dsArquivo.Tables.Count == 0)
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no arquivo. Não existem linhas para serem importadas.";
                    return blnSucesso;
                }

                //Valida Qtde de Colunas (3)
                int countColumns = 3;
                if (dsArquivo.Tables[0].Columns.Count != countColumns)
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no layout do arquivo. Número de campos não confere.";
                    return blnSucesso;
                }

                //Valida a existência dos Campos
                string[] arrLayoutArquivo = new string[countColumns];
                arrLayoutArquivo.SetValue("Login", 0);
                arrLayoutArquivo.SetValue("Descricao", 1);
                arrLayoutArquivo.SetValue("Pontos", 2);

                foreach (var item in arrLayoutArquivo)
                {
                    if (!dsArquivo.Tables[0].Columns.Contains(item))
                    {
                        //Atualiza o status
                        ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                        blnSucesso = false;
                        msg = "Erro no layout do arquivo. Nomes dos campos não conferem.";
                        return blnSucesso;
                    }
                }

                //Valida se o crédito é apenas numérico

                foreach (DataRow item in dsArquivo.Tables[0].Rows)
                {
                    if (item["Pontos"].GetType() != typeof(double))
                    {
                        //Atualiza o status
                        ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                        blnSucesso = false;
                        msg = "Coluna pontos somente aceita valores numéricos, favor verificar o arquivo e enviar novamente.";
                        return blnSucesso;
                    }
                }

                //Importa para o banco de dados (bulk)
                if (!PontuacaoService.ImportarArquivoPontuacao(dsArquivo.Tables[0], arquivo.Id))
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no layout do arquivo. Dados inválidos.";
                    return blnSucesso;
                }


                //Processo registros no banco (Procedure)
                int countErro = 0;
                var login = LoginHelper.GetLoginModel();

                if (!PontuacaoService.ProcessaPontuacaoArquivo(arquivo.Id, periodoId, out countErro, login.Id))
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroDadosInvalidos);
                    string strErro = "Erro no processamento do arquivo. Dados inválidos.";

                    //Erros > 0, exporta arquivo
                    if (countErro > 0)
                    {
                        //Retorno o endereço para download do arquivo de retorno
                        urlDownloadArquivoErro = PontuacaoService.ExportaArquivoPontuacaoErro(arquivo.Id);
                    }

                    blnSucesso = false;
                    msg = strErro;
                    return blnSucesso;
                }

                //Ok, Atualiza o status processado
                ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.Processado);

                //Retorna sucesso
                blnSucesso = true;
                msg = "Arquivo importado com sucesso!";
                return blnSucesso;
            }
            catch (Exception)
            {
                ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);
                throw;
            }
        }

        internal List<AprovacaoResultadoCalculadoModel> CarregaResultadoCalculadoAprovao(int campanhaId, int campanhaPeriodoId)
        {
            return CampanhaService.SelecionaResultadoCalculadoAprovao(campanhaId, campanhaPeriodoId);
        }

        internal List<GrupoLTM.WebSmart.Admin.Models.PontuacaoModel> CarregaAprovacaoPontuacao(int? tipoPontuacaoId)
        {
            List<GrupoLTM.WebSmart.Admin.Models.PontuacaoModel> lstPontuacao = new List<GrupoLTM.WebSmart.Admin.Models.PontuacaoModel>();

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPontuacao = context.CreateRepository<Pontuacao>();
                int statusPontuacao = Convert.ToInt32(EnumDomain.StatusPontuacao.PendenteEnvio);

                var list = repPontuacao.Filter<Pontuacao>(x => x.TipoPontuacaoId == tipoPontuacaoId && x.StatusPontuacaoId == statusPontuacao).Select(l => new
                {
                    l.Id,
                    l.ParticipanteId,
                    l.Participante.Login,
                    l.Participante.CPF,
                    l.Participante.Nome,
                    l.TipoPontuacaoId,
                    tipoPOntuacao = l.TipoPontuacao.Nome,
                    l.StatusPontuacaoId,
                    statusPontuacao = l.StatusPontuacao.Nome,
                    l.Descricao,
                    l.Pontos,
                    Periodo = l.Periodo.Nome,
                    l.DataInclusao
                }).ToList();

                foreach (var item in list)
                {
                    lstPontuacao.Add(new GrupoLTM.WebSmart.Admin.Models.PontuacaoModel
                    {
                        Id = item.Id,
                        ParticipanteId = item.ParticipanteId,
                        Login = item.Login,
                        Cpf = item.CPF,
                        Nome = item.Nome,
                        TipoPontuacaoId = item.TipoPontuacaoId,
                        TipoPontuacao = item.tipoPOntuacao,
                        StatusPontuacaoId = item.StatusPontuacaoId,
                        StatusPontuacao = item.statusPontuacao,
                        DescricaoPonto = item.Descricao,
                        Ponto = (float)item.Pontos,
                        Periodo = item.Periodo,
                        DataInclusao = item.DataInclusao.ToString()

                    });
                }

                return lstPontuacao;
            }
        }

        internal List<DTO.CampanhaModel> getCampanha(int? catalogoId, int? idTipoArquivo, int? anoCampanha)
        {
            List<SelectListItem> itens = new List<SelectListItem>();

            if (idTipoArquivo == 0) idTipoArquivo = null;

            var retorno = _campanhaService.ObterCampanhasPorCatalogoTipoArquivo(catalogoId, idTipoArquivo, anoCampanha);

            return retorno;
        }

        internal List<TipoArquivo> getTipoArquivoCatalogo(int? catalogoId)
        {
            List<SelectListItem> itens = new List<SelectListItem>();

            if (catalogoId == 0) catalogoId = null;

            var retorno = _arquivoService.ObterTipoArquivoPorCatalogo(catalogoId);

            return retorno;
        }

        internal List<int> GetAnoCampanhas()
        {
            return new ProgramaIncentivoService().GetAll().OrderBy(y => y.Ano).Select(x => x.Ano ?? 0).Distinct().ToList();
        }

        internal List<StatusArquivo> getStatusArquivos()
        {
            List<SelectListItem> itens = new List<SelectListItem>();

            var retorno = _arquivoService.ObterStatusArquivos();

            return retorno;
        }


        private byte[] geraCSVImportacaoErro(List<ImportacaoErroDetalheModel> lstErros)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id Arquivo;Nome Arquivo;Programa de incentivo;Data e hora do processamento pela LTM;Data e Hora do erro;ERRO;Linha;Lote;Conteúdo da Linha;Registro;CSV\r\t");

            foreach (var erro in lstErros)
                sb.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}\r\t",
                    erro.ArquivoId,
                    erro.Nome,
                    erro.TipoArquivo,
                    erro.DataInclusao,
                    erro.DataInclusaoErro,
                    erro.DescricaoErro,
                    erro.IdOrigemNormalizada,
                    erro.LoteId,
                    erro.LinhaConteudo,
                    erro.TipoRegistro,
                    erro.CSV));

            //return Encoding.ASCII.GetBytes(sb.ToString());
            return Encoding.Default.GetBytes(sb.ToString());

        }
        private byte[] geraCSVAprovacaoPontuacao(List<AprovacaoPontosModel> lstArquivo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Data e hora do processamento pela LTM;Nome do arquivo enviado pela Avon;Total de revendedores processados;Total de pontos importados;Total de pontos para liberação de crédito\r\t");

            foreach (var arquivo in lstArquivo)
                sb.Append(string.Format("{0};{1};{2};{3};{4}\r\t", arquivo.DataTerminoProcessamento, arquivo.Nome, arquivo.QuantidadeRevendedorasProcessadas, arquivo.PontosImportados, arquivo.PontosLiberados));

            //return Encoding.ASCII.GetBytes(sb.ToString());
            return Encoding.Default.GetBytes(sb.ToString());

        }
        private byte[] geraCSVCancelamento(List<CancelamentoPontosModel> lstArquivo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Data e hora do processamento pela LTM;Campanha;Tipo de Arquivo;Nome do arquivo enviado pela Avon;Total de pontos importados;Total de pontos para cancelamento;Total de pontos liberados;Total de pontos cancelados\r\t");

            foreach (var arquivo in lstArquivo)
                sb.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}\r\t", arquivo.DataProcessamentoTexto, arquivo.IncentiveProgramDescriptionHeader, arquivo.TipoArquivo, arquivo.Nome, arquivo.TotalPontos, arquivo.PontosPendentes, arquivo.PontosDisponiveis, arquivo.PontosCancelados));

            //return Encoding.ASCII.GetBytes(sb.ToString());
            return Encoding.Default.GetBytes(sb.ToString());

        }
        private byte[] geraCSVArquivoPontos(List<ArquivoPontosModel> lstArquivo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Inclusão;Campanha;Tipo;Punch | Crédito;Status;Qtd. Linhas;Qtd. Revendedoras;Pontos Disponíveis;Pontos Pendentes;Pontos Cancelados\r\t");

            foreach (var arquivo in lstArquivo)
                sb.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}\r\t", arquivo.DataInclusao, arquivo.IncentiveProgramDescriptionHeader, arquivo.TipoArquivo, arquivo.Nome, arquivo.StatusArquivoNome, arquivo.QuantidadeLinhas, arquivo.QuantidadeRevendedorasProcessadas, arquivo.PontosDisponiveisInicial, arquivo.PontosPendentesInicial, arquivo.PontosCanceladosInicial));

            return Encoding.Default.GetBytes(sb.ToString());

        }

        #endregion

    }

}
