using AutoMapper;
using GrupoLTM.WebSmart.Domain.DTO;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.Importacao;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Infrastructure.TXT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class ArquivoService
    {
        public static Arquivo CadastrarArquivo(string filePath, string nomeArquivo, string nomeArquivoGerado, EnumDomain.TipoArquivo eTipoArquivo)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repArquivo = context.CreateRepository<Arquivo>();

                    var arquivo = new Arquivo
                    {
                        Nome = Path.GetFileName(nomeArquivo),
                        NomeGerado = nomeArquivoGerado,
                        TipoArquivoId = Convert.ToInt32(eTipoArquivo),
                        StatusArquivoId = Convert.ToInt32(EnumDomain.StatusArquivo.Enviado),
                        Ativo = true,
                        DataAlteracao = DateTime.Now,
                        DataInclusao = DateTime.Now
                    };

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repArquivo.Create(arquivo);
                        repArquivo.SaveChanges();
                        scope.Complete();
                    }

                    return arquivo;
                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro no Cadastro de Arquivo", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("CadastrarArquivo({0},{1},{2},{3})", filePath, nomeArquivo, nomeArquivoGerado, eTipoArquivo.ToString()), "jobCatalog");
                throw new Exception("Erro no ao cadastrar o arquivo", ex);
            }
        }

        public static Arquivo CadastrarArquivo(string filePath, string nomeArquivo, string nomeArquivoGerado, EnumDomain.TipoArquivo eTipoArquivo, int? campanhaId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //Grava o arquivo na tabela de Arquivos
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    Arquivo arquivo = new Arquivo();
                    arquivo.Nome = Path.GetFileName(nomeArquivo);
                    arquivo.NomeGerado = nomeArquivoGerado;
                    arquivo.TipoArquivoId = Convert.ToInt32(eTipoArquivo);
                    arquivo.StatusArquivoId = Convert.ToInt32(EnumDomain.StatusArquivo.Enviado);
                    arquivo.Ativo = true;
                    arquivo.DataAlteracao = DateTime.Now;
                    arquivo.DataInclusao = DateTime.Now;
                    if (campanhaId.HasValue)
                        arquivo.CampanhaId = campanhaId;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repArquivo.Create(arquivo);
                        repArquivo.SaveChanges();
                        scope.Complete();
                    }
                    return arquivo;

                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static bool AtualizaArquivo(int idArquivo, EnumDomain.StatusArquivo eStatusArquivo)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    var arquivo = repArquivo.Find<Arquivo>(idArquivo);
                    if (arquivo != null)
                    {
                        arquivo.StatusArquivoId = Convert.ToInt32(eStatusArquivo);
                        arquivo.DataAlteracao = DateTime.Now;
                    }

                    repArquivo.Update(arquivo);
                    repArquivo.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static bool AtualizaArquivo(int idArquivo, string caminhoLogDetalhado, string caminhoLogErro)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    var arquivo = repArquivo.Find<Arquivo>(idArquivo);
                    if (arquivo != null)
                    {
                        arquivo.CaminhoLogDetalhado = caminhoLogDetalhado == null ? arquivo.CaminhoLogDetalhado : caminhoLogDetalhado;
                        arquivo.CaminhoLogErro = caminhoLogErro == null ? arquivo.CaminhoLogErro : caminhoLogErro;
                        arquivo.DataAlteracao = DateTime.Now;
                    }

                    repArquivo.Update(arquivo);
                    repArquivo.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool AtualizaArquivoCSVGerado(int idArquivo, string caminhoLog)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    var arquivo = repArquivo.Find<Arquivo>(idArquivo);

                    arquivo.CaminhoCSV = caminhoLog ?? arquivo.CaminhoLogErro;
                    arquivo.CSVGeradoArquivo = true;
                    arquivo.DataAlteracao = DateTime.Now;

                    repArquivo.Update(arquivo);
                    repArquivo.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro na atualização do arquivo para o CSV gerado, arquivoId: " + idArquivo, ex.Message, "GrupoLTM.WebSmart.Services", "AtualizaArquivoCSVGerado", "jobCatalog");
                throw ex;
            }
        }

        public void LimparExtratoRedisPorLogin(List<int> ids)
        {
            Task.Run(() =>
            {
                if (ids == null)
                {
                    return;
                }
                string idsConvertidos = string.Empty;

                ExtratoService _extratoservice = new ExtratoService();

                foreach (var id in ids)
                {
                    List<string> Logins = ObterRasPorArquivoId(Convert.ToInt32(id));
                    _extratoservice.LimparExtratoCachePorLogin(Logins);
                }
            });
        }

        private List<string> ObterRasPorArquivoId(int IdArquivo)
        {
            try
            {
                List<string> retorno = new List<string>();

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_SEL_ParticipanteLoginPorArquivo", conn);
                    cmd.CommandTimeout = 2400;  // 40 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdArquivo", SqlDbType.Int).Value = IdArquivo;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno.Add(reader["Login"] != DBNull.Value ? (string)reader["Login"] : "");
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ProcessarTotaisArquivo(int arquivoId, string proc)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;

            var listParam = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ArquivoId", Value = arquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input },
                    new SqlParameter { ParameterName = "@ProcessadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output }
                };

            DataProvider.NonqueryProc(proc, listParam);

            var result = (bool)listParam[1].Value;

            if (!result)
                throw new ProcessamentoException($"A procedure {proc} retonou status erro");
        }

        public static bool AtualizarArquivoProcessado(int idArquivo, int? tempoProcessamento, int? quantidadeLinhas, bool reprocessado = false)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repArquivo = context.CreateRepository<Arquivo>();
                    var arquivo = repArquivo.Find<Arquivo>(idArquivo);

                    arquivo.TempoProcessamento = tempoProcessamento;
                    arquivo.DataTerminoProcessamento = DateTime.Now;
                    arquivo.QuantidadeLinhas = quantidadeLinhas;

                    if (!reprocessado)
                        arquivo.StatusArquivoId = (int)EnumDomain.StatusArquivo.Processado;

                    repArquivo.Update(arquivo);
                    repArquivo.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateArquivoInconsistencia(int idArquivo)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repArquivo = context.CreateRepository<Arquivo>();
                    var arquivo = repArquivo.Find<Arquivo>(idArquivo);
                    var _arquivoService = new ArquivoService();

                    if (arquivo != null)
                    {
                        arquivo.DataTerminoProcessamento = DateTime.Now;
                        arquivo.StatusArquivoId = (int)EnumDomain.StatusArquivo.Inconsistente;
                        repArquivo.Update(arquivo);
                        repArquivo.SaveChanges();

                        ReprovarPontuacoes(idArquivo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro na atualização do Arquivo para insconsistência", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("UpdateArquivoInconsistencia(idArquivo:{0})", idArquivo.ToString()), "jobCatalog");
                throw ex;
            }

        }

        public static bool UpdateArquivoExtratoProcessado(int idArquivo)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repArquivo = context.CreateRepository<Arquivo>();

                    var arquivo = repArquivo.Find<Arquivo>(idArquivo);

                    arquivo.ExtratoProcessado = true;

                    repArquivo.Update(arquivo);
                    repArquivo.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro ao atualizar o arquivo para extrato processado, arquivoId: " + idArquivo, ex.Message, "GrupoLTM.WebSmart.Services", string.Format("UpdateArquivoExtratoProcessado(idArquivo:{0})", idArquivo), "jobCatalog");
                throw ex;
            }

        }

        public static Arquivo ObterArquivoPorNome(string nomeArquivo, EnumDomain.StatusArquivo? statusArquivo = null)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repository = context.CreateRepository<Arquivo>();

                var query = repository.Filter<Arquivo>(x => x.Nome.ToUpper() == nomeArquivo.ToUpper() && x.Ativo);

                if (statusArquivo != null)
                    query = query.Where(x => x.StatusArquivoId == (int)statusArquivo);

                return query.FirstOrDefault();
            }
        }

        public static List<Arquivo> ObterArquivoPorStatusTipo(EnumDomain.StatusArquivo statusArquivo, EnumDomain.TipoArquivo tipoArquivo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<Arquivo>();
                return repArquivo.Filter<Arquivo>(x => x.StatusArquivoId == (int)statusArquivo
                                                    && x.TipoArquivoId == (int)tipoArquivo
                                                    && x.Ativo).ToList();
            }
        }

        public static Arquivo ObterArquivoPorNome(string nomeArquivo, EnumDomain.TipoArquivo tipoArquivo)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repArquivo = context.CreateRepository<Arquivo>();
                    var arquivo = repArquivo.Filter<Arquivo>(x => x.Nome.ToUpper() == nomeArquivo.ToUpper() && x.TipoArquivoId == (int)tipoArquivo
                                                        && x.Ativo).OrderByDescending(y => y.Id).FirstOrDefault();
                    return arquivo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter o arquivo por nome", ex);
            }
        }

        public static Arquivo ObterArquivoPorId(int arquivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<Arquivo>();
                return repArquivo.Filter<Arquivo>(x => x.Id == arquivoId && x.Ativo).FirstOrDefault();
            }
        }

        public static Arquivo CadastrarArquivoLive(Arquivo arquivo, int usuarioAdm)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //Grava o arquivo na tabela de Arquivos
                    IRepository repArquivo = context.CreateRepository<Arquivo>();

                    arquivo.UsuarioAdmId = usuarioAdm;
                    arquivo.StatusArquivoId = Convert.ToInt32(EnumDomain.StatusArquivo.PendenteEnvioFtpLive);
                    arquivo.DataInclusao = DateTime.Now;
                    arquivo.Ativo = true;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repArquivo.Create(arquivo);
                        repArquivo.SaveChanges();
                        scope.Complete();
                    }
                    return arquivo;

                }
            }
            catch (Exception)
            {
                return new Arquivo();
                throw;
            }

        }

        public static ArquivoConfiguracao ObterPorIdTipoArquivo(int idTipoArquivo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<ArquivoConfiguracao>();
                return repArquivo.Filter<ArquivoConfiguracao>(x => x.TipoArquivoId == idTipoArquivo && x.Ativo).FirstOrDefault();
            }
        }

        public static List<Arquivo> ListaPendenteProcessamentoRetorno()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repArquivo = context.CreateRepository<Arquivo>();

                return repArquivo.Filter<Arquivo>(x => x.TipoArquivoId == (int)EnumDomain.TipoArquivo.EnvioPontuacaoLive &&
                    x.StatusArquivoId == (int)EnumDomain.StatusArquivo.PendenteRetornoFtpLive &&
                    x.Ativo).ToList();
            }
        }

        public static bool AtualizarRetornoPontuacaoEnviadaPorArquivo(int idArquivo, int idStatus)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    Arquivo arquivo = repArquivo.Find<Arquivo>(x => x.Id == idArquivo);

                    arquivo.StatusArquivoId = idStatus;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repArquivo.Create(arquivo);
                        repArquivo.SaveChanges();
                        scope.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AtualizarRetornoPontuacaoEnviadaErroPorArquivo(int idArquivo, int idStatusArquivo, int idStatusPontuacao)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    Arquivo arquivo = repArquivo.Find<Arquivo>(x => x.Id == idArquivo);

                    // TODO: Remover tudo e executar procedure

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repArquivo.Create(arquivo);
                        repArquivo.SaveChanges();
                        scope.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DashboardModel ObterConsolidadoPontuacaoDashboard(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                DashboardModel retorno = new DashboardModel();
                retorno.Itens = new List<DashBoardItem>();

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_ObterConsolidadoPontuacaoDashboard", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (catalogoId.HasValue && catalogoId.Value > 0)
                        cmd.Parameters.Add("@CatalogoId", SqlDbType.Int).Value = catalogoId;

                    cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = dtInicio;
                    cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = dtFim.AddDays(1);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno.Itens.Add(new DashBoardItem()
                            {
                                TipoArquivo = reader["TipoArquivo"] != DBNull.Value ? (string)reader["TipoArquivo"] : null,
                                LinhasIntegradas = reader["LinhasIntegradas"] != DBNull.Value ? (long)reader["LinhasIntegradas"] : 0,
                                LinhasComErro = reader["LinhasComErro"] != DBNull.Value ? (long)reader["LinhasComErro"] : 0,
                                RevendedoresProcessados = reader["RevendedoresProcessados"] != DBNull.Value ? (long)reader["RevendedoresProcessados"] : 0,
                                PontosDisponiveis = reader["PontosDisponiveis"] != DBNull.Value ? (long)reader["PontosDisponiveis"] : 0,
                                PontosCancelados = reader["PontosCancelados"] != DBNull.Value ? (long)reader["PontosCancelados"] : 0,
                                PontosPendentes = reader["PontosPendentes"] != DBNull.Value ? (long)reader["PontosPendentes"] : 0
                            });
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                // Totaliza retorno
                if (retorno.Itens.Count > 0)
                {
                    retorno.LinhasIntegradas = retorno.Itens.Sum(x => x.LinhasIntegradas);
                    retorno.LinhasComErro = retorno.Itens.Sum(x => x.LinhasComErro);
                    retorno.RevendedoresProcessados = retorno.Itens.Sum(x => x.RevendedoresProcessados);
                    retorno.PontosDisponiveis = retorno.Itens.Sum(x => x.PontosDisponiveis);
                    retorno.PontosCancelados = retorno.Itens.Sum(x => x.PontosCancelados);
                    retorno.PontosPendentes = retorno.Itens.Sum(x => x.PontosPendentes);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ArquivoMonitoramentoModel ObterArquivosMonitoramento(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                ArquivoMonitoramentoModel retorno = new ArquivoMonitoramentoModel();
                retorno.ArquivosPontuacaoAvon = new List<ArquivoPontuacaoAvon>();
                retorno.ResgatesAvon = new List<DTO.ResgatesAvon>();
                retorno.ArquivosCreditoLive = new List<ArquivoCreditoLive>();

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_ObterArquivosMonitoramento", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (catalogoId.HasValue && catalogoId.Value > 0)
                        cmd.Parameters.Add("@CatalogoId", SqlDbType.Int).Value = catalogoId;

                    cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = dtInicio;
                    cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = dtFim.AddDays(1);

                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        // Popula Dados de Pontuação
                        ds.Tables[0].AsEnumerable().ToList().ForEach(item =>
                        {
                            retorno.ArquivosPontuacaoAvon.Add(new ArquivoPontuacaoAvon()
                            {
                                Nome = item.Field<string>("Nome"),
                                Catalogo = item.Field<string>("NomeCatalogo"),
                                TipoArquivoId = item.Field<int>("TipoArquivoId"),
                                ArquivoId = item.Field<int>("ArquivoId"),
                                DataInclusao = item.Field<DateTime>("DataInclusao"),
                                CaminhoLogDetalhado = !string.IsNullOrEmpty(item.Field<string>("CaminhoLogDetalhado")) ?
                                    string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, item.Field<string>("CaminhoLogDetalhado"), Settings.Caminho.StorageLogToken) : null,
                                CaminhoLogErro = !string.IsNullOrEmpty(item.Field<string>("CaminhoLogErro")) ?
                                    string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, item.Field<string>("CaminhoLogErro"), Settings.Caminho.StorageLogToken) : null
                            });
                        });

                        // Popula Dados de Resgate
                        ds.Tables[1].AsEnumerable().ToList().ForEach(item =>
                        {
                            retorno.ResgatesAvon.Add(new DTO.ResgatesAvon()
                            {
                                DataInclusao = item.Field<DateTime>("DataInclusao"),
                                Catalogo = item.Field<string>("NomeCatalogo"),
                                UrlArquivoResgatesGerais = item.Field<string>("UrlArquivoResgatesGerais"),
                                UrlArquivoResgatesAvon = item.Field<string>("UrlArquivoResgatesAvon")
                            });
                        });

                        // Popula Arquivos de Crédito
                        ds.Tables[2].AsEnumerable().ToList().ForEach(item =>
                        {
                            retorno.ArquivosCreditoLive.Add(new ArquivoCreditoLive()
                            {
                                Id = item.Field<int>("Id"),
                                DataInclusao = item.Field<DateTime>("DataInclusao"),
                                QtdLinhasProcessadas = item.Field<int>("QtdLinhasProcessadas"),
                                QtdPontos = item.Field<double>("QtdPontos")
                            });
                        });

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Arquivo> ObterArquivosProcessados(DateTime dtProcessamento)
        {
            try
            {
                DateTime dtFim = dtProcessamento.AddDays(1);

                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    return repArquivo.Filter<Arquivo>(x => x.StatusArquivoId == (int)EnumDomain.StatusArquivo.Processado
                        && x.DataInclusao > dtProcessamento
                        && x.DataInclusao < dtFim
                        && x.Ativo).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Arquivo> ObterArquivosParaGeracaoLogDetalhado()
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();
                    return repArquivo.Filter<Arquivo>(x => x.StatusArquivoId == (int)EnumDomain.StatusArquivo.Processado
                        && x.CaminhoLogDetalhado == null
                        && x.Ativo).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EnviarEmailStatusProcessamento(List<Arquivo> arquivos, int loteId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<h1>Processamento Lote: " + loteId.ToString() + "</h1>");
                sb.Append("<table style=\"font-size: 12px; font-family: Arial, Verdane, Helvetica;\" cellpadding=\"5\" border=\"1\">");
                sb.Append(" <thead>");
                sb.Append("     <tr>");
                sb.Append("         <th>Id</th>");
                sb.Append("         <th>Nome</th>");
                sb.Append("         <th>Data Inclusao</th>");
                sb.Append("         <th>Data Termino Processamento</th>");
                sb.Append("         <th>Qtd. Linhas Processadas</th>");
                sb.Append("         <th>Qtd. Revendedoras Processadas</th>");
                sb.Append("         <th>Pontos Disponiveis</th>");
                sb.Append("         <th>Pontos Cancelados</th>");
                sb.Append("         <th>Pontos Pendentes</th>");
                sb.Append("     </tr>");
                sb.Append(" </thead>");
                sb.Append(" <tbody>");

                foreach (var arquivo in arquivos)
                {
                    sb.Append("     <tr style=\"text - align: center;\">");
                    sb.Append(string.Format("<td>{0}</td>", arquivo.Id));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.Nome));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss")));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.DataTerminoProcessamento != null ? ((DateTime)arquivo.DataTerminoProcessamento).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.QuantidadeLinhas));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.QuantidadeRevendedorasProcessadas));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.PontosDisponiveis));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.PontosCancelados));
                    sb.Append(string.Format("<td>{0}</td>", arquivo.PontosPendentes));
                    sb.Append("     </tr>");
                }

                sb.Append(" </tbody>");
                sb.Append("</table>");

                Infrastructure.Mail.Email.EnviarEmail("Processamento de Arquivo de Pontuação SFTP Avon. Lote: " + loteId.ToString(), sb.ToString(), true);
            }
            catch (Exception ex)
            {
                GravarLogErro(string.Format("Erro ao Enviar e-mail de status de processamento. Lote:{0}.", loteId.ToString()), ex.Message, "GrupoLTM.WebSmartServices - AvonMMA", "EnviarEmailStatusProcessamento", "jobCatalog");

                throw ex;
            }
        }

        public List<AprovacaoPontosModel> ObterPontosAprovacao(int catalogoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();

                    var arquivosPontos = repArquivo.Filter<Arquivo>(x => x.StatusArquivoId == (int)EnumDomain.StatusArquivo.Processado
                        && x.CSVGeradoArquivo && x.Ativo).OrderByDescending(x => x.DataTerminoProcessamento).ToList();

                    List<AprovacaoPontosModel> pontos = CreateAprovacaoPontos(arquivosPontos);

                    return pontos;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AprovacaoPontosModel> ObterPontosAprovacao(int? catalogoId, int start, int regExibir, out int total)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repArquivo = context.CreateRepository<Arquivo>();

                    var aceitaSemPontuacao = new int[] { (int)EnumDomain.TipoArquivo.Migracao, (int)EnumDomain.TipoArquivo.Alianca, (int)EnumDomain.TipoArquivo.Natal };

                    var lstArquivosPontos = repArquivo.Filter<Arquivo>(x => x.StatusArquivoId == (int)EnumDomain.StatusArquivo.Processado
                                                                         && x.Ativo
                                                                         && x.CSVGeradoArquivo
                                                                         && (x.PontosDisponiveis > 0 || x.PontosPendentes > 0 || x.PontosCancelados > 0 || aceitaSemPontuacao.Contains(x.TipoArquivoId))
                                                                         && (catalogoId == null || (x.CatalogoId == catalogoId || x.CatalogosCP.Any(c => c.CatalogoId == catalogoId))))
                                                                         .OrderByDescending(x => x.DataTerminoProcessamento);

                    total = lstArquivosPontos.Count();

                    var list = lstArquivosPontos.Skip(start).Take(regExibir).ToList();

                    var pontos = CreateAprovacaoPontos(list);

                    return pontos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AprovacaoResgatesOfflineModel> ObterResgatesOfflineAprovacao(int start, int regExibir, out int total)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();

                    var lstArquivosResgateOffline = repArquivo.Filter<Arquivo>(x => x.StatusArquivoId == (int)EnumDomain.StatusArquivo.Enviado
                                                                        && x.TipoArquivoId == (int)EnumDomain.TipoArquivo.ResgateOffline
                                                                        && x.Ativo
                                                                        ).OrderBy(x => x.DataInclusao);

                    total = lstArquivosResgateOffline.Count();

                    List<AprovacaoResgatesOfflineModel> resgatesOffline = new List<AprovacaoResgatesOfflineModel>();
                    resgatesOffline = CreateAprovacaoResgatesOffline(lstArquivosResgateOffline.Skip(start).Take(regExibir).ToList());

                    return resgatesOffline;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AprovacaoResgatesOfflineModel> ObterListaArquivosResgatesOffline(int start, int regExibir, DateTime? dtInicio, DateTime? dtFim, int? statusArquivoId, out int total)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();

                    var lstArquivosResgateOffline = repArquivo.Filter<Arquivo>(x =>
                                                                        (x.TipoArquivoId == (int)EnumDomain.TipoArquivo.ResgateOffline
                                                                        || x.TipoArquivoId == (int)EnumDomain.TipoArquivo.ForcarPrimeiroAcesso)
                                                                        && (statusArquivoId == null || x.StatusArquivoId == statusArquivoId)
                                                                        && x.Ativo
                                                                        && (dtInicio == null || x.DataInclusao >= dtInicio)
                                                                        && (dtFim == null || x.DataInclusao <= dtFim)
                                                                        ).OrderByDescending(x => x.DataInclusao);

                    total = lstArquivosResgateOffline.Count();

                    List<AprovacaoResgatesOfflineModel> resgatesOffline = new List<AprovacaoResgatesOfflineModel>();
                    resgatesOffline = CreateAprovacaoResgatesOffline(lstArquivosResgateOffline.Skip(start).Take(regExibir).ToList());

                    return resgatesOffline;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CancelamentoPontosModel> ObterPontosCancelamento(int? catalogoId, out int total)
        {
            try
            {
                List<CancelamentoPontosModel> retorno = new List<CancelamentoPontosModel>();

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_ObterArquivosCancelamento", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CatalogoId", SqlDbType.Int).Value = catalogoId;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!string.IsNullOrWhiteSpace(reader["DataTerminoProcessamento"].ToString()))
                            {
                                retorno.Add(new CancelamentoPontosModel()
                                {
                                    ArquivoId = reader["ArquivoId"] != DBNull.Value ? (int)reader["ArquivoId"] : 0,
                                    Nome = reader["Nome"] != DBNull.Value ? (string)reader["Nome"] : null,
                                    TipoArquivoId = reader["TipoArquivoId"] != DBNull.Value ? (int)reader["TipoArquivoId"] : 0,
                                    TipoArquivo = reader["TipoArquivo"] != DBNull.Value ? (string)reader["TipoArquivo"] : null,
                                    TotalPontos = reader["TotalPontos"] != DBNull.Value ? Convert.ToSingle(reader["TotalPontos"]) : 0,
                                    PontosDisponiveis = reader["PontosDisponiveis"] != DBNull.Value ? Convert.ToSingle(reader["PontosDisponiveis"]) : 0,
                                    PontosPendentes = reader["PontosPendentes"] != DBNull.Value ? Convert.ToSingle(reader["PontosPendentes"]) : 0,
                                    PontosCancelados = reader["PontosCancelados"] != DBNull.Value ? Convert.ToSingle(reader["PontosCancelados"]) : 0,
                                    CSV = reader["CaminhoCSV"] != DBNull.Value ? string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, (string)reader["CaminhoCSV"], Settings.Caminho.StorageLogToken) : null,
                                    DataTerminoProcessamento = Convert.ToDateTime(reader["DataTerminoProcessamento"]),
                                    DataProcessamentoTexto = Convert.ToString(reader["DataTerminoProcessamento"]),
                                    IncentiveProgramDescriptionHeader = reader["IncentiveProgramDescriptionHeader"] != DBNull.Value ? (string)reader["IncentiveProgramDescriptionHeader"] : null
                                });
                            }
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                total = retorno.Count();

                retorno = retorno.OrderByDescending(x => x.DataTerminoProcessamento).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Arquivo> GetAll()
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Arquivo>();


                    var lstArquivos = repArquivo.All<Arquivo>().ToList();

                    return lstArquivos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StatusArquivo> ObterStatusArquivos()
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repStatusArquivo = context.CreateRepository<StatusArquivo>();


                    var lstStatusArquivos = repStatusArquivo.All<StatusArquivo>().ToList();

                    return lstStatusArquivos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TipoArquivo> ObterTipoArquivoPorCatalogo(int? catalogoId)
        {
            List<TipoArquivo> list = new List<TipoArquivo>();

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "[JP_PRC_TipoArquivoPorCatalogo]";
            //total = 0;

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@catalogoId", Value = catalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            //de para
            foreach (DataRow row in table.Rows)
            {
                list.Add(new TipoArquivo()
                {
                    Id = (int)row["Id"],
                    Nome = (string)row["Nome"]

                });
            }

            return list;
        }

        public List<ArquivoPontosModel> ObterArquivoPontos(int? catalogoId, DateTime DataInicial, DateTime DataFinal, int? campanhaId, int? tipoArquivoId, string statusArquivoId, int? anoCampanha, out int total)
        {
            var retorno = new List<ArquivoPontosModel>();

            using (var connection = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_ObterRelatorioArquivos", connection)
                {
                    CommandTimeout = 2400, // 40 minutos
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@CatalogoId", SqlDbType.Int).Value = catalogoId;
                cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = DataInicial;
                cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = DataFinal;

                if (campanhaId != null)
                    cmd.Parameters.Add("@CampanhaId", SqlDbType.Int).Value = campanhaId;

                if (tipoArquivoId != null)
                    cmd.Parameters.Add("@TipoArquivoId", SqlDbType.Int).Value = tipoArquivoId;

                if (statusArquivoId != null)
                    cmd.Parameters.Add("@StatusArquivoId", SqlDbType.VarChar).Value = statusArquivoId;

                if (anoCampanha != null)
                    cmd.Parameters.Add("@AnoCampanha", SqlDbType.Int).Value = anoCampanha;

                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var statusarquivo = new EnumDomain.StatusArquivo();
                        var statusarquivocredito = new EnumDomain.StatusArquivo();

                        var NomeArquivo = string.Empty;
                        var StatusArquivo = string.Empty;
                        var DescricaoStatusArquivo = string.Empty;

                        if ((int)reader["StatusArquivoCreditoId"] != 0 && reader["NomeArquivoPontos"] != null && reader["NomeArquivoCredito"] != null)
                        {
                            statusarquivocredito = (EnumDomain.StatusArquivo)Enum.ToObject(typeof(EnumDomain.StatusArquivo), (int)reader["StatusArquivoCreditoId"]);
                            NomeArquivo = (string)reader["NomeArquivoPontos"] + " | " + (string)reader["NomeArquivoCredito"];
                            DescricaoStatusArquivo = EnumDomain.GetDescription(statusarquivocredito);
                            StatusArquivo = (string)reader["StatusArquivoCredito"];
                        }
                        else
                        {
                            statusarquivo = (EnumDomain.StatusArquivo)Enum.ToObject(typeof(EnumDomain.StatusArquivo), (int)reader["StatusArquivoId"]);
                            NomeArquivo = reader["NomeArquivoPontos"] != null ? (string)reader["NomeArquivoPontos"] : "";
                            DescricaoStatusArquivo = EnumDomain.GetDescription(statusarquivo);
                            StatusArquivo = (string)reader["StatusArquivoPontos"];
                        }

                        retorno.Add(new ArquivoPontosModel()
                        {
                            ArquivoId = reader["ArquivoId"] != DBNull.Value ? (int)reader["ArquivoId"] : 0,
                            IncentiveProgramDescriptionHeader = reader["IncentiveProgramDescriptionHeader"] != DBNull.Value ? (string)reader["IncentiveProgramDescriptionHeader"] : "",
                            Nome = NomeArquivo,
                            TipoArquivoId = reader["TipoArquivoId"] != DBNull.Value ? (int)reader["TipoArquivoId"] : 0,
                            TipoArquivo = reader["TipoArquivo"] != DBNull.Value ? (string)reader["TipoArquivo"] : "",
                            DataTerminoProcessamento = Convert.ToDateTime(reader["DataTerminoProcessamento"]),
                            StatusArquivoNome = StatusArquivo,
                            DataInclusao = Convert.ToDateTime(reader["DataInclusao"]),
                            QuantidadeLinhas = reader["QuantidadeLinhas"] != DBNull.Value ? Convert.ToSingle(reader["QuantidadeLinhas"]) : 0,
                            QuantidadeRevendedorasProcessadas = reader["QuantidadeRevendedorasProcessadas"] != DBNull.Value ? Convert.ToSingle(reader["QuantidadeRevendedorasProcessadas"]) : 0,
                            PontosDisponiveisInicial = reader["PontosDisponiveisInicial"] != DBNull.Value ? Convert.ToSingle(reader["PontosDisponiveisInicial"]) : 0,
                            PontosPendentesInicial = reader["PontosPendentesInicial"] != DBNull.Value ? Convert.ToSingle(reader["PontosPendentesInicial"]) : 0,
                            PontosCanceladosInicial = reader["PontosCanceladosInicial"] != DBNull.Value ? Convert.ToSingle(reader["PontosCanceladosInicial"]) : 0,
                            CSV = reader["CaminhoCSV"] != DBNull.Value ? string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, (string)reader["CaminhoCSV"], Settings.Caminho.StorageLogToken) : "",
                            StatusArquivoId = reader["StatusArquivoId"] != DBNull.Value ? (int)reader["StatusArquivoId"] : 0,
                            DescricaoStatusArquivo = DescricaoStatusArquivo
                        });
                    }

                    connection.Close();
                    cmd.Dispose();
                }
            }

            total = retorno.Count();
            retorno = retorno.OrderByDescending(x => x.DataInclusao).ToList();

            return retorno;
        }

        public List<ErroImportacaoArquivoModel> ObterLogImportacaoErro(DateTime DataInicial, DateTime DataFinal, int start, int regExibir, out int total)
        {
            try
            {
                List<ErroImportacaoArquivoModel> retorno = new List<ErroImportacaoArquivoModel>();

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_ObterLogErroImportacao", conn);
                    cmd.CommandTimeout = 2400; // 40 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = DataInicial;
                    cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = DataFinal;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno.Add(new ErroImportacaoArquivoModel()
                            {
                                ArquivoId = reader["ArquivoId"] != DBNull.Value ? (int)reader["ArquivoId"] : 0,
                                Nome = reader["Nome"] != DBNull.Value ? (string)reader["Nome"] : "",
                                TipoArquivoId = reader["TipoArquivoId"] != DBNull.Value ? (int)reader["TipoArquivoId"] : 0,
                                TipoArquivo = reader["TipoArquivo"] != DBNull.Value ? (string)reader["TipoArquivo"] : "",
                                DataInclusao = Convert.ToDateTime(reader["DataInclusao"]),
                                DataInclusaoErro = reader["DataInclusaoErro"] != DBNull.Value ? Convert.ToDateTime(reader["DataInclusaoErro"]) : Convert.ToDateTime(reader["DataInclusao"]),
                                DescricaoErro = reader["DescricaoErro"] != DBNull.Value ? (string)reader["DescricaoErro"] : "",
                                QtdLinhasErro = reader["QtdLinhasComErro"] != DBNull.Value ? (int)reader["QtdLinhasComErro"] : 0,
                                TipoRegistro = reader["TipoRegistro"] != DBNull.Value ? (string)reader["TipoRegistro"] : "",
                                CSV = reader["CSV"] != DBNull.Value ? string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, (string)reader["CSV"], Settings.Caminho.StorageLogToken) : ""
                            });
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                total = retorno.Count();
                retorno = retorno.OrderByDescending(x => x.DataInclusao).Skip(start).Take(regExibir).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ImportacaoErroDetalheModel> ObterImportacaoErroCSV(int arquivoId)
        {
            var retorno = new List<ImportacaoErroDetalheModel>();

            using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                SqlCommand cmd = new SqlCommand("JP_PRC_ObterLogErroImportacaoDetalhe", conn);
                cmd.CommandTimeout = 2400; // 40 minuitos
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@arquivoId", SqlDbType.Int).Value = arquivoId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.Add(new ImportacaoErroDetalheModel()
                        {
                            ArquivoId = reader["ArquivoId"] != DBNull.Value ? (int)reader["ArquivoId"] : 0,
                            Nome = reader["Nome"] != DBNull.Value ? (string)reader["Nome"] : "",
                            TipoArquivoId = reader["TipoArquivoId"] != DBNull.Value ? (int)reader["TipoArquivoId"] : 0,
                            TipoArquivo = reader["TipoArquivo"] != DBNull.Value ? (string)reader["TipoArquivo"] : "",
                            DataInclusao = Convert.ToDateTime(reader["DataInclusao"]),
                            DataInclusaoErro = reader["DataInclusaoErro"] != DBNull.Value ? Convert.ToDateTime(reader["DataInclusaoErro"]) : Convert.ToDateTime(reader["DataInclusao"]),
                            DescricaoErro = reader["DescricaoErro"] != DBNull.Value ? (string)reader["DescricaoErro"] : "",
                            IdOrigemNormalizada = reader["IdOrigemNormalizada"] != DBNull.Value ? (int)reader["IdOrigemNormalizada"] : 0,
                            LoteId = reader["LoteId"] != DBNull.Value ? (int)reader["LoteId"] : 0,
                            LinhaConteudo = reader["LinhaConteudo"] != DBNull.Value ? (string)reader["LinhaConteudo"] : "",
                            TipoRegistro = reader["TipoRegistro"] != DBNull.Value ? (string)reader["TipoRegistro"] : "",
                            CSV = reader["CSV"] != DBNull.Value ? string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, (string)reader["CSV"], Settings.Caminho.StorageLogToken) : ""

                        });
                    }

                    conn.Close();
                    cmd.Dispose();
                }
            }
            return retorno;
        }

        public void AprovarPontuacoes(string ids)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_AprovaPontuacoes", conn);
                    cmd.CommandTimeout = 2400;  // 40 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ids", SqlDbType.VarChar).Value = ids;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ReprovarPontuacoes(string ids)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_ReprovaPontuacoes", conn);
                    cmd.CommandTimeout = 2400;  // 40 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdsArquivos", SqlDbType.VarChar).Value = ids;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelarPontuacoes(string ids)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_CancelaPontuacoes", conn);
                    cmd.CommandTimeout = 2400;  // 40 minutos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdsArquivos", SqlDbType.VarChar).Value = ids;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Métodos Privados"

        public AprovacaoPontosModel CreateAprovacaoPontos(Arquivo entity)
        {
            var total = (entity.PontosPendentes ?? 0) + (entity.PontosCancelados ?? 0) + (entity.PontosDisponiveis ?? 0);

            var nomeProgramaIncentivo = string.Empty;

            using (var context = UnitOfWorkFactory.Create())
            {
                var repProgramaIncentivoCatalogoArquivo = context.CreateRepository<ProgramaIncentivoCatalogoArquivo>();
                var repProgramaIncentivo = context.CreateRepository<ProgramaIncentivo>();

                var programaIncentivoCatalogoArquivo = repProgramaIncentivoCatalogoArquivo.Find<ProgramaIncentivoCatalogoArquivo>(x => x.ArquivoId == entity.Id);
                if (programaIncentivoCatalogoArquivo != null)
                {
                    var programaIncentivo = repProgramaIncentivo.Find<ProgramaIncentivo>(x => x.Id == programaIncentivoCatalogoArquivo.ProgramaIncentivoId);
                    nomeProgramaIncentivo = programaIncentivo.Nome;
                }
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Arquivo, AprovacaoPontosModel>()
                    .ForMember(dst => dst.ArquivoId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.TipoArquivo, opt => opt.MapFrom(src => src.TipoArquivo.Nome))
                    .ForMember(dst => dst.IncentiveProgramDescriptionHeader, opt => opt.MapFrom(src => nomeProgramaIncentivo))
                    .ForMember(dst => dst.DataTerminoProcessamento, opt => opt.MapFrom(src => src.DataTerminoProcessamento != null ? ((DateTime)src.DataTerminoProcessamento).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty))
                    .ForMember(dst => dst.PontosLiberados, opt => opt.MapFrom(src => src.PontosDisponiveis))
                    .ForMember(dst => dst.PontosPendentes, opt => opt.MapFrom(src => src.PontosPendentes))
                    .ForMember(dst => dst.PontosCancelados, opt => opt.MapFrom(src => src.PontosCancelados))
                    .ForMember(dst => dst.PontosImportados, opt => opt.MapFrom(src => total))
                    .ForMember(dst => dst.CSV, opt => opt.MapFrom(src => string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, src.CaminhoCSV, Settings.Caminho.StorageLogToken)))
                    .ForMember(dst => dst.Campanhas, opt => opt.MapFrom(src => string.Join(" ", src.CatalogosCP.Where(x => x.Ativo).Select(x => x.CP).Distinct().ToArray())));
            });

            var mapper = config.CreateMapper();

            return mapper.Map<Arquivo, AprovacaoPontosModel>(entity);
        }

        private List<AprovacaoPontosModel> CreateAprovacaoPontos(List<Arquivo> entities)
        {
            List<AprovacaoPontosModel> lstRetorno = new List<AprovacaoPontosModel>();

            foreach (var entity in entities)
                lstRetorno.Add(CreateAprovacaoPontos(entity));

            return lstRetorno;
        }

        private List<AprovacaoResgatesOfflineModel> CreateAprovacaoResgatesOffline(List<Arquivo> arquivos)
        {

            List<AprovacaoResgatesOfflineModel> resgates = new List<AprovacaoResgatesOfflineModel>();
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repResgateOffline = context.CreateRepository<ResgateOffLine>();
                IRepository repForcarPrimeiroAcesso = context.CreateRepository<ForcarPrimeiroAcesso>();
                IRepository repCatalogo = context.CreateRepository<Catalogo>();

                foreach (Arquivo a in arquivos)
                {
                    AprovacaoResgatesOfflineModel resgate = new AprovacaoResgatesOfflineModel();

                    var resgatesOffline = repResgateOffline.Filter<ResgateOffLine>(x => x.ArquivoUploadId == a.Id);
                    var forcarPrimeirosAcesso = repForcarPrimeiroAcesso.Filter<ForcarPrimeiroAcesso>(m => m.ArquivoId == a.Id);
                    string catalogos = "";
                    string productSku = "";
                    decimal valorPontos = 0;
                    decimal valorReal = 0;
                    if (resgatesOffline != null && resgatesOffline.Count() > 0)
                    {
                        foreach (ResgateOffLine r in resgatesOffline)
                        {
                            var catalogo = repCatalogo.Filter<Catalogo>(y => y.Codigo == r.CatalogId).FirstOrDefault();
                            catalogos += catalogo != null ? string.Format("{0}{1}", catalogo.Nome, ". ") : "";
                        }
                        productSku = resgatesOffline.FirstOrDefault().Product;
                        valorPontos = resgatesOffline.FirstOrDefault().Value;
                        valorReal = resgatesOffline.FirstOrDefault().ValueReal;
                    }

                    resgate.ArquivoId = a.Id;
                    resgate.Catalogos = catalogos;
                    resgate.DataInclusao = a.DataInclusao.ToString();
                    resgate.NomeArquivoUpload = a.Nome;
                    resgate.NomeProdutoSku = productSku;
                    int? quantidadeRevendedorasProcessadas = (from x in forcarPrimeirosAcesso select x.Login).Distinct().Count();
                    resgate.QuantidadeRevendedorasProcessadas = quantidadeRevendedorasProcessadas != null ? quantidadeRevendedorasProcessadas : 0;
                    int? quantidadeProdutosResgatados = forcarPrimeirosAcesso.Where(y => y.IdPedido != 0).Sum(x => x.quantity);
                    resgate.QuantidadeProdutosResgatados = quantidadeProdutosResgatados != null ? quantidadeProdutosResgatados : 0;
                    resgate.PontosResgatados = quantidadeProdutosResgatados != null ? quantidadeProdutosResgatados * valorPontos : 0;
                    resgate.ValorPontos = valorPontos;
                    resgate.ValorReal = valorReal;
                    long? quantidadePedidosSucesso = forcarPrimeirosAcesso.Where(y => y.IdPedido != 0).Count();
                    resgate.QuantidadePedidosSucesso = quantidadePedidosSucesso != null ? quantidadePedidosSucesso : 0;
                    resgate.TipoArquivo = a.TipoArquivo.Nome;
                    resgate.StatusArquivo = a.StatusArquivo.Nome;
                    resgate.CSV = a.CaminhoCSV != null ? a.CaminhoCSV : "";
                    resgates.Add(resgate);
                }
            }

            return resgates;
        }

        #endregion

        public List<PontuacaoCsvModel> ObterArquivoCsv(int arquivoId)
        {
            var retorno = new List<PontuacaoCsvModel>();

            using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_ObterArquivoCSV", conn)
                {
                    CommandTimeout = 2400, // 40 minuitos
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@arquivoId", SqlDbType.Int).Value = arquivoId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.Add(new PontuacaoCsvModel()
                        {
                            DataInclusao = Convert.ToDateTime(reader["DATA_HORA"]),
                            NomeArquivo = reader["NOME_ARQUIVO_AVON"].ToString(),
                            AccountNumber = reader["AccountNumber"].ToString(),
                            TipoPrograma = reader["TipoPrograma"].ToString(),
                            CP = reader["CP"].ToString(),
                            TotalPontosRevendedor = Convert.ToDecimal(reader["Total_Pontos"]),
                            Indicante = reader["Indicante"].ToString(),
                            AccountNumberIndicada = reader["AccountNumber Indicada"].ToString(),
                            Indicada = reader["Indicada"].ToString(),
                            StatusPoints = reader["StatusPoints"].ToString(),
                        });
                    }

                    conn.Close();
                    cmd.Dispose();
                }
            }
            return retorno;
        }

        public List<NatalDetail> ObterArquivoCsvNatal(int arquivoId)
        {
            var retorno = new List<NatalDetail>();

            using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_NatalArquivoCsv", conn)
                {
                    CommandTimeout = 2400, // 40 minuitos
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@arquivoId", SqlDbType.Int).Value = arquivoId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.Add(new NatalDetail()
                        {
                            DataInclusao = Convert.ToDateTime(reader["DataInclusao"]),
                            RepresentativeAccountNumber = Convert.ToInt32(reader["RepresentativeAccountNumber"]),
                            RepresentativeName = reader["RepresentativeName"].ToString(),
                            RepresentativeLevel = reader["RepresentativeLevel"].ToString(),
                            TotalSaleCp20 = Convert.ToInt64(reader["TotalSaleCp20"]),
                            PaymentPendingCp20 = Convert.ToBoolean(reader["PaymentPendingCp20"]),
                            TotalSaleCp02 = Convert.ToInt64(reader["TotalSaleCp02"]),
                            PaymentPendingCp02 = Convert.ToBoolean(reader["PaymentPendingCp02"]),
                            TotalSaleCp03 = Convert.ToInt64(reader["TotalSaleCp03"]),
                            PaymentPendingCp03 = Convert.ToBoolean(reader["PaymentPendingCp03"]),
                            TotalSalePresents = Convert.ToInt64(reader["TotalSalePresents"]),
                            AwardLevel = Convert.ToInt32(reader["AwardLevel"]),
                            Award = reader["Award"].ToString()
                        });
                    }

                    conn.Close();
                    cmd.Dispose();
                }
            }

            return retorno;
        }

        public List<AliancaDetail> ObterArquivoCsvAlianca(int arquivoId)
        {
            try
            {

                var retorno = new List<AliancaDetail>();

                using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    var cmd = new SqlCommand("JP_PRC_AliancaArquivoCsv", conn)
                    {
                        CommandTimeout = 2400, // 40 minuitos
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("@arquivoId", SqlDbType.Int).Value = arquivoId;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno.Add(new AliancaDetail()
                            {
                                LoteId = Convert.ToInt32(reader["LoteId"]),
                                DataInclusao = Convert.ToDateTime(reader["DataInclusao"]),
                                EmpresariaAccountNumber = reader["EmpresariaAccountNumber"].ToString(),
                                MadrinhaAccountNumber = reader["MadrinhaAccountNumber"].ToString(),
                                MadrinhaNome = reader["MadrinhaNome"].ToString(),
                                RepresentativeAccountNumber = reader["RepresentativeAccountNumber"].ToString(),
                                RepresentativeName = reader["RepresentativeName"].ToString(),
                                CpPrimeiroPedido = Convert.ToInt32(reader["CpPrimeiroPedido"]),
                                AnoCpPrimeiroPedido = Convert.ToInt32(reader["AnoCpPrimeiroPedido"]),
                                PrimeiroPedidoPago = Convert.ToBoolean(reader["PrimeiroPedidoPago"]),
                                PontosConquistadosPrimeiroPedido = Convert.ToInt64(reader["PontosConquistadosPrimeiroPedido"]),
                                AtingiuVendas = Convert.ToBoolean(reader["AtingiuVendas"]),
                                PontosConquistados = Convert.ToInt64(reader["PontosConquistados"]),
                                PontosConquistadosEmpresaria = Convert.ToInt64(reader["PontosConquistadosEmpresaria"]),
                                PontosConquistadosEquipe = Convert.ToInt64(reader["PontosConquistadosEquipe"])
                            });
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                ex.Data.Add("ArquivoId", arquivoId);
                throw new ProcessamentoException("Não foi possível obter a lista do arquivo csv", ex);
            }
        }

        private static void GravarLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "ArquivoService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }

        public static bool CorrigeVisualizacaoExtratoFezJus()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_PRC_CorrigeVisualizacaoExtratoFezJus", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;


                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();

                    return true;
                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro na execução da JP_PRC_CorrigeVisualizacaoExtratoFezJus:", ex.Message, "GrupoLTM.Avon.MMA.Pontuacao.RetornoBaseToBase", "GrupoLTM.WebSmart.Services.CorrigeVisualizacaoExtratoFezJus", "jobCatalog");
                return false;
            }
        }

        public static void ApprovalByPassLive()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnectionProcessMktPlace()))
                {
                    SqlCommand cmd = new SqlCommand("JP_ApprovalByPassAccountHolder", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = ConfiguracaoService.ApprovalByPassProjectId();
                    cmd.Parameters.Add("@ConfigurationRequestId", SqlDbType.Int).Value = ConfiguracaoService.ApprovalByPassConfigurationRequestId();

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Dispose();

                }
            }
            catch (Exception ex)
            {
                GravarLogErro("Erro na execução do ApprovalByPassLive:", ex.Message, "GrupoLTM.Avon.MMA.Pontuacao.RetornoBaseToBase", "GrupoLTM.WebSmart.Services.ApprovalByPassLive", "jobCatalog");
            }
        }

        public static ArquivoCreditoLote CadastrarArquivoCreditoLote(ArquivoCreditoLote arquivo)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    //Grava o arquivo na tabela de Arquivos
                    IRepository repArquivo = context.CreateRepository<ArquivoCreditoLote>();
                    arquivo.DataInclusao = DateTime.Now;

                    if (arquivo.Id > 0)
                    {
                        arquivo.DataAlteracao = DateTime.Now;
                    }

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repArquivo.Create(arquivo);
                        repArquivo.SaveChanges();
                        scope.Complete();
                    }
                    return arquivo;

                }
            }
            catch (Exception e)
            {

                return new ArquivoCreditoLote();
                throw;
            }

        }

        public static List<ArquivoCreditoLote> ObterArquivoCreditoLotePorArquivoId(int arquivoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<ArquivoCreditoLote>();
                    var retornoList = new List<ArquivoCreditoLote>();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        retornoList = repArquivo.Filter<ArquivoCreditoLote>(x => x.ArquivoCreditoLotePaiId == arquivoId || x.ArquivoId == arquivoId).ToList();
                        scope.Complete();
                    }
                    return retornoList;
                }
            }
            catch (Exception e)
            {
                return new List<ArquivoCreditoLote>();
                throw;
            }

        }

        public List<ProgramaIncentivo> GetProgramaIncentivoPorArquivoId(int arquivoId, EnumDomain.TipoArquivo tipoArquivo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                return context.ProgramaIncentivoRepository().ObterPorArquivoId(arquivoId, tipoArquivo);
            }
        }

        public void AtualizarPontosConquistados(int arquivoCreditoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                context.ArquivoRepository().AtualizarPontosConquistados(arquivoCreditoId);
            }
        }

        public List<ExtratoPunch> ObterExtrato(int arquivoId, EnumDomain.TipoArquivo tipoArquivo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                return context.ArquivoRepository().ObterExtrato(arquivoId, tipoArquivo);
            }
        }

        public int ObterArquivoDeCreditoId(int arquivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                return context.ArquivoRepository().ObterArquivoDeCreditoId(arquivoId);
            }
        }

        public List<ImportacaoErro> ObterLogErroArquivo(int arquivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                return context.ArquivoRepository().ObterLogErroArquivo(arquivoId);
            }
        }

        public static ImportacaoErro CadastrarLogImportacaoErro(ImportacaoErro importacaoErro)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repository = context.CreateRepository<ImportacaoErro>();

                    repository.Create(importacaoErro);

                    return importacaoErro;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao cadastrar o log de importação erro", ex);
            }
        }

        public static void CadastrarErroImportacao(int loteId, int loteIdArquivo, string nomeArquivo, int? participanteId, int linhaArquivo, EnumDomain.TipoArquivo tipo)
        {
            var log = new ImportacaoErro
            {
                LoteId = loteId,
                TipoArquivoId = (int)tipo,
                TipoRegistro = "D",
                DescricaoErro = $"Pontuação ja processada no arquivo {nomeArquivo} - lote {loteIdArquivo}",
                DataInclusao = DateTime.Now,
                ParticipanteId = participanteId,
                LinhaArquivo = linhaArquivo,
            };

            CadastrarLogImportacaoErro(log);
        }
    }
}
