using AutoMapper;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.ExtensionMethods;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;


namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class SmsAvulsoController : BaseController
    {
        private readonly SMSService smsService = new SMSService();
        private readonly ArquivoService arquivoService = new ArquivoService();
        
        private static readonly string FILE_PATH = "sms-agendamentos/";
        private static readonly string FILE_PATH_BASERA = "sms-agendamentos/baseAvulso/";
        private static readonly string FILE_PATH_BASEIMAGEM = "whatsapp/";

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var list = smsService.ObterAgendamentosAtivos().Where(x => x.SMSTipoId == 2);
            var models = new List<SMSAgendamentoModel>();

            if (list != null && list.Count() > 0)
            {
        
                foreach (var item in list)
                {
                    var model = Mapper.Map<SMSAgendamentoModel>(item);
                    model.RecorrenciaDescricao = item.Recorrencia.GetEnumDescription();
                    models.Add(model);
                    
                }
            }

            return View(models);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Create()
        {
            MontarViewBag();

            var model = new SMSAgendamentoModel
            {
                Id = Guid.NewGuid()
            };

            model.InicioDisparos = DateTime.Today.AddHours(DateTime.Now.Hour + 1);

            return View("CreateEdit", model);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(SMSAgendamentoModel model)
        {
            await ProcessarArquivo(model);

            if (!ValidarModel(model))
                return ViewCreateEdit(model);

            var agendamento = Mapper.Map<SMSAgendamento>(model);

            smsService.AdicionarAgendamento(agendamento);

            await ProcessarImagem(model);
            await ProcessarRAsDoArquivo(model);

            return RedirectToAction("Index");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Edit(Guid id)
        {

            var existente = smsService.ObterAgendamento(id);
            if (existente == null)
                return RedirectToAction("Create");

            MontarViewBag();

            var model = Mapper.Map<SMSAgendamentoModel>(existente);
        
            if (model.UploadBaseRAId.HasValue)
            {
                var arquivo = ArquivoService.ObterArquivoPorId(model.UploadBaseRAId.Value);
                model.UploadBaseRANome = arquivo?.Nome;
                model.UploadBaseRANomeGerado = arquivo?.NomeGerado;
            }

            return View("CreateEdit", model);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Edit(SMSAgendamentoModel model)
        {
            await ProcessarArquivo(model);

            if (!ValidarModel(model))
                return ViewCreateEdit(model);

            var existente = smsService.ObterAgendamento(model.Id);

            Mapper.Map(model, existente, model.GetType(), existente.GetType());

            smsService.AtualizarAgendamento(existente, true);
            
            await ProcessarRAsDoArquivo(model);

            return RedirectToAction("Index");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Ativar(Guid id)
        {
            var agendamento = smsService.ObterAgendamento(id);
            agendamento.Ativar();
            smsService.AtualizarAgendamento(agendamento);
            return RedirectToAction("Index");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Inativar(Guid id)
        {
            var agendamento = smsService.ObterAgendamento(id);
            agendamento.Inativar();
            smsService.AtualizarAgendamento(agendamento);
            return RedirectToAction("Index");
        }

        [System.Web.Mvc.HttpGet]
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

        [System.Web.Mvc.HttpGet]
        public ActionResult DownloadArquivo(int id)
        {
            var filePath = FILE_PATH_BASERA;
            var arquivo = ArquivoService.ObterArquivoPorId(id);

            var stream = DownloadFile.Download(arquivo.NomeGerado, filePath);
            return File(stream, "text/plain", arquivo.Nome);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult DownloadModelo()
        {
            var stream = DownloadFile.Download("ModeloAvulso.csv", FILE_PATH);
            return File(stream, "text/plain", "modelo importação Avulso.csv");
        }

        #region metodos privados

        private void MontarViewBag()
        {
            ViewBag.TipoRecorrencias = MontarTipoRecorrencias();
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
        private async Task<SMSAgendamentoImagem> ProcessarImagem(SMSAgendamentoModel model)
        {
            if (model.ArquivoUploadBaseImagem == null)
                return null;

            var file = model.ArquivoUploadBaseImagem;
            var id = model.Id;
            var imagem = SalvarImagem(file, id);

            if (imagem == null)
                return null;

            return imagem;
        }
        private SMSAgendamentoImagem SalvarImagem(HttpPostedFileBase file, Guid id)
        {
            if (file == null)
                return null;

            file.InputStream.Position = 0;

            var fileName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + file.FileName.Substring(file.FileName.LastIndexOf("."));

            var result = UploadFile.UploadWhatsApp(
                file, fileName,
                new string[] { ".jpg", ".png" },
                 Convert.ToInt32(Settings.TamanhoArquivos.TamanhoMaximoKBExcel), FILE_PATH_BASEIMAGEM);

            if (!result.arquivoSalvo)
            {
                ModelState.AddModelError("ArquivoUploadBaseImagem", result.mensagem);
                return null;
            }

            var imagem = ImagemService.CadastrarImagem(
                fileName,
                FILE_PATH_BASEIMAGEM,
                id);

            return imagem;
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

            await smsService.AtualizarInfDoAgendamentoAvulso(agendamentoId, ras.ToArray());
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

        private bool ValidarModel(SMSAgendamentoModel model)
        {
            if (!ModelState.IsValid)
                return false;

            if (!ValidarQuantidadeCaracteresDaMensagem(model))
                return false;

            if (!ValidarArquivo(model))
                return false;
            
            return true;
        }

        private bool ValidarArquivo(SMSAgendamentoModel model)
        {
            if (model.ArquivoUploadBaseRA == null)
                return true;

            int linha = 0;
            var _linhasDeErro = new List<int>();
            var colunas = "";
            
            model.ArquivoUploadBaseRA.InputStream.Position = 0;
            var srReader = new System.IO.StreamReader(model.ArquivoUploadBaseRA.InputStream);
            

            while ((colunas = srReader.ReadLine()) != null)
            {
                linha++;

                string[] coluna = colunas.Split(';');
                Response.Write(coluna[0] + " - " + coluna[1]
                               + ": " + coluna[2] + "<br />");

                if (linha == 1)
                continue;

                if (coluna[0].Trim().Count() != 11 )
                {
                    _linhasDeErro.Add(linha);
                           continue;
                }
                
            }

         if (_linhasDeErro.Count() > 0)
            {
                var msgErro = $"As seguintes linhas contém Telefones inválidos: {String.Join(", ", _linhasDeErro)}";
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

        private ActionResult ViewCreateEdit(SMSAgendamentoModel model)
        {
            MontarViewBag();
            return View("CreateEdit", model);
        }

        #endregion

    }
}