using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class MenuController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repMenu = context.CreateRepository<Menu>();
                List<MenuModel> listMenuModel = new List<MenuModel>();

                var Menus = new int?[] {
                    (int?)Domain.Enums.EnumDomain.Menu.MenuPrincipal,
                    (int?)Domain.Enums.EnumDomain.Menu.MenuRedeSocial,
                    (int?)Domain.Enums.EnumDomain.Menu.MenuRodape,
                    (int?)Domain.Enums.EnumDomain.Menu.MenuSuperior
                };

                foreach (var item in repMenu.Filter<Menu>(
                    x => Menus.Contains(x.MenuPaiId)))
                {
                    listMenuModel.Add(new MenuModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Titulo = item.Titulo,
                        Ordem = item.Ordem,
                        MenuPai = item.Menu2.Nome
                    });

                    
                    //Carrega o segundo nível do Menu
                    CarregaMenuItem(listMenuModel, item.Id, repMenu);

                }
                return View(listMenuModel);
            }
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repMenu = context.CreateRepository<Menu>();
                var Menu = repMenu.Find<Menu>(Id);

                MenuModel menuModel = new MenuModel();

                if (Menu != null)
                {
                    menuModel.Id = Menu.Id;
                    menuModel.Nome = Menu.Nome;
                    menuModel.Titulo = Menu.Titulo;
                    menuModel.Ativo = Menu.Ativo;
                    menuModel.DataAlteracao = Menu.DataAlteracao;
                    menuModel.Ordem = Menu.Ordem;
                    menuModel.Link = Menu.Link;
                    menuModel.MenuPaiId = Menu.MenuPaiId;

                    //Perfis de Acesso
                    ArrayList arrPerfilId = new ArrayList();
                    foreach (var item in Menu.MenuPerfil.Where(x => x.Ativo == true).ToList())
                    {
                        arrPerfilId.Add(item.PerfilId);
                    }
                    menuModel.ArrPerfilId = arrPerfilId;

                    //Estrutura de Acesso
                    ArrayList arrEstruturaId = new ArrayList();
                    foreach (var item in Menu.MenuEstrutura.Where(x => x.Ativo == true).ToList())
                    {
                        arrEstruturaId.Add(item.EstruturaId);
                    }
                    menuModel.ArrEstruturaId = arrEstruturaId;

                }

                return View(menuModel);
            }
        }

        public ActionResult Lote()
        {
            return View();
        }

        #endregion

        #region "Actions Post"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Edit(MenuModel MenuModel)
        {
            try
            {
                var data = new object();

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repMenu = context.CreateRepository<Menu>();
                    var Menu = repMenu.Find<Menu>(MenuModel.Id);

                    if (Menu != null)
                    {
                        //Preenche o objeto
                        Menu.DataAlteracao = DateTime.Now;
                        Menu.Ativo = MenuModel.Ativo;
                        Menu.Titulo = MenuModel.Titulo;
                        Menu.Ordem = MenuModel.Ordem;

                        //Atualiza se não for nulo (Redes Sociais, possuem link)
                        if (MenuModel.Link != null)
                        {
                            Menu.Link = MenuModel.Link;
                        }

                        //Perfis de Acesso
                        IRepository repMenuPerfil = context.CreateRepository<MenuPerfil>();
                        var listMenuPerfil = repMenuPerfil.Filter<MenuPerfil>(x => x.MenuId == Menu.Id && x.Ativo == true).ToList();

                        //Perfis Update Atuais
                        foreach (var item in listMenuPerfil)
                        {
                            item.Ativo = false;
                            item.DataAlteracao = DateTime.Now;
                        }

                        //Verifica se o perfil foi selecionada
                        //if (MenuModel.PerfilId == null)
                        //{
                        //    data = new { ok = false, msg = "Por favor, selecione o campo Perfis de Acesso." };
                        //    return Json(data, JsonRequestBehavior.AllowGet);
                        //}

                        if (MenuModel.PerfilId != null)
                        {
                            foreach (var item in MenuModel.PerfilId)
                            {
                                if (item > 0)
                                {
                                    listMenuPerfil.Add(new MenuPerfil
                                    {
                                        MenuId = Menu.Id,
                                        Ativo = true,
                                        PerfilId = item,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });
                                }
                            }
                        }
                        Menu.MenuPerfil = listMenuPerfil;

                        //Estrurura de Acesso
                        IRepository repMenuEstrutura = context.CreateRepository<MenuEstrutura>();
                        var listMenuEstrutura = repMenuPerfil.Filter<MenuEstrutura>(x => x.MenuId == Menu.Id && x.Ativo == true).ToList();

                        //Perfis Update Atuais
                        foreach (var item in listMenuEstrutura)
                        {
                            item.Ativo = false;
                            item.DataAlteracao = DateTime.Now;
                        }
                        
                        //Verifica se a Estrutura foi selecionada
                        //if (MenuModel.EstruturaId == null)
                        //{
                        //    data = new { ok = false, msg = "Por favor, selecione o campo Estrutura de Acesso." };
                        //    return Json(data, JsonRequestBehavior.AllowGet);
                        //}

                        if (MenuModel.EstruturaId != null)
                        {
                            foreach (var item in MenuModel.EstruturaId)
                            {
                                if (item > 0)
                                {
                                    listMenuEstrutura.Add(new MenuEstrutura
                                    {
                                        MenuId = Menu.Id,
                                        Ativo = true,
                                        EstruturaId = item,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });
                                }
                            }
                        }
                        Menu.MenuEstrutura = listMenuEstrutura;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repMenu.Update(Menu);
                            repMenu.SaveChanges();
                            scope.Complete();
                        }

                        data = new { ok = true, msg = "Dados salvos com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, Menu não encontrado." };
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
        public ActionResult Lote(int[] EstruturaId, int[] PerfilId, int[] MenuId)
        {
            try
            {
                var data = new object();

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repMenu = context.CreateRepository<Menu>();

                    foreach (var item in MenuId)
                    {
                        var Menu = repMenu.Find<Menu>(item);

                        if (Menu != null)
                        {
                            //Preenche o objeto
                            Menu.DataAlteracao = DateTime.Now;

                            //Perfis de Acesso
                            IRepository repMenuPerfil = context.CreateRepository<MenuPerfil>();
                            var listMenuPerfil = repMenuPerfil.Filter<MenuPerfil>(x => x.MenuId == Menu.Id && x.Ativo == true).ToList();

                            //Perfis Update Atuais
                            foreach (var itemPerfil in listMenuPerfil)
                            {
                                itemPerfil.Ativo = false;
                                itemPerfil.DataAlteracao = DateTime.Now;
                            }

                            if (PerfilId != null)
                            {
                                foreach (var itemPerfil in PerfilId)
                                {
                                    if (itemPerfil > 0)
                                    {
                                        listMenuPerfil.Add(new MenuPerfil
                                        {
                                            MenuId = Menu.Id,
                                            Ativo = true,
                                            PerfilId = itemPerfil,
                                            DataInclusao = DateTime.Now,
                                            DataAlteracao = DateTime.Now
                                        });
                                    }
                                }
                            }
                            Menu.MenuPerfil = listMenuPerfil;

                            //Estrurura de Acesso
                            IRepository repMenuEstrutura = context.CreateRepository<MenuEstrutura>();
                            var listMenuEstrutura = repMenuPerfil.Filter<MenuEstrutura>(x => x.MenuId == Menu.Id && x.Ativo == true).ToList();

                            //Perfis Update Atuais
                            foreach (var itemEstrutura in listMenuEstrutura)
                            {
                                itemEstrutura.Ativo = false;
                                itemEstrutura.DataAlteracao = DateTime.Now;
                            }

                            if (EstruturaId != null)
                            {
                                foreach (var itemEstrutura in EstruturaId)
                                {
                                    if (itemEstrutura > 0)
                                    {
                                        listMenuEstrutura.Add(new MenuEstrutura
                                        {
                                            MenuId = Menu.Id,
                                            Ativo = true,
                                            EstruturaId = itemEstrutura,
                                            DataInclusao = DateTime.Now,
                                            DataAlteracao = DateTime.Now
                                        });
                                    }
                                }
                            }
                            Menu.MenuEstrutura = listMenuEstrutura;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repMenu.Update(Menu);
                                repMenu.SaveChanges();
                                scope.Complete();
                            }
                        }                        
                    }
                    data = new { ok = true, msg = "Dados salvos com sucesso." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region "Internal Functions"

        internal void CarregaMenuItem(List<MenuModel> listMenuModel, int? menuPaiId, IRepository repMenu)
        {
            foreach (var item in repMenu.Filter<Menu>(x => x.MenuPaiId == menuPaiId))
            {
                listMenuModel.Add(new MenuModel
                {
                    DataInclusao = item.DataInclusao,
                    DataAlteracao = item.DataAlteracao,
                    Ativo = item.Ativo,
                    Id = item.Id,
                    Nome = item.Nome,
                    Titulo = item.Titulo,
                    Ordem = item.Ordem,
                    MenuPai = item.Menu2.Nome
                });
            }
        }

        #endregion

    }
}
