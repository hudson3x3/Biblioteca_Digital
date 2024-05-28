using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using System.Transactions;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using System.Collections;
using System.IO;
using GrupoLTM.WebSmart.Admin.Attributes;


namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class ConteudoController : BaseController
    {

        #region "Actions"

        public ActionResult Index(string Pagina)
        {
            //Configura pagina
            var tipoModuloConteudoModel = new TipoModuloConteudoModel
            {
                TipoModuloModel = PaginaConteudo(Pagina)
            };

            //Carrega Conteúdos
            List<ConteudoModel> listConteudoModel = new List<ConteudoModel>();
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();

                var moduloId = Convert.ToInt32(tipoModuloConteudoModel.TipoModuloModel.ModuloFixo);

                foreach (var item in repConteudo.Filter<Conteudo>(
                    x => x.Ativo == true
                    && x.ModuloId == moduloId
                    ).OrderBy(x => x.Nome).ToList())
                {
                    listConteudoModel.Add(new ConteudoModel
                    {
                        Id = item.Id,
                        TipoModuloId = item.Modulo.TipoModuloId,
                        ModuloId = item.ModuloId,
                        Nome = item.Nome,
                        Titulo = item.Titulo,
                        Descricao = item.Descricao,
                        Pretexto = item.Pretexto,
                        Texto = item.Texto,
                        LinkAcesso = item.LinkAcesso,
                        Alt = item.Alt,
                        ImgP = item.ImgP,
                        ImgM = item.ImgM,
                        imgG = item.imgG,
                        LinkDownload = item.LinkDownload,
                        DataInicio = item.DataInicio,
                        DataFim = item.DataFim,
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        AtivoHome = item.AtivoHome
                    });
                }
            }
            tipoModuloConteudoModel.ConteudoModel = listConteudoModel;

            //Retorno para a View com as duas models
            return View(tipoModuloConteudoModel);
        }

        public ActionResult Create(string Pagina)
        {
            //Configura pagina
            TipoModuloConteudoModel tipoModuloConteudoModel = new TipoModuloConteudoModel();
            tipoModuloConteudoModel.TipoModuloModel = PaginaConteudo(Pagina);
            return View(tipoModuloConteudoModel);
        }

        public ActionResult Edit(int Id)
        {
            TipoModuloConteudoModel tipoModuloConteudoModel = new TipoModuloConteudoModel();

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConteudo = context.CreateRepository<Conteudo>();
                var Conteudo = repConteudo.Find<Conteudo>(Id);

                var conteudoModel = new ConteudoModel();

                if (Conteudo == null)
                    //Redireciona para a Home (Conteúdo não encontrado)
                    return RedirectToAction("Index", "Home");

                conteudoModel.Id = Conteudo.Id;
                conteudoModel.TipoModuloId = Conteudo.Modulo.TipoModuloId;
                conteudoModel.ModuloId = Conteudo.ModuloId;
                conteudoModel.Nome = Conteudo.Nome;
                conteudoModel.Titulo = Conteudo.Titulo;
                conteudoModel.Descricao = Conteudo.Descricao;
                conteudoModel.Pretexto = Conteudo.Pretexto;
                conteudoModel.Texto = Conteudo.Texto;
                conteudoModel.LinkAcesso = Conteudo.LinkAcesso;
                conteudoModel.Alt = Conteudo.Alt;
                conteudoModel.ImgP = Conteudo.ImgP;
                conteudoModel.ImgM = Conteudo.ImgM;
                conteudoModel.imgG = Conteudo.imgG;
                conteudoModel.LinkDownload = Conteudo.LinkDownload;
                conteudoModel.DataInicio = Conteudo.DataInicio;
                conteudoModel.DataFim = Conteudo.DataFim;
                conteudoModel.DataInclusao = Conteudo.DataInclusao;
                conteudoModel.DataAlteracao = Conteudo.DataAlteracao;
                conteudoModel.Ativo = Conteudo.Ativo;
                conteudoModel.AtivoHome = Conteudo.AtivoHome;

                //Perfis de Acesso
                ArrayList arrPerfilId = new ArrayList();
                foreach (var item in Conteudo.ConteudoPerfil.Where(x => x.Ativo == true).ToList())
                {
                    arrPerfilId.Add(item.PerfilId);
                }
                conteudoModel.ArrPerfilId = arrPerfilId;

                //Estrutura de Acesso
                ArrayList arrEstruturaId = new ArrayList();
                foreach (var item in Conteudo.ConteudoEstrutura.Where(x => x.Ativo == true).ToList())
                {
                    arrEstruturaId.Add(item.EstruturaId);
                }
                conteudoModel.ArrEstruturaId = arrEstruturaId;

                //Configura pagina
                tipoModuloConteudoModel.TipoModuloModel = PaginaConteudo(Conteudo.Modulo.TipoModulo.Nome);

                //Conteudo
                tipoModuloConteudoModel.ConteudoFirstModel = conteudoModel;

                //Retorna para a view
                return View(tipoModuloConteudoModel);

            }
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Create(ConteudoModel conteudoModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repConteudo = context.CreateRepository<Conteudo>();

                    //Verifica se o conteúdo já existe para o mesmo módulo
                    if (repConteudo.Filter<Conteudo>(
                        x => x.Nome.ToLower() == conteudoModel.Nome.ToLower()
                            && x.ModuloId == conteudoModel.ModuloId
                            && x.Ativo).Any())
                    {
                        data = new { ok = false, msg = "Conteúdo já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    if (repConteudo.Filter<Conteudo>(
     x => x.Nome.ToLower() == conteudoModel.Nome.ToLower()
         && x.ModuloId == conteudoModel.ModuloId
         && x.Ativo).Any())
                    {
                        data = new { ok = false, msg = "Conteúdo já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Upload Arquivo (Obrigatório para Downloads)
                        if (conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Downloads))
                        {
                            if (conteudoModel.FileArquivo != null)
                            {
                                var uploadFileResult = UploadFile.Upload(
                                    conteudoModel.FileArquivo,
                                    Settings.Extensoes.ExtensoesPermitidasArquivos,
                                    int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem), "downloads/");

                                if (!uploadFileResult.arquivoSalvo)
                                {
                                    data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    conteudoModel.LinkDownload = uploadFileResult.nomeArquivoGerado;
                                }
                            }
                            else
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Arquivo." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }


                        //Upload Imagem (Obrigatório para Notícias, Mecânica, Banners e SkinBackg)
                        if (conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Mecanica) || conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Background) || conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Noticias) || conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Banners))
                        {
                            if (conteudoModel.FileImagem != null)
                            {
                                var uploadFileResult = UploadFile.Upload(
                                    conteudoModel.FileImagem,
                                    Settings.Extensoes.ExtensoesPermitidasImagens,
                                    int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                                    "original/");

                                if (!uploadFileResult.arquivoSalvo)
                                {
                                    data = new { ok = false, msg = "Não foi possível gravar a Imagem. " + uploadFileResult.mensagem };
                                    return Json(data, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    //Convert a imagem para 7 resoluções

                                    ConverterImagens(conteudoModel.FileImagem.InputStream, uploadFileResult.nomeArquivoGerado);
                                    //ConverterImagens(uploadFileResult.nomeArquivoGerado, "original/" + uploadFileResult.nomeArquivoGerado);


                                    //Nome da imagem gerada
                                    conteudoModel.imgG = uploadFileResult.nomeArquivoGerado;
                                }
                            }
                            else
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Imagem." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }

                        //Preenche o objeto conteúdo
                        Conteudo Conteudo = new Conteudo();
                        Conteudo.Nome = conteudoModel.Nome;
                        Conteudo.ModuloId = conteudoModel.ModuloId;
                        Conteudo.Titulo = conteudoModel.Titulo;
                        Conteudo.Descricao = conteudoModel.Descricao;
                        Conteudo.Pretexto = conteudoModel.Pretexto;
                        Conteudo.Texto = conteudoModel.Texto;
                        Conteudo.LinkAcesso = conteudoModel.LinkAcesso;
                        Conteudo.Alt = conteudoModel.Alt;
                        Conteudo.ImgP = conteudoModel.ImgP;
                        Conteudo.ImgM = conteudoModel.ImgM;
                        Conteudo.imgG = conteudoModel.imgG;
                        Conteudo.LinkDownload = conteudoModel.LinkDownload;
                        Conteudo.DataInicio = conteudoModel.DataInicio;
                        Conteudo.DataFim = conteudoModel.DataFim;
                        Conteudo.DataInclusao = DateTime.Now;
                        Conteudo.DataAlteracao = DateTime.Now;
                        Conteudo.Ativo = true;
                        Conteudo.AtivoHome = conteudoModel.AtivoHome;

                        //Perfis de Acesso
                        IRepository repConteudoPerfil = context.CreateRepository<ConteudoPerfil>();
                        List<ConteudoPerfil> listConteudoPerfil = new List<ConteudoPerfil>();

                        //Verifica se o perfil foi selecionada
                        if (conteudoModel.PerfilId == null && conteudoModel.ModuloId != Convert.ToInt32(EnumDomain.ModuloFixo.Background))
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Perfis de Acesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                        //Adiciona o perfil se for selecionado
                        if (conteudoModel.PerfilId != null)
                        {
                            foreach (var item in conteudoModel.PerfilId)
                            {
                                if (item > 0)
                                {
                                    listConteudoPerfil.Add(new ConteudoPerfil
                                    {
                                        ConteudoId = Conteudo.Id,
                                        Ativo = true,
                                        PerfilId = item,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });
                                }
                            }
                            Conteudo.ConteudoPerfil = listConteudoPerfil;
                        }


                        //Estrurura de Acesso
                        IRepository repConteudoEstrutura = context.CreateRepository<ConteudoEstrutura>();
                        List<ConteudoEstrutura> listConteudoEstrutura = new List<ConteudoEstrutura>();

                        //Verifica se a Estrutura foi selecionada
                        if (conteudoModel.EstruturaId == null && conteudoModel.ModuloId != Convert.ToInt32(EnumDomain.ModuloFixo.Background))
                        {
                            data = new { ok = false, msg = "Por favor, selecione o campo Estrutura de Acesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }

                        //Adiciona o Estrutura se for selecionado
                        if (conteudoModel.EstruturaId != null)
                        {
                            foreach (var item in conteudoModel.EstruturaId)
                            {
                                if (item > 0)
                                {
                                    listConteudoEstrutura.Add(new ConteudoEstrutura
                                    {
                                        ConteudoId = Conteudo.Id,
                                        Ativo = true,
                                        EstruturaId = item,
                                        DataInclusao = DateTime.Now,
                                        DataAlteracao = DateTime.Now
                                    });
                                }
                            }
                            Conteudo.ConteudoEstrutura = listConteudoEstrutura;
                        }


                        //Grava a informações no banco de dados
                        using (TransactionScope scope = new TransactionScope())
                        {
                            repConteudo.Create(Conteudo);
                            repConteudo.SaveChanges();
                            scope.Complete();
                        }

                        data = new { ok = true, msg = "Dados salvos com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Edit(ConteudoModel conteudoModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();
                    IRepository repConteudo = context.CreateRepository<Conteudo>();
                    var Conteudo = repConteudo.Find<Conteudo>(conteudoModel.Id);

                    //Verifica se o Conteudo existe
                    if (Conteudo != null)
                    {
                        //Verifica se o conteúdo já existe para o mesmo módulo
                        if (repConteudo.Filter<Conteudo>(x => x.Nome == conteudoModel.Nome && x.Id != conteudoModel.Id && x.Ativo && x.ModuloId == conteudoModel.ModuloId).ToList().Any())
                        {
                            data = new { ok = false, msg = "Conteúdo já cadastrado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Upload Arquivo (Obrigatório para Downloads)
                            if (conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Downloads))
                            {
                                //Verifica se o arquivo foi postado
                                if (conteudoModel.FileArquivo != null)
                                {

                                    var uploadFileResult = UploadFile.Upload(
                                        conteudoModel.FileArquivo,
                                        Settings.Extensoes.ExtensoesPermitidasArquivos,
                                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                                        "downloads/");

                                    if (!uploadFileResult.arquivoSalvo)
                                    {
                                        data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                                        return Json(data, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        conteudoModel.LinkDownload = uploadFileResult.nomeArquivoGerado;
                                    }
                                }
                                else
                                {
                                    //Se não, mantem o do banco de dados
                                    conteudoModel.LinkDownload = Conteudo.LinkDownload;
                                }
                            }

                            //Upload Imagem (Obrigatório para Notícias, Mecânica, Banners e SkinBackg)
                            if (conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Mecanica) || conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Background) || conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Noticias) || conteudoModel.TipoModuloId == Convert.ToInt32(EnumDomain.TipoModulo.Banners))
                            {
                                //Verifica se o arquivo foi postado
                                if (conteudoModel.FileImagem != null)
                                {

                                    var uploadFileResult = UploadFile.Upload(
                                        conteudoModel.FileImagem,
                                        Settings.Extensoes.ExtensoesPermitidasImagens,
                                        int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                                        "original/");

                                    if (!uploadFileResult.arquivoSalvo)
                                    {
                                        data = new { ok = false, msg = "Não foi possível gravar a Imagem. " + uploadFileResult.mensagem };
                                        return Json(data, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        //Convert a imagem para 7 resoluções
                                        ConverterImagens(conteudoModel.FileImagem.InputStream, uploadFileResult.nomeArquivoGerado);

                                        //Nome da imagem gerada
                                        conteudoModel.imgG = uploadFileResult.nomeArquivoGerado;
                                    }
                                }
                                else
                                {
                                    //Se não, matem a do banco de dados
                                    conteudoModel.imgG = Conteudo.imgG;
                                }
                            }

                            //Preenche o objeto conteúdo
                            Conteudo.Nome = conteudoModel.Nome;
                            Conteudo.ModuloId = conteudoModel.ModuloId;
                            Conteudo.Titulo = conteudoModel.Titulo;
                            Conteudo.Descricao = conteudoModel.Descricao;
                            Conteudo.Pretexto = conteudoModel.Pretexto;
                            Conteudo.Texto = conteudoModel.Texto;
                            Conteudo.LinkAcesso = conteudoModel.LinkAcesso;
                            Conteudo.Alt = conteudoModel.Alt;
                            Conteudo.ImgP = conteudoModel.ImgP;
                            Conteudo.ImgM = conteudoModel.ImgM;
                            Conteudo.imgG = conteudoModel.imgG;
                            Conteudo.LinkDownload = conteudoModel.LinkDownload;
                            Conteudo.DataInicio = conteudoModel.DataInicio;
                            Conteudo.DataFim = conteudoModel.DataFim;
                            Conteudo.DataAlteracao = DateTime.Now;
                            Conteudo.Ativo = true;
                            Conteudo.AtivoHome = conteudoModel.AtivoHome;

                            //Perfis de Acesso
                            IRepository repConteudoPerfil = context.CreateRepository<ConteudoPerfil>();
                            var listConteudoPerfil = repConteudoPerfil.Filter<ConteudoPerfil>(x => x.ConteudoId == Conteudo.Id && x.Ativo == true).ToList();
                            //Perfis Update Atuais
                            foreach (var item in listConteudoPerfil)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }
                            //Perfis Insert novos (Verifica se o perfil foi selecionada)
                            if (conteudoModel.PerfilId == null && conteudoModel.ModuloId != Convert.ToInt32(EnumDomain.ModuloFixo.Background))
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Perfis de Acesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }

                            //Adiciona o perfil, se for selecionado
                            if (conteudoModel.PerfilId != null)
                            {
                                foreach (var item in conteudoModel.PerfilId)
                                {
                                    if (item > 0)
                                    {
                                        listConteudoPerfil.Add(new ConteudoPerfil
                                        {
                                            ConteudoId = Conteudo.Id,
                                            Ativo = true,
                                            PerfilId = item,
                                            DataInclusao = DateTime.Now,
                                            DataAlteracao = DateTime.Now
                                        });
                                    }
                                }
                                Conteudo.ConteudoPerfil = listConteudoPerfil;
                            }

                            //Estrutura de Acesso
                            IRepository repConteudoEstrutura = context.CreateRepository<ConteudoEstrutura>();
                            var listConteudoEstrutura = repConteudoEstrutura.Filter<ConteudoEstrutura>(x => x.ConteudoId == Conteudo.Id && x.Ativo == true).ToList();
                            //Estrutura Update Atuais
                            foreach (var item in listConteudoEstrutura)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }
                            //Estrutura Insert novos (Verifica se a estrutura foi selecionada)
                            if (conteudoModel.EstruturaId == null && conteudoModel.ModuloId != Convert.ToInt32(EnumDomain.ModuloFixo.Background))
                            {
                                data = new { ok = false, msg = "Por favor, selecione o campo Estrutra de Acesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }

                            //Adiciona a estrutura
                            if (conteudoModel.EstruturaId != null)
                            {
                                foreach (var item in conteudoModel.EstruturaId)
                                {
                                    if (item > 0)
                                    {
                                        listConteudoEstrutura.Add(new ConteudoEstrutura
                                        {
                                            ConteudoId = Conteudo.Id,
                                            Ativo = true,
                                            EstruturaId = item,
                                            DataInclusao = DateTime.Now,
                                            DataAlteracao = DateTime.Now
                                        });
                                    }
                                }
                                Conteudo.ConteudoEstrutura = listConteudoEstrutura;
                            }


                            //Grava a informações no banco de dados
                            using (TransactionScope scope = new TransactionScope())
                            {
                                repConteudo.Update(Conteudo);
                                repConteudo.SaveChanges();
                                scope.Complete();
                            }

                            data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Não foi possível salvar os dados, conteúdo não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível salvar os dados. " + exc.Message };
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
                    IRepository repConteudo = context.CreateRepository<Conteudo>();

                    var Conteudo = repConteudo.Find<Conteudo>(Id);

                    if (Conteudo != null)
                    {
                        Conteudo.DataAlteracao = DateTime.Now;
                        Conteudo.Ativo = false;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repConteudo.Update(Conteudo);
                            repConteudo.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Conteudo inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, conteudo não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a conteudo." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Internal Functions"

        internal TipoModuloModel PaginaConteudo(string Pagina)
        {
            TipoModuloModel tipoModuloModel = new TipoModuloModel();

            var pagina = StringHelper.RemoverAcentos(Pagina.ToLower());

            switch (pagina)
            {
                case "noticias":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Noticias);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Noticias;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Noticias;
                    tipoModuloModel.Nome = "Noticias";
                    tipoModuloModel.TituloPagina = "Noticias";
                    break;
                case "videos":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Videos);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Videos;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Videos;
                    tipoModuloModel.Nome = "Vídeos";
                    tipoModuloModel.TituloPagina = "Vídeos";
                    break;
                case "downloads":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Downloads);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Downloads;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Downloads;
                    tipoModuloModel.Nome = "Downloads";
                    tipoModuloModel.TituloPagina = "Downloads";
                    break;
                case "banners":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Banners);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Banners;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Banners;
                    tipoModuloModel.Nome = "Banners";
                    tipoModuloModel.TituloPagina = "Banners";
                    break;
                case "background":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Background);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Background;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Background;
                    tipoModuloModel.Nome = "Background";
                    tipoModuloModel.TituloPagina = "Background";
                    break;
                case "mecanica":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Mecanica);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Mecanica;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Mecanica;
                    tipoModuloModel.Nome = "Mecânica";
                    tipoModuloModel.TituloPagina = "Mecânica";
                    break;
                case "regulamento":
                    tipoModuloModel.Id = Convert.ToInt32(EnumDomain.TipoModulo.Regulamento);
                    tipoModuloModel.TipoModulo = EnumDomain.TipoModulo.Regulamento;
                    tipoModuloModel.ModuloFixo = EnumDomain.ModuloFixo.Regulamento;
                    tipoModuloModel.Nome = "Regulamento";
                    tipoModuloModel.TituloPagina = "Regulamento";
                    break;
            }

            return tipoModuloModel;
        }

        //internal bool ConverterImagens(string nomeArquivo, string imagemCaminho)
        //{
        //    try
        //    {
        //        bool bln = false;
        //        Imagem imagem = new Imagem();

        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "bannerHome/", 1000, 300);
        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "boxHome/", 360, 194);
        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "galeria/", 278, 278);
        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "galeriaCategoria/", 260, 368);
        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "galeriaInterna/", 500, 500);
        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "noticiaInterna/", 561, 423);
        //        bln = imagem.ConverteImagem(nomeArquivo, imagemCaminho, "noticiasLista/", 520, 261);
        //        return bln;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}

        internal bool ConverterImagens(Stream arquivo, string name)
        {
            try
            {
                bool bln = false;
                Imagem imagem = new Imagem();

                bln = imagem.ConverteImagem(arquivo, name, "bannerHome", 1000, 300);
                bln = imagem.ConverteImagem(arquivo, name, "boxHome", 360, 194);
                bln = imagem.ConverteImagem(arquivo, name, "galeria", 278, 278);
                bln = imagem.ConverteImagem(arquivo, name, "galeriaCategoria", 260, 368);
                bln = imagem.ConverteImagem(arquivo, name, "galeriaInterna", 500, 500);
                bln = imagem.ConverteImagem(arquivo, name, "noticiaInterna", 561, 423);
                bln = imagem.ConverteImagem(arquivo, name, "noticiasLista", 520, 261);
                return bln;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        #endregion
    }
}
