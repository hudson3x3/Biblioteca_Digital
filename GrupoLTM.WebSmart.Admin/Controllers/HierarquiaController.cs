using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using System.Data;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class HierarquiaController : BaseController
    {
        private IUnitOfWork _context;

        public HierarquiaController()
        {
            _context = UnitOfWorkFactory.Create();
        }

        #region "Actions"

        public ActionResult Lote()
        {
            return View(new HierarquiaModel());
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult Individual(int ID)
        {
            ParticipanteService participanteService = new ParticipanteService();
            ViewData["ID"] = ID.ToString();

            var participante = participanteService.ObterParticipante(ID);

            return View(new ParticipanteModel 
            { 
                Nome = participante.Nome,
                Id = participante.Id
            });
        }


        #endregion

        #region "Actions Posts"

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult Lote(HttpPostedFileBase FileArquivo, int PeriodoId)
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
                            "hierarquia/");

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
                            if (!CarregaHierarquia(
                                "hierarquia/" + uploadFileResult.nomeArquivoGerado,
                                FileArquivo.FileName,
                                uploadFileResult.nomeArquivoGerado,
                               out msg, 
                                out urlDownloadArquivoErro,
                                PeriodoId))
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

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult ObterHierarquiaPorNivel(string pID, string pPeriodoID, string pNivel, string pIdParticipantePai)
        {
            int ID, PeriodoID, Nivel, idParticipantePai;
            var data = new object();

            if (!Int32.TryParse(pID, out ID))
            {
                data = new { ok = false, msg = "Parâmetro ID inválido." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            if (!Int32.TryParse(pPeriodoID, out PeriodoID))
            {
                data = new { ok = false, msg = "Parâmetro Período inválido." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            if (!Int32.TryParse(pNivel, out Nivel))
            {
                data = new { ok = false, msg = "Parâmetro Nivel inválido." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            if (!Int32.TryParse(pIdParticipantePai, out idParticipantePai))
            {
                data = new { ok = false, msg = "Parâmetro filtro do nível inválido." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            var dsHierarquia = HierarquiaService.ObterHierarquiaPorNivelParticipante(Nivel,PeriodoID,ID);

            var listHierarquia = new List<HierarquiaIndividualModel>();

            foreach (DataRow row in dsHierarquia.Rows)
            {

                var model = new HierarquiaIndividualModel
                {
                    Login = row["Login"].ToString(),
                    NivelHierarquia = Convert.ToInt32(row["NivelHierarquia"]),
                    Nome = row["Nome"].ToString(),
                    ParticipanteId = Convert.ToInt32(row["ParticipanteId"]),
                    PerfilID = Convert.ToInt32(row["PerfilID"]),
                    PerfilPaiID = row.Field<int?>("PerfilPaiID"),
                    EstaNaHierarquia = Convert.ToBoolean(row["EstaNaHierarquia"]),
                    PeriodoId = Convert.ToInt32(row["PeriodoId"]),
                    Perfil = row["PerfilNome"].ToString()
                };

                listHierarquia.Add(model);
            };

            data = new { ok = true, msg = listHierarquia };
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        public ActionResult ObterHierarquia(string pID, string pPeriodoID)
        {
            int ID,PeriodoID;
            var data = new object();

            if (!Int32.TryParse(pID, out ID))
            {
                data = new { ok = false, msg = "Parâmetro ID inválido." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            if (!Int32.TryParse(pPeriodoID, out PeriodoID))
            {
                data = new { ok = false, msg = "Parâmetro Período inválido." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            
            var dsHierarquia = HierarquiaService.ObterNivelHierarquia(ID,PeriodoID);

            if (dsHierarquia.Rows.Count > 0)
            {

                var nivelHierarquia = dsHierarquia.Rows[0]["NivelHierarquia"].ToString();
                var possuiHierarquia = dsHierarquia.Rows[0]["PossuiHierarquia"].ToString();

                if (!string.IsNullOrWhiteSpace(possuiHierarquia))
                {
                    data = new { ok = true, msg = new { NivelHierarquia = nivelHierarquia, PossuiHierarquia = bool.Parse(possuiHierarquia) } };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data = new { ok = false, msg = "Participante sem nível/Perfil associado." };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            return View();
        }

        [CustomAuthorizeAttribute(EnumDomain.Perfis.Administrador)]
        [HttpPost]
        public ActionResult SalvarHierarquia(string pID,string pPeriodoID,string nivel1,string nivel2,string nivel3,string nivel4,string nivel5,string nivel6,string nivel7,string nivel8,string nivel9,string nivel10)
        {
            var data = new object();

            var sucesso = HierarquiaService.SalvarHierarquiaIndividual
                (
                    Convert.ToInt32(pID),
                    Convert.ToInt32(pPeriodoID),
                    Convert.ToInt32(nivel1),
                    Convert.ToInt32(nivel2),
                    Convert.ToInt32(nivel3),
                    Convert.ToInt32(nivel4),
                    Convert.ToInt32(nivel5),
                    Convert.ToInt32(nivel6),
                    Convert.ToInt32(nivel7),
                    Convert.ToInt32(nivel8),
                    Convert.ToInt32(nivel9),
                    Convert.ToInt32(nivel10)
                );

            data = new { ok = sucesso, msg = sucesso ? "Dados salvos com sucesso." : "Erro ao salvar a hierarquia, tente novamente mais tarde" };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Internal Functions"

        internal bool CarregaHierarquia(string filePath, string nomeArquivo, string nomeArquivoGerado, out string msg, out string urlDownloadArquivoErro, int PeriodoId)
        {
            //Grava o arquivo no banco de dados
            var arquivo = ArquivoService.CadastrarArquivo(filePath, nomeArquivo, nomeArquivoGerado, EnumDomain.TipoArquivo.Hierarquia);
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

                //Valida Qtde de Colunas (10)
                int countColumns = 10;
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
                arrLayoutArquivo.SetValue("LoginNivel1", 0);
                arrLayoutArquivo.SetValue("LoginNivel2", 1);
                arrLayoutArquivo.SetValue("LoginNivel3", 2);
                arrLayoutArquivo.SetValue("LoginNivel4", 3);
                arrLayoutArquivo.SetValue("LoginNivel5", 4);
                arrLayoutArquivo.SetValue("LoginNivel6", 5);
                arrLayoutArquivo.SetValue("LoginNivel7", 6);
                arrLayoutArquivo.SetValue("LoginNivel8", 7);
                arrLayoutArquivo.SetValue("LoginNivel9", 8);
                arrLayoutArquivo.SetValue("LoginNivel10", 9);

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
                if (!ParticipanteService.ImportarArquivoParticipanteHierarquia(dsArquivo.Tables[0], arquivo.Id, PeriodoId))
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroNaoImportado);

                    blnSucesso = false;
                    msg = "Erro no layout do arquivo. Dados inválidos.";
                    return blnSucesso;
                }


                //Processo registros no banco (Procedure)
                int countErro = 0;
                if (!ParticipanteService.ProcessaParticipanteHierarquiaArquivo(arquivo.Id, PeriodoId, out countErro))
                {
                    //Atualiza o status
                    ArquivoService.AtualizaArquivo(arquivo.Id, EnumDomain.StatusArquivo.ErroDadosInvalidos);
                    string strErro = "Erro no processamento do arquivo. Dados inválidos.";

                    //Erros > 0, exporta arquivo
                    if (countErro > 0)
                    {
                        //Retorno o endereço para download do arquivo de retorno
                        urlDownloadArquivoErro = ParticipanteService.ExportaArquivoParticipanteHierarquiaErro(arquivo.Id);
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

        #endregion

    }
}
