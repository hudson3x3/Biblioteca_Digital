using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using System.Transactions;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Services;
using System.Data;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class EstruturaController : BaseController
    {

        #region "Services"

        private readonly EstruturaService _EstruturaService;

        #endregion

        #region "Actions"

        public ActionResult Index()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutura = context.CreateRepository<Estrutura>();
                List<EstruturaModel> listEstruturaModel = new List<EstruturaModel>();
                foreach (var item in repEstrutura.Filter<Estrutura>(x => x.Ativo == true).OrderBy(x => x.Nome).ToList())
                {
                    listEstruturaModel.Add(new EstruturaModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Tipo = item.TipoEstrutura.Nome,
                        PaiId = item.PaiId,
                        NomePai = (item.PaiId.HasValue ? item.Estrutura2.Nome : "")
                    });
                }
                return View(listEstruturaModel);
            }
        }

        public ActionResult Create()
        {
            return View(new EstruturaModel());
        }

        public ActionResult Edit(int Id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutura = context.CreateRepository<Estrutura>();
                var Estrutura = repEstrutura.Find<Estrutura>(Id);

                EstruturaModel estruturaModel = new EstruturaModel();

                if (Estrutura != null)
                {
                    estruturaModel.Ativo = Estrutura.Ativo;
                    estruturaModel.Id = Estrutura.Id;
                    estruturaModel.Nome = Estrutura.Nome;
                    estruturaModel.Tipo = Estrutura.Tipo;
                    estruturaModel.DataAlteracao = Estrutura.DataAlteracao;
                    estruturaModel.DataInclusao = Estrutura.DataInclusao;
                    estruturaModel.PaiId = Estrutura.PaiId;
                    estruturaModel.TipoEstruturaId = Estrutura.TipoEstruturaId;
                }

                return View(estruturaModel);
            }
        }

        public ActionResult Lote()
        {
            return View(new EstruturaModel());
        }

        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Create(EstruturaModel estruturaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repEstrutura = context.CreateRepository<Estrutura>();

                    if (repEstrutura.Filter<Estrutura>(x => x.Nome.ToLower() == estruturaModel.Nome.ToLower() && x.Ativo == true).ToList().Count() > 0)
                    {
                        var data = new { ok = false, msg = "Estrutura já cadastrada." };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Estrutura Estrutura = new Estrutura();
                        Estrutura.Nome = estruturaModel.Nome;
                        Estrutura.Tipo = "";
                        Estrutura.TipoEstruturaId = estruturaModel.TipoEstruturaId;
                        Estrutura.Ativo = true;
                        Estrutura.DataAlteracao = DateTime.Now;
                        Estrutura.DataInclusao = DateTime.Now;
                        Estrutura.PaiId = estruturaModel.PaiId;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            repEstrutura.Create(Estrutura);
                            repEstrutura.SaveChanges();
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
        public ActionResult Edit(EstruturaModel estruturaModel)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repEstrutura = context.CreateRepository<Estrutura>();
                    var Estrutura = repEstrutura.Find<Estrutura>(estruturaModel.Id);

                    if (Estrutura != null)
                    {
                        if (repEstrutura.Filter<Estrutura>(x => x.Nome == estruturaModel.Nome && x.Id != estruturaModel.Id && x.Ativo == true).ToList().Count() > 0)
                        {
                            var data = new { ok = false, msg = "Estrutura já cadastrada." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Preenche o objeto
                            Estrutura.Nome = estruturaModel.Nome;
                            Estrutura.Tipo = "";
                            Estrutura.TipoEstruturaId = estruturaModel.TipoEstruturaId;
                            Estrutura.DataAlteracao = DateTime.Now;
                            Estrutura.PaiId = estruturaModel.PaiId;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                repEstrutura.Update(Estrutura);
                                repEstrutura.SaveChanges();
                                scope.Complete();
                            }

                            var data = new { ok = true, msg = "Dados salvos com sucesso." };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = new { ok = false, msg = "Não foi possível salvar os dados, estrutura não encontrado." };
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
                var bln = _EstruturaService.InativarEstrutura(Id);

                if (bln)
                {
                    var data = new { ok = true, msg = "Estrutura inativada com sucesso." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = new { ok = false, msg = "Não foi possível salvar os dados, estrutura não encontrada." };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exc)
            {
                var data = new { ok = false, msg = "Não foi possível inativar a estrutura." + exc.Message };
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
                        
                        var uploadFileResult = UploadFile.Upload(
                            FileArquivo,
                            Settings.Extensoes.ExtensoesPermitidasArquivos,
                            int.Parse(Settings.TamanhoArquivos.TamanhoMaximoKBImagem),
                            "estrutura/");

                        if (!uploadFileResult.arquivoSalvo)
                        {
                            data = new { ok = false, msg = "Não foi possível gravar o Arquivo. " + uploadFileResult.mensagem };
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            string mensagem = "";

                            //Carrega a estrutura
                            var bln = CarregaEstrutura(
                                "estrutura/" + uploadFileResult.nomeArquivoGerado, 
                                FileArquivo.FileName,
                                uploadFileResult.nomeArquivoGerado,
                                out mensagem);
                            
                            if (bln)
                            {
                                data = new { ok = true, msg = "Arquivo gravado com sucesso." };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                data = new { ok = false, msg = mensagem };
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

        #endregion

        #region "Internal Functions"

        internal bool CarregaEstrutura(string filePath, string nomeArquivo, string nomeArquivoGerado, out string mensagem)
        {
            //Grava o arquivo
            var bln = true;
            mensagem = "";
            var arquivo = ArquivoService.CadastrarArquivo(filePath, nomeArquivo, nomeArquivoGerado, EnumDomain.TipoArquivo.Estrurura);

            try
            {
                var dsArquivo = ExcelDataReader.OpenExcel(filePath);

                List<Estrutura> listEstrutura = new List<Estrutura>();

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repEstrutura = context.CreateRepository<Estrutura>();

                    using (TransactionScope scope = new TransactionScope())
                    {

                        //Valida o Tipo Estrutura, Header do Excel
                        if (!VerificaTipoEstrutura(dsArquivo.Tables[0].Columns))
                        {
                            ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);
                            mensagem = "Não foi possível gravar o Arquivo. Layout do arquivo não confere com os tipos de estrutura cadastradas.";
                            scope.Complete();
                            return false;
                        }
                        
                        //Antes de importar o arquivo, inativa todas as estruturas atuais
                        InativaEstrurura();

                        //For por Coluna do Excel
                        for (int i = 0; i < dsArquivo.Tables[0].Columns.Count; i++)
                        {

                            //O Nome da Coluna é TipoEstrutura
                            string TipoEstrutura = dsArquivo.Tables[0].Columns[i].ColumnName;

                            //DataTable com a primeira planilha
                            DataTable dtEstruturaRows = dsArquivo.Tables[0];

                            //Group by por coluna
                            var listEstruturaGroup = from r in dtEstruturaRows.AsEnumerable()
                                                     group r by r.Field<string>(i) into g
                                                     select new
                                                     {
                                                         Estrurura = g.Key,
                                                         TipoEstrurura = TipoEstrutura
                                                     };

                            //A primeira é o PAI da hierarquia, portando não possuí pai
                            if (i == 0)
                            {
                                foreach (var item in listEstruturaGroup)
                                {
                                    //Verifica o registro no excel não é nulo
                                    if (item.Estrurura != null)
                                    {

                                        //Busca no banco e verifica se a estrutura já existe
                                        var estrutura = repEstrutura.Filter<Estrutura>(x => x.Nome == item.Estrurura).FirstOrDefault();
                                        //Se sim, grava
                                        if (estrutura != null)
                                        {
                                            estrutura.Nome = item.Estrurura;
                                            estrutura.Tipo = TipoEstrutura;
                                            estrutura.DataAlteracao = DateTime.Now;
                                            estrutura.Ativo = true;
                                            estrutura.PaiId = null;

                                            repEstrutura.Update(estrutura);
                                            repEstrutura.SaveChanges();
                                        }
                                        else
                                        {
                                            //Se não, cria
                                            repEstrutura.Create(new Estrutura
                                            {
                                                Nome = item.Estrurura,
                                                Tipo = item.TipoEstrurura,
                                                DataInclusao = DateTime.Now,
                                                DataAlteracao = DateTime.Now,
                                                Ativo = true
                                            });
                                        }
                                    }

                                }
                            }
                            else
                            {
                                //A partir da segunda planilha, buscamos pelo pai
                                foreach (var item in listEstruturaGroup)
                                {

                                    //Verifica o registro no excel não é nulo
                                    if (item.Estrurura != null)
                                    {
                                        //Pega a estrutura pai, no excel (coluna anterior
                                        var estruturaPai = from r in dtEstruturaRows.AsEnumerable()
                                                           where r.Field<string>(i) == item.Estrurura
                                                           select new
                                                           {
                                                               estruturaPai = r.Field<string>(i - 1)
                                                           };

                                        int? PaiId = null;
                                        string estruturaPaiRow = estruturaPai.FirstOrDefault().estruturaPai;
                                        //Pega o Pai
                                        var EstrururaPai = repEstrutura.Filter<Estrutura>(x => x.Nome == estruturaPaiRow).FirstOrDefault();
                                        if (EstrururaPai != null)
                                        {
                                            PaiId = EstrururaPai.Id;
                                        }

                                        //Busca no banco e verifica se a estrutura já existe
                                        var estrutura = repEstrutura.Filter<Estrutura>(x => x.Nome == item.Estrurura).FirstOrDefault();

                                        //Se sim, grava
                                        if (estrutura != null)
                                        {
                                            estrutura.Nome = item.Estrurura;
                                            estrutura.Tipo = TipoEstrutura;
                                            estrutura.DataAlteracao = DateTime.Now;
                                            estrutura.Ativo = true;
                                            estrutura.PaiId = PaiId;
                                            repEstrutura.Update(estrutura);
                                            repEstrutura.SaveChanges();
                                        }
                                        else
                                        {
                                            //Se não, cria
                                            repEstrutura.Create(new Estrutura
                                            {
                                                Nome = item.Estrurura,
                                                Tipo = item.TipoEstrurura,
                                                DataInclusao = DateTime.Now,
                                                DataAlteracao = DateTime.Now,
                                                Ativo = true,
                                                PaiId = PaiId
                                            });
                                        }
                                    }
                                }
                            }

                            //Salva no banco de dados
                            repEstrutura.SaveChanges();
                        }

                        //Salvar
                        scope.Complete();
                    }
                }
                ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.Processado);
                mensagem = "Arquivo gravado com sucesso.";
                return bln;
            }
            catch (Exception exc)
            {
                ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);
                mensagem = "Não foi possível gravar o arquivo.<br>" + exc.Message;
                return false;
            }           
        }

        internal void InativaEstrurura()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repEstrutura = context.CreateRepository<Estrutura>();

                foreach (var item in repEstrutura.Filter<Estrutura>(x => x.Ativo == true).ToList())
                {
                    item.Ativo = false;
                    item.DataAlteracao = DateTime.Now;
                    repEstrutura.Update(item);
                    repEstrutura.SaveChanges();
                }
            }
        }

        internal bool VerificaTipoEstrutura(DataColumnCollection Colunas)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                bool bln = true;
                IRepository repTipoEstrutura = context.CreateRepository<TipoEstrutura>();
                var listTipoEstrutura = repTipoEstrutura.Filter<TipoEstrutura>(x => x.Ativo == true).ToList();
                for (int i = 0; i < Colunas.Count; i++)
                {
                    if (listTipoEstrutura.Where(x => x.Nome == Colunas[i].ColumnName).Count() == 0)
                    {
                        bln = false;
                        break;
                    }
                }
                return bln;
            }
        }

        #endregion

    }
}
