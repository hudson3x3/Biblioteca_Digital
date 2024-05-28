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
    public class TipoEstruturaController : BaseController
    {

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();
                List<TipoEstruturaModel> listTipoEstruturaModel = new List<TipoEstruturaModel>();
                foreach (var item in repTipoEstrutura.Filter<TipoEstrutura>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listTipoEstruturaModel.Add(new TipoEstruturaModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome
                    });
                }
                return View(listTipoEstruturaModel);
            }
        }

        public ActionResult Create()
        {
            return View(new TipoEstruturaModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();
                var TipoEstrutura = repTipoEstrutura.Find<TipoEstrutura>(Id);

                TipoEstruturaModel TipoEstruturaModel = new TipoEstruturaModel();

                if (TipoEstrutura != null)
                {
                    TipoEstruturaModel.Ativo = TipoEstrutura.Ativo;
                    TipoEstruturaModel.Id = TipoEstrutura.Id;
                    TipoEstruturaModel.Nome = TipoEstrutura.Nome;
                    TipoEstruturaModel.DataAlteracao = TipoEstrutura.DataAlteracao;
                    TipoEstruturaModel.DataInclusao = TipoEstrutura.DataInclusao;

                }

                return View(TipoEstruturaModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(TipoEstruturaModel TipoEstruturaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();

                    if (repTipoEstrutura.Filter<TipoEstrutura>(x => x.Nome.ToLower() == TipoEstruturaModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Tipo Estrutura já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TipoEstrutura TipoEstrutura = new TipoEstrutura();
                        TipoEstrutura.Nome = TipoEstruturaModel.Nome;
                        TipoEstrutura.Ativo = true;
                        TipoEstrutura.DataAlteracao = DateTime.Now;
                        TipoEstrutura.DataInclusao = DateTime.Now;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repTipoEstrutura.Create(TipoEstrutura);
                            repTipoEstrutura.SaveChanges();
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
        public ActionResult Edit(TipoEstruturaModel TipoEstruturaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();
                    var TipoEstrutura = repTipoEstrutura.Find<TipoEstrutura>(TipoEstruturaModel.Id);

                    if (TipoEstrutura != null)
                    {
                        if (repTipoEstrutura.Filter<TipoEstrutura>(x => x.Nome == TipoEstruturaModel.Nome && x.Id != TipoEstruturaModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            var data = new { ok = false, msg = "Tipo Estrutura já cadastrada." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            TipoEstrutura.Nome = TipoEstruturaModel.Nome;
                            TipoEstrutura.DataAlteracao = DateTime.Now;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repTipoEstrutura.Update(TipoEstrutura);
                                repTipoEstrutura.SaveChanges();
                                scope.Complete();
                            }

                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Tipo Estrutura não encontrado." };
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
                    IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();

                    var TipoEstrutura = repTipoEstrutura.Find<TipoEstrutura>(Id);

                    if (TipoEstrutura != null)
                    {
                        TipoEstrutura.DataAlteracao = DateTime.Now;
                        TipoEstrutura.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repTipoEstrutura.Update(TipoEstrutura);
                            repTipoEstrutura.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Tipo Estrutura inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Tipo Estrutura não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o Tipo Estrutura." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
