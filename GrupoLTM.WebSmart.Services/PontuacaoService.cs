using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Services.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GrupoLTM.WebSmart.Services
{
    public class PontuacaoService
    {
        public static bool AprovarPontuacao(string pontuacao, int usuarioAdmId)
        {
            try
            {
                if (!String.IsNullOrEmpty(pontuacao))
                {
                    DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
                    string proc = "JP_UPD_AprovacaoPontuacao";

                    var conexao = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;

                    List<SqlParameter> listParam = new List<SqlParameter>();
                    listParam.Add(new SqlParameter { ParameterName = "@IDSPontuacao", Value = pontuacao, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                    listParam.Add(new SqlParameter { ParameterName = "@IDUsuarioAdm", Value = usuarioAdmId, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                    DataProvider.NonqueryProc(proc, listParam);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public static bool ReprovarPontuacao(string pontuacao, int usuarioAdmId)
        {
            try
            {
                if (!String.IsNullOrEmpty(pontuacao))
                {
                    DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
                    string proc = "JP_UPD_ReprovacaoPontuacao";

                    var conexao = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;

                    List<SqlParameter> listParam = new List<SqlParameter>();
                    listParam.Add(new SqlParameter { ParameterName = "@IDSPontuacao", Value = pontuacao, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                    listParam.Add(new SqlParameter { ParameterName = "@IDUsuarioAdm", Value = usuarioAdmId, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                    DataProvider.NonqueryProc(proc, listParam);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public static DataTable ListaImportacaoPontuacaoErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpPontuacaoErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ListaImportacaoParticipanteHierarquiaErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpParticipanteHierarquiaErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static bool ProcessaPontuacaoArquivo(int ArquivoId, int periodoId, out int countErro, int usuarioId)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_Pontuacao";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioAdmId", Value = usuarioId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoId", Value = periodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[0].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[1].Value;

            return blnSucesso;
        }

        public static bool ImportarArquivoPontuacao(DataTable dtParticipante, int ArquivoId)
        {
            using (var bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName = "dbo.PontuacaoImportacao";
                try
                {
                    dtParticipante.Columns.Add("ArquivoId");
                    foreach (DataRow dr in dtParticipante.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                    }

                    foreach (var column in dtParticipante.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    bulkCopy.WriteToServer(dtParticipante);
                    return true;
                }
                catch (Exception ex)
                {
                    GravarLogErro("Erro na importação do Arquivo", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("ImportacaoArquivoPontuacao(ArquivoId = {0})", ArquivoId.ToString()), "jobCatalog");

                    return false;
                }
            }
        }

        public static string ExportaArquivoPontuacaoErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoPontuacaoErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "Pontuacao/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "Pontuacao/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public List<WebSmart.DTO.DesempenhoModel> SelecionarPontuacaoParticipante(int participanteId, out double totalPontuacao)
        {
            List<WebSmart.DTO.DesempenhoModel> PontuacaoParticipante = new List<DTO.DesempenhoModel>();

            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPontuacao = context.CreateRepository<Pontuacao>();
                IRepository repPontuacaoTipo = context.CreateRepository<TipoPontuacao>();

                //Lista Pontuacao
                var pontuacaoList = repPontuacao.Filter<Pontuacao>(x => x.ParticipanteId == participanteId &&
                                                                        x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.Creditada).OrderBy(x => x.TipoPontuacao.Nome).ToList();

                //Coleta os tipos de pontuaçoes obtidas pelo participantes
                var tipoPontuacao = pontuacaoList.Select(x => x.TipoPontuacaoId).Distinct().ToList();

                //Seta o total de Pontuação Adquirida.
                totalPontuacao = pontuacaoList.Sum(x => x.Pontos);

                foreach (int item in tipoPontuacao)
                {
                    //seleciona a descrição do item em questão.
                    var pontuacaoTipo = repPontuacaoTipo.Filter<TipoPontuacao>(x => x.Id == item).FirstOrDefault();

                    //Popula a lista de desempenho de pontuacoes obtidas de acordo com o tipo.
                    var pontuacaoPorTipo = pontuacaoList.Where(x => x.TipoPontuacaoId == item).ToList();

                    PontuacaoParticipante.Add(new DTO.DesempenhoModel
                    {
                        TipoPontuacao = pontuacaoTipo.Nome,
                        TipoPontuacaoId = item,
                        Pontuacao = BuildPontuacao(pontuacaoPorTipo),
                        TotalPontos = pontuacaoPorTipo.Sum(x => x.Pontos)
                    });
                }
            }
            return PontuacaoParticipante;
        }

        private static List<DTO.PontuacaoModel> BuildPontuacao(IEnumerable<Pontuacao> Pontos)
        {
            List<DTO.PontuacaoModel> Pontuacao = new List<DTO.PontuacaoModel>();

            if (Pontos.Count() > 0)
            {
                foreach (Pontuacao p in Pontos)
                {
                    Pontuacao.Add(new DTO.PontuacaoModel
                    {
                        Descricao = p.Descricao,
                        Pontos = p.Pontos,
                        PeriodoId = p.PeriodoId,
                        Periodo = p.Periodo == null ? "" : p.Periodo.Nome,
                        PontuacaoTipoId = p.TipoPontuacaoId,
                        PontuacaoTipo = p.TipoPontuacao.Nome,
                        DataInclusao = p.DataInclusao,
                        DataAlteracao = p.DataAlteracao,
                        Status = p.StatusPontuacao.Nome
                    });
                }
                return Pontuacao;
            }
            return null;
        }

        public static void InserirPontuacao(int loteId, string proc)
        {
            try
            {
                DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;

                var listParam = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@LoteId", Value = loteId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input },
                        new SqlParameter { ParameterName = "@inserePontuacaoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output }
                    };

                DataProvider.NonqueryProc(proc, listParam);

                var result = (bool)listParam[1].Value;

                if (!result)
                    throw new ApplicationException("Falha ao inserir a pontuação, proc: " + proc);
            }
            catch (Exception ex)
            {
                GravarLogErro(string.Format("Erro ao Inserir as pontuações de Indicação. Lote:{0}.", loteId.ToString()), ex.Message, "GrupoLTM.WebSmartServices - AvonMMA", "InserePontuacaoIndicacao", "jobCatalog");
                throw ex;
            }
        }

        static void GravarLogErro(string Erro, string Mensagem, string Source, string Metodo, string codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "PontuacaoService",
                Pagina = string.Empty,
                Codigo = codigo
            };

            var logErroService = new LogErroService();

            logErroService.SalvarLogErro(logErro);
        }

        public List<PontuacaoEnvioModel> ObterPontuacoesPendentes(int idStatusPontuacao, int? arquivoId = null)
        {
            var lstRetorno = new List<PontuacaoEnvioModel>();

            using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_LitarPontuacoes", conn)
                {
                    CommandTimeout = 2400,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@statusPontuacao", SqlDbType.Int).Value = idStatusPontuacao;

                if (arquivoId.HasValue)
                    cmd.Parameters.Add("@arquivoId", SqlDbType.Int).Value = arquivoId.Value;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //DateTime? data = null;
                        lstRetorno.Add(new PontuacaoEnvioModel()
                        {
                            MktPlaceCatalogoId = (long)reader["MktPlaceCatalogoId"],
                            IdCampanha = (int)reader["IdCampanha"],
                            IdOrigem = (long)reader["IdOrigem"],
                            IdEmpresa = (int)reader["IdEmpresa"],
                            Id = (int)reader["Id"],
                            Login = (string)reader["Login"],
                            Nome = reader["nome"].ToString(),
                            Descricao = (string)reader["Descricao"],
                            Pontos = (double)reader["Pontos"],
                            //DataInclusao = (DateTime)reader["Datainclusao"],
                            ArquivoIdOrigem = (int)reader["ArquivoIdOrigem"],
                            ArquivoNome = (string)reader["ArquivoNome"],
                            DataExpiracao = (DateTime)reader["ExpirationDate"],
                            //DataExpiracao = reader["ExpirationDate"].ToString() != "" ? (DateTime)reader["ExpirationDate"] : data,
                            IdProgramaIncentivo = (int)reader["ProgramaIncentivoId"]
                        });
                    }

                    conn.Close();
                    cmd.Dispose();
                }
            }

            return lstRetorno;
        }

        public static void AtualizaStatusPontuacao(List<PontuacaoEnvioModel> lstPontuacao,
            EnumDomain.StatusPontuacao statusAtual,
            EnumDomain.StatusPontuacao statusNovo,
            string erro,
            int? bankLotId,
            long programaIncentivoId,
            string arquivoNome)
        {
            var idInicio = lstPontuacao.OrderBy(x => x.Id).FirstOrDefault().Id;
            var idFim = lstPontuacao.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            var arquivoIdOrigem = lstPontuacao.Select(x => x.ArquivoIdOrigem).FirstOrDefault();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    var cmd = new SqlCommand("JP_PRC_AtualizarStatusPontuacao", conn)
                    {
                        CommandTimeout = 2400, // 40 minuitos
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("@inicio", SqlDbType.Int).Value = idInicio;
                    cmd.Parameters.Add("@fim", SqlDbType.Int).Value = idFim;
                    cmd.Parameters.Add("@arquivoIdOrigem", SqlDbType.Int).Value = arquivoIdOrigem;
                    cmd.Parameters.Add("@statusPontuacaoAntigo", SqlDbType.Int).Value = (int)statusAtual;
                    cmd.Parameters.Add("@statusPontuacaoNovo", SqlDbType.Int).Value = (int)statusNovo;
                    cmd.Parameters.Add("@bankLotId", SqlDbType.Int).Value = bankLotId;
                    cmd.Parameters.Add("@erroRequest", SqlDbType.NVarChar).Value = erro;
                    cmd.Parameters.Add("@programaIncentivoId", SqlDbType.Int).Value = programaIncentivoId;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogProcessamento.LogErro("Erro ao atualiza o status da pontuação, bankLotId: " + bankLotId, "RequestCreditService", "RequisitarCredito", ex, arquivoNome);
            }
        }

        public static bool AtualizaPontuacaoPorArquivoId(int idArquivo, int statusPontuacaoId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_AtualizaPontuacaoStatusLive", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@arquivoId", SqlDbType.Int).Value = idArquivo;
                    cmd.Parameters.Add("@statusPontuacaoId", SqlDbType.Int).Value = statusPontuacaoId;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();

                    return true;
                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro ao executar a proc: JP_PRC_AtualizaPontuacaoStatusLive", ex.Message, "GrupoLTM.WebSmartServices - AvonMMA", "AtualizaPontuacaoPorArquivoId", "jobCatalog");
                return false;
            }
        }

        public static bool AtualizarRetornoPontuacaoEnviada(DataTable dtPontuacaoRetorno, int? BankLotId)
        {
            bool bRetorno = false;

            try
            {
                // Realiza Bulk Insert dos dados de retorno
                using (var bulkCopy = new SqlBulkCopy(ConfiguracaoService.GetDatabaseConnection()))
                {
                    bulkCopy.BatchSize = 10000;
                    bulkCopy.BulkCopyTimeout = 3600;
                    bulkCopy.DestinationTableName = dtPontuacaoRetorno.TableName;
                    bulkCopy.WriteToServer(dtPontuacaoRetorno);

                    bulkCopy.Close();
                    dtPontuacaoRetorno.Clear();
                }

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_AtualizaPontuacaoBulk", conn);
                    cmd.Parameters.Add("@bankLotId", SqlDbType.Int).Value = BankLotId;
                    cmd.CommandTimeout = 24000; // 400 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bRetorno = reader["bRetorno"] != null ? (Int32)reader["bRetorno"] == 1 ? true : false : false;
                        }
                    }
                    conn.Close();
                    cmd.Dispose();

                    return bRetorno;
                }
            }
            catch (Exception exception)
            {

                GravarLogErro(string.Format("Erro ao Atualizar pontuações. Arquivo:{0}.", BankLotId), exception.Message, "GrupoLTM.WebSmartServices - AvonMMA", "AtualizarRetornoPontuacaoEnviada", "jobCatalog");
                return bRetorno;
            }
        }

        public static List<Pontuacao> ListapontuacaoPorArquivo(int? BankLotId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<Pontuacao>();

                return repArquivo.Filter<Pontuacao>(x => x.BankLotId == BankLotId &&
                                                                       (x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.Enviado ||
                                                                       x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.AguardandoProcessamento ||
                                                                       x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.EmProcessamento ||
                                                                       x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.Criada ||
                                                                       x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.ParticipantesCriados)).ToList();
            }
        }

        public static List<Pontuacao> ListaPendenteProcessamentoPayment()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<Pontuacao>();
                return repArquivo.Filter<Pontuacao>(x => x.BankLotId != null && x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.Enviado ||
                                                         x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.AguardandoProcessamento ||
                                                         x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.EmProcessamento ||
                                                         x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.Criada ||
                                                         x.StatusPontuacaoId == (int)EnumDomain.StatusPontuacao.ParticipantesCriados).Distinct().ToList();
            }
        }

        public static List<Pontuacao> ListarPontuacaoEnviada(int arquivoId)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repArquivo = context.CreateRepository<Pontuacao>();

                return repArquivo.Filter<Pontuacao>(x => x.ArquivoIdOrigem == arquivoId).ToList();
            }
        }
    }
}
