using System;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class ProgramaIncentivoController : Controller
    {
        #region "Serviços"

        private readonly ProgramaIncentivoService _programaIncentivoService;

        #endregion

        public ProgramaIncentivoController()
        {
            _programaIncentivoService = new ProgramaIncentivoService(); 
        }

        #region "Actions"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult Index()
        {
            return View(_programaIncentivoService.ListarProgramaIncentivos().Where(x => x.Nome != "SMS_Avulso"));
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult Create()
        {
            return View(new ProgramaIncentivoModel());
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult Edit(int id)
        {
            var programaIncentivo = _programaIncentivoService.ObterProgramaIncentivoPorId(id);
            var bEditarExcluir = _programaIncentivoService.ObterProgramaIncentivoCategoriaArquivoPorId(id);
            ProgramaIncentivoModel programaIncentivoModel = new ProgramaIncentivoModel
            {
                Id = programaIncentivo.Id,
                Nome = programaIncentivo.Nome,
                IdOrigem = programaIncentivo.IdOrigem,
                Ano = programaIncentivo.Ano,
                EditarExcluir = bEditarExcluir
            };
            
            return View(programaIncentivoModel);
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Edit(ProgramaIncentivoModel programaIncentivoModel)
        {
            try
            {                
                _programaIncentivoService.EditarProgramaIncentivo(programaIncentivoModel);

                var data = new { ok = true, msg = "Campanha alterada com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível alterar a Campanha." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(ProgramaIncentivoModel programaIncentivoModel)
        {
            try
            {
                object data;
                if (_programaIncentivoService.ObterProgramaIncentivo(programaIncentivoModel.Nome) != null)
                {
                    data = new { ok = false, msg = "Campanha já cadastrada." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                _programaIncentivoService.CriarProgramaIncentivo(programaIncentivoModel);
                data = new { ok = true, msg = "Campanha cadastrada com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível cadastrar a Campanha." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var bln = _programaIncentivoService.DeletarProgramaIncentivo(Id);

                if (bln)
                {
                    var data = new { ok = true, msg = "Campanha excluída com sucesso." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = new { ok = false, msg = "Não foi possível salvar os dados, Campanha não encontrado." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível excluir a Campanha." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
