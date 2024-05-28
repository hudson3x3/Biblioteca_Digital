using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class ParticipanteController : BaseController
    {
        #region "Actions"

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new ParticipanteModel());
        }

        public ActionResult Vago()
        {
            return View(new ParticipanteModel());
        }

        public ActionResult Edit(int Id)
        {
            ParticipanteModel ParticipanteModel = new ParticipanteModel();
            ParticipanteModel = CarregaParticipante(Id, null, null, null).FirstOrDefault();

            if (ParticipanteModel != null)
            {
                return View(ParticipanteModel);
            }
            else
            {
                return View(new ParticipanteModel());
            }
        }

        public ActionResult Lote()
        {
            return View(new ParticipanteModel());
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(ParticipanteModel ParticipanteModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repParticipante = context.CreateRepository<Participante>();

                    switch ((EnumDomain.TipoValidacaoPositiva)GrupoLTM.WebSmart.Admin.Helpers.LoginHelper.GetLoginModel().TipoValidacaoPositivaId)
                    {
                        case EnumDomain.TipoValidacaoPositiva.CNPJ1Campo:
                            if (ParticipanteModel.CNPJ != null && repParticipante.Filter<Participante>(x => x.CNPJ == ParticipanteModel.CNPJ).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "CNPJ já cadastrado" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case EnumDomain.TipoValidacaoPositiva.CNPJeCPF2Campos:
                            if (ParticipanteModel.CNPJ != null && ParticipanteModel.CPF != null && repParticipante.Filter<Participante>(x => x.CNPJ == ParticipanteModel.CNPJ
                                && x.CPF == ParticipanteModel.CPF).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "CNPJ e CPF já cadastrado" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        //case EnumDomain.TipoValidacaoPositiva.Codigocliente1Campo:
                        //    if (ParticipanteModel.Codigo != null && repParticipante.Filter<Participante>(x => x.Codigo == ParticipanteModel.Codigo).ToList().Count() > 0)
                        //    {
                        //        var data = new { ok = false, msg = "Código já cadastrado" };
                        //        return Json(data, JsonRequestBehavior.AllowGet);
                        //    }
                        //    break;
                        //case EnumDomain.TipoValidacaoPositiva.CodigoeCNPJ2Campos:
                        //    if (ParticipanteModel.CNPJ != null && ParticipanteModel.Codigo != null && repParticipante.Filter<Participante>(x => x.CNPJ == ParticipanteModel.CNPJ
                        //        && x.Codigo == ParticipanteModel.Codigo).ToList().Count() > 0)
                        //    {
                        //        var data = new { ok = false, msg = "CNPJ e código já cadastrado" };
                        //        return Json(data, JsonRequestBehavior.AllowGet);
                        //    }
                        //    break;
                        //case EnumDomain.TipoValidacaoPositiva.CodigoeCPF2Campos:
                        //    if (ParticipanteModel.CPF != null && ParticipanteModel.Codigo != null && repParticipante.Filter<Participante>(x => x.CPF == ParticipanteModel.CPF
                        //        && x.Codigo == ParticipanteModel.Codigo).ToList().Count() > 0)
                        //    {
                        //        var data = new { ok = false, msg = "CPF e código já cadastrado" };
                        //        return Json(data, JsonRequestBehavior.AllowGet);
                        //    }
                        //    break;
                        case EnumDomain.TipoValidacaoPositiva.CPF:
                            if (ParticipanteModel.CPF != null && repParticipante.Filter<Participante>(x => x.CPF == ParticipanteModel.CPF).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "CPF já cadastrado" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case EnumDomain.TipoValidacaoPositiva.CPFeDataNascimento2Campos:
                            if (ParticipanteModel.CPF != null && ParticipanteModel.DataNascimento != null && repParticipante.Filter<Participante>(x => x.CPF == ParticipanteModel.CPF
                                && x.DataNascimento == ParticipanteModel.DataNascimento).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "CPF e data de nascimento já cadastrado" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case EnumDomain.TipoValidacaoPositiva.CPFouCNPJ1Campo:
                            if (repParticipante.Filter<Participante>(x => x.CPF == ParticipanteModel.CPF
                                || x.CNPJ == ParticipanteModel.CNPJ).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "CPF/CNPJ já cadastrado" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case EnumDomain.TipoValidacaoPositiva.Email1Campo:
                            if (ParticipanteModel.Email != null && repParticipante.Filter<Participante>(x => x.Email == ParticipanteModel.Email).ToList().Count() > 0)
                            {
                                var data = new { ok = false, msg = "E-mail já cadastrado" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        default:
                            break;
                    }

                    if (repParticipante.Filter<Participante>(x => x.Login.ToLower() == ParticipanteModel.Login.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Participante já cadastrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Participante Participante = new Participante();
                        Participante.StatusId = Convert.ToInt32(EnumDomain.StatusParticipante.PreCadastrado);
                        //Participante.Codigo = ParticipanteModel.Codigo;
                        Participante.Login = ParticipanteModel.Login;
                        Participante.Senha = ParticipanteModel.Senha;
                        Participante.Nome = ParticipanteModel.Nome;
                        Participante.CPF = ParticipanteModel.CPF;
                        Participante.RazaoSocial = ParticipanteModel.RazaoSocial;
                        Participante.CNPJ = ParticipanteModel.CNPJ;
                        Participante.NomeFantasia = ParticipanteModel.NomeFantasia;
                        Participante.RG = ParticipanteModel.RG;
                        Participante.Sexo = ParticipanteModel.Sexo;
                        Participante.DataNascimento = ParticipanteModel.DataNascimento;
                        Participante.Endereco = ParticipanteModel.Endereco;
                        Participante.Numero = ParticipanteModel.Numero;
                        Participante.Complemento = ParticipanteModel.Complemento;
                        Participante.Bairro = ParticipanteModel.Bairro;
                        Participante.CEP = ParticipanteModel.CEP;
                        Participante.Cidade = ParticipanteModel.Cidade;
                        Participante.EstadoId = ParticipanteModel.EstadoId;
                        Participante.DDDCel = ParticipanteModel.DDDCel;
                        Participante.Celular = ParticipanteModel.Celular;
                        Participante.DDDTel = ParticipanteModel.DDDTel;
                        Participante.Telefone = ParticipanteModel.Telefone;
                        Participante.DDDTelComercial = ParticipanteModel.DDDTelComercial;
                        Participante.TelefoneComercial = ParticipanteModel.TelefoneComercial;
                        Participante.Ativo = true;
                        Participante.DataInclusao = DateTime.Now;
                        Participante.DataAlteracao = DateTime.Now;
                        Participante.OptInComunicacaoFisica = ParticipanteModel.OptInComunicacaoFisica;
                        Participante.OptInEmail = ParticipanteModel.OptInEmail;
                        Participante.OptInSms = ParticipanteModel.OptInSms;
                        Participante.ParticipanteTeste = ParticipanteModel.ParticipanteTeste;
                        Participante.Email = ParticipanteModel.Email;

                        List<ParticipantePerfil> listParticipantePerfil = new List<ParticipantePerfil>();
                        listParticipantePerfil.Add(new ParticipantePerfil
                        {
                            ParticipanteId = Participante.Id,
                            PerfilId = Convert.ToInt32(ParticipanteModel.PerfilId),
                            DataAlteracao = DateTime.Now,
                            DataInclusao = DateTime.Now,
                            Ativo = true
                        });
                        Participante.ParticipantePerfil = listParticipantePerfil;

                        List<ParticipanteEstrutura> listParticipanteEstrutura = new List<ParticipanteEstrutura>();
                        listParticipanteEstrutura.Add(new ParticipanteEstrutura
                        {
                            ParticipanteId = Participante.Id,
                            EstruturaId = Convert.ToInt32(ParticipanteModel.EstruturaId),
                            DataInclusao = DateTime.Now,
                            DataAlteracao = DateTime.Now,
                            Ativo = true
                        });
                        Participante.ParticipanteEstrutura = listParticipanteEstrutura;



                        using (TransactionScope scope = new TransactionScope())
                        {
                            repParticipante.Create(Participante);
                            repParticipante.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Dados salvos com sucesso." };
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
        public ActionResult CadastrarVago(ParticipanteModel ParticipanteModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repParticipante = context.CreateRepository<Participante>();

                    Participante Participante = new Participante();
                    Participante.StatusId = Convert.ToInt32(EnumDomain.StatusParticipante.PreCadastrado);
                    Participante.Nome = ParticipanteModel.Nome;
                    Participante.ParticipanteVago = true;
                    Participante.Ativo = true;
                    Participante.DataInclusao = DateTime.Now;
                    Participante.DataAlteracao = DateTime.Now;
                    Participante.OptInComunicacaoFisica = false;
                    Participante.OptInEmail = false;
                    Participante.OptInSms = false;
                    Participante.ParticipanteTeste = false;

                    List<ParticipantePerfil> listParticipantePerfil = new List<ParticipantePerfil>();
                    listParticipantePerfil.Add(new ParticipantePerfil
                    {
                        ParticipanteId = Participante.Id,
                        PerfilId = Convert.ToInt32(ParticipanteModel.PerfilId),
                        DataAlteracao = DateTime.Now,
                        DataInclusao = DateTime.Now,
                        Ativo = true
                    });
                    Participante.ParticipantePerfil = listParticipantePerfil;

                    List<ParticipanteEstrutura> listParticipanteEstrutura = new List<ParticipanteEstrutura>();
                    listParticipanteEstrutura.Add(new ParticipanteEstrutura
                    {
                        ParticipanteId = Participante.Id,
                        EstruturaId = Convert.ToInt32(ParticipanteModel.EstruturaId),
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        Ativo = true
                    });
                    Participante.ParticipanteEstrutura = listParticipanteEstrutura;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repParticipante.Create(Participante);
                        repParticipante.SaveChanges();
                        scope.Complete();
                    }

                    var data = new { ok = true, msg = "Dados salvos com sucesso." };
                    return Json(data, JsonRequestBehavior.AllowGet);

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
        public ActionResult Edit(ParticipanteModel ParticipanteModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repParticipante = context.CreateRepository<Participante>();
                    var Participante = repParticipante.Find<Participante>(ParticipanteModel.Id);

                    if (Participante != null)
                    {
                        if (repParticipante.Filter<Participante>(x => x.Login == ParticipanteModel.Login && x.Id != ParticipanteModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            var data = new { ok = false, msg = "Participante já cadastrado." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            //Participante.Codigo = ParticipanteModel.Codigo;
                            Participante.Login = ParticipanteModel.Login;
                            Participante.StatusId = ParticipanteModel.StatusId;

                            if (ParticipanteModel.Nome != null)
                            {
                                Participante.Nome = ParticipanteModel.Nome;
                                Participante.CPF = ParticipanteModel.CPF;
                            }

                            if (ParticipanteModel.RazaoSocial != null)
                            {
                                Participante.RazaoSocial = ParticipanteModel.RazaoSocial;
                                Participante.NomeFantasia = ParticipanteModel.NomeFantasia;
                                Participante.CNPJ = ParticipanteModel.CNPJ;
                            }

                            Participante.RG = ParticipanteModel.RG;
                            Participante.Sexo = ParticipanteModel.Sexo;
                            Participante.DataNascimento = ParticipanteModel.DataNascimento;
                            Participante.Endereco = ParticipanteModel.Endereco;
                            Participante.Numero = ParticipanteModel.Numero;
                            Participante.Complemento = ParticipanteModel.Complemento;
                            Participante.Bairro = ParticipanteModel.Bairro;
                            Participante.CEP = ParticipanteModel.CEP;
                            Participante.Cidade = ParticipanteModel.Cidade;
                            Participante.EstadoId = ParticipanteModel.EstadoId;
                            Participante.DDDCel = ParticipanteModel.DDDCel;
                            Participante.Celular = ParticipanteModel.Celular;
                            Participante.DDDTel = ParticipanteModel.DDDTel;
                            Participante.Telefone = ParticipanteModel.Telefone;
                            Participante.DDDTelComercial = ParticipanteModel.DDDTelComercial;
                            Participante.TelefoneComercial = ParticipanteModel.TelefoneComercial;
                            Participante.OptInComunicacaoFisica = ParticipanteModel.OptInComunicacaoFisica;
                            Participante.OptInEmail = ParticipanteModel.OptInEmail;
                            Participante.OptInSms = ParticipanteModel.OptInSms;
                            Participante.ParticipanteTeste = ParticipanteModel.ParticipanteTeste;
                            Participante.Email = ParticipanteModel.Email;


                            //Desligamento
                            if (Convert.ToInt32(EnumDomain.StatusParticipante.Desligado) == ParticipanteModel.StatusId)
                            {
                                Participante.DataDesligamento = DateTime.Now;
                            }
                            else if (Convert.ToInt32(EnumDomain.StatusParticipante.Inativo) == ParticipanteModel.StatusId)
                            {
                                //Inativa a última hierarquia Cadastrada para o participante
                                //TODO: ALTERAR PARA O NOVO MODELO DE HIERARQUIA
                                //var listParticipanteHierarquia = Participante.hie.Where(x => x.Ativo == true).OrderByDescending(x => x.Id).ToList();
                                //foreach (var item in listParticipanteHierarquia)
                                //{
                                //    item.Ativo = false;
                                //    item.DataAlteracao = DateTime.Now;
                                //}
                            }
                            else
                            {
                                Participante.DataDesligamento = null;
                            }
                            Participante.DataAlteracao = DateTime.Now;

                            //Perfil
                            IRepository repParticipantePerfil = context.CreateRepository<ParticipantePerfil>();
                            var listParticipantePerfil = repParticipantePerfil.Filter<ParticipantePerfil>(x => x.ParticipanteId == ParticipanteModel.Id && x.Ativo == true).ToList();
                            foreach (var item in listParticipantePerfil)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }
                            listParticipantePerfil.Add(new ParticipantePerfil
                            {
                                Ativo = true,
                                DataAlteracao = DateTime.Now,
                                DataInclusao = DateTime.Now,
                                ParticipanteId = ParticipanteModel.Id,
                                PerfilId = Convert.ToInt32(ParticipanteModel.PerfilId)
                            });
                            Participante.ParticipantePerfil = listParticipantePerfil;

                            //Estrutura
                            IRepository repParticipanteEstrutura = context.CreateRepository<ParticipanteEstrutura>();
                            var listParticipanteEstrutura = repParticipanteEstrutura.Filter<ParticipanteEstrutura>(x => x.ParticipanteId == ParticipanteModel.Id && x.Ativo == true).ToList();
                            foreach (var item in listParticipanteEstrutura)
                            {
                                item.Ativo = false;
                                item.DataAlteracao = DateTime.Now;
                            }
                            listParticipanteEstrutura.Add(new ParticipanteEstrutura
                            {
                                Ativo = true,
                                DataAlteracao = DateTime.Now,
                                DataInclusao = DateTime.Now,
                                ParticipanteId = ParticipanteModel.Id,
                                EstruturaId = Convert.ToInt32(ParticipanteModel.EstruturaId)
                            });
                            Participante.ParticipanteEstrutura = listParticipanteEstrutura;

                            //Grava
                            using (TransactionScope scope = new TransactionScope())
                            {
                                repParticipante.Update(Participante);
                                repParticipante.SaveChanges();
                                scope.Complete();
                            }

                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Participante não encontrado." };
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
                    IRepository repParticipante = context.CreateRepository<Participante>();

                    var Participante = repParticipante.Find<Participante>(Id);

                    if (Participante != null)
                    {
                        Participante.DataAlteracao = DateTime.Now;
                        Participante.Ativo = false;
                        Participante.StatusId = Convert.ToInt32(EnumDomain.StatusParticipante.Inativo);

                        //Inativa a hierarquia
                        //TODO: ALTERAR PARA O NOVO MODELO DE HIERARQUIA
                        //var listParticipanteHierarquia = Participante.ParticipanteHierarquia.Where(x => x.Ativo == true).ToList();
                        //foreach (var item in listParticipanteHierarquia)
                        //{
                        //    item.Ativo = false;
                        //    item.DataAlteracao = DateTime.Now;
                        //}
                        //Participante.ParticipanteHierarquia = listParticipanteHierarquia;

                        //Inativa a perfil
                        //var listParticipantePerfil = Participante.ParticipantePerfil.Where(x => x.Ativo == true).ToList();
                        //foreach (var item in listParticipantePerfil)
                        //{
                        //    item.Ativo = false;
                        //    item.DataAlteracao = DateTime.Now;
                        //}
                        //Participante.ParticipantePerfil = listParticipantePerfil;

                        //Inativa a Estrutura
                        //var listParticipanteEstrutura = Participante.ParticipanteEstrutura.Where(x => x.Ativo == true).ToList();
                        //foreach (var item in listParticipanteEstrutura)
                        //{
                        //    item.Ativo = false;
                        //    item.DataAlteracao = DateTime.Now;
                        //}
                        //Participante.ParticipanteEstrutura = listParticipanteEstrutura;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repParticipante.Update(Participante);
                            repParticipante.SaveChanges();
                            scope.Complete();
                        }

                        var data = new { ok = true, msg = "Participante inativado com sucesso." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, Participante não encontrado." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar o Participante." + exc.Message };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Lote(HttpPostedFileBase FileArquivo)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var data = new object();

                    IRepository repEstrutura = context.CreateRepository<Estrutura>();

                    if (FileArquivo != null)
                    {
                        //Faz o upload

                        var uploadFileResult = UploadFile.Upload(
                            FileArquivo,
                            Settings.Extensoes.ExtensoesPermitidasArquivos,
                            int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                            "participante/");

                        //Não salvou, erro
                        if (!uploadFileResult.arquivoSalvo)
                        {
                            data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Carrega importação
                            string msg = "";
                            string urlDownloadArquivoErro = "";

                            //Carrega importacao, importa arquivo, processa e retorna arquivo de erro
                            if (!CarregaParticipanteImportacao(
                                "participante/" + uploadFileResult.nomeArquivoGerado,
                                FileArquivo.FileName,
                                uploadFileResult.nomeArquivoGerado,
                                out msg, out urlDownloadArquivoErro))
                            {
                                data = new { ok = false, msg = msg, arquivoErro = urlDownloadArquivoErro };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            else //Sucesso
                            {
                                data = new { ok = true, msg = "Arquivo gravado com sucesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        data = new { ok = false, msg = "Por favor, selecione o campo Arquivo." };
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

        public ActionResult PartialParticipanteConsulta(int? EstruturaId, int? PerfilId, int? StatusId)
        {
            return PartialView("PartialParticipanteConsulta", CarregaParticipante(null, EstruturaId, PerfilId, StatusId));
        }


        [HttpGet]
        public JsonResult ListarParticipanteConsulta(int? EstruturaId, int? PerfilId, int? StatusId, string CPF, string Login, string CNPJ, string nome, TableParameter param)
        {
            int total = 0;

            var list = CarregaParticipanteResumo(null, EstruturaId, PerfilId, StatusId, param.iDisplayLength, param.iDisplayStart, param.iSortCol_0, param.sSortDir_0, CPF, Login, CNPJ, nome, out total);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                aaData = list
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Internal Functions"

        internal List<ParticipanteModel> CarregaParticipante(int? Id, int? EstruturaId, int? PerfilId, int? StatusId)
        {
            try
            {
                List<ParticipanteModel> listParticipante = new List<ParticipanteModel>();

                //Consulta de detalhe
                if (Id > 0)
                {
                    int total = 0;
                    var participantes = ParticipanteService.ListaParticipante(Id, EstruturaId, PerfilId, StatusId, 0, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, out total);

                    foreach (DataRow row in participantes.Rows)
                    {
                        listParticipante.Add(new ParticipanteModel
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            PerfilId = row.Field<int?>("PerfilId"),
                            Perfil = row["Perfil"].ToString(),
                            EstruturaId = row.Field<int?>("EstruturaId"),
                            Estrutura = row["Estrutura"].ToString(),
                            Login = row["Login"].ToString(),
                            Codigo = row["Codigo"].ToString(),
                            Senha = row["Senha"].ToString(),
                            StatusId = Convert.ToInt32(row["StatusId"]),
                            Status = row["StatusParticipante"].ToString(),
                            Nome = row["Nome"].ToString(),
                            RazaoSocial = row["RazaoSocial"].ToString(),
                            NomeFantasia = row["NomeFantasia"].ToString(),
                            CNPJ = row["CNPJ"].ToString(),
                            CPF = row["CPF"].ToString(),
                            RG = row["RG"].ToString(),
                            Sexo = row["Sexo"].ToString(),
                            DataNascimento = row.Field<DateTime?>("DataNascimento"),
                            Endereco = row["Endereco"].ToString(),
                            Numero = row["Numero"].ToString(),
                            Complemento = row["Complemento"].ToString(),
                            Bairro = row["Bairro"].ToString(),
                            CEP = row["CEP"].ToString(),
                            Cidade = row["Cidade"].ToString(),
                            EstadoId = row.Field<int?>("EstadoId"),
                            Estado = row["Estado"].ToString(),
                            DDDCel = row["DDDCel"].ToString(),
                            Celular = row["Celular"].ToString(),
                            DDDTel = row["DDDTel"].ToString(),
                            Telefone = row["Telefone"].ToString(),
                            DDDTelComercial = row["DDDTelComercial"].ToString(),
                            TelefoneComercial = row["TelefoneComercial"].ToString(),
                            Ativo = Convert.ToBoolean(row["Ativo"]),
                            ParticipanteTeste = row.Field<Boolean?>("ParticipanteTeste"),
                            DataCadastro = row.Field<DateTime?>("DataCadastro"),
                            DataDesligamento = row.Field<DateTime?>("DataDesligamento"),
                            DataInclusao = Convert.ToDateTime(row["DataInclusao"]),
                            DataAlteracao = row.Field<DateTime?>("DataAlteracao"),
                            OptInComunicacaoFisica = row.Field<Boolean?>("OptInComunicacaoFisica"),
                            OptInEmail = row.Field<Boolean?>("OptInEmail"),
                            OptInSms = row.Field<Boolean?>("OptInSms"),
                            Email = row["Email"].ToString()
                        });
                    }
                }
                else
                {
                    int total = 0;
                    var participantes = ParticipanteService.ListaParticipante(Id,
                                                                                EstruturaId,
                                                                                PerfilId,
                                                                                StatusId,
                                                                                0,
                                                                                0,
                                                                                0,
                                                                                string.Empty,
                                                                                string.Empty,
                                                                                string.Empty,
                                                                                string.Empty,
                                                                                string.Empty,
                                                                                out total);

                    foreach (DataRow row in participantes.Rows)
                    {
                        listParticipante.Add(new ParticipanteModel
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Perfil = row["Perfil"].ToString(),
                            Estrutura = row["Estrutura"].ToString(),
                            Login = row["Login"].ToString(),
                            Status = row["StatusParticipante"].ToString(),
                            Nome = row["Nome"].ToString(),
                            RazaoSocial = row["RazaoSocial"].ToString(),
                            CNPJ = row["CNPJ"].ToString(),
                            CPF = row["CPF"].ToString(),
                            DataNascimento = row.Field<DateTime?>("DataNascimento"),
                            DataCadastro = row.Field<DateTime?>("DataCadastro"),
                        });
                    }
                }

                return listParticipante;
            }
            catch (Exception ex)
            {
                string teste = ex.Message;
                throw;
            }

        }

        internal List<ParticipanteResumoModel> CarregaParticipanteResumo(int? Id, int? EstruturaId, int? PerfilId, int? StatusId, int Quantidade, int Inicio, int sortColumn, string sortDirection, string CPF, string Login, string CNPJ, string nome, out int TotalCount)
        {
            try
            {
                List<ParticipanteResumoModel> listParticipante = new List<ParticipanteResumoModel>();
                int total = 0;

                var participante = ParticipanteService.ListaParticipante(Id, EstruturaId, PerfilId, StatusId, Inicio, Quantidade, sortColumn, sortDirection, CPF, Login, CNPJ, nome, out total);

                TotalCount = total;

                foreach (DataRow row in participante.Rows)
                {
                    listParticipante.Add(new ParticipanteResumoModel
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Perfil = row["Perfil"].ToString(),
                        Estrutura = row["Estrutura"].ToString(),
                        Login = row["Login"].ToString(),
                        Status = row["StatusParticipante"].ToString(),
                        Nome = row["Nome"].ToString(),
                        RazaoSocial = row["RazaoSocial"].ToString(),
                        CNPJ = row["CNPJ"].ToString(),
                        CPF = row["CPF"].ToString(),
                        DataNascimento = row.Field<DateTime?>("DataNascimento") == null ? "" : Convert.ToDateTime(row.Field<DateTime?>("DataNascimento")).ToString("dd/MM/yyyy"),
                        DataCadastro = row.Field<DateTime?>("DataCadastro") == null ? "" : Convert.ToDateTime(row.Field<DateTime?>("DataCadastro")).ToString("dd/MM/yyyy"),
                    });
                }


                return listParticipante;
            }
            catch (Exception ex)
            {
                string teste = ex.Message;
                throw;
            }

        }

        internal bool CarregaParticipanteImportacao(string filePath, string nomeArquivo, string nomeArquivoGerado, out string msg, out string urlDownloadArquivoErro)
        {
            //Grava o arquivo no banco de dados
            var arquivo = ArquivoService.CadastrarArquivo(filePath, nomeArquivo, nomeArquivoGerado, EnumDomain.TipoArquivo.ParticipanteLote);
            bool blnSucesso = false;
            msg = "";
            urlDownloadArquivoErro = "";

            try
            {
                //Arquivo para dataset
                var dsArquivo = ExcelDataReader.OpenExcel(filePath);

                //Verifica se existem linhas
                if (dsArquivo.Tables.Count == 0)
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no arquivo. Não existem linhas para serem importadas.";
                    return blnSucesso;
                }

                //Valida Qtde de Colunas (12)
                int countColumns = 12;
                if (dsArquivo.Tables[0].Columns.Count != countColumns)
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no layout do arquivo. Número de campos não confere.";
                    return blnSucesso;
                }

                //Valida a existência dos Campos
                string[] arrLayoutArquivo = new string[countColumns];
                arrLayoutArquivo.SetValue("Estrutura", 0);
                arrLayoutArquivo.SetValue("Perfil", 1);
                arrLayoutArquivo.SetValue("Codigo", 2);
                arrLayoutArquivo.SetValue("Login", 3);
                arrLayoutArquivo.SetValue("Nome", 4);
                arrLayoutArquivo.SetValue("CPF", 5);
                arrLayoutArquivo.SetValue("RazaoSocial", 6);
                arrLayoutArquivo.SetValue("CNPJ", 7);
                arrLayoutArquivo.SetValue("DataNascimento", 8);
                arrLayoutArquivo.SetValue("Email", 9);
                arrLayoutArquivo.SetValue("DDDCel", 10);
                arrLayoutArquivo.SetValue("Celular", 11);

                foreach (var item in arrLayoutArquivo)
                {
                    if (!dsArquivo.Tables[0].Columns.Contains(item))
                    {
                        //Atualiza o status
                        ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                        blnSucesso = false;
                        msg = "Erro no layout do arquivo. Nomes dos campos não conferem.";
                        return blnSucesso;
                    }
                }

                //Importa para o banco de dados (bulk)
                if (!ParticipanteService.ImportarArquivoParticipante(dsArquivo.Tables[0], arquivo.Id))
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no layout do arquivo. Dados inválidos.";
                    return blnSucesso;
                }


                //Processo registros no banco (Procedure)
                int countErro = 0;
                if (!ParticipanteService.ProcessaParticipanteArquivo(arquivo.Id, out countErro))
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroDadosInvalidos);
                    string strErro = "Erro no processamento do arquivo. Dados inválidos.";

                    //Erros > 0, exporta arquivo
                    if (countErro > 0)
                    {
                        //Retorno o endereço para download do arquivo de retorno
                        urlDownloadArquivoErro = ParticipanteService.ExportaArquivoParticipanteErro(arquivo.Id);
                    }

                    blnSucesso = false;
                    msg = strErro;
                    return blnSucesso;
                }

                //Ok, Atualiza o status processado
                ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.Processado);

                //Retorna sucesso
                blnSucesso = true;
                msg = "Arquivo importado com sucesso!";
                return blnSucesso;
            }
            catch (Exception)
            {
                ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);
                throw;
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string codigo, string sessao)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "ParticipanteController",
                Pagina = string.Empty,
                Codigo = codigo,
                UsuarioSessao = sessao
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
        #endregion

    }
    public class TableParameter
    {
        public string sEcho { get; set; }
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
    }
    /// <summary>
    /// Represents the required data for a response from a request by DataTables.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JQueryDataTablesResponse<T>
    {
        public JQueryDataTablesResponse(IEnumerable<T> items,
            int totalRecords,
            int totalDisplayRecords,
            int sEcho)
        {
            aaData = items;
            iTotalRecords = totalRecords;
            iTotalDisplayRecords = totalDisplayRecords;
            this.sEcho = sEcho;
        }

        /// <summary>
        /// Sets the Total records, before filtering (i.e. the total number of records in the database)
        /// </summary>
        public int iTotalRecords { get; private set; }

        /// <summary>
        /// Sets the Total records, after filtering 
        /// (i.e. the total number of records after filtering has been applied - 
        /// not just the number of records being returned in this result set)
        /// </summary>
        public int iTotalDisplayRecords { get; private set; }

        /// <summary>
        /// Sets an unaltered copy of sEcho sent from the client side. This parameter will change with each 
        /// draw (it is basically a draw count) - so it is important that this is implemented. 
        /// Note that it strongly recommended for security reasons that you 'cast' this parameter to an 
        /// integer in order to prevent Cross Site Scripting (XSS) attacks.
        /// </summary>
        public int sEcho { get; private set; }

        /// <summary>
        /// Sets the data in a 2D array (Array of JSON objects). Note that you can change the name of this 
        /// parameter with sAjaxDataProp.
        /// </summary>
        public IEnumerable<T> aaData { get; private set; }
    }
}
