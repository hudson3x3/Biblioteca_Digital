using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using System.Transactions;
using System.Collections;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Infrastructure.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class SubMecanicaSimuladorController : BaseController
    {
        private readonly SubMecanicaSimuladorService _subMecanicaSimuladorService = new SubMecanicaSimuladorService();

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repSubMecanicaSimulador = context.CreateRepository<SubMecanicaSimulador>();
                List<SubMecanicaSimuladorModel> listSubMecanicaSimuladorModel = new List<SubMecanicaSimuladorModel>();
                foreach (var item in repSubMecanicaSimulador.Filter<SubMecanicaSimulador>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listSubMecanicaSimuladorModel.Add(new SubMecanicaSimuladorModel
                    {
                        DataAlteracao = item.DataAlteracao,
                        DataInclusao = item.DataInclusao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Descricao = item.Descricao
                    });
                }
                return View(listSubMecanicaSimuladorModel);
            }
        }

        public ActionResult Create()
        {
            return View(new SubMecanicaSimuladorModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repSubMecanicaSimulador = context.CreateRepository<SubMecanicaSimulador>();

                var subMecanicaSimulador = repSubMecanicaSimulador.Find<SubMecanicaSimulador>(Id);

                SubMecanicaSimuladorModel subMecanicaSimuladorModel = new SubMecanicaSimuladorModel();

                if (subMecanicaSimulador != null)
                {
                    subMecanicaSimuladorModel.Ativo = subMecanicaSimulador.Ativo;
                    subMecanicaSimuladorModel.Id = subMecanicaSimulador.Id;
                    subMecanicaSimuladorModel.Descricao = subMecanicaSimulador.Descricao;
                    subMecanicaSimuladorModel.LinkDownload = subMecanicaSimulador.LinkDownload;
                }

                //Mecanica
                ArrayList arrMecanicaId = new ArrayList();
                foreach (var item in subMecanicaSimulador.MecanicaSubMecanicaSimulador)
                {
                    arrMecanicaId.Add(item.IdMecanicaSimulador);
                }
                subMecanicaSimuladorModel.ArrMecanicaSimuladorId = arrMecanicaId;

                //Campanha
                ArrayList arrCampanhaId = new ArrayList();
                foreach (var item in subMecanicaSimulador.MecanicaSubMecanicaSimulador)
                {
                    arrCampanhaId.Add(item.IdCampanhaSimulador);
                }
                subMecanicaSimuladorModel.ArrCampanhaSimuladorId = arrCampanhaId;


                return View(subMecanicaSimuladorModel);
            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(SubMecanicaSimuladorModel subMecanicaSimuladorModel)
        {
            try
            {
                var data = new object();
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    SubMecanicaSimulador subMecanicaSimulador = new SubMecanicaSimulador();
                    IRepository repSubMecanicaSimulador = context.CreateRepository<SubMecanicaSimulador>();

                    if (repSubMecanicaSimulador.Filter<SubMecanicaSimulador>(x => x.Descricao.ToLower() == subMecanicaSimuladorModel.Descricao.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        data = new { ok = false, msg = "SubMecanica já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (subMecanicaSimuladorModel.FileArquivo != null)
                        {
                            var uploadFileResult = UploadFile.Upload(
                                subMecanicaSimuladorModel.FileArquivo,
                                Settings.Extensoes.ExtensoesPermitidasImagens,
                                int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBPdf), "imagemSubMecanica/");

                            if (!uploadFileResult.arquivoSalvo)
                            {
                                data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                subMecanicaSimuladorModel.LinkDownload = uploadFileResult.nomeArquivoGerado;
                            }
                        }

                        //Vinculo de SubMecanica
                        List<MecanicaSubMecanicaSimulador> listMecanicaSubMecanicaSimulador = new List<MecanicaSubMecanicaSimulador>();
                        if (subMecanicaSimuladorModel.IdMecanicaSimulador == null)
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Mecânica." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        if (subMecanicaSimuladorModel.IdCampanhaSimulador == null)
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Campanha." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                        //Os selecionados na tela SubMecanica
                        foreach (var item in subMecanicaSimuladorModel.IdMecanicaSimulador)
                        {
                            foreach (var itemCampanha in subMecanicaSimuladorModel.IdCampanhaSimulador)
                            {
                                if (item > 0 && itemCampanha > 0)
                                {
                                    listMecanicaSubMecanicaSimulador.Add(new MecanicaSubMecanicaSimulador
                                    {
                                        IdSubMecanicaSimulador = subMecanicaSimuladorModel.Id,
                                        IdMecanicaSimulador = item,
                                        IdCampanhaSimulador = itemCampanha,
                                        Ativo = true,
                                        DataAlteracao = DateTime.Now,
                                        DataInclusao = DateTime.Now
                                    });
                                }

                            }
                        }
                        subMecanicaSimulador.MecanicaSubMecanicaSimulador = listMecanicaSubMecanicaSimulador;

                        subMecanicaSimulador.LinkDownload = subMecanicaSimuladorModel.LinkDownload;
                        subMecanicaSimulador.Descricao = subMecanicaSimuladorModel.Descricao;
                        subMecanicaSimulador.Ativo = true;
                        subMecanicaSimulador.DataAlteracao = DateTime.Now;
                        subMecanicaSimulador.DataInclusao = DateTime.Now;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repSubMecanicaSimulador.Create(subMecanicaSimulador);
                            repSubMecanicaSimulador.SaveChanges();
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
        public ActionResult Edit(SubMecanicaSimuladorModel subMecanicaSimuladorModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repSubMecanicaSimulador = context.CreateRepository<SubMecanicaSimulador>();
                    IRepository repMecanicaSubMecanicaSimulador = context.CreateRepository<MecanicaSubMecanicaSimulador>();
                    var subMecanicaSimulador = repSubMecanicaSimulador.Find<SubMecanicaSimulador>(subMecanicaSimuladorModel.Id);
                    var arquivoAlterado = false;
                    UploadFileResult uploadFileResult = null;

                    //Verifica se o arquivo foi postado
                    if (subMecanicaSimuladorModel.FileArquivo != null)
                    {

                        uploadFileResult = UploadFile.Upload(
                        subMecanicaSimuladorModel.FileArquivo,
                        Settings.Extensoes.ExtensoesPermitidasImagens,
                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                        "imagemSubMecanica/");

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
                        subMecanicaSimuladorModel.LinkDownload = subMecanicaSimulador.LinkDownload;
                    }

                    if (subMecanicaSimulador != null)
                    {
                        if (repSubMecanicaSimulador.Filter<SubMecanicaSimulador>(x => x.Descricao == subMecanicaSimuladorModel.Descricao && x.Id != subMecanicaSimuladorModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            data = new { ok = false, msg = "Já existe uma SubMecânica cadastrada com o Nome informado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            if (arquivoAlterado)
                            {
                                subMecanicaSimulador.LinkDownload = uploadFileResult.nomeArquivoGerado;
                            }
                            subMecanicaSimulador.Descricao = subMecanicaSimuladorModel.Descricao;
                            subMecanicaSimulador.DataAlteracao = DateTime.Now;

                            //Vinculo de SubMecanica
                            List<MecanicaSubMecanicaSimulador> listMecanicaSubMecanicaSimulador = new List<MecanicaSubMecanicaSimulador>();
                            if (subMecanicaSimuladorModel.IdMecanicaSimulador == null)
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Mecânica." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            if (subMecanicaSimuladorModel.IdCampanhaSimulador == null)
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Campanha." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }

                            for (var i = 0; i < subMecanicaSimulador.MecanicaSubMecanicaSimulador.Count; i++)
                            {
                                var obj = subMecanicaSimulador.MecanicaSubMecanicaSimulador.ToArray()[i];
                                subMecanicaSimulador.MecanicaSubMecanicaSimulador.Remove(obj);
                            }

                            //Os selecionados na tela
                            foreach (var item in subMecanicaSimuladorModel.IdMecanicaSimulador)
                            {
                                foreach (var itemCampanha in subMecanicaSimuladorModel.IdCampanhaSimulador)
                                {
                                    if (item > 0 && itemCampanha > 0)
                                    {
                                        listMecanicaSubMecanicaSimulador.Add(new MecanicaSubMecanicaSimulador
                                        {
                                            IdSubMecanicaSimulador = subMecanicaSimulador.Id,
                                            IdMecanicaSimulador = item,
                                            IdCampanhaSimulador = itemCampanha,
                                            Ativo = true,
                                            DataAlteracao = DateTime.Now,
                                            DataInclusao = DateTime.Now
                                        });
                                    }

                                }
                            }
                            subMecanicaSimulador.MecanicaSubMecanicaSimulador = listMecanicaSubMecanicaSimulador;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repSubMecanicaSimulador.Update(subMecanicaSimulador);
                                repSubMecanicaSimulador.SaveChanges();
                                scope.Complete();
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, SubMecânica não encontrada." };
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
                    IRepository repSubMecanicaSimulador = context.CreateRepository<SubMecanicaSimulador>();

                    var subMecanicaSimulador = repSubMecanicaSimulador.Find<SubMecanicaSimulador>(Id);

                    if (subMecanicaSimulador != null)
                    {
                        subMecanicaSimulador.DataAlteracao = DateTime.Now;
                        subMecanicaSimulador.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repSubMecanicaSimulador.Update(subMecanicaSimulador);
                            repSubMecanicaSimulador.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "SubMecânica inativada com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, SubMecânica não encontrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a SubMecânica." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Internal Functions"


        #endregion

    }
}
