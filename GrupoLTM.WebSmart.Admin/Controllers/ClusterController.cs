using System;
using System.Globalization;
using GrupoLTM.WebSmart.Admin.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Facade;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class ClusterController : BaseController
    {
        private readonly ClusterService _service = new ClusterService();
        private readonly ClusterProductService _serviceProduct = new ClusterProductService();
        private readonly CookieManager _cookie = new CookieManager();
        private readonly string _tempIdKey = "ClusterTempId";

        // GET: Cluster
        public ActionResult Index()
        {
            var model = _service.Listar();

            return View(model);
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult Create()
        {
            var model = new ClusterModel();

            return View(model);
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult Edit(int id)
        {
            var model = _service.GetById(id);

            if (_cookie.IsSet(_tempIdKey))
                _cookie.Remove(_tempIdKey);

            return View("Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Upload()
        {
            var linhas = 0;

            try
            {
                if (Request.Files.Count > 0)
                {
                    var tempId = Guid.NewGuid().ToString();

                    _cookie.Set(_tempIdKey, tempId, 1440);

                    var file = Request.Files[0];

                    if (file != null)
                        linhas = await _service.UploadParticipantesAsync(file.InputStream, tempId);
                }
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, message = ex.Message, linhas }, JsonRequestBehavior.AllowGet);
            }

            var data = new { ok = true, linhas };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Save(ClusterModel model)
        {
            var data = new { ok = true, msg = "Cluster criado com sucesso." };
            try
            {
                DateTime inicio;
                DateTime fim;

                if (string.IsNullOrWhiteSpace(model.MensagemErroValidacao))
                {
                    data = new { ok = false, msg = "Mensagem de erro de validação inválida." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (!DateTime.TryParseExact(model.DataInicioFormatado, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out inicio))
                {
                    data = new { ok = false, msg = "Data de início inválida." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (!DateTime.TryParseExact(model.DataFimFormatado, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out fim))
                {
                    data = new { ok = false, msg = "Data final inválida." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (DateTime.Compare(fim, inicio) <= 0)
                {
                    data = new { ok = false, msg = "Data inicial deve ser menor que a data final." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                if (DateTime.Compare(fim.Date, DateTime.Now.Date) <= 0)
                {
                    data = new { ok = false, msg = "Data final deve ser maior que a data atual." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                
                if (DateTime.Compare(inicio.Date, DateTime.Now.Date) <= 0)
                {
                    data = new { ok = false, msg = "Data inicial deve ser maior que a data atual." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                model.DataInicio = inicio;
                model.DataFim = fim;

                if (_cookie.IsSet(_tempIdKey))
                    model.ClusterIdTemp = _cookie.Get<string>(_tempIdKey);

                model.LoginAdmin = LoginHelper.GetLoginModel().Login;

                if (model.Id <= 0)
                    _service.CadastrarCluster(model);
                else
                {
                    _service.AtualizarCluster(model);
                    data = new { ok = true, msg = "Cluster alterado com sucesso." };
                }

                if (_cookie.IsSet(_tempIdKey))
                    _cookie.Remove(_tempIdKey);
            }
            catch (Exception exc)
            {
                data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarParticipante(string ra, int clusterId)
        {
            ClusterRAModel retorno = null;

            if (_cookie.IsSet(_tempIdKey))
            {
                var tempId = _cookie.Get<string>(_tempIdKey);
                retorno = _service.BuscarParticipante(ra, tempId);
            }
            else if (clusterId > 0)
                retorno = _service.BuscarParticipante(ra, clusterId);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarProdutos(string nome)
        {
            var result = _serviceProduct.ProcurarProdutos(nome);

            var data = new { ok = true, result };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult ExcluirRA(ClusterRAModel model)
        {
            var data = new { ok = true, msg = "Participante excluído com sucesso." };

            try
            {
                if (_cookie.IsSet(_tempIdKey))
                    _service.ExcluirParticipanteTemp(model.IdClusterRA);
            }
            catch (Exception ex)
            {
                data = new { ok = false, msg = "Erro ao excluir participante. " + ex.Message };
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Encerrar(int id)
        {
            var data = new { ok = true, msg = "Cluster encerrado com sucesso." };

            try
            {
                _service.EncerrarCluster(id, LoginHelper.GetLoginModel().Login);
            }
            catch (Exception ex)
            {
                data = new { ok = false, msg = "Erro ao encerrar Cluster. " + ex.Message };
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}