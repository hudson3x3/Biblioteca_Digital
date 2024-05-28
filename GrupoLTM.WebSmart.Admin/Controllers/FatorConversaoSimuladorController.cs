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
    public class FatorConversaoSimuladorController : BaseController
    {
        private readonly FatorConversaoSimuladorService _fatorConversaoSimuladorService = new FatorConversaoSimuladorService();

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repFatorConversaoSimulador = context.CreateRepository<FatorConversaoSimulador>();
                List<FatorConversaoSimuladorModel> listFatorConversaoSimuladorModel = new List<FatorConversaoSimuladorModel>();
                foreach (var item in repFatorConversaoSimulador.Filter<FatorConversaoSimulador>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listFatorConversaoSimuladorModel.Add(new FatorConversaoSimuladorModel
                    {
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Descricao = item.Descricao
                    });
                }
                return View(listFatorConversaoSimuladorModel);
            }
        }

        public ActionResult Create()
        {
            FatorConversaoSimuladorModel model = new FatorConversaoSimuladorModel();
            model.FatorConversaoPontosSimulador = new List<FatorConversaoPontosSimuladorModel>();

            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repFatorConversaoSimulador = context.CreateRepository<FatorConversaoSimulador>();
                var fatorConversaoSimulador = repFatorConversaoSimulador.Find<FatorConversaoSimulador>(Id);

                FatorConversaoSimuladorModel fatorConversaoSimuladorModel = new FatorConversaoSimuladorModel();

                if (fatorConversaoSimulador != null)
                {
                    fatorConversaoSimuladorModel.Id = fatorConversaoSimulador.Id;
                    fatorConversaoSimuladorModel.Nome = fatorConversaoSimulador.Nome;
                    fatorConversaoSimuladorModel.Descricao = fatorConversaoSimulador.Descricao;
                    fatorConversaoSimuladorModel.Mensagem = fatorConversaoSimulador.Mensagem;
                    fatorConversaoSimuladorModel.TipoPonto = fatorConversaoSimulador.TipoPonto;
                    fatorConversaoSimuladorModel.TipoConversao = fatorConversaoSimulador.TipoConversao;
                    fatorConversaoSimuladorModel.MultiplicadorValor = fatorConversaoSimulador.MultiplicadorValor;
                    fatorConversaoSimuladorModel.MultiplicadorPontos = fatorConversaoSimulador.MultiplicadorPontos;

                    fatorConversaoSimuladorModel.Ativo = fatorConversaoSimulador.Ativo;
                    fatorConversaoSimuladorModel.FatorConversaoPontosSimulador = fatorConversaoSimulador.FatorConversaoPontosSimulador.Where(x => x.Ativo == true).Select(x => new FatorConversaoPontosSimuladorModel()
                    {
                        Id = x.Id,
                        IdFatorConversaoSimulador = x.IdFatorConversaoSimulador,
                        ValorInicial = x.ValorInicial,
                        ValorFinal = x.ValorFinal,
                        Pontos = x.Pontos
                    }).ToList();
                }

                //SubMecanica
                ArrayList arrSubMecanicaId = new ArrayList();

                foreach (var item in fatorConversaoSimulador.FatorConversaoMecanicaSimulador.Where(x => x.Ativo == true && x.IdFatorConversaoSimulador == fatorConversaoSimuladorModel.Id).ToList())
                {
                    if (item.IdSubMecanicaSimulador > 0)
                        arrSubMecanicaId.Add(item.IdSubMecanicaSimulador);
                }

                fatorConversaoSimuladorModel.ArrSubMecanicaSimuladorId = arrSubMecanicaId;

                return View(fatorConversaoSimuladorModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(FatorConversaoSimuladorModel fatorConversaoSimuladorModel)
        {
            try
            {

                var data = new object();
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repFatorConversaoSimulador = context.CreateRepository<FatorConversaoSimulador>();
                    IRepository repSubMecanicaSimulador = context.CreateRepository<SubMecanicaSimulador>();
                    IRepository repFatorConversaoMecanicaSimulador = context.CreateRepository<FatorConversaoMecanicaSimulador>();




                    if (repFatorConversaoSimulador.Filter<FatorConversaoSimulador>(x => x.Nome.ToLower() == fatorConversaoSimuladorModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = "Fator de Conversão já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        FatorConversaoSimulador fatorConversaoSimulador = new FatorConversaoSimulador();
                        fatorConversaoSimulador.Nome = fatorConversaoSimuladorModel.Nome;
                        fatorConversaoSimulador.Descricao = fatorConversaoSimuladorModel.Descricao;
                        fatorConversaoSimulador.Mensagem = fatorConversaoSimuladorModel.Mensagem;
                        fatorConversaoSimulador.TipoPonto = fatorConversaoSimuladorModel.TipoPonto;
                        fatorConversaoSimulador.TipoConversao = fatorConversaoSimuladorModel.TipoConversao;
                        fatorConversaoSimulador.MultiplicadorValor = fatorConversaoSimuladorModel.MultiplicadorValor;
                        fatorConversaoSimulador.MultiplicadorPontos = fatorConversaoSimuladorModel.MultiplicadorPontos;

                        fatorConversaoSimulador.Ativo = true;
                        fatorConversaoSimulador.DataAlteracao = DateTime.Now;
                        fatorConversaoSimulador.DataInclusao = DateTime.Now;

                        //Vinculo de SubMecanica
                        List<FatorConversaoMecanicaSimulador> listFatorConversaoMecanicaSimulador = new List<FatorConversaoMecanicaSimulador>();

                        if (fatorConversaoSimuladorModel.IdSubMecanicaSimulador == null || (fatorConversaoSimuladorModel.IdSubMecanicaSimulador[0] == 0 && fatorConversaoSimuladorModel.IdSubMecanicaSimulador.Length == 1))
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo SubMecânica." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            int SubMecanicaId = fatorConversaoSimuladorModel.IdSubMecanicaSimulador[0];
                            var sub = repFatorConversaoMecanicaSimulador.Find<FatorConversaoMecanicaSimulador>(x => x.Ativo == true && x.IdSubMecanicaSimulador == SubMecanicaId);
                            if (sub != null)
                            {
                                data = new { ok = false, msg = "A Sub Mecânica informada já esta vinculada a um fator de conversão." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }

                        foreach (var item in fatorConversaoSimuladorModel.IdSubMecanicaSimulador)
                        {
                            if (item > 0)
                            {
                                listFatorConversaoMecanicaSimulador.Add(new FatorConversaoMecanicaSimulador
                                {
                                    IdFatorConversaoSimulador = fatorConversaoSimuladorModel.Id,
                                    Ativo = true,
                                    IdSubMecanicaSimulador = item,
                                    DataAlteracao = DateTime.Now,
                                    DataInclusao = DateTime.Now
                                });
                            }
                        }
                        fatorConversaoSimulador.FatorConversaoMecanicaSimulador = listFatorConversaoMecanicaSimulador;


                        //Vinculo de Pontos
                        if (!string.IsNullOrEmpty(fatorConversaoSimuladorModel.PontosSimulador))
                        {

                            List<FatorConversaoPontosSimulador> listFatorConversaoPontosSimulador = new List<FatorConversaoPontosSimulador>();

                            foreach (var item in fatorConversaoSimuladorModel.PontosSimulador.Split(','))
                            {
                                FatorConversaoPontosSimulador model = new FatorConversaoPontosSimulador();
                                foreach (var obj in item.Split('_'))
                                {
                                    if (obj.Contains("valorinicial"))
                                    {
                                        model.ValorInicial = Convert.ToInt32(obj.Replace("valorinicial", ""));
                                    }
                                    else if (obj.Contains("valorfinal"))
                                    {
                                        model.ValorFinal = Convert.ToInt32(obj.Replace("valorfinal", ""));
                                    }
                                    else if (obj.Contains("pontos"))
                                    {
                                        model.Pontos = Convert.ToInt32(obj.Replace("pontos", ""));
                                    }
                                }

                                listFatorConversaoPontosSimulador.Add(new FatorConversaoPontosSimulador
                                {
                                    IdFatorConversaoSimulador = fatorConversaoSimuladorModel.Id,
                                    ValorInicial = model.ValorInicial,
                                    ValorFinal = model.ValorFinal,
                                    Pontos = model.Pontos,
                                    Ativo = true,
                                    DataInclusao = DateTime.Now,
                                    DataAlteracao = DateTime.Now
                                });

                            }
                            fatorConversaoSimulador.FatorConversaoPontosSimulador = listFatorConversaoPontosSimulador;

                        }
                        using (TransactionScope scope = new TransactionScope())
                        {
                            repFatorConversaoSimulador.Create(fatorConversaoSimulador);
                            repFatorConversaoSimulador.SaveChanges();
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
        public ActionResult Edit(FatorConversaoSimuladorModel fatorConversaoSimuladorModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repFatorConversaoSimulador = context.CreateRepository<FatorConversaoSimulador>();
                    var fatorConversaoSimulador = repFatorConversaoSimulador.Find<FatorConversaoSimulador>(fatorConversaoSimuladorModel.Id);
                    IRepository repFatorConversaoMecanicaSimulador = context.CreateRepository<FatorConversaoMecanicaSimulador>();
                    IRepository repFatorConversaoPontosSimulador = context.CreateRepository<FatorConversaoPontosSimulador>();

                    if (fatorConversaoSimulador != null)
                    {
                        if (repFatorConversaoSimulador.Filter<FatorConversaoSimulador>(x => x.Nome == fatorConversaoSimuladorModel.Nome && x.Id != fatorConversaoSimuladorModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            data = new { ok = false, msg = "Já existe um Fator de Conversão cadastrado com o Nome informado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            fatorConversaoSimulador.Nome = fatorConversaoSimuladorModel.Nome;
                            fatorConversaoSimulador.Descricao = fatorConversaoSimuladorModel.Descricao;
                            fatorConversaoSimulador.Mensagem = fatorConversaoSimuladorModel.Mensagem;
                            fatorConversaoSimulador.TipoPonto = fatorConversaoSimuladorModel.TipoPonto;
                            fatorConversaoSimulador.TipoConversao = fatorConversaoSimuladorModel.TipoConversao;
                            fatorConversaoSimulador.MultiplicadorValor = fatorConversaoSimuladorModel.MultiplicadorValor;
                            fatorConversaoSimulador.MultiplicadorPontos = fatorConversaoSimuladorModel.MultiplicadorPontos;

                            fatorConversaoSimulador.DataAlteracao = DateTime.Now;

                            //Vinculo de SubMecanica
                            List<FatorConversaoMecanicaSimulador> listFatorConversaoMecanicaSimulador = new List<FatorConversaoMecanicaSimulador>();

                            if (fatorConversaoSimuladorModel.IdSubMecanicaSimulador == null || (fatorConversaoSimuladorModel.IdSubMecanicaSimulador[0] == 0 && fatorConversaoSimuladorModel.IdSubMecanicaSimulador.Length == 1))
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo SubMecânica." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                int SubMecanicaId = fatorConversaoSimuladorModel.IdSubMecanicaSimulador[0];
                                var sub = repFatorConversaoMecanicaSimulador.Find<FatorConversaoMecanicaSimulador>(x => x.Ativo == true && x.IdSubMecanicaSimulador == SubMecanicaId && x.IdFatorConversaoSimulador != fatorConversaoSimuladorModel.Id);
                                if (sub != null)
                                {
                                    data = new { ok = false, msg = "A Sub Mecânica informada já esta vinculada a um fator de conversão." };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                            }

                            //Atualiza as mecanicas e submecanicas atuais para false
                            listFatorConversaoMecanicaSimulador = repFatorConversaoMecanicaSimulador.Filter<FatorConversaoMecanicaSimulador>(x => x.IdFatorConversaoSimulador == fatorConversaoSimulador.Id && x.Ativo == true).ToList();
                            foreach (var item in listFatorConversaoMecanicaSimulador)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }

                            foreach (var item in fatorConversaoSimuladorModel.IdSubMecanicaSimulador)
                            {
                                if (item > 0)
                                {
                                    listFatorConversaoMecanicaSimulador.Add(new FatorConversaoMecanicaSimulador
                                    {
                                        IdFatorConversaoSimulador = fatorConversaoSimuladorModel.Id,
                                        Ativo = true,
                                        IdSubMecanicaSimulador = item,
                                        DataAlteracao = DateTime.Now,
                                        DataInclusao = DateTime.Now
                                    });
                                }
                            }
                            fatorConversaoSimulador.FatorConversaoMecanicaSimulador = listFatorConversaoMecanicaSimulador;

                            //Vinculo de Pontos
                            List<FatorConversaoPontosSimulador> listFatorConversaoPontosSimulador = new List<FatorConversaoPontosSimulador>();

                            listFatorConversaoPontosSimulador = repFatorConversaoPontosSimulador.Filter<FatorConversaoPontosSimulador>(x => x.IdFatorConversaoSimulador == fatorConversaoSimulador.Id && x.Ativo == true).ToList();
                            foreach (var item in listFatorConversaoPontosSimulador)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }
                            if (!string.IsNullOrEmpty(fatorConversaoSimuladorModel.PontosSimulador))
                            {
                                foreach (var item in fatorConversaoSimuladorModel.PontosSimulador.Split(','))
                                {
                                    FatorConversaoPontosSimulador model = new FatorConversaoPontosSimulador();
                                    foreach (var obj in item.Split('_'))
                                    {
                                        if (obj.Contains("valorinicial"))
                                        {
                                            model.ValorInicial = Convert.ToInt32(obj.Replace("valorinicial", ""));
                                        }
                                        else if (obj.Contains("valorfinal"))
                                        {
                                            model.ValorFinal = Convert.ToInt32(obj.Replace("valorfinal", ""));
                                        }
                                        else if (obj.Contains("pontos"))
                                        {
                                            model.Pontos = Convert.ToInt32(obj.Replace("pontos", ""));
                                        }
                                    }

                                    listFatorConversaoPontosSimulador.Add(new FatorConversaoPontosSimulador
                                    {
                                        IdFatorConversaoSimulador = fatorConversaoSimuladorModel.Id,
                                        ValorInicial = model.ValorInicial,
                                        ValorFinal = model.ValorFinal,
                                        Pontos = model.Pontos,
                                        Ativo = true,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });

                                }
                                fatorConversaoSimulador.FatorConversaoPontosSimulador = listFatorConversaoPontosSimulador;
                            }
                            using (TransactionScope scope = new TransactionScope())
                            {
                                repFatorConversaoSimulador.Update(fatorConversaoSimulador);
                                repFatorConversaoSimulador.SaveChanges();
                                scope.Complete();
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, Fator de Conversão não encontrado." };
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
                    IRepository repFatorConversaoSimulador = context.CreateRepository<FatorConversaoSimulador>();

                    var fatorConversaoSimulador = repFatorConversaoSimulador.Find<FatorConversaoSimulador>(Id);

                    if (fatorConversaoSimulador != null)
                    {
                        fatorConversaoSimulador.DataAlteracao = DateTime.Now;
                        fatorConversaoSimulador.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repFatorConversaoSimulador.Update(fatorConversaoSimulador);
                            repFatorConversaoSimulador.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Fator de Conversão inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Fator de Conversão não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o Fator de Conversão." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Internal Functions"


        #endregion

    }
}
