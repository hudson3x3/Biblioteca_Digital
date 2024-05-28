using System;
using System.Data;
using System.Linq;
using GrupoLTM.WebSmart.DTO;
using System.Data.SqlClient;
using System.Collections.Generic;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Domain.Models.Importacao;
using GrupoLTM.Avon.MMA.Estornos.SmsEstorno.Models;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Services
{
    public class EstornoService
    {
        public static Estorno BuscarPorNome(string nome)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<Estorno>();

                    var estorno = rep.Filter<Estorno>(x => x.Nome.ToLower() == nome.ToLower()).IncludeEntity(x => x.Lotes).FirstOrDefault();

                    return estorno;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível buscar o arquivo de estorno por nome", ex);
            }
        }

        public List<EstornoModel> ListarEstornos(string nome, int start, int regExibir, out int total, params EstornoStatus[] statusFiltro)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repEstorno = context.CreateRepository<Estorno>();
                    var repAprovacao = context.CreateRepository<LogAprovacaoArquivo>();

                    var query = repEstorno.Filter<Estorno>(x => x.Ativo && statusFiltro.Contains(x.Status));

                    if (!string.IsNullOrEmpty(nome))
                        query = query.Where(x => x.Nome.Contains(nome));

                    query = query.OrderByDescending(x => x.Id);

                    total = query.Count();

                    var estornos = query.Skip(start).Take(regExibir).ToList();

                    var estornosAprovadosIds = estornos.Where(x => x.Status == EstornoStatus.Aprovado || x.Status >= EstornoStatus.EmProcessoLive).Select(x => x.Id).ToList();

                    var aprovados = new List<LogAprovacaoArquivo>();

                    if (estornosAprovadosIds.Any())
                        aprovados = repAprovacao.Filter<LogAprovacaoArquivo>(x => estornosAprovadosIds.Contains(x.ArquivoId)).ToList();

                    return MapModel(estornos, aprovados);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível consultar a lista de estornos", ex);
            }
        }

        private List<EstornoModel> MapModel(List<Estorno> estornos, List<LogAprovacaoArquivo> aprovados)
        {
            var model = new List<EstornoModel>();

            foreach (var estorno in estornos)
            {
                var estornoModel = new EstornoModel
                {
                    EstornoId = estorno.Id,
                    Nome = estorno.Nome,
                    TotalPedido = estorno.TotalPedido,
                    TotalPontos = estorno.TotalPontos,
                    Processado = estorno.Status == EstornoStatus.Processado,
                    Inconsistente = estorno.Status == EstornoStatus.Inconsistente,
                    Status = StringHelper.GetEnumDescription(estorno.Status),
                    DataInclusao = estorno.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss"),
                    Csv = string.Format("{0}{1}{2}", Settings.Caminho.StorageLogPath, estorno.CaminhoCsv, Settings.Caminho.StorageLogToken)
                };

                var aprovado = aprovados.FirstOrDefault(x => x.ArquivoId == estorno.Id);

                if (aprovado != null)
                {
                    estornoModel.DataAprovacao = aprovado.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss");
                    estornoModel.LoginAprovacao = aprovado.Login;
                }

                model.Add(estornoModel);
            }

            return model;
        }

        public static Estorno Cadastrar(string nome)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<Estorno>();

                    var estorno = new Estorno
                    {
                        Nome = nome,
                        Status = EstornoStatus.EmProcesso,
                        DataInclusao = DateTime.Now,
                        Ativo = true
                    };

                    rep.Create(estorno);

                    return estorno;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível cadastrar estorno", ex);
            }
        }

        public void CadastrarResponseRequisicao(List<EstornoRequisicao> requests)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    context.CreateRepository<EstornoRequisicao>().AddRange(requests).Wait();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi cadastrar o response da requisição de estorno", ex);
            }
        }

        public void CadastrarStatusSms(List<ItemCallBackWebHook> itens)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repCall = context.CreateRepository<ItemCallBackWebHook>();

                    repCall.AddRange(itens).Wait();

                    repCall.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi cadastrar os status de sms de estorno enviados", ex);
            }
        }

        public void CadastrarEstornoSms(List<EstornoSms> estornoSms)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                    context.CreateRepository<EstornoSms>().AddRange(estornoSms).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi cadastrar os status de sms de estorno enviados", ex);
            }
        }

        public void CadastrarEstornoEmail(List<EstornoEmail> estornoEmail)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                    context.CreateRepository<EstornoEmail>().AddRange(estornoEmail).Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi cadastrar os registros de estorno email", ex);
            }
        }

        public static EstornoLote CadastrarLote(int estornoId, int linhaInicio, int linhaFim)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repLote = context.CreateRepository<EstornoLote>();

                    var lote = new EstornoLote
                    {
                        EstornoId = estornoId,
                        DataInclusao = DateTime.Now,
                        LinhaInicio = linhaInicio,
                        LinhaFim = linhaFim,
                        Ativo = true
                    };

                    repLote.Create(lote);

                    return lote;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível cadastrar o lote para o estorno id: " + estornoId, ex);
            }
        }

        public static void CadastrarLogImportacaoErroEstorno(EstornoImportacaoErro estornoImportacaoErro)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repository = context.CreateRepository<EstornoImportacaoErro>();

                    repository.Create(estornoImportacaoErro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao cadastrar o log de importação erro", ex);
            }
        }

        public static void LogArquivoEmBranco(int estornoId)
        {
            var lote = LoteService.ConsultarLotesEstorno(estornoId).FirstOrDefault();

            if (lote == null)
                lote = LoteService.CadastrarLoteEstorno(estornoId, 0, 0);

            var log = new EstornoImportacaoErro
            {
                EstornoLoteId = lote.Id,
                DescricaoErro = "Arquivo em branco",
                DataInclusao = DateTime.Now,
            };

            CadastrarLogImportacaoErroEstorno(log);
        }

        public List<EstornoPedidoCsv> ObterPedidosCsv(int estornoId)
        {
            var lista = new List<EstornoPedidoCsv>();

            using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_EstornoPedidosCSV", conn)
                {
                    CommandTimeout = 2400, // 40 minuitos
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@EstornoId", SqlDbType.Int).Value = estornoId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pedido = new EstornoPedidoCsv()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            EstornoLoteId = Convert.ToInt32(reader["EstornoLoteId"]),
                            PedidoId = Convert.ToInt64(reader["PedidoId"]),
                            PedidoPaiId = Convert.ToInt64(reader["PedidoPaiId"]),
                            CodigoProduto = reader["CodigoProduto"].ToString(),
                            NomeProduto = reader["NomeProduto"].ToString(),
                            Quantidade = Convert.ToInt32(reader["Quantidade"]),
                            Motivo = Convert.ToInt32(reader["Motivo"]).ToEnum<EstornoMotivo>(),
                            Parcial = Convert.ToBoolean(reader["Parcial"]),
                            TotalPontos = Convert.ToInt64(reader["TotalPontos"]),
                            AccountNumber = reader["AccountNumber"].ToString(),
                            ErroRequisicao = reader["ErroRequisicao"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["ErroRequisicao"]),
                            MensagemRequisicao = reader["MensagemRequisicao"] == DBNull.Value ? null : reader["MensagemRequisicao"].ToString(),
                            ErroSms = reader["ErroSms"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["ErroSms"]),
                            Destinatario = reader["Destinatario"] == DBNull.Value ? null : reader["Destinatario"].ToString(),
                            Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                            ParticipanteNome = reader["ParticipanteNome"] == DBNull.Value ? null : reader["ParticipanteNome"].ToString(),
                            MensagemEmail = reader["MensagemEmail"] == DBNull.Value ? null : reader["MensagemEmail"].ToString(),
                            ErroEmail = reader["ErroEmail"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["ErroEmail"]),
                        };

                        if (reader["SendStatusSms"] != DBNull.Value)
                        {
                            var statusNumber = Convert.ToInt32(reader["SendStatusSms"]);
                            pedido.SendStatusSms = statusNumber.ToEnum<SendStatusMovile>();
                        }

                        if (reader["DeliveredStatusSms"] != DBNull.Value)
                        {
                            var statusNumber = Convert.ToInt32(reader["DeliveredStatusSms"]);
                            pedido.DeliveredStatusSms = statusNumber.ToEnum<DeliveredStatusMovile>();
                        }

                        lista.Add(pedido);
                    }

                    conn.Close();
                    cmd.Dispose();
                }
            }

            return lista;
        }

        public List<EstornoImportacaoErro> ObterPedidosCsvErro(int estornoId)
        {
            var lista = new List<EstornoImportacaoErro>();

            using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_EstornoPedidosErro", conn)
                {
                    CommandTimeout = 2400, // 40 minuitos
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@EstornoId", SqlDbType.Int).Value = estornoId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new EstornoImportacaoErro()
                        {
                            PedidoId = Convert.ToInt32(reader["PedidoId"]),
                            CodigoProduto = reader["CodigoProduto"].ToString(),
                            EstornoLoteId = Convert.ToInt32(reader["EstornoLoteId"]),
                            Linha = Convert.ToInt32(reader["Linha"]),
                            LinhaConteudo = reader["LinhaConteudo"].ToString(),
                            DescricaoErro = reader["DescricaoErro"].ToString(),
                            DataInclusao = Convert.ToDateTime(reader["DataInclusao"])
                        });
                    }

                    conn.Close();
                    cmd.Dispose();
                }
            }

            return lista;
        }

        public static void AtualizaArquivoCsvGerado(int estornoId, string caminhoLog)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<Estorno>();
                    var estorno = rep.Find<Estorno>(estornoId);

                    estorno.CaminhoCsv = caminhoLog;
                    estorno.DataAlteracao = DateTime.Now;

                    rep.Update(estorno);
                    rep.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AtualizarStatusEstorno(int estornoId, EstornoStatus status, EnvioSmsStatus? envioSmsStatus = null, EnvioEmailStatus? envioEmailStatus = null)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<Estorno>();

                    var estorno = rep.Find<Estorno>(estornoId);

                    estorno.Status = status;
                    estorno.DataAlteracao = DateTime.Now;

                    if (envioSmsStatus.HasValue)
                        estorno.EnvioSmsStatus = envioSmsStatus.Value;

                    if (envioEmailStatus.HasValue)
                        estorno.EnvioEmailStatus = envioEmailStatus.Value;

                    rep.Update(estorno);
                    rep.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AtualizarLoteProcessado(int estornoLoteId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<EstornoLote>();

                    var estorno = rep.Find<EstornoLote>(estornoLoteId);

                    estorno.Processado = true;
                    estorno.DataAlteracao = DateTime.Now;

                    rep.Update(estorno);
                    rep.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AtualizarLoteErro(int estornoLoteId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<EstornoLote>();

                    var estorno = rep.Find<EstornoLote>(estornoLoteId);

                    estorno.Erro = true;
                    estorno.Processado = true;
                    estorno.DataAlteracao = DateTime.Now;

                    rep.Update(estorno);
                    rep.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static bool VerificarEstornoLotesErro(int estornoId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<EstornoLote>();
                    return rep.Filter<EstornoLote>(x => x.EstornoId == estornoId && x.Erro).Any();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível verificar os lotes do arquivo id: " + estornoId, ex);
            }
        }

        public Estorno ObterEstornoPorStatus(params EstornoStatus[] status)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repEstorno = context.CreateRepository<Estorno>();

                    return repEstorno.Filter<Estorno>(x => status.Contains(x.Status)).IncludeEntity(x => x.Lotes).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                var statusDescription = string.Join(", ", status.Select(x => StringHelper.GetEnumDescription(x)));
                throw new Exception("Não foi possível consultar a lista de estornos com status: " + statusDescription, ex);
            }
        }

        public Estorno ObterEstornoPendenteCominucacao()
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repEstorno = context.CreateRepository<Estorno>();

                    return repEstorno.Filter<Estorno>(x => (x.EnvioEmailStatus == EnvioEmailStatus.PendenteEnvio || x.EnvioSmsStatus == EnvioSmsStatus.PendenteEnvio)
                                                        && (x.Status == EstornoStatus.EstornoRealizadoSucesso || x.Status == EstornoStatus.EstornoRealizadoErro))
                                                           .IncludeEntity(x => x.Lotes).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível consultar a lista de estornos para envio da comunicação", ex);
            }
        }

        public List<EstornoPedido> ObterPedidosEstorno(int estornoLoteId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repEstorno = context.CreateRepository<EstornoPedido>();

                    return repEstorno.Filter<EstornoPedido>(x => x.EstornoLoteId == estornoLoteId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível consultar a lista de pedidos do estorno para o lote id " + estornoLoteId, ex);
            }
        }

        public List<EstornoPedidoComunicacao> ObterPedidosEnvioComunicacao(int estornoLoteId)
        {
            try
            {
                var lista = new List<EstornoPedidoComunicacao>();

                using (var conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    var cmd = new SqlCommand("JP_PRC_EstornoPedidoComunicacao", conn)
                    {
                        CommandTimeout = 2400, // 40 minuitos
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add("@EstornoLoteId", SqlDbType.Int).Value = estornoLoteId;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pedido = new EstornoPedidoComunicacao()
                            {
                                EstornoPedidoId = Convert.ToInt32(reader["EstornoPedidoId"]),
                                PedidoId = Convert.ToInt64(reader["PedidoId"]),
                                PedidoPaiId = Convert.ToInt64(reader["PedidoPaiId"]),
                                NomeProduto = reader["NomeProduto"].ToString(),
                                Celular = reader["Celular"].ToString(),
                                DDD = reader["DDD"].ToString(),
                                Email = reader["Email"].ToString(),
                                ParticipanteNome = reader["ParticipanteNome"].ToString(),
                                SmsErro = reader["SmsErro"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["SmsErro"]),
                                EmailErro = reader["EmailErro"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["EmailErro"]),
                            };

                            if (reader["Motivo"] != DBNull.Value)
                            {
                                var statusNumber = Convert.ToInt32(reader["Motivo"]);
                                pedido.Motivo = statusNumber.ToEnum<EstornoMotivo>();
                            }

                            if (reader["EnvioSmsStatus"] != DBNull.Value)
                            {
                                var statusNumber = Convert.ToInt32(reader["EnvioSmsStatus"]);
                                pedido.EnvioSmsStatus = statusNumber.ToEnum<EnvioSmsStatus>();
                            }

                            if (reader["EnvioEmailStatus"] != DBNull.Value)
                            {
                                var statusNumber = Convert.ToInt32(reader["EnvioEmailStatus"]);
                                pedido.EnvioEmailStatus = statusNumber.ToEnum<EnvioEmailStatus>();
                            }

                            lista.Add(pedido);
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível obter a lista de pedidos para envio de sms", ex);
            }
        }
    }
}
