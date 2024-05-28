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
    public class RespostaController : BaseController
    {
        #region "Actions"

        public ActionResult Index(int PerguntaId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repResposta = context.CreateRepository<Resposta>();
                List<RespostaModel> listRespostaModel = new List<RespostaModel>();
                foreach (var item in repResposta.Filter<Resposta>(x => x.PerguntaId == PerguntaId && x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listRespostaModel.Add(new RespostaModel
                    {

                        Id = item.Id,
                        QuestionarioId = item.Pergunta.QuestionarioId,
                        TipoQuestionario = (Domain.Enums.EnumDomain.TipoQuestionario)item.Pergunta.Questionario.TipoQuestionarioId,
                        Questionario = item.Pergunta.Questionario.Nome,
                        PerguntaId = item.PerguntaId,
                        Pergunta = item.Pergunta.Nome,
                        TipoResposta = (Domain.Enums.EnumDomain.TipoResposta)item.Pergunta.TipoRespostaId,
                        Nome = item.Nome,
                        Ativo = item.Ativo,
                        RespostaCorreta = item.RespostaCorreta,
                        Ordem = item.ordem,
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao

                    });
                }

                return Json(listRespostaModel, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(RespostaModel respostaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repResposta = context.CreateRepository<Resposta>();

                    if (repResposta.Filter<Resposta>(x => x.PerguntaId == respostaModel.PerguntaId && x.Nome.ToLower() == respostaModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Resposta já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (repResposta.Filter<Resposta>(x => x.PerguntaId == respostaModel.PerguntaId && x.ordem == respostaModel.Ordem && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Ordem já cadastrada em uma das resposta." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Resposta Resposta = new Resposta();
                        Resposta.PerguntaId = respostaModel.PerguntaId;
                        Resposta.Nome = respostaModel.Nome;
                        Resposta.Ativo = true;
                        Resposta.RespostaCorreta = respostaModel.RespostaCorreta;
                        Resposta.ordem = respostaModel.Ordem;
                        Resposta.DataInclusao = DateTime.Now;
                        Resposta.DataAlteracao = DateTime.Now;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repResposta.Create(Resposta);
                            repResposta.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Dados salvos com sucesso.", RespostaId = Resposta.Id };
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
                    IRepository repResposta = context.CreateRepository<Resposta>();

                    var Resposta = repResposta.Find<Resposta>(Id);

                    if (Resposta != null)
                    {
                        Resposta.DataAlteracao = DateTime.Now;
                        Resposta.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repResposta.Update(Resposta);
                            repResposta.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Resposta inativada com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, resposta não encontrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a resposta." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
