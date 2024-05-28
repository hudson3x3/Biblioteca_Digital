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
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class GrupoItemController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repGrupoItem = context.CreateRepository<GrupoItem>();
                List<GrupoItemModel> listGrupoItemModel = new List<GrupoItemModel>();

                foreach (var item in repGrupoItem.Filter<GrupoItem>(x => x.Ativo == true && (x.GrupoItem2.Ativo == true || x.GrupoItem2 == null)).OrderBy(x => x.Nome).ToList())
                {
                    listGrupoItemModel.Add(new GrupoItemModel
                    {
                        DataInclusao = item.DataInclusao,
                        Dataalteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        PaiId = item.PaiId,
                        Descricao = item.Descricao,
                        Codigo = item.Codigo,
                        NomePai = (item.PaiId.HasValue ? item.GrupoItem2.Nome : "")
                    });
                }
                return View(listGrupoItemModel);
            }
        }

        public ActionResult Create()
        {          
            return View(new GrupoItemModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repGrupoItem = context.CreateRepository<GrupoItem>();
                var GrupoItem = repGrupoItem.Find<GrupoItem>(Id);

                GrupoItemModel GrupoItemModel = new GrupoItemModel();

                if (GrupoItem != null)
                {
                    GrupoItemModel.Ativo = GrupoItem.Ativo;
                    GrupoItemModel.Id = GrupoItem.Id;
                    GrupoItemModel.Nome = GrupoItem.Nome;
                    GrupoItemModel.Dataalteracao = GrupoItem.DataAlteracao;
                    GrupoItemModel.DataInclusao = GrupoItem.DataInclusao;
                    GrupoItemModel.Descricao = GrupoItem.Descricao;
                    GrupoItemModel.Codigo = GrupoItem.Codigo;
                    GrupoItemModel.PaiId = GrupoItem.PaiId;
                }

                return View(GrupoItemModel);
            }
        }

        public ActionResult Lote()
        {
            return View(new GrupoItemModel());
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(GrupoItemModel GrupoItemModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repGrupoItem = context.CreateRepository<GrupoItem>();
                    IRepository repGrupoItemSuperior = context.CreateRepository<GrupoItem>();


                    //Verifica se o Grupo Item já existe (Por código).
                    if (repGrupoItem.Filter<GrupoItem>(x => x.Codigo.ToLower() == GrupoItemModel.Codigo.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Grupo Item já cadastrado com o código informado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        GrupoItem GrupoItem = new GrupoItem();
                        GrupoItem.Nome = GrupoItemModel.Nome;
                        GrupoItem.Ativo = true;
                        GrupoItem.DataAlteracao = DateTime.Now;
                        GrupoItem.DataInclusao = DateTime.Now;
                        GrupoItem.Descricao = GrupoItemModel.Descricao;
                        GrupoItem.Codigo = GrupoItemModel.Codigo;

                        if (GrupoItemModel.PaiId != null)
                        {
                            GrupoItem.PaiId = GrupoItemModel.PaiId;
                        }

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repGrupoItem.Create(GrupoItem);
                            repGrupoItem.SaveChanges();
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
        public ActionResult Edit(GrupoItemModel GrupoItemModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repGrupoItem = context.CreateRepository<GrupoItem>();
                    IRepository repGrupoItemSuperior = context.CreateRepository<GrupoItem>();

                    if(GrupoItemModel.Id == GrupoItemModel.PaiId)
                    {
                        var data = new { ok = false, msg = "O Grupo Item não pode ser pai dele mesmo." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {     
                        var GrupoItem = repGrupoItem.Find<GrupoItem>(GrupoItemModel.Id);

                        if (GrupoItem != null)
                        {
                            if (repGrupoItem.Filter<GrupoItem>(x => x.Nome == GrupoItemModel.Nome && x.Id != GrupoItemModel.Id && x.Ativo == true).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "Grupo Item já cadastrado com o código informado." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                //Preenche o objeto
                                GrupoItem.PaiId = GrupoItemModel.PaiId;
                                GrupoItem.Nome = GrupoItemModel.Nome;
                                GrupoItem.Descricao = GrupoItemModel.Descricao;
                                GrupoItem.Codigo = GrupoItemModel.Codigo;
                                GrupoItem.DataAlteracao = DateTime.Now;

                                using (TransactionScope scope = new TransactionScope())
                                {
                                    repGrupoItem.Update(GrupoItem);
                                    repGrupoItem.SaveChanges();
                                    scope.Complete();
                                }
                           
                                var data = new { ok = true, msg = "Dados salvos com sucesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            var data = new { ok = false, msg = "Não foi possível salvar os dados, Grupo Item não encontrado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

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
                    IRepository repGrupoItem = context.CreateRepository<GrupoItem>();

                    var GrupoItem = repGrupoItem.Find<GrupoItem>(Id);
                    
                    //Inativa os grupos itens ligados a campanhas
                    var listCampanhaGrupoItem = GrupoItem.CampanhaGrupoItem.Where(x => x.GrupoItemId == Id).ToList();
                    foreach (var item in listCampanhaGrupoItem)
                    {
                        item.DataAlteracao = DateTime.Now;
                        item.Ativo = false;
                    }

                    if (GrupoItem != null)
                    {
                        GrupoItem.DataAlteracao = DateTime.Now;
                        GrupoItem.DataInativacao = DateTime.Now;
                        GrupoItem.Ativo = false;
                        GrupoItem.CampanhaGrupoItem = listCampanhaGrupoItem;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repGrupoItem.Update(GrupoItem);
                            repGrupoItem.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Grupo Item inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Grupo Item não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o Grupo Item." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        
        
        #endregion
    }
}
