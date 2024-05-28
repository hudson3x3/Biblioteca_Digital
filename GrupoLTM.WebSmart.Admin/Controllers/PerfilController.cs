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
using GrupoLTM.WebSmart.Services;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class PerfilController : BaseController
    {
        #region "Services"

        private readonly PerfilService _perfilService;

        #endregion

        #region "Constructor"

        public PerfilController()
        {
            _perfilService = new PerfilService();
        }

        #endregion

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();
                List<PerfilModel> listPerfilModel = new List<PerfilModel>();
                foreach (var item in repPerfil.Filter<Perfil>(x => x.Ativo == true && x.Adm == false).OrderBy(x => x.Nome).ToList())
                {
                    listPerfilModel.Add(new PerfilModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Adm = item.Adm,
                        PaiId = item.PaiId,
                        NomePai = (item.PaiId.HasValue ? item.Perfil2.Nome : ""),
                        NivelHierarquia = item.NivelHierarquia
                    });
                }
                return View(listPerfilModel);
            }
        }

        public ActionResult Create()
        {          
            return View(new PerfilModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();
                var Perfil = repPerfil.Find<Perfil>(Id);

                PerfilModel perfilModel = new PerfilModel();

                if (Perfil != null)
                {
                    perfilModel.Ativo = Perfil.Ativo;
                    perfilModel.Id = Perfil.Id;
                    perfilModel.Nome = Perfil.Nome;
                    perfilModel.DataAlteracao = Perfil.DataAlteracao;
                    perfilModel.DataInclusao = Perfil.DataInclusao;
                    perfilModel.Adm = Perfil.Adm;
                    perfilModel.PaiId = Perfil.PaiId;
                    perfilModel.NivelHierarquia = Perfil.NivelHierarquia;

                }

                return View(perfilModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(PerfilModel perfilModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPerfil = context.CreateRepository<Perfil>();
                    IRepository repPerfilSuperior = context.CreateRepository<Perfil>();

                    //Cria os níveis de hierarquia automaticamente.
                    var hierarquiaSuperior = repPerfilSuperior.Filter<Perfil>(x => x.Ativo == true && x.Id == perfilModel.PaiId).FirstOrDefault();
                    if (hierarquiaSuperior == null)
                    {
                        perfilModel.NivelHierarquia = 1;
                    }
                    else if (hierarquiaSuperior.NivelHierarquia < 10)
                    {
                        perfilModel.NivelHierarquia = (int)hierarquiaSuperior.NivelHierarquia + 1;
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Número máximo de níveis de hierarquia atingido(10 níveis)." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                    //Verifica se o perfil já existe.
                    if (repPerfil.Filter<Perfil>(x => x.Nome.ToLower() == perfilModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Perfil já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Perfil Perfil = new Perfil();
                        Perfil.Nome = perfilModel.Nome;
                        Perfil.Ativo = true;
                        Perfil.DataAlteracao = DateTime.Now;
                        Perfil.DataInclusao = DateTime.Now;
                        if (perfilModel.PaiId != null)
                        {
                            Perfil.PaiId = perfilModel.PaiId;
                        }
                        Perfil.NivelHierarquia = perfilModel.NivelHierarquia;
                        Perfil.Adm = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repPerfil.Create(Perfil);
                            repPerfil.SaveChanges();
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
        public ActionResult Edit(PerfilModel perfilModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPerfil = context.CreateRepository<Perfil>();
                    IRepository repPerfilSuperior = context.CreateRepository<Perfil>();


                    var Perfil = repPerfil.Find<Perfil>(perfilModel.Id);

                    //Cria os níveis de hierarquia automaticamente.
                    var hierarquiaSuperior = repPerfilSuperior.Filter<Perfil>(x => x.Ativo == true && x.Id == perfilModel.PaiId).FirstOrDefault();
                    if (hierarquiaSuperior == null)
                    {
                        perfilModel.NivelHierarquia = 1;
                    }
                    else if (hierarquiaSuperior.NivelHierarquia < 10)
                    {
                        perfilModel.NivelHierarquia = (int)hierarquiaSuperior.NivelHierarquia + 1;
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Número máximo de níveis de hierarquia atingido(10 níveis)." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }

                    if (Perfil != null)
                    {
                        if (repPerfil.Filter<Perfil>(x => x.Nome == perfilModel.Nome && x.Id != perfilModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            var data = new { ok = false, msg = "Perfil já cadastrado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            Perfil.Nome = perfilModel.Nome;
                            Perfil.DataAlteracao = DateTime.Now;
                            Perfil.PaiId = perfilModel.PaiId;
                            Perfil.NivelHierarquia = perfilModel.NivelHierarquia;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repPerfil.Update(Perfil);
                                repPerfil.SaveChanges();
                                scope.Complete();
                            }
                           
                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, perfil não encontrado." };
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
                _perfilService.DesabilitarPerfil(Id);

                var data = new { ok = true, msg = "Perfil inativado com sucesso." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o perfil." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }            
        }

        #endregion
    }
}
