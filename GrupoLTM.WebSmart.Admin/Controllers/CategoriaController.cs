using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services;
using System.Threading.Tasks;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Helpers;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class CatalogoController : Controller
    {
        #region "Serviços"

        private readonly CatalogoService _catalogoService;
        private readonly CatalogoCPService _catalogoCpService;

        #endregion

        public CatalogoController()
        {
            _catalogoService = new CatalogoService();
            _catalogoCpService = new CatalogoCPService();   
        }

        #region "Actions"

        public ActionResult Index()
        {

            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();
                var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                return View(_catalogoService.ListarCatalogos(usuario.PerfilId));
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult Create()
        {
            return View(new CatalogoModel());
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult Edit(int id)
        {
            var catalogo = _catalogoService.ObterCatalogoPorId(id);
            CatalogoModel catalogoModel = new CatalogoModel
            {
                Id = catalogo.Id,
                Nome = catalogo.Nome,
                Autor = catalogo.Autor,
                //MktPlaceCatalogoId = catalogo.MktPlaceCatalogoId,
                Codigo = catalogo.Codigo,
                //PrimeiroAcesso = true,
                Qtd = catalogo.Qtd,
                DataInclusao =  DateTime.Now,
                DataAlteracao = DateTime.Now,
                //IdCampanha = catalogo.IdCampanha,
                //IdEmpresa = catalogo.IdEmpresa,
                //IdOrigem = catalogo.IdOrigem,
                //AppIdMktPlace = catalogo.AppIdMktPlace,
                //ConversionRate = catalogo.ConversionRate
            };
            
            return View(catalogoModel);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Edit(CatalogoModel catalogoModel)
        {
            try
            {
                _catalogoService.EditarCatalogo(catalogoModel);

                var data = new { ok = true, msg = "Catálogo alterado com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a catálogo." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(CatalogoModel catalogoModel)
        {
            try
            {
                object data;
                if (_catalogoService.ObterCatalogo(catalogoModel.Codigo) != null)
                {
                    data = new { ok = false, msg = "Catálogo já cadastrado." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                _catalogoService.CriarCatalogo(catalogoModel);
                data = new { ok = true, msg = "Catálogo cadastrado com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a catálogo." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var bln = _catalogoService.InativarCatalogo(Id);

                if (bln)
                {
                    var data = new { ok = true, msg = "Catálogo inativada com sucesso." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = new { ok = false, msg = "Não foi possível salvar os dados, catálogo não encontrada." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a catálogo." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpGet]
        public ActionResult CadastroCPs(int id)
        {
            var nome = _catalogoService.ListarPorId(id).Nome;

            ViewBag.Catalogo = nome;
            ViewBag.CatalogoId = id;

            // Obtém CPs
            List<CatalogoCPModel> cps = _catalogoCpService.ObterCPsPorCatalogoId(id);

            return View(cps);
        }

        [HttpPost]
        public async Task<JsonResult> InserirCPs(List<CatalogoCPModel> cps, int[] idsRemover = null)
        {
            try
            {
                Task t1 = Task.Factory.StartNew(() => _catalogoCpService.Salvar(cps));
                Task t2 = Task.Factory.StartNew(() => _catalogoCpService.Remover(idsRemover));

                await t1;
                await t2;

                return Json(new { Sucesso = true });
            }
            catch(Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        #endregion

    }
}
