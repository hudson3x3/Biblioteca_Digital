using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Models;
using System.Transactions;
using System.Collections;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Enums;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class QuestionarioController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repQuestionario = context.CreateRepository<Questionario>();
                List<QuestionarioModel> listQuestionarioModel = new List<QuestionarioModel>();
                foreach (var item in repQuestionario.Filter<Questionario>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listQuestionarioModel.Add(new QuestionarioModel
                    {
                        Id = item.Id,
                        TipoQuestionarioId = item.TipoQuestionarioId,
                        TipoQuestionario = item.TipoQuestionario.Nome,
                        Nome = item.Nome,
                        Descricao = item.Descricao,
                        Ativo = item.Ativo,
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim
                    });
                }
                return View(listQuestionarioModel);
            }
        }

        public ActionResult Create()
        {
            return View(new QuestionarioModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repQuestionario = context.CreateRepository<Questionario>();
                var Questionario = repQuestionario.Find<Questionario>(Id);

                QuestionarioModel questionarioModel = new QuestionarioModel();

                if (Questionario != null)
                {
                    questionarioModel.Id = Questionario.Id;
                    questionarioModel.TipoQuestionarioId = Questionario.TipoQuestionarioId;
                    questionarioModel.Nome = Questionario.Nome;
                    questionarioModel.Descricao = Questionario.Descricao;
                    questionarioModel.Ativo = Questionario.Ativo;
                    questionarioModel.DataInclusao = Questionario.DataInclusao;
                    questionarioModel.DataAlteracao = Questionario.DataAlteracao;
                    questionarioModel.DataInicio = Questionario.DataInicio;
                    questionarioModel.DataFim = Questionario.DataFim;

                    //Perfis de Acesso
                    ArrayList arrPerfilId = new ArrayList();
                    foreach (var item in Questionario.QuestionarioPerfil.Where(x => x.Ativo == true).ToList())
                    {
                        arrPerfilId.Add(item.PerfilId);
                    }
                    questionarioModel.ArrPerfilId = arrPerfilId;

                    //Estrutura de Acesso
                    ArrayList arrEstruturaId = new ArrayList();
                    foreach (var item in Questionario.QuestionarioEstrutura.Where(x => x.Ativo == true).ToList())
                    {
                        arrEstruturaId.Add(item.EstruturaId);
                    }
                    questionarioModel.ArrEstruturaId = arrEstruturaId;

                }

                return View(questionarioModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(QuestionarioModel questionarioModel)
        {
            try
            {
                var data = new object();
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repQuestionario = context.CreateRepository<Questionario>();

                    if (repQuestionario.Filter<Questionario>(x => x.Nome.ToLower() == questionarioModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = "Questionário já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Questionario Questionario = new Questionario();
                        Questionario.Id = questionarioModel.Id;
                        Questionario.TipoQuestionarioId = questionarioModel.TipoQuestionarioId;
                        Questionario.Nome = questionarioModel.Nome;
                        Questionario.Descricao = questionarioModel.Descricao;
                        Questionario.Ativo = true;
                        Questionario.DataInclusao = DateTime.Now;
                        Questionario.DataAlteracao = DateTime.Now;
                        Questionario.DataInicio = questionarioModel.DataInicio;
                        Questionario.DataFim = questionarioModel.DataFim;

                        //Perfis de Acesso
                        IRepository repQuestionarioPerfil = context.CreateRepository<QuestionarioPerfil>();
                        List<QuestionarioPerfil> listQuestionarioPerfil = new List<QuestionarioPerfil>();

                        if (questionarioModel.PerfilId == null)
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Perfis de Acesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                        foreach (var item in questionarioModel.PerfilId)
                        {
                            if (item > 0)
                            {
                                listQuestionarioPerfil.Add(new QuestionarioPerfil
                                {
                                    QuestionarioId = Questionario.Id,
                                    Ativo = true,
                                    PerfilId = item,
                                    DataInclusao = DateTime.Now,
                                    DataAlteracao = DateTime.Now
                                });
                            }
                        }
                        Questionario.QuestionarioPerfil = listQuestionarioPerfil;                    

                        //Estrurura de Acesso
                        IRepository repQuestionarioEstrutura = context.CreateRepository<QuestionarioEstrutura>();
                        List<QuestionarioEstrutura> listQuestionarioEstrutura = new List<QuestionarioEstrutura>();

                        if (questionarioModel.EstruturaId == null)
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Estrutura de Acesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                        foreach (var item in questionarioModel.EstruturaId)
                        {
                            if (item > 0)
                            {
                                listQuestionarioEstrutura.Add(new QuestionarioEstrutura
                                {
                                    QuestionarioId = Questionario.Id,
                                    Ativo = true,
                                    EstruturaId = item,
                                    DataInclusao = DateTime.Now,
                                    DataAlteracao = DateTime.Now
                                });
                            }
                        }
                        Questionario.QuestionarioEstrutura = listQuestionarioEstrutura;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repQuestionario.Create(Questionario);
                            repQuestionario.SaveChanges();
                            scope.Complete();
                        }

                        data = new { ok = true, msg = "Dados salvos com sucesso." };
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
        public ActionResult Edit(QuestionarioModel questionarioModel)
        {
            try
            {                
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repQuestionario = context.CreateRepository<Questionario>();
                    var Questionario = repQuestionario.Find<Questionario>(questionarioModel.Id);

                    if (Questionario != null)
                    {
                        if (repQuestionario.Filter<Questionario>(x => x.Nome == questionarioModel.Nome && x.Id != questionarioModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            data = new { ok = false, msg = "Questionário já cadastrado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            Questionario.TipoQuestionarioId = questionarioModel.TipoQuestionarioId;
                            Questionario.Nome = questionarioModel.Nome;
                            Questionario.Descricao = questionarioModel.Descricao;
                            Questionario.DataAlteracao = DateTime.Now;
                            Questionario.DataInicio = questionarioModel.DataInicio;
                            Questionario.DataFim = questionarioModel.DataFim;

                            //Perfis de Acesso
                            IRepository repQuestionarioPerfil = context.CreateRepository<QuestionarioPerfil>();
                            var listQuestionarioPerfil = repQuestionarioPerfil.Filter<QuestionarioPerfil>(x => x.QuestionarioId == Questionario.Id && x.Ativo == true).ToList();
                            //Perfis Update Atuais
                            foreach (var item in listQuestionarioPerfil)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }

                            //Perfis Insert novos (Verifica se os perfis foram selecionados)
                            if (questionarioModel.PerfilId == null)
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Perfis de Acesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            foreach (var item in questionarioModel.PerfilId)
                            {
                                if (item > 0)
                                {
                                    listQuestionarioPerfil.Add(new QuestionarioPerfil
                                    {
                                        QuestionarioId = Questionario.Id,
                                        Ativo = true,
                                        PerfilId = item,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });
                                }
                            }
                            Questionario.QuestionarioPerfil = listQuestionarioPerfil;

                            //Estrutura de Acesso
                            IRepository repQuestionarioEstrutura = context.CreateRepository<QuestionarioEstrutura>();
                            var listQuestionarioEstrutura = repQuestionarioEstrutura.Filter<QuestionarioEstrutura>(x => x.QuestionarioId == Questionario.Id && x.Ativo == true).ToList();
                            //Estrutura Update Atuais
                            foreach (var item in listQuestionarioEstrutura)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }

                            //Estrutura Insert novos (Verifica a estrutura foi selecionada)
                            if (questionarioModel.EstruturaId == null)
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Estrutura de Acesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            foreach (var item in questionarioModel.EstruturaId)
                            {
                                if (item > 0)
                                {
                                    listQuestionarioEstrutura.Add(new QuestionarioEstrutura
                                    {
                                        QuestionarioId = Questionario.Id,
                                        Ativo = true,
                                        EstruturaId = item,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });
                                }
                            }
                            Questionario.QuestionarioEstrutura = listQuestionarioEstrutura;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repQuestionario.Update(Questionario);
                                repQuestionario.SaveChanges();
                                scope.Complete();
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, Questionário não encontrado." };
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
                    IRepository repQuestionario = context.CreateRepository<Questionario>();

                    var Questionario = repQuestionario.Find<Questionario>(Id);

                    if (Questionario != null)
                    {
                        Questionario.DataAlteracao = DateTime.Now;
                        Questionario.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repQuestionario.Update(Questionario);
                            repQuestionario.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Questionário inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Questionário não encontrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o Questionário." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
