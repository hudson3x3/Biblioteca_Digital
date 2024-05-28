using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using System.Data;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.DTO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using GrupoLTM.WebSmart.Admin.Helpers;
using GrupoLTM.WebSmart.Domain.Repository;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class ResgateOfflineController : BaseController
    {
        private ArquivoService _arquivoService;
        private readonly PrimeiroAcessoService primeiroAcessoService = new PrimeiroAcessoService();
        private CatalogoService _catalogoService = new CatalogoService();
        private static readonly string FILE_PATH_BASEACESSO = "forcarprimeiroacesso/baseRA/";
        private ParticipanteService _participanteService = new ParticipanteService();


        public ResgateOfflineController()
        {
            this._arquivoService = new ArquivoService();
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult AprovacaoResgateOffline()
        {
            return View();
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador, EnumDomain.Perfis.Aluno)]
        [HttpGet]
        public ActionResult ForcarPrimeiroAcesso()
        {
            ForcarPrimeiroAcessoLoteModel acesso = new ForcarPrimeiroAcessoLoteModel();
            return View(acesso);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult IncluirResgatesOffline()
        {
            Models.ResgateOffLineModel resgate = new Models.ResgateOffLineModel();
            resgate.ddlCatalogo = getCatalogos(true);
            return View(resgate);
        }
        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult RelArquivoResgateOffline()
        {
            ViewBag.StatusArquivo = getStatusArquivos();
            return View();
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarAprovacaoResgatesOffline()
        {
            try
            {
                this.inicializaDadosDatatable();
                int total = 0;

                List<AprovacaoResgatesOfflineModel> lstRetorno = _arquivoService.ObterResgatesOfflineAprovacao(startExibir, regExibir, out total);

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
        public JsonResult ListarArquivosResgatesOffline(DateTime? dtInicio, DateTime? dtFim, int? statusArquivoId)
        {
            try
            {
                this.inicializaDadosDatatable();
                int total = 0;

                List<AprovacaoResgatesOfflineModel> lstRetorno = _arquivoService.ObterListaArquivosResgatesOffline(startExibir, regExibir, dtInicio, dtFim, statusArquivoId, out total);

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
                gravaLogErro("Erro ao Listar Arquivos de Resgate Offline e Primeiro Acesso", ex.Message, "GrupoLTM.WebSmart.Admin", string.Format("ListarArquivosResgatesOffline"), "jobCatalog");
                return Json(new { Sucesso = false, Mensagem = "Erro ao Listar Arquivos de Resgate Offline e Primeiro Acesso" + ex.Message });
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public JsonResult AprovarResgatesOffline(List<int> ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    ArquivoService.AtualizaArquivo(id, EnumDomain.StatusArquivo.Aprovado);
                    gravaLogAprovacaoArquivo(id, "AprovarResgateOffline");
                }
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao aprovar arquivos", ex.Message, "GrupoLTM.WebSmart.Admin", string.Format("AprovarResgatesOffline(ArquivosId:{0})", ids), "jobCatalog");
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }
        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public JsonResult ReprovarResgatesOffline(List<int> ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    ArquivoService.AtualizaArquivo(id, EnumDomain.StatusArquivo.Reprovado);
                    gravaLogAprovacaoArquivo(id, "ReprovarResgateOffline");
                }
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao aprovar arquivos", ex.Message, "GrupoLTM.WebSmart.Admin", string.Format("AprovarResgatesOffline(ArquivosId:{0})", ids), "jobCatalog");
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public async Task<JsonResult> GetProductSku(string productSkuId)
        {
            try
            {
                this.inicializaDadosDatatable();
                int mktPlaceCatalogoId = ConfiguracaoService.MktPlaceCatalogoLogarComo();
                List<Services.Model.ProductSku> productSkus = new List<Services.Model.ProductSku>();
                Services.Model.ProductSku product = await ObterProductSku(productSkuId, mktPlaceCatalogoId);
                if (product != null)
                    productSkus.Add(product);


                List<Services.Model.ProductSku> productSku = await ObterProductSkuByOriginalProductSku(product.defaultSku.originalProductSkuId, product.VendorId);
                int total = productSku.Count;

                return Json(new
                {
                    aaData = productSku.Skip(startExibir).Take(regExibir).ToList(),
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
        public async Task<JsonResult> GetProductSkuByOriginalProductSku(string OriginalProductSkuId)
        {
            try
            {
                this.inicializaDadosDatatable();
                var vendorId = 0;
                List<Services.Model.ProductSku> productSku = await ObterProductSkuByOriginalProductSku(OriginalProductSkuId, vendorId);
                int total = productSku.Count;
                return Json(new
                {
                    aaData = productSku.Skip(startExibir).Take(regExibir).ToList(),
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
        public FileResult DownloadCSVResgateOffline(int id)
        {
            List<CSVResgateOffLineModel> lstRetorno = primeiroAcessoService.ObterCSVResgateOffLine(id);
            string filename = string.Format("RESGATES_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return File(gerarCSVResgateOffline(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        private byte[] gerarCSVResgateOffline(List<CSVResgateOffLineModel> lstResgate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("RA;ProductSkuId;ProdutoId;Produto;Valor;Valor Real;Original Sku;Fator De Conversão;Name;Quantidade;Pedido Id;Erro;DataEnvio\r\t");

            foreach (var r in lstResgate)
                sb.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}\r\t", r.Username, r.ProductSku, r.ProductId, r.Product, r.Value, r.ValueReal, r.OriginalSku, r.ConversionRate, r.Name, r.Quantity, r.PedidoId, r.Erro, r.DataEnvio == DateTime.MinValue ? "" : r.DataEnvio.ToString()));

            //return Encoding.ASCII.GetBytes(sb.ToString());
            return Encoding.Default.GetBytes(sb.ToString());
        }
        internal List<StatusArquivo> getStatusArquivos()
        {
            List<SelectListItem> itens = new List<SelectListItem>();

            var retorno = _arquivoService.ObterStatusArquivos().Where(x =>
            x.Id == (int)EnumDomain.StatusArquivo.Aprovado
            || x.Id == (int)EnumDomain.StatusArquivo.Enviado
            || x.Id == (int)EnumDomain.StatusArquivo.Inconsistente
            || x.Id == (int)EnumDomain.StatusArquivo.Reprovado
            || x.Id == (int)EnumDomain.StatusArquivo.Processado
            ).ToList();

            return retorno;
        }
        internal async Task<Services.Model.ProductSku> ObterProductSku(string productSkuId, int mktPlaceCatalogoId)
        {
            try
            {
                var participantes = ParticipanteService.ListaParticipanteAtivoDmenos1(mktPlaceCatalogoId);
                var accountNumber = "";
                var mktPlaceParticipantId = "";
                foreach (DataRow row in participantes.Rows)
                {
                    accountNumber = row["accountNumber"].ToString();
                    mktPlaceParticipantId = row["mktPlaceParticipantId"].ToString();

                    if (accountNumber != "")
                        break;
                }
                Services.Model.TokenErrors error = new Services.Model.TokenErrors();
                var productSku = await MarketPlaceService.GetProductSku(productSkuId, accountNumber, Convert.ToInt32(mktPlaceParticipantId), mktPlaceCatalogoId);
                if (productSku != null)
                {
                    var catalogo = _catalogoService.ObterCatalogoContext(mktPlaceCatalogoId);
                    //productSku.ConversionRate = catalogo != null ? catalogo.ConversionRate : 100;
                    productSku.ValueReal = Convert.ToDecimal(productSku.defaultSku.sellingPrice / productSku.ConversionRate);
                    productSku.CatalogoId = catalogo.Codigo;
                }
                return productSku;
            }
            catch (Exception ex)
            {
                gravaLogProcessamento("Error" + ex.Message, "ObterProdutoSku", "ObterProdutoSku", "Admin");
                throw ex;
            }
        }
        internal async Task<List<Services.Model.ProductSku>> ObterProductSkuByOriginalProductSku(string originalProductSkuId, long? vendorId)
        {
            try
            {

                using (var context = UnitOfWorkFactory.Create())
                {
                    var repUsuario = context.CreateRepository<UsuarioAdm>();
                    var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                    List<DTO.CatalogoModel> catalogos = new List<DTO.CatalogoModel>();
                    catalogos = _catalogoService.ListarCatalogos(usuario.PerfilId).Where(x => x.Ativo == true).ToList();

                    List<Services.Model.ProductSku> productSkus = new List<Services.Model.ProductSku>();

                    foreach (DTO.CatalogoModel cat in catalogos)
                    {
                        try
                        {
                            //if (cat.MktPlaceSupplierId == null)
                            //    continue;

                            var participantes = ParticipanteService.ListaParticipanteAtivoDmenos1(Convert.ToInt32(cat.Codigo));

                            string accountNumber = "";
                            var mktPlaceParticipantId = "";

                            foreach (DataRow row in participantes.Rows)
                            {
                                accountNumber = row["accountNumber"].ToString();
                                mktPlaceParticipantId = row["mktPlaceParticipantId"].ToString();
                                if (accountNumber != "")
                                {
                                    break;
                                }
                            }
                            if (mktPlaceParticipantId == "")
                                continue;
                            Services.Model.TokenErrors error = new Services.Model.TokenErrors();

                            //Buscar o product sku apartir do Original Product Sku                    
                            Services.Model.ProductSku originalProductSku = new Services.Model.ProductSku();

                            var mktPlaceSupplierId = vendorId > 0 ? Convert.ToInt32(vendorId) : Convert.ToInt32(cat.Codigo);

                            originalProductSku = await MarketPlaceService.GetProductSkuByOrignalProductSku(originalProductSkuId, accountNumber, Convert.ToInt32(mktPlaceParticipantId), Convert.ToInt32(cat.Codigo), mktPlaceSupplierId);

                            if (originalProductSku != null)
                            {
                                Services.Model.ProductSku productSku = new Services.Model.ProductSku();
                                productSku = await MarketPlaceService.GetProductSku(originalProductSku.defaultSku.productSkuId, accountNumber, Convert.ToInt32(mktPlaceParticipantId), Convert.ToInt32(cat.Codigo));

                                //productSku.ConversionRate = cat.ConversionRate == 0 ? 100 : cat.ConversionRate;
                                productSku.ValueReal = originalProductSku.defaultSku.defaultPrice != null ? Convert.ToDecimal(originalProductSku.defaultSku.defaultPrice) : 0;
                                productSku.CatalogoId = cat.Codigo;

                                productSkus.Add(productSku);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                            throw ex;
                        }

                    }

                    return productSkus.ToList();
                }            
            }
            catch (Exception ex)
            {
                gravaLogProcessamento("Error" + ex.Message, "ObterProdutoSku", "ObterProdutoSku", "Admin");
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ForcarPrimeiroAcesso(ForcarPrimeiroAcessoLoteModel primeiroAcessoLote)
        {

            var data = new object();
            if (primeiroAcessoLote.ArquivoUploadBaseRA.ContentLength == 0)
            {
                data = new { ok = false, msg = "O Arquivo não pode estar em branco. Por favor fazer o Download do arquivo de exemplo e tente novamente." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            HttpPostedFileBase ArquivoUploadBaseRA = primeiroAcessoLote.ArquivoUploadBaseRA;
            primeiroAcessoLote.PageName = "extrato";
            primeiroAcessoLote.IdAdmin = LoginHelper.GetLoginModel().Id;

            try
            {
                if (ArquivoUploadBaseRA == null)
                    return null;

                ArquivoUploadBaseRA.InputStream.Position = 0;

                var filePath = FILE_PATH_BASEACESSO;
                var result = UploadFile.Upload(
                    ArquivoUploadBaseRA,
                    new string[] { ".txt", ".csv" },
                    Convert.ToInt32(Settings.TamanhoArquivos.TamanhoMaximoKBExcel),
                    filePath);

                if (!result.arquivoSalvo)
                {
                    data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + result.mensagem };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                var arquivo = ArquivoService.CadastrarArquivo(
                    filePath,
                    ArquivoUploadBaseRA.FileName,
                    result.nomeArquivoGerado,
                    Domain.Enums.EnumDomain.TipoArquivo.ForcarPrimeiroAcesso);

                await GravarListaForcarPrimeiroAcesso(arquivo.NomeGerado, FILE_PATH_BASEACESSO, arquivo.Id, primeiroAcessoLote);

                data = new { ok = true, msg = "Arquivo importado com sucesso. Aguarde o processamento." };
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                data = new { ok = false, msg = "Ocorreu uma falha na importação. Tente novamente. Erro:" + ex.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> IncluirResgatesOffline(Models.ResgateOffLineModel resgateoffline)
        {
            try
            {
                resgateoffline.listaResgateoffline = new List<Models.ResgateOffLine>();
                HttpPostedFileBase ArquivoUploadBaseRA = resgateoffline.ArquivoUploadBaseRA;
                if (ArquivoUploadBaseRA == null)
                    return null;
                if (resgateoffline.producktsSkuByCatalogo == null && !String.IsNullOrEmpty(resgateoffline.jsonProducktSkuByCatalogo))
                    resgateoffline.producktsSkuByCatalogo = JsonConvert.DeserializeObject<List<ProducktSkuByCatalogo>>(resgateoffline.jsonProducktSkuByCatalogo);

                foreach (ProducktSkuByCatalogo producktSkuByCatalogo in resgateoffline.producktsSkuByCatalogo)
                {
                    Models.ResgateOffLine item = new Models.ResgateOffLine();
                    Services.Model.ProductSku produto = await ObterProductSku(producktSkuByCatalogo.ProductSku, producktSkuByCatalogo.MktPlaceCatalogoId);

                    if (produto != null)
                    {
                        item.ProductId = produto.ProductId;
                        item.Product = produto.Product;
                        item.ProductSku = produto.defaultSku.productSkuId;
                        item.Value = Convert.ToDecimal(produto.defaultSku.sellingPrice);
                        item.ValueReal = Convert.ToDecimal(produto.defaultSku.sellingPrice / produto.ConversionRate);
                        item.OriginalSku = produto.defaultSku.originalProductSkuId;
                        item.ConversionRate = produto.ConversionRate;
                        item.MktPlaceCatalogoId = (int)produto.CatalogoId;
                        item.UrlImage = (string)produto.defaultSku.skuImages[0].mediumImage;
                        Catalogo catalogoModel = new Catalogo();
                        catalogoModel = _catalogoService.ObterCatalogo(item.MktPlaceCatalogoId);
                        if (catalogoModel != null)
                        {
                            //item.ProjectId = catalogoModel.IdCampanha;
                            item.CatalogId = catalogoModel.Codigo;
                            item.PageName = "extrato";
                            item.IdAdmin = LoginHelper.GetLoginModel().Id;
                            resgateoffline.listaResgateoffline.Add(item);
                        }
                    }
                }

                ArquivoUploadBaseRA.InputStream.Position = 0;

                var filePath = FILE_PATH_BASEACESSO;
                var result = UploadFile.Upload(
                    ArquivoUploadBaseRA,
                    new string[] { ".txt", ".csv" },
                    Convert.ToInt32(Settings.TamanhoArquivos.TamanhoMaximoKBExcel),
                    filePath);
                var data = new object();

                if (!result.arquivoSalvo)
                {
                    data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + result.mensagem };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                var arquivo = ArquivoService.CadastrarArquivo(
                    filePath,
                    ArquivoUploadBaseRA.FileName,
                    result.nomeArquivoGerado,
                    Domain.Enums.EnumDomain.TipoArquivo.ResgateOffline);

                await GravarListaResgatePrimeiroAcesso(arquivo.NomeGerado, FILE_PATH_BASEACESSO, arquivo.Id, resgateoffline.listaResgateoffline);

                data = new { ok = true, msg = "Arquivo importado com sucesso. Aguarde o processamento." };
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var data = new { ok = false, msg = "Ocorreu uma falha na importação. Tente novamente. Erro: " + ex.Message };

                if (ex.Message == "Já foi adicionado um item com a mesma chave.")
                {
                    data = new { ok = false, msg = "Ocorreu uma falha na importação. Erro: Existe registros duplicados no arquivo." };
                }
   
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }

        private async Task GravarListaResgatePrimeiroAcesso(string filePathName, string FILE_PATH_BASEACESSO, int arquivoId, List<Models.ResgateOffLine> resgateoffline)
        {
            Stream stream = null;

            stream = DownloadFile.Download(filePathName, FILE_PATH_BASEACESSO);
            stream.Position = 0;

            var ras = ObterRAs(stream);

            var admin = LoginHelper.GetLoginModel();
            if (admin == null)
                new Exception();

            foreach (Models.ResgateOffLine item in resgateoffline)
            {
                await primeiroAcessoService.IncluirProdutoResgateOffLine(arquivoId, item.CatalogId, item.PageName, item.IdAdmin, item.ProjectId, item.ProductSku, item.ProductId, item.Product, item.Value, item.ValueReal, item.OriginalSku, item.ConversionRate, item.UrlImage);
            }
            await primeiroAcessoService.AtualizarRAsPrimeiroAcesso(arquivoId, ras);

        }

        private async Task GravarListaForcarPrimeiroAcesso(string filePathName, string FILE_PATH_BASEACESSO, int arquivoId, ForcarPrimeiroAcessoLoteModel primeiroAcessoLote)
        {
            Stream stream = null;

            stream = DownloadFile.Download(filePathName, FILE_PATH_BASEACESSO);
            stream.Position = 0;

            var ras = ObterRAs(stream);

            var admin = LoginHelper.GetLoginModel();
            if (admin == null)
                new Exception();

            await primeiroAcessoService.IncluirForcarPrimeiroAcessoCatalogo(arquivoId, primeiroAcessoLote.PageName, primeiroAcessoLote.IdAdmin);

            await primeiroAcessoService.AtualizarRAsPrimeiroAcesso(arquivoId, ras);
        }
        private Dictionary<string, int> ObterRAs(Stream stream)
        {
            string slinha;
            Dictionary<string, int> ras = new Dictionary<string, int>();

            stream.Position = 0;
            var srReader = new System.IO.StreamReader(stream);

            while ((slinha = srReader.ReadLine()) != null)
            {
                var lItem = slinha.Split(',');

                int quantity = 0;
                if (lItem.Count() > 0 && lItem[0].Trim() != "")
                {
                    if (lItem.Count() == 2)
                    {
                        Int32.TryParse(lItem[1].Trim(), out quantity);
                    }
                    ras.Add(lItem[0].Trim(), quantity);
                }
            }
            return ras;
        }

        [HttpPost]
        public ActionResult ForcarPrimeiroAcessoMktPlace(string Login, string pageName)
        {
            try
            {
                int idAdmin = LoginHelper.GetLoginModel().Id;
                PrimeiroAcessoService primeiroAcesso = new PrimeiroAcessoService();
                string retorno = primeiroAcesso.ForcarPrimeiroAcessoMktPlace(Login, pageName, idAdmin);
                var data = new { Sucesso = true, Mensagem = retorno };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gravaLogErro("Ocorreu um erro ao Processar Primeiro Acesso Marketplace", ex.Message, "LoginController", "ForcarPrimeiroAcessoMktPlace", "jobCatalog");
                var data = new { Sucesso = false, Mensagem = "Ocorreu um erro inesperado" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        internal void gravaLogAprovacaoArquivo(int ArquivoId, string Acao)
        {
            LogAprovacaoArquivo logAprovacaoArquivo = new LogAprovacaoArquivo()
            {
                Acao = Acao,
                Login = LoginHelper.GetLoginModel().Login,
                IP = HttpContext.Request.UserHostAddress.ToString(),
                ArquivoId = ArquivoId,
                DataInclusao = DateTime.Now
            };

            new LogAprovacaoArquivoService().GravarLogAprovacaoArquivo(logAprovacaoArquivo);
        }
        static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "ResgateOfflineController",
                Pagina = string.Empty,
                Codigo = codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
        static void gravaLogProcessamento(string Mensagem, string Source, string Metodo, string codigo)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + Mensagem);
            var logProcessamento = new LogProcessamentoDb()
            {
                Metodo = Metodo,
                Controller = "ResgateOfflineController",
                Codigo = codigo,
                Mensagem = Mensagem,
                Source = Source,
                DataInclusao = DateTime.Now
            };

            var logService = new LogService();
            logService.GravarLogProcessamento(logProcessamento);
        }

    }
}