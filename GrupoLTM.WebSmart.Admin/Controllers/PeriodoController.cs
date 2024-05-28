using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using System.Transactions;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Enums;
namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class PeriodoController : BaseController
    {

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPeriodo = context.CreateRepository<Periodo>();
                List<PeriodoModel> listPeriodoModel = new List<PeriodoModel>();
                foreach (var item in repPeriodo.Filter<Periodo>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listPeriodoModel.Add(new PeriodoModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        valor = item.valor
                    });
                }
                return View(listPeriodoModel);
            }
        }

        public ActionResult Create()
        {
            return View(new PeriodoModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPeriodo = context.CreateRepository<Periodo>();
                var Periodo = repPeriodo.Find<Periodo>(Id);

                PeriodoModel periodoModel = new PeriodoModel();

                if (Periodo != null)
                {
                    periodoModel.Ativo = Periodo.Ativo;
                    periodoModel.Id = Periodo.Id;
                    periodoModel.Nome = Periodo.Nome;
                    periodoModel.DataAlteracao = Periodo.DataAlteracao;
                    periodoModel.DataInclusao = Periodo.DataInclusao;
                    periodoModel.valor = Periodo.valor;
                }

                return View(periodoModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(PeriodoModel periodoModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPeriodo = context.CreateRepository<Periodo>();

                    if (repPeriodo.Filter<Periodo>(x => x.Nome.ToLower() == periodoModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Período já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Periodo Periodo = new Periodo();
                        Periodo.Nome = periodoModel.Nome;
                        Periodo.Ativo = true;
                        Periodo.DataAlteracao = DateTime.Now;
                        Periodo.DataInclusao = DateTime.Now;
                        Periodo.valor = periodoModel.valor;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repPeriodo.Create(Periodo);
                            repPeriodo.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Dados salvos com sucesso." };
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

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Edit(PeriodoModel periodoModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPeriodo = context.CreateRepository<Periodo>();
                    var Periodo = repPeriodo.Find<Periodo>(periodoModel.Id);

                    if (Periodo != null)
                    {
                        if (repPeriodo.Filter<Periodo>(x => x.Nome == periodoModel.Nome && x.Id != periodoModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            var data = new { ok = false, msg = "Período já cadastrado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            Periodo.Nome = periodoModel.Nome;
                            Periodo.DataAlteracao = DateTime.Now;
                            Periodo.valor = periodoModel.valor;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repPeriodo.Update(Periodo);
                                repPeriodo.SaveChanges();
                                scope.Complete();
                            }

                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, periodo não encontrado." };
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

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPeriodo = context.CreateRepository<Periodo>();

                    var Periodo = repPeriodo.Find<Periodo>(Id);

                    if (Periodo != null)
                    {
                        Periodo.DataAlteracao = DateTime.Now;
                        Periodo.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repPeriodo.Update(Periodo);
                            repPeriodo.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Periodo inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, periodo não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o periodo." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
