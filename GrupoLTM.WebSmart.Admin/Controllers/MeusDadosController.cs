using System;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using System.Transactions;
using GrupoLTM.WebSmart.Admin.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class MeusDadosController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                //Repositorio Usuario
                IRepository repUsuario = context.CreateRepository<UsuarioAdm>();

                //Recupera o usuário pela sessão
                var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().Id);

                //Preenche a model
                MeusDadosModel meusDadosModel = new MeusDadosModel();
                if (usuario != null)
                {
                    meusDadosModel.email = usuario.Email;
                    meusDadosModel.login = usuario.Login;
                    meusDadosModel.nome = usuario.Nome;
                }

                //Retorna a model
                return View(meusDadosModel);
            }
        }

        #endregion

        #region "Actions Posts"

        
        [HttpPost]
        public ActionResult Alterar(MeusDadosModel meusDadosModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //Repositorio Usuario
                    IRepository repUsuario = context.CreateRepository<UsuarioAdm>();

                    //Recupera o usuário pela sessão
                    var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().Id);

                    if (usuario != null)
                    {
                        //Atualiza o objeto usuário
                        usuario.Email = meusDadosModel.email;
                        usuario.Nome = meusDadosModel.nome;
                        usuario.DataAlteracao = DateTime.Now;

                        //Salva a senha
                        using (TransactionScope scope = new TransactionScope())
                        {
                            repUsuario.Update(usuario);
                            repUsuario.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Dados salvos com sucesso." };
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
                var data = new { ok = false, msg = "Não foi possível salvar os dados." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
