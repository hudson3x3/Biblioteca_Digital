using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class PerguntaController : BaseController
    {

        #region "Actions"

        public ActionResult Index(int QuestionarioId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPergunta = context.CreateRepository<Pergunta>();
                List<PerguntaModel> listPerguntaModel = new List<PerguntaModel>();
                foreach (var item in repPergunta.Filter<Pergunta>(x => x.QuestionarioId == QuestionarioId && x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listPerguntaModel.Add(new PerguntaModel
                    {
                        Id = item.Id,
                        QuestionarioId = item.QuestionarioId,
                        TipoRespostaId = item.TipoRespostaId,
                        TipoResposta = item.TipoResposta.Nome,
                        Nome = item.Nome,
                        Ativo = item.Ativo,
                        AtivoPontos = item.AtivoPontos,
                        ValorPontos = item.ValorPontos,
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        TipoQuestionario = (EnumDomain.TipoQuestionario)item.Questionario.TipoQuestionarioId
                    });
                }

                IRepository repQuestionario = context.CreateRepository<Questionario>();
                var Questionario = repQuestionario.Find<Questionario>(QuestionarioId);
                QuestionarioModel questionarioModel = new QuestionarioModel();
                
                if (Questionario != null)
                {
                    questionarioModel.Id = Questionario.Id;
                    questionarioModel.TipoQuestionarioId = Questionario.TipoQuestionarioId;
                    questionarioModel.TipoQuestionario = Questionario.TipoQuestionario.Nome;
                    questionarioModel.Nome = Questionario.Nome;
                    questionarioModel.Descricao = Questionario.Descricao;
                    questionarioModel.Ativo = Questionario.Ativo;
                    questionarioModel.DataInclusao = Questionario.DataInclusao;
                    questionarioModel.DataAlteracao = Questionario.DataAlteracao;
                    questionarioModel.DataInicio = Questionario.DataInicio;
                    questionarioModel.DataFim = Questionario.DataFim;                   
                }
                var tuple = new Tuple<QuestionarioModel, List<PerguntaModel>>(questionarioModel, listPerguntaModel.ToList());
                return View(tuple);                
            }
        }

        public ActionResult Create(int QuestionarioId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repQuestionario = context.CreateRepository<Questionario>();
                var Questionario = repQuestionario.Find<Questionario>(QuestionarioId);
                PerguntaModel perguntaModel = new PerguntaModel();
                perguntaModel.Questionario = Questionario.Nome;
                perguntaModel.QuestionarioId = Questionario.Id;
                perguntaModel.TipoQuestionario = (EnumDomain.TipoQuestionario)Questionario.TipoQuestionarioId;
                return View(perguntaModel);
            }
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPergunta = context.CreateRepository<Pergunta>();
                var Pergunta = repPergunta.Find<Pergunta>(Id);

                PerguntaModel perguntaModel = new PerguntaModel();

                if (Pergunta != null)
                {
                    perguntaModel.Id = Pergunta.Id;
                    perguntaModel.QuestionarioId = Pergunta.QuestionarioId;
                    perguntaModel.Questionario = Pergunta.Questionario.Nome;
                    perguntaModel.TipoRespostaId = Pergunta.TipoRespostaId;
                    perguntaModel.Nome = Pergunta.Nome;
                    perguntaModel.Ativo = Pergunta.Ativo;
                    perguntaModel.AtivoPontos = Pergunta.AtivoPontos;
                    perguntaModel.ValorPontos = Pergunta.ValorPontos;
                    perguntaModel.DataInclusao = Pergunta.DataInclusao;
                    perguntaModel.DataAlteracao = Pergunta.DataAlteracao;
                    perguntaModel.TipoQuestionario = (EnumDomain.TipoQuestionario)Pergunta.Questionario.TipoQuestionarioId;
                }

                return View(perguntaModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(PerguntaModel perguntaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPergunta = context.CreateRepository<Pergunta>();

                    if (repPergunta.Filter<Pergunta>(x => x.QuestionarioId == perguntaModel.QuestionarioId && x.Nome.ToLower() == perguntaModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Pergunta já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Pergunta Pergunta = new Pergunta();
                        Pergunta.QuestionarioId = perguntaModel.QuestionarioId;
                        Pergunta.Nome = perguntaModel.Nome;

                        if (perguntaModel.TipoQuestionario != EnumDomain.TipoQuestionario.Faq)
                        {
                            Pergunta.TipoRespostaId = perguntaModel.TipoRespostaId;
                            Pergunta.Ativo = true;
                            Pergunta.AtivoPontos = perguntaModel.AtivoPontos;                                                        
                        }
                        else
                        {
                            Pergunta.TipoRespostaId = Convert.ToInt32(EnumDomain.TipoResposta.Aberta);
                            Pergunta.Ativo = true;
                            Pergunta.AtivoPontos = false;
                        }

                        Pergunta.ValorPontos = perguntaModel.ValorPontos;
                        Pergunta.DataInclusao = DateTime.Now;
                        Pergunta.DataAlteracao = DateTime.Now;

                        //se marcou ativoPontos precisa informar o valor
                        if ((Pergunta.AtivoPontos) && (perguntaModel.ValorPontos == null))
                        {
                            var data = new { ok = false, msg = "Por favor, preencha o campo Valor Pontos." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                repPergunta.Create(Pergunta);
                                repPergunta.SaveChanges();
                                scope.Complete();
                            }

                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
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
        public ActionResult Edit(PerguntaModel perguntaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repPergunta = context.CreateRepository<Pergunta>();
                    var Pergunta = repPergunta.Find<Pergunta>(perguntaModel.Id);

                    if (Pergunta != null)
                    {
                        if (repPergunta.Filter<Pergunta>(x => x.QuestionarioId == perguntaModel.QuestionarioId && x.Nome == perguntaModel.Nome && x.Id != perguntaModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            var data = new { ok = false, msg = "Pergunta já cadastrada." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            Pergunta.QuestionarioId = perguntaModel.QuestionarioId;
                            Pergunta.Nome = perguntaModel.Nome;
                            Pergunta.DataAlteracao = DateTime.Now;
                            Pergunta.ValorPontos = perguntaModel.ValorPontos;

                            if (perguntaModel.TipoQuestionario != EnumDomain.TipoQuestionario.Faq)
                            {
                                Pergunta.TipoRespostaId = perguntaModel.TipoRespostaId;
                                Pergunta.Ativo = true;
                                Pergunta.AtivoPontos = perguntaModel.AtivoPontos;
                            }
                            else
                            {
                                Pergunta.TipoRespostaId = Convert.ToInt32(EnumDomain.TipoResposta.Aberta);
                                Pergunta.Ativo = true;
                                Pergunta.AtivoPontos = false;
                            }

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repPergunta.Update(Pergunta);
                                repPergunta.SaveChanges();
                                scope.Complete();
                            }

                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, pergunta não encontrada." };
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
                    IRepository repPergunta = context.CreateRepository<Pergunta>();

                    var Pergunta = repPergunta.Find<Pergunta>(Id);

                    if (Pergunta != null)
                    {
                        Pergunta.DataAlteracao = DateTime.Now;
                        Pergunta.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repPergunta.Update(Pergunta);
                            repPergunta.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Pergunta inativada com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, pergunta não encontrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a pergunta." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
