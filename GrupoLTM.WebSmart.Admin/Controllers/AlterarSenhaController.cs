using System;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using System.Transactions;
using GrupoLTM.WebSmart.Admin.Helpers;
using GrupoLTM.WebSmart.Admin.Models;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class AlterarSenhaController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Alterar(string senhaAtual, string novaSenha)
        {
            try
            {
                var data = new object();

                using (var context = UnitOfWorkFactory.Create())
                {
                    var repUsuario = context.CreateRepository<UsuarioAdm>();

                    var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().Id);

                    if (usuario != null)
                    {
                        //if (usuario.Senha != Infrastructure.Cripto.HexEncoding.Encriptar(senhaAtual))
                        if (usuario.Senha != senhaAtual)
                        {
                            data = new { ok = false, msg = "Senha atual incorreta." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //if(usuario.Senha == Infrastructure.Cripto.HexEncoding.Encriptar(novaSenha))
                            if (usuario.Senha == novaSenha)
                            {
                                data = new { ok = false, msg = "Nova senha precisa ser diferente da atual." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }

                            //usuario.Senha = Infrastructure.Cripto.HexEncoding.Encriptar(novaSenha);
                            usuario.Senha = novaSenha;
                            usuario.EmailRecuperacaoEnviado = false;
                            usuario.DataAlteracao = DateTime.Now;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repUsuario.Update(usuario);
                                repUsuario.SaveChanges();
                                scope.Complete();
                            }

                            var loginModel = new LoginModel
                            {
                                Id = usuario.Id,
                                PerfilId = usuario.PerfilId,
                                Login = usuario.Login,
                                Nome = usuario.Nome,
                                Email = usuario.Email,
                                EmailRecuperacaoEnviado = usuario.EmailRecuperacaoEnviado
                            };

                            LoginHelper.SetLoginModel(loginModel);

                            data = new { ok = true, msg = "Senha alterada com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível alterar a senha" };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Erro ao alterar a senha." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
