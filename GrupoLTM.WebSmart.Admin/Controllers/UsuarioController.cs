using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using System.Transactions;
using System.Collections;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class UsuarioController : BaseController
    {
       private readonly UsuarioAdminService _usuarioAdminService = new UsuarioAdminService();

        #region "Actions"

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult Index()
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();
                var listUsuarioModel = new List<UsuarioAdmModel>();

                var lista = repUsuario.Filter<UsuarioAdm>(x => x.Ativo).OrderBy(x => x.Nome).ToList();

                foreach (var item in lista)
                {
                    listUsuarioModel.Add(new UsuarioAdmModel
                    {
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        Email = item.Email,
                        Matricula = item.Matricula,
                        Ativo = item.Ativo,
                        PerfilId = item.PerfilId,
                        Id = item.Id,
                        Login = item.Login,
                        Nome = item.Nome,
                        Perfil = item.Perfil.Nome
                    });
                }

                return View(listUsuarioModel);
            }
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult Create()
        {
            return View(new UsuarioAdmModel());
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult ValidaLogin()
        {
            return View(new UsuarioAdmModel());
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        public ActionResult Edit(int Id)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();
                var usuario = repUsuario.Find<UsuarioAdm>(Id);

                var usuarioModel = new UsuarioAdmModel();

                if (usuario != null)
                {
                    usuarioModel.Ativo = usuario.Ativo;
                    usuarioModel.PerfilId = usuario.PerfilId;
                    usuarioModel.Id = usuario.Id;
                    usuarioModel.Login = usuario.Login;
                    usuarioModel.Nome = usuario.Nome;
                    usuarioModel.Email = usuario.Email;

                    //Menu de Acessos
                    var arrMenulId = new ArrayList();

                    foreach (var item in usuario.UsuarioAdmMenu.Where(x => x.Ativo == true).ToList())
                    {
                        arrMenulId.Add(item.MenuId);
                    }

                    usuarioModel.ArrMenuId = arrMenulId;
                }

                return View(usuarioModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult ValidaLogin(string login)
        {
            try
            {
                var data = new object();
                
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repUsuario = context.CreateRepository<UsuarioAdm>();

                    if (repUsuario.Filter<UsuarioAdm>(x => x.Login.ToLower() == login.ToLower()).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = $"O login {login} já está sendo utilizado."};
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                data = new { };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Valição de login encontrou erro." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(UsuarioAdmModel usuarioModel)
        {
            try
            {
                var data = new object();

                using (var context = UnitOfWorkFactory.Create())
                {
                    var repUsuario = context.CreateRepository<UsuarioAdm>();

                    if (repUsuario.Filter<UsuarioAdm>(x => x.Login.ToLower() == usuarioModel.Login.ToLower()).Any())
                    {
                        data = new { ok = false, msg = "Usuário já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        UsuarioAdm usuario = new UsuarioAdm
                        {
                            Nome = usuarioModel.Nome,
                            Email = usuarioModel.Email,
                            Login = usuarioModel.Login,
                            Senha = (Guid.NewGuid().ToString().Substring(0, 6)),
                            Matricula = usuarioModel.Matricula,
                            SerieCursar = usuarioModel.SerieCursar,
                            Periodo = usuarioModel.Periodo, 
                            PerfilId = usuarioModel.PerfilId,
                            Ativo = true,
                            DataAlteracao = DateTime.Now,
                            DataInclusao = DateTime.Now
                        };

                        //Acesso aos Menus
                        var listUsuarioAdmMenu = new List<UsuarioAdmMenu>();

                        if (usuarioModel.MenuId == null)
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Menu de Acesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                        //Adiciona os menus ao usuário criado
                        //Default, Menu Admin (Pai Menu)
                        listUsuarioAdmMenu.Add(new UsuarioAdmMenu
                        {
                            UsuarioAdmId = usuarioModel.Id,
                            Ativo = true,
                            MenuId = Convert.ToInt32(EnumDomain.Menu.MenuAdmin),
                            DataAlteracao = DateTime.Now,
                            DataInclusao = DateTime.Now
                        });

                        //Os selecionados na tela
                        foreach (var item in usuarioModel.MenuId)
                        {
                            if (item > 0)
                            {
                                listUsuarioAdmMenu.Add(new UsuarioAdmMenu
                                {
                                    UsuarioAdmId = usuarioModel.Id,
                                    Ativo = true,
                                    MenuId = item,
                                    DataAlteracao = DateTime.Now,
                                    DataInclusao = DateTime.Now
                                });
                            }
                        }

                        usuario.UsuarioAdmMenu = listUsuarioAdmMenu;

                        //Cria o usuário
                        using (TransactionScope scope = new TransactionScope())
                        {
                            repUsuario.Create(usuario);
                            repUsuario.SaveChanges();
                            scope.Complete();
                        }

                        data = new { ok = true, msg = "Dados salvos com sucesso. " + "Senha:" + usuario.Senha };

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

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Edit(UsuarioAdmModel usuarioModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repUsuario = context.CreateRepository<UsuarioAdm>();
                    var usuario = repUsuario.Find<UsuarioAdm>(usuarioModel.Id);

                    if (usuario != null)
                    {
                        if (repUsuario.Filter<UsuarioAdm>(x => x.Login.ToLower() == usuarioModel.Login.ToLower()).Any())
                        {
                            data = new { ok = false, msg = "Já existe um usuário cadastrado com o login informado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            usuario.Nome = usuarioModel.Nome;
                            usuario.Email = usuarioModel.Email;
                            usuario.Login = usuarioModel.Login;

                            var _senha = Guid.NewGuid().ToString().Substring(0, 6);
                            if (usuarioModel.ReenviarSenha)
                            {
                                usuario.Senha = _senha;
                            }

                            usuario.PerfilId = usuarioModel.PerfilId;
                            usuario.DataAlteracao = DateTime.Now;

                            //Acesso aos Menus
                            if (usuarioModel.MenuId == null)
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Menu de Acesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            IRepository repUsuarioAdmMenu = context.CreateRepository<UsuarioAdmMenu>();

                            //Atualiza os menus atuais para false
                            var MenuPaiIdAdmin = Convert.ToInt32(EnumDomain.Menu.MenuAdmin);
                            var listUsuarioAdmMenu = repUsuarioAdmMenu.Filter<UsuarioAdmMenu>(x => x.UsuarioAdmId == usuarioModel.Id && x.Ativo == true && x.MenuId != MenuPaiIdAdmin).ToList();
                            foreach (var item in listUsuarioAdmMenu)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }

                            //Adiciona os novos menus ao usuário em edição
                            foreach (var item in usuarioModel.MenuId)
                            {
                                if (item > 0)
                                {
                                    listUsuarioAdmMenu.Add(new UsuarioAdmMenu
                                    {
                                        UsuarioAdmId = usuarioModel.Id,
                                        Ativo = true,
                                        MenuId = item,
                                        DataAlteracao = DateTime.Now,
                                        DataInclusao = DateTime.Now
                                    });
                                }
                            }
                            usuario.UsuarioAdmMenu = listUsuarioAdmMenu;


                            using (TransactionScope scope = new TransactionScope())
                            {
                                repUsuario.Update(usuario);
                                repUsuario.SaveChanges();
                                scope.Complete();
                            }

                            if (usuarioModel.ReenviarSenha)
                            {
                                _usuarioAdminService.EnviarEmailDeCadastroDeUsuario(usuario.Login);
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, usuário não encontrado." };
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

        [CustomAuthorize(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repUsuario = context.CreateRepository<UsuarioAdm>();

                    var usuario = repUsuario.Find<UsuarioAdm>(Id);

                    if (usuario != null)
                    {
                        usuario.DataAlteracao = DateTime.Now;
                        usuario.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repUsuario.Update(usuario);
                            repUsuario.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Usuário inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, usuário não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o usuário." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
