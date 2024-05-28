using AutoMapper;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Helpers;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.ExtensionMethods;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class SmsController : BaseController
    {
        private readonly SMSService smsService = new SMSService();
        private readonly ArquivoService arquivoService = new ArquivoService();
        private readonly ImagemService imagemService = new ImagemService();
        private readonly CatalogoService catalogoService = new CatalogoService();
        private readonly ProgramaIncentivoService programaService = new ProgramaIncentivoService();
        private readonly SMSService _smsService = new SMSService();

        private static readonly string FILE_PATH = "sms-agendamentos/";
        private static readonly string FILE_PATH_BASERA = "sms-agendamentos/baseRA/";

        #region "Metodos Get"
        [HttpGet]
        public ActionResult Index()
        {
            var list = smsService.ObterAgendamentosAtivos().Where(x => x.SMSTipoId == 1);
            var models = new List<SMSAgendamentoModel>();

            if (list != null && list.Count() > 0)
            {

                using (var context = UnitOfWorkFactory.Create())
                {
                    var repUsuario = context.CreateRepository<UsuarioAdm>();
                    var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                    var catalogos = catalogoService.ListarCatalogos(usuario.PerfilId);

                    foreach (var item in list)
                    {
                        var model = Mapper.Map<SMSAgendamentoModel>(item);
                        var nomeCatalogo = item.MktPlaceCatalogoId == 0 ? "Todos os Catálogos" : catalogos.FirstOrDefault(x => x.Codigo == item.MktPlaceCatalogoId)?.Nome;
                        model.MktPlaceCatalogo = nomeCatalogo;
                        model.RecorrenciaDescricao = item.Recorrencia.GetEnumDescription();
                        models.Add(model);
                    }

                }
            }

            return View(models);
        }

        public ActionResult RelSMSStatusEnvio()
        {
            return View(getCatalogos());
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarSMSStatusAgendamento(int? CatalogoId, Guid? SMSAgendamentoId, int? SMSTipoId, string TipoEnvioMensagem,
            string DDDCelular, DateTime? InicioDisparo, DateTime? FimDisparo, string InicioEnviadoEm, DateTime? FimEnviadoEm)
        {
            try
            {
                this.inicializaDadosDatatable();

                List<SMSStatusAgendamento> lstRetorno = new List<SMSStatusAgendamento>();
                lstRetorno = ObterSMSStatusAgendamento(CatalogoId, SMSAgendamentoId, SMSTipoId, TipoEnvioMensagem, DDDCelular, InicioDisparo, FimDisparo, InicioEnviadoEm, FimEnviadoEm).Skip(startExibir).Take(regExibir).ToList();

                int total = lstRetorno != null ? lstRetorno.Count() : 0;

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
                gravaLogErro("Erro ao Listar SMS Status Agendamento", ex.Message, "GrupoLTM.WebSmart.Admin", "ListarSMSStatusAgendamento", "jobCatalog");
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        
        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public JsonResult ListarSMSAgendamentos(int? CatalogoId, Guid? SMSAgendamentoId, int? SMSTipoId, string TipoEnvioMensagem,
            string DDDCelular, DateTime? InicioDisparo, DateTime? FimDisparo, string InicioEnviadoEm, DateTime? FimEnviadoEm)
        {
            long mktPlaceCatalogoId = 0;
            if (CatalogoId != null)
            {
                var MktPlaceCatalogo = catalogoService.ObterCatalogoPorId(CatalogoId.Value);
                mktPlaceCatalogoId = MktPlaceCatalogo == null ? 0 : MktPlaceCatalogo.Codigo;
            }
            var total = 0;
            try
            {
                this.inicializaDadosDatatable();

                var list = smsService.ObterSMSAgendamentos(mktPlaceCatalogoId, SMSAgendamentoId, SMSTipoId, TipoEnvioMensagem, DDDCelular, InicioDisparo, FimDisparo, InicioEnviadoEm, FimEnviadoEm, out total);
                List<SMSAgendamentoTable> smsAgendamentoTable = new List<SMSAgendamentoTable>();

                if (list != null && list.Count() > 0)
                {

                    using (var context = UnitOfWorkFactory.Create())
                    {
                        var repUsuario = context.CreateRepository<UsuarioAdm>();
                        var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                        var catalogos = catalogoService.ListarCatalogos(usuario.PerfilId);

                        foreach (var item in list)
                        {
                            var nomeCatalogo = item.MktPlaceCatalogoId == 0 ? "Todos os Catálogos" : catalogos.FirstOrDefault(x => x.Codigo == item.MktPlaceCatalogoId)?.Nome;

                            SMSAgendamentoTable smsagenda = new SMSAgendamentoTable();

                            smsagenda.Id = item.Id;
                            smsagenda.Titulo = item.Titulo;
                            smsagenda.TextoMensagem = item.TextoMensagem;
                            smsagenda.TipoEnvioMensagem = item.TipoEnvioMensagem;
                            smsagenda.MktPlaceCatalogo = nomeCatalogo;
                            smsagenda.RecorrenciaDescricao = item.Recorrencia.GetEnumDescription();
                            smsagenda.TipoBase = item.Upload ? "Upload" : "Dinâmica";
                            smsagenda.Status = (item.Status).GetEnumDescription();
                            smsagenda.InicioDisparos = item.InicioDisparos;
                            smsagenda.CSV = "item.CSV";
                            // smsagenda.TipoBaseImagem = item.UploadImagem ? "Upload" : "Dinâmica";
                            //smsagenda.Imagem = "Item.Imagem";

                            smsAgendamentoTable.Add(smsagenda);
                        }

                    }

                }

                return Json(new
                {
                    aaData = smsAgendamentoTable.Skip(startExibir).Take(regExibir).ToList(),
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao Listar SMS Agendamento", ex.Message, "GrupoLTM.WebSmart.Admin", "ListarSMSAgendamento", "jobCatalog");

                return Json(new
                {
                    aaData = "",
                    iTotalDisplayRecords = total,
                    iTotalRecords = total,
                    sEcho = echo
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<SMSStatusAgendamento> ObterSMSStatusAgendamento(int? CatalogoId, Guid? SMSAgendamentoId, int? SMSTipoId, string TipoEnvioMensagem,
            string DDDCelular, DateTime? InicioDisparo, DateTime? FimDisparo, string InicioEnviadoEm, DateTime? FimEnviadoEm)
        {
            long mktPlaceCatalogoId = 0;
            if (CatalogoId != null)
            {
                var MktPlaceCatalogo = catalogoService.ObterCatalogoPorId(CatalogoId.Value);
                mktPlaceCatalogoId = MktPlaceCatalogo == null ? 0 : MktPlaceCatalogo.Codigo;
            }
            if (SMSTipoId == 0)
                SMSTipoId = null;
            if (DDDCelular == "")
                DDDCelular = null;
            try
            {
                int total;

                List<SMSStatusAgendamento> lstRetorno = _smsService.ObterSMSStatusAgendamento(mktPlaceCatalogoId, SMSAgendamentoId, SMSTipoId, TipoEnvioMensagem, DDDCelular, InicioDisparo, FimDisparo, InicioEnviadoEm, FimEnviadoEm, out total).ToList();

                return lstRetorno;
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro ao Listar SMS Status Agendamento", ex.Message, "GrupoLTM.WebSmart.Admin", "ObterSMSStatusAgendamento", "jobCatalog");
                return null;
            }
        }

        [HttpGet]
        public ActionResult RelSMSStatusEnvio(int? idCatalogo, int? IdSMSTipo)
        {

            ViewBag.SMSTipo = getSMSTipo();
            ViewBag.TipoEnvioMensagem = MontarTipoMensagem();
            if (IdSMSTipo > 0 && IdSMSTipo > 0)
                ViewBag.SMSAgendamentos = _smsService.ObterSMSAgendamentoPorCatalogoTipo(idCatalogo, IdSMSTipo);
            else
                ViewBag.SMSAgendamentos = new List<SMSAgendamento>();

            return View(getCatalogos());
        }

        [HttpPost]
        public JsonResult ObterSMSAgendamentoPorCatalogoTipo(int? idCatalogo, int? idSMSTipo)
        {
            var list = _smsService.ObterSMSAgendamentoPorCatalogoTipo(idCatalogo, idSMSTipo);
            var data = new object();

            List<SMSAgendamentoLista> ListRetorno = new List<SMSAgendamentoLista>();

            foreach (SMSAgendamento agendamento in list)
            {
                ListRetorno.Add(new SMSAgendamentoLista
                {
                    Id = agendamento.Id,
                    Titulo = agendamento.Titulo
                });
            }

            if (list.Count() > 0)
            {
                data = new { ok = true, smsAgendamentos = ListRetorno};
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = new { ok = false };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]

        [HttpGet]
        public FileResult DownloadCsv(Guid? SMSAgendamentoId)
        {
            List<SMSStatusAgendamento> lstRetorno = new List<SMSStatusAgendamento>();
            lstRetorno = ObterSMSStatusAgendamento(null, SMSAgendamentoId,null, null, null, null, null, null, null);

            string filename = string.Format("AGENDAMENTOS_{0}.csv", SMSAgendamentoId.ToString());

            return File(geraCSVAprovacaoPontuacao(lstRetorno), System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        private byte[] geraCSVAprovacaoPontuacao(List<SMSStatusAgendamento> lstAgendamento)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Título;Celular;Tipo Envio;Mensagem;Centro de Custo;Agendado Para;Enviado Em;Status Envio;Motivo\r\t");

            if (lstAgendamento != null)
            {
                foreach (var agendamento in lstAgendamento)
                    sb.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}\r\t", agendamento.Titulo,
                        agendamento.Celular, agendamento.TipoEnvio, agendamento.Mensagem, agendamento.CentroCusto,
                        agendamento.AgendadoPara, agendamento.EnviadoEm, agendamento.StatusMensagem, agendamento.Motivo));
            }
            return Encoding.Default.GetBytes(sb.ToString());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new SMSAgendamentoModel
            {
                Id = Guid.NewGuid()
            };

            model.Ano = MontarViewBag(model.ProgramasId);
            model.InicioDisparos = DateTime.Today.AddHours(DateTime.Now.Hour + 1);
            model.InicioPeriodoCredito = DateTime.Today.AddHours(DateTime.Now.Hour + 1);
            model.FimPeriodoCredito = DateTime.Today.AddHours(DateTime.Now.Hour + 1);

            return View("CreateEdit", model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {

            var existente = smsService.ObterAgendamento(id);
            if (existente == null)
                return RedirectToAction("Create");
            
            var model = Mapper.Map<SMSAgendamentoModel>(existente);

            model.ProgramasId = smsService.ObterProgramasDoAgendamento(id).Select(x => x.ProgramaIncentivoId).ToArray();

            model.Ano = MontarViewBag(model.ProgramasId);

            if (model.UploadBaseRAId.HasValue)
            {
                var arquivo = ArquivoService.ObterArquivoPorId(model.UploadBaseRAId.Value);
                model.UploadBaseRANome = arquivo?.Nome;
                model.UploadBaseRANomeGerado = arquivo?.NomeGerado;
            }

            return View("CreateEdit", model);
        }

        [HttpGet]
        public ActionResult Ativar(Guid id)
        {
            var agendamento = smsService.ObterAgendamento(id);
            agendamento.Ativar();
            smsService.AtualizarAgendamento(agendamento);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Inativar(Guid id)
        {
            var agendamento = smsService.ObterAgendamento(id);
            agendamento.Inativar();
            smsService.AtualizarAgendamento(agendamento);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Execucoes(Guid id)
        {
            var model = new SMSExecucoesModel();

            var agendamento = smsService.ObterAgendamento(id);
            var execucoes = smsService.ObterExecucoes(id);

            if (agendamento != null)
                model.Agendamento = Mapper.Map<SMSAgendamentoModel>(agendamento);

            if (execucoes != null && execucoes.Count() > 0)
                model.Execucoes = Mapper.Map<IEnumerable<SMSExecucaoModel>>(execucoes);

            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadArquivo(int id)
        {
            var filePath = FILE_PATH_BASERA;
            var arquivo = ArquivoService.ObterArquivoPorId(id);

            var stream = DownloadFile.Download(arquivo.NomeGerado, filePath);
            return File(stream, "text/plain", arquivo.Nome);
        }

        [HttpGet]
        public ActionResult DownloadModelo()
        {
            var stream = DownloadFile.Download("modelo.csv", FILE_PATH);
            return File(stream, "text/plain", "modelo importação RAs.csv");
        }
        
        #endregion

        #region "Metodos Post"
        [HttpPost]
        public async Task<ActionResult> Create(SMSAgendamentoModel model)
        {
            await ProcessarArquivo(model);
         
            if(model.PeriodoFechado > 0 )
            {
                model.InicioPeriodoCredito = null;
                model.FimPeriodoCredito = null;
            }else {
                model.PeriodoFechado = 0;
            }

            if (!ValidarModel(model))
                return ViewCreateEdit(model);

            var agendamento = Mapper.Map<SMSAgendamento>(model);

            smsService.AdicionarAgendamento(agendamento);
            await ProcessarImagem(model);

            smsService.AtualizarProgramasDoAgendamento(agendamento.Id, model.ProgramasId);

            await ProcessarRAsDoArquivo(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SMSAgendamentoModel model)
        {
            await ProcessarArquivo(model);
            await ProcessarImagem(model);

            if (!ValidarModel(model))
                return ViewCreateEdit(model);

            var existente = smsService.ObterAgendamento(model.Id);

            Mapper.Map(model, existente, model.GetType(), existente.GetType());

            smsService.AtualizarAgendamento(existente, true);

            smsService.AtualizarProgramasDoAgendamento(existente.Id, model.ProgramasId);

            await ProcessarRAsDoArquivo(model);

            return RedirectToAction("Index");
        }

        #endregion

        #region "Internal Functions"
        internal IEnumerable<SMSTipo> getSMSTipo()
        {
            IEnumerable<SMSTipo> retorno = _smsService.ObterSMSTipoPorCatalogo();
            return retorno;
        }

        #endregion

        #region "Metodos Privados"

        private int MontarViewBag(int[] programasIdSelecionados)
        {
            ViewBag.Catalogos = MontarCatalogos();
            ViewBag.TipoRecorrencias = MontarTipoRecorrencias();
            ViewBag.TipoEnvioMensagem = MontarTipoMensagem();
            return MontarProgramas(programasIdSelecionados);
        }

        private int MontarProgramas(int[] programasIdSelecionados)
        {
            var list = programaService.GetAll();

            var todosOsProgramas = list.Where(x => x.Nome != "SMS_Avulso");
            ViewBag.TodosOsProgramas = todosOsProgramas;

            var anos = todosOsProgramas.Select(x => x.Ano).Distinct().OrderByDescending(x => x);
            var anoSelecionado = anos.FirstOrDefault();

            ViewBag.AnoProgramas = new SelectList(anos, anoSelecionado);

            var selecionados = todosOsProgramas.Where(x => programasIdSelecionados.Contains(x.Id)).OrderByDescending(x => x.Ano);
            var programaAno = todosOsProgramas.Where(x => x.Ano == anoSelecionado && selecionados.All(y => y.Id != x.Id));
            var programaList = selecionados.Concat(programaAno);

            ViewBag.Programas = new MultiSelectList(programaList, "Id", "Nome");

            return anoSelecionado ?? 0;
        }

        private IEnumerable<SelectListItem> MontarCatalogos(bool inDate = false)
        {
            List<CatalogoModel> catalogoList = new List<CatalogoModel>();

            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();
                var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                if (!inDate)
                    catalogoList = catalogoService.ListarCatalogos(usuario.PerfilId).OrderBy(x => x.Id).ToList();
                else
                    catalogoList = catalogoService.ListarCatalogos(usuario.PerfilId).Where(x => x.DataInclusao < DateTime.Now && x.DataAlteracao > DateTime.Now)
                        .OrderBy(x => x.Id).ToList();

                return new SelectList(catalogoList, "MktPlaceCatalogoId", "Nome");
            }
        }
        private IEnumerable<SelectListItem> MontarTipoRecorrencias()
        {
            var tipoRecorrencias = Enum.GetValues(typeof(SMSTipoRecorrencia)) as SMSTipoRecorrencia[];

            var selectListItems = new List<SelectListItem>();

            foreach (var tipo in tipoRecorrencias)
            {
                var item = new SelectListItem
                {
                    Text = tipo.GetEnumDescription(),
                    Value = ((int)tipo).ToString()
                };

                selectListItems.Add(item);
            }

            return new SelectList(selectListItems, "Value", "Text");
        }
        private IEnumerable<SelectListItem> MontarTipoMensagem()
        {
            var tipoMensagem = Enum.GetValues(typeof(TipoMensagem)) as TipoMensagem[];

            var selectListItems = new List<SelectListItem>();

            foreach (var tipo in tipoMensagem)
            {
                var item = new SelectListItem
                {
                    Text = tipo.GetEnumDescription(),
                    Value = ((int)tipo).ToString()
                };

                selectListItems.Add(item);
            }

            return new SelectList(selectListItems, "Value", "Text");
        }

        #region Upload Imagem

        private async Task<SMSAgendamentoImagem> ProcessarImagem(SMSAgendamentoModel model)
        {
            if (model.ArquivoUploadBaseImagem == null)
                return null;

            var file = model.ArquivoUploadBaseImagem;
            var id = model.Id;
            var imagem =  SalvarImagem(file, id);

            if (imagem == null)
                return null;

            return  imagem;
        }
        private SMSAgendamentoImagem SalvarImagem(HttpPostedFileBase file, Guid id)
        {
            if (file == null)
                return null;

            file.InputStream.Position = 0;

            var fileName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + file.FileName.Substring(file.FileName.LastIndexOf("."));

            var filePath = "whatsapp";
            var result = UploadFile.UploadWhatsApp(
                file,fileName,
                new string[] { ".jpg", ".png" }, Convert.ToInt32(Settings.TamanhoArquivos.TamanhoMaximoKBExcel), filePath);

            if (!result.arquivoSalvo)
            {
                ModelState.AddModelError("ArquivoUploadBaseImagem", result.mensagem);
                return null;
            }

            var imagem = ImagemService.CadastrarImagem(
                fileName,
                filePath,
                id);

            return imagem;
        }
   
        #endregion

        #region Upload Arquivo
        private Arquivo SalvarArquivo(HttpPostedFileBase file)
        {
            if (file == null)
                return null;

            file.InputStream.Position = 0;

            var filePath = FILE_PATH_BASERA;
            var result = UploadFile.Upload(
                file,
                new string[] { ".txt", ".csv" },
                Convert.ToInt32(Settings.TamanhoArquivos.TamanhoMaximoKBExcel),
                filePath);

            if (!result.arquivoSalvo)
            {
                ModelState.AddModelError("ArquivoUploadBaseRA", result.mensagem);
                return null;
            }

            var arquivo = ArquivoService.CadastrarArquivo(
                filePath,
                file.FileName,
                result.nomeArquivoGerado,
                Domain.Enums.EnumDomain.TipoArquivo.SMSAgendamentoBaseRA);

            return arquivo;
        }
        private async Task<Arquivo> ProcessarArquivo(SMSAgendamentoModel model)
        {
            if (model.ArquivoUploadBaseRA == null)
                return null;

            var file = model.ArquivoUploadBaseRA;

            var arquivo = SalvarArquivo(file);

            model.UploadBaseRAId = arquivo?.Id;
            model.UploadBaseRANome = arquivo?.Nome;
            model.UploadBaseRANomeGerado = arquivo?.NomeGerado;

            if (arquivo == null)
                return null;

            return arquivo;
        }

        private async Task ProcessarRAsDoArquivo(SMSAgendamentoModel model)
        {
            if (!model.UploadBaseRAId.HasValue)
                return;

            var agendamentoId = model.Id;
            Stream stream = null;

            if (model.ArquivoUploadBaseRA != null)
            {
                stream = model.ArquivoUploadBaseRA.InputStream;
            }
            else
            {
                var arquivo = ArquivoService.ObterArquivoPorId(model.UploadBaseRAId.Value);
                stream = DownloadFile.Download(arquivo.NomeGerado, FILE_PATH_BASERA);
            }

            stream.Position = 0;

            var ras = ObterRAs(stream);

            await smsService.AtualizarRAsDoAgendamento(agendamentoId, ras.ToArray());
        }
        private string[] ObterRAs(Stream stream)
        {
            var linha = 0;
            string ra = "";
            var ras = new List<string>();

            stream.Position = 0;
            var srReader = new System.IO.StreamReader(stream);

            while ((ra = srReader.ReadLine()) != null)
            {
                linha++;

                if (linha == 1)
                    continue;

                ras.Add(ra.Trim());
            }

            return ras.ToArray();
        }
        #endregion

        #region Validações
        private bool ValidarModel(SMSAgendamentoModel model)
        {
            if (!ModelState.IsValid)
                return false;

            if (!ValidarQuantidadeCaracteresDaMensagem(model))
                return false;

            if (!ValidarArquivo(model))
                return false;

            if (!ValidarProgramas(model))
            {
                ModelState.AddModelError("ValidacaoProgramas", "Selecione apenas um Programa para envio da mensagem.");
                return false;
            }

            model.ProgramasId = model.ProgramasId.Where(x => x != 0).ToArray();

            return true;
        }
        private bool ValidarProgramas(SMSAgendamentoModel model)
        {
            return !(model.ProgramasId.Count() > 1 && model.TextoMensagem.ToLower().Contains("@programa"));
        }
        private bool ValidarArquivo(SMSAgendamentoModel model)
        {
            if (model.ArquivoUploadBaseRA == null)
                return true;

            int linha = 0;
            var _linhasDeErro = new List<int>();
            var ra = "";

            model.ArquivoUploadBaseRA.InputStream.Position = 0;
            var srReader = new System.IO.StreamReader(model.ArquivoUploadBaseRA.InputStream);

            while ((ra = srReader.ReadLine()) != null)
            {
                linha++;

                if (linha == 1)
                    continue;

                if (ra.Trim().Count() != 8)
                {
                    _linhasDeErro.Add(linha);
                    continue;
                }

                int n;
                if (Int32.TryParse(ra, out n) == false)
                {
                    _linhasDeErro.Add(linha);
                    continue;
                }
            }

            if (_linhasDeErro.Count() > 0)
            {
                var msgErro = $"As seguintes linhas contém RAs inválidos: {String.Join(", ", _linhasDeErro)}";
                ModelState.AddModelError("ArquivoUploadBaseRA", msgErro);
                return false;
            }

            return true;
        }
        private bool ValidarQuantidadeCaracteresDaMensagem(SMSAgendamentoModel model)
        {
            var mensagem = model.TextoMensagem;
            var qtdAdicional = 0;

            foreach (var tag in Settings.SMS.TagsDinamicas.Tags)
            {
                if (!mensagem.Contains(tag))
                    continue;

                while (1 == 1)
                {
                    var index = mensagem.IndexOf(tag);

                    if (index < 0)
                        break;

                    mensagem = mensagem.Remove(index, tag.Count());
                    qtdAdicional += Settings.SMS.TagsDinamicas.Tamanhos[tag];
                }
            }

            var qtdChars = mensagem.Count() + qtdAdicional;

            if (qtdChars > 160)
            {
                ModelState.AddModelError("TextoMensagem", $"O texto da mensagem está com {qtdChars} caracteres, ele deve conter no máximo 160 caracteres. Obs: As tags dinâmicas alteram o tamanho final do texto conforme informações de cada tag.");
                return false;
            }

            return true;
        }
        #endregion

        private ActionResult ViewCreateEdit(SMSAgendamentoModel model)
        {
            model.Ano = MontarViewBag(model.ProgramasId);
            return View("CreateEdit", model);
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
                Controller = "Sms",
                Pagina = string.Empty,
                Codigo = codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
        #endregion

    }
}