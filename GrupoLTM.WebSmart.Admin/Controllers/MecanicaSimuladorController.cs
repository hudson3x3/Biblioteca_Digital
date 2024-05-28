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
    public class MecanicaSimuladorController : BaseController
    {
        private readonly MecanicaSimuladorService _mecanicaSimuladorService = new MecanicaSimuladorService();

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repMecanicaSimulador = context.CreateRepository<MecanicaSimulador>();
                List<MecanicaSimuladorModel> listMecanicaSimuladorModel = new List<MecanicaSimuladorModel>();
                foreach (var item in repMecanicaSimulador.Filter<MecanicaSimulador>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listMecanicaSimuladorModel.Add(new MecanicaSimuladorModel
                    {
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Descricao = item.Descricao
                    });
                }
                return View(listMecanicaSimuladorModel);
            }
        }

        public ActionResult Create()
        {
            MecanicaSimuladorModel obj = new MecanicaSimuladorModel();
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repIconeSimulador = context.CreateRepository<IconeSimulador>();

                var iconeSimuladorList = repIconeSimulador.Filter<IconeSimulador>(x => x.Tipo == (short)EnumDomain.TipoIconeSimulador.Mecanica && x.Ativo == true).ToList();

                obj.IconeSimuladorList = iconeSimuladorList.Select(x => new IconeSimuladorModel
                {
                    Nome = x.Nome,
                    CaminhoImagem = x.CaminhoImagem,
                    Descricao = x.Descricao,
                    Id = x.Id
                }).ToList();
            }

            return View(obj);
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repMecanicaSimulador = context.CreateRepository<MecanicaSimulador>();
                IRepository repIconeSimulador = context.CreateRepository<IconeSimulador>();

                var iconeSimuladorList = repIconeSimulador.Filter<IconeSimulador>(x => x.Tipo == (short)EnumDomain.TipoIconeSimulador.Mecanica && x.Ativo == true).ToList();
                var mecanicaSimulador = repMecanicaSimulador.Find<MecanicaSimulador>(Id);

                MecanicaSimuladorModel mecanicaSimuladorModel = new MecanicaSimuladorModel();

                if (mecanicaSimulador != null)
                {
                    mecanicaSimuladorModel.Ativo = mecanicaSimulador.Ativo;
                    mecanicaSimuladorModel.Id = mecanicaSimulador.Id;
                    mecanicaSimuladorModel.Nome = mecanicaSimulador.Nome;
                    mecanicaSimuladorModel.Descricao = mecanicaSimulador.Descricao;
                    mecanicaSimuladorModel.IdIconeSimulador = mecanicaSimulador.IdIconeSimulador;


                    //Campanhas
                    ArrayList arrCampanhaId = new ArrayList();
                    foreach (var item in mecanicaSimulador.CampanhaMecanicaSimulador.Where(x => x.Ativo == true && x.IdMecanicaSimulador == mecanicaSimuladorModel.Id).ToList())
                    {
                        arrCampanhaId.Add(item.IdCampanhaSimulador);
                    }

                    //SubMecanica
                    //ArrayList arrSubMecanicaId = new ArrayList();

                    //foreach (var item in mecanicaSimulador.MecanicaSubMecanicaSimulador.Where(x => x.Ativo == true && x.IdMecanicaSimulador == mecanicaSimuladorModel.Id).ToList())
                    //{
                    //    if (item.IdSubMecanicaSimulador > 0)
                    //        arrSubMecanicaId.Add(item.IdSubMecanicaSimulador);
                    //}

                    //mecanicaSimuladorModel.ArrSubMecanicaSimuladorId = arrSubMecanicaId;
                    mecanicaSimuladorModel.ArrCampanhaSimuladorId = arrCampanhaId;
                }

                mecanicaSimuladorModel.IconeSimuladorList = iconeSimuladorList.Select(x => new IconeSimuladorModel
                {
                    Nome = x.Nome,
                    CaminhoImagem = x.CaminhoImagem,
                    Descricao = x.Descricao,
                    Id = x.Id
                }).ToList();

                return View(mecanicaSimuladorModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(MecanicaSimuladorModel mecanicaSimuladorModel)
        {
            try
            {
                var data = new object();
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repMecanicaSimulador = context.CreateRepository<MecanicaSimulador>();

                    if (repMecanicaSimulador.Filter<MecanicaSimulador>(x => x.Nome.ToLower() == mecanicaSimuladorModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = "Mecânica já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        MecanicaSimulador mecanicaSimulador = new MecanicaSimulador();
                        mecanicaSimulador.Nome = mecanicaSimuladorModel.Nome;
                        mecanicaSimulador.Descricao = mecanicaSimuladorModel.Descricao;
                        mecanicaSimulador.IdIconeSimulador = mecanicaSimuladorModel.IdIconeSimulador;
                        mecanicaSimulador.Ativo = true;
                        mecanicaSimulador.DataAlteracao = DateTime.Now;
                        mecanicaSimulador.DataInclusao = DateTime.Now;

                        //Vinculo de Campanha
                        List<CampanhaMecanicaSimulador> listCampanhaMecanicaSimulador = new List<CampanhaMecanicaSimulador>();
                        if (mecanicaSimuladorModel.IdCampanhaSimulador == null)
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Campanha." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        //Os selecionados na tela Campanha
                        foreach (var item in mecanicaSimuladorModel.IdCampanhaSimulador)
                        {
                            if (item > 0)
                            {
                                listCampanhaMecanicaSimulador.Add(new CampanhaMecanicaSimulador
                                {
                                    IdMecanicaSimulador = mecanicaSimuladorModel.Id,
                                    Ativo = true,
                                    IdCampanhaSimulador = item,
                                    DataAlteracao = DateTime.Now,
                                    DataInclusao = DateTime.Now
                                });
                            }
                        }
                        mecanicaSimulador.CampanhaMecanicaSimulador = listCampanhaMecanicaSimulador;

                        //Vinculo de SubMecanica
                        //List<MecanicaSubMecanicaSimulador> listMecanicaSubMecanicaSimulador = new List<MecanicaSubMecanicaSimulador>();
                        //if (mecanicaSimuladorModel.IdSubMecanicaSimulador == null)
                        //{
                        //    data = new { ok = false, msg = "Por favor, selecione o campo SubMecânica." };
                        //    return Json(data, JsonRequestBehavior.AllowGet);
                        //}


                        //Os selecionados na tela SubMecanica
                        //foreach (var item in mecanicaSimuladorModel.IdSubMecanicaSimulador)
                        //{
                        //    if (item > 0)
                        //    {
                        //        listMecanicaSubMecanicaSimulador.Add(new MecanicaSubMecanicaSimulador
                        //        {
                        //            IdMecanicaSimulador = mecanicaSimuladorModel.Id,
                        //            Ativo = true,
                        //            IdSubMecanicaSimulador = item,
                        //            DataAlteracao = DateTime.Now,
                        //            DataInclusao = DateTime.Now
                        //        });
                        //    }
                        //}
                        //mecanicaSimulador.MecanicaSubMecanicaSimulador = listMecanicaSubMecanicaSimulador;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repMecanicaSimulador.Create(mecanicaSimulador);
                            repMecanicaSimulador.SaveChanges();
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
        public ActionResult Edit(MecanicaSimuladorModel mecanicaSimuladorModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repMecanicaSimulador = context.CreateRepository<MecanicaSimulador>();
                    IRepository repCampanhaMecanicaSimulador = context.CreateRepository<CampanhaMecanicaSimulador>();
                    //IRepository repMecanicaSubMecanicaSimulador = context.CreateRepository<MecanicaSubMecanicaSimulador>();
                    var mecanicaSimulador = repMecanicaSimulador.Find<MecanicaSimulador>(mecanicaSimuladorModel.Id);

                    if (mecanicaSimulador != null)
                    {
                        if (repMecanicaSimulador.Filter<MecanicaSimulador>(x => x.Nome == mecanicaSimuladorModel.Nome && x.Id != mecanicaSimuladorModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            data = new { ok = false, msg = "Já existe uma Mecanica cadastrada com o Nome informado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            mecanicaSimulador.Nome = mecanicaSimuladorModel.Nome;
                            mecanicaSimulador.IdIconeSimulador = mecanicaSimuladorModel.IdIconeSimulador;
                            mecanicaSimulador.Descricao = mecanicaSimuladorModel.Descricao;
                            mecanicaSimulador.DataAlteracao = DateTime.Now;

                            //Vinculo de Campanha
                            List<CampanhaMecanicaSimulador> listCampanhaMecanicaSimulador = new List<CampanhaMecanicaSimulador>();
                            if (mecanicaSimuladorModel.IdCampanhaSimulador == null)
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Campanha." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }

                            //Atualiza as campanhas atuais para false
                            listCampanhaMecanicaSimulador = repCampanhaMecanicaSimulador.Filter<CampanhaMecanicaSimulador>(x => x.IdMecanicaSimulador == mecanicaSimulador.Id && x.Ativo == true).ToList();
                            foreach (var item in listCampanhaMecanicaSimulador)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }

                            //Os selecionados na tela
                            foreach (var item in mecanicaSimuladorModel.IdCampanhaSimulador)
                            {
                                if (item > 0)
                                {
                                    listCampanhaMecanicaSimulador.Add(new CampanhaMecanicaSimulador
                                    {
                                        IdMecanicaSimulador = mecanicaSimuladorModel.Id,
                                        Ativo = true,
                                        IdCampanhaSimulador = item,
                                        DataAlteracao = DateTime.Now,
                                        DataInclusao = DateTime.Now
                                    });
                                }
                            }
                            mecanicaSimulador.CampanhaMecanicaSimulador = listCampanhaMecanicaSimulador;

                            //Vinculo de SubMecanica
                            //List<MecanicaSubMecanicaSimulador> listMecanicaSubMecanicaSimulador = new List<MecanicaSubMecanicaSimulador>();
                            //if (mecanicaSimuladorModel.IdSubMecanicaSimulador == null)
                            //{
                            //    data = new { ok = false, msg = "Por favor, selecione o campo SubMecânica." };
                            //    return Json(data, JsonRequestBehavior.AllowGet);
                            //}

                            //Atualiza as SubMecanica atuais para false
                            //listMecanicaSubMecanicaSimulador = repMecanicaSubMecanicaSimulador.Filter<MecanicaSubMecanicaSimulador>(x => x.IdMecanicaSimulador == mecanicaSimulador.Id && x.Ativo == true).ToList();
                            //foreach (var item in listMecanicaSubMecanicaSimulador)
                            //{
                            //    item.Ativo = false;
                            //    item.DataAlteracao = DateTime.Now;
                            //}

                            //Os selecionados na tela
                            //foreach (var item in mecanicaSimuladorModel.IdSubMecanicaSimulador)
                            //{
                            //    if (item > 0)
                            //    {
                            //        listMecanicaSubMecanicaSimulador.Add(new MecanicaSubMecanicaSimulador
                            //        {
                            //            IdMecanicaSimulador = mecanicaSimuladorModel.Id,
                            //            Ativo = true,
                            //            IdSubMecanicaSimulador = item,
                            //            DataAlteracao = DateTime.Now,
                            //            DataInclusao = DateTime.Now
                            //        });
                            //    }
                            //}
                            //mecanicaSimulador.MecanicaSubMecanicaSimulador = listMecanicaSubMecanicaSimulador;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repMecanicaSimulador.Update(mecanicaSimulador);
                                repMecanicaSimulador.SaveChanges();
                                scope.Complete();
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, Mecânica não encontrada." };
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
                    IRepository repMecanicaSimulador = context.CreateRepository<MecanicaSimulador>();

                    var mecanicaSimulador = repMecanicaSimulador.Find<MecanicaSimulador>(Id);

                    if (mecanicaSimulador != null)
                    {
                        mecanicaSimulador.DataAlteracao = DateTime.Now;
                        mecanicaSimulador.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repMecanicaSimulador.Update(mecanicaSimulador);
                            repMecanicaSimulador.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Mecânica inativada com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Mecânica não encontrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a Mecânica." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Internal Functions"


        #endregion

    }
}
