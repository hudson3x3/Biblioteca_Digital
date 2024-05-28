using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using System.Transactions;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class CampanhaSimuladorController : BaseController
    {
        private readonly CampanhaSimuladorService _campanhaSimuladorService = new CampanhaSimuladorService();

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaSimulador = context.CreateRepository<CampanhaSimulador>();
                List<CampanhaSimuladorModel> listCampanhaSimuladorModel = new List<CampanhaSimuladorModel>();
                foreach (var item in repCampanhaSimulador.Filter<CampanhaSimulador>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listCampanhaSimuladorModel.Add(new CampanhaSimuladorModel
                    {
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        NumeroCampanha = item.NumeroCampanha,
                        AnoCampanha = item.AnoCampanha,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Descricao = item.Descricao
                    });
                }
                return View(listCampanhaSimuladorModel);
            }
        }

        public ActionResult obterCampanha()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {

                IRepository repCampanhaSimulador = context.CreateRepository<CampanhaSimulador>();
                var data = repCampanhaSimulador.All<CampanhaSimulador>().ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            return View(new CampanhaSimuladorModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCampanhaSimulador = context.CreateRepository<CampanhaSimulador>();
                var campanhaSimulador = repCampanhaSimulador.Find<CampanhaSimulador>(Id);

                CampanhaSimuladorModel campanhaSimuladorModel = new CampanhaSimuladorModel();

                if (campanhaSimulador != null)
                {
                    campanhaSimuladorModel.Ativo = campanhaSimulador.Ativo;
                    campanhaSimuladorModel.Id = campanhaSimulador.Id;
                    campanhaSimuladorModel.Nome = campanhaSimulador.Nome;
                    campanhaSimuladorModel.Descricao = campanhaSimulador.Descricao;
                    campanhaSimuladorModel.LinkDownload = campanhaSimulador.LinkDownload;
                    campanhaSimuladorModel.NumeroCampanha = campanhaSimulador.NumeroCampanha;
                    campanhaSimuladorModel.AnoCampanha = campanhaSimulador.AnoCampanha;

                }

                return View(campanhaSimuladorModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Create(CampanhaSimuladorModel campanhaSimuladorModel)
        {
            try
            {
                var data = new object();

                if (campanhaSimuladorModel.FileArquivo != null)
                {
                    var uploadFileResult = UploadFile.Upload(
                        campanhaSimuladorModel.FileArquivo,
                        Settings.Extensoes.ExtensoesPermitidasArquivosRegulamento,
                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBPdf), "regulamento/");

                    if (!uploadFileResult.arquivoSalvo)
                    {
                        data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        campanhaSimuladorModel.LinkDownload = uploadFileResult.nomeArquivoGerado;
                    }
                }

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repCampanhaSimulador = context.CreateRepository<CampanhaSimulador>();

                    if (repCampanhaSimulador.Filter<CampanhaSimulador>(x => x.Nome.ToLower() == campanhaSimuladorModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = "Campanha já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (repCampanhaSimulador.Filter<CampanhaSimulador>(x => x.NumeroCampanha == campanhaSimuladorModel.NumeroCampanha && x.Ativo == true && x.AnoCampanha == campanhaSimuladorModel.AnoCampanha).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = "Número da Campanha já esta vinculada a outra campanha no mesmo ano." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else if (campanhaSimuladorModel.AnoCampanha < DateTime.Now.Year)
                    {
                        data = new { ok = false, msg = "Ano da Campanha não pode ser menor que o ano atual." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        CampanhaSimulador campanhaSimulador = new CampanhaSimulador();
                        campanhaSimulador.Nome = campanhaSimuladorModel.Nome;
                        campanhaSimulador.Descricao = campanhaSimuladorModel.Descricao;
                        campanhaSimulador.NumeroCampanha = campanhaSimuladorModel.NumeroCampanha;
                        campanhaSimulador.AnoCampanha = campanhaSimuladorModel.AnoCampanha;
                        campanhaSimulador.Ativo = true;
                        campanhaSimulador.DataAlteracao = DateTime.Now;
                        campanhaSimulador.DataInclusao = DateTime.Now;
                        campanhaSimulador.LinkDownload = campanhaSimuladorModel.LinkDownload;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repCampanhaSimulador.Create(campanhaSimulador);
                            repCampanhaSimulador.SaveChanges();
                            repCampanhaSimulador.Dispose();
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
        public ActionResult Edit(CampanhaSimuladorModel campanhaSimuladorModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repCampanhaSimulador = context.CreateRepository<CampanhaSimulador>();
                    var campanhaSimulador = repCampanhaSimulador.Find<CampanhaSimulador>(campanhaSimuladorModel.Id);
                    var arquivoAlterado = false;
                    UploadFileResult uploadFileResult = null;

                    //Verifica se o arquivo foi postado
                    if (campanhaSimuladorModel.FileArquivo != null)
                    {

                        uploadFileResult = UploadFile.Upload(
                        campanhaSimuladorModel.FileArquivo,
                        Settings.Extensoes.ExtensoesPermitidasArquivosRegulamento,
                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                        "regulamento/");

                        if (!uploadFileResult.arquivoSalvo)
                        {
                            data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            arquivoAlterado = true;
                        }
                    }
                    else
                    {
                        //Se não, mantem o do banco de dados
                        campanhaSimuladorModel.LinkDownload = campanhaSimulador.LinkDownload;
                    }

                    if (campanhaSimulador != null)
                    {
                        if (repCampanhaSimulador.Filter<CampanhaSimulador>(x => x.Nome == campanhaSimuladorModel.Nome && x.Id != campanhaSimuladorModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            data = new { ok = false, msg = "Já existe uma Campanha cadastrada com o Nome informado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else if (repCampanhaSimulador.Filter<CampanhaSimulador>(x => x.NumeroCampanha == campanhaSimuladorModel.NumeroCampanha && x.Ativo == true && x.AnoCampanha == campanhaSimuladorModel.AnoCampanha && x.Id != campanhaSimuladorModel.Id).ToList().Count() > 0)
                        {
                            data = new { ok = false, msg = "Número da Campanha já esta vinculada a outra campanha no mesmo ano." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else if (campanhaSimuladorModel.AnoCampanha < DateTime.Now.Year)
                        {
                            data = new { ok = false, msg = "Ano da Campanha não pode ser menor que o ano atual." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            campanhaSimulador.Nome = campanhaSimuladorModel.Nome;
                            campanhaSimulador.NumeroCampanha = campanhaSimuladorModel.NumeroCampanha;
                            campanhaSimulador.AnoCampanha = campanhaSimuladorModel.AnoCampanha;
                            campanhaSimulador.Descricao = campanhaSimuladorModel.Descricao;
                            campanhaSimulador.DataAlteracao = DateTime.Now;
                            if (arquivoAlterado)
                            {
                                campanhaSimulador.LinkDownload = uploadFileResult.nomeArquivoGerado;
                            }

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repCampanhaSimulador.Update(campanhaSimulador);
                                repCampanhaSimulador.SaveChanges();
                                repCampanhaSimulador.Dispose();
                                scope.Complete();
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, Campanha não encontrada." };
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
                    IRepository repCampanhaSimulador = context.CreateRepository<CampanhaSimulador>();

                    var campanhaSimulador = repCampanhaSimulador.Find<CampanhaSimulador>(Id);

                    if (campanhaSimulador != null)
                    {
                        campanhaSimulador.DataAlteracao = DateTime.Now;
                        campanhaSimulador.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repCampanhaSimulador.Update(campanhaSimulador);
                            repCampanhaSimulador.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Campanha inativada com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Campanha não encontrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a Campanha." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Internal Functions"


        #endregion

    }
}
