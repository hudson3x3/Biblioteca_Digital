using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.Importacao;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class LogService : BaseService<LogRaiz>
    {
        public bool VerificarLogRaizPorToken(string token)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<LogRaiz>();
                var _catalogo = repCatalogo.Filter<LogRaiz>(x => x.TokenLtm == token).FirstOrDefault();

                if (_catalogo == null)
                    return false;

                return true;
            }
        }

        public bool GravarLogProcessamento(LogProcessamentoDb log)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repLog = context.CreateRepository<LogProcessamentoDb>();

                LogProcessamentoDb _logProcessamento = new LogProcessamentoDb
                {
                    Pagina = log.Pagina,
                    Metodo = log.Metodo,
                    Controller = log.Controller,
                    Evento = log.Evento,
                    Codigo = log.Codigo,
                    Mensagem = log.Mensagem,
                    Source = log.Source,
                    UsuarioSessao = log.UsuarioSessao,
                    DataInclusao = log.DataInclusao,
                    DadosEntrada = log.DadosEntrada,
                    DadosSaida = log.DadosSaida
                };

                using (TransactionScope scope = new TransactionScope())
                {
                    repLog.Create(_logProcessamento);
                    repLog.SaveChanges();
                    scope.Complete();
                }
            }
            return true;
        }

        public LogIntegracao GravarLogintegracao(LogIntegracao log)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repLog = context.CreateRepository<LogIntegracao>();

                using (TransactionScope scope = new TransactionScope())
                {
                    repLog.Create(log);
                    repLog.SaveChanges();
                    scope.Complete();
                }
            }

            return log;
        }

        public LogRaiz GravarLogRaiz(LogRaiz log)
        {
            //return;

            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repLog = context.CreateRepository<LogRaiz>();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repLog.Create(log);
                        repLog.SaveChanges();
                        scope.Complete();
                    }
                }

                return log;

            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public LogAcao GravarLogAcao(LogAcao log)
        {
            //return;

            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repLog = context.CreateRepository<LogAcao>();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repLog.Create(log);
                        repLog.SaveChanges();
                        scope.Complete();
                    }
                }

                return log;

            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public void GravarLog(LogModel log)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repLog = context.CreateRepository<Domain.Models.Log>();
                    var _log = new Domain.Models.Log
                    {
                        IdParticipante = log.IdParticipante,
                        LogDescricao = log.LogDescricao,
                        Ip = log.Ip,
                        Action = log.Action,
                        Controller = log.Controller,
                        TipoLog = log.TipoLog,
                        HostName = log.HostName,
                        Metodo = log.Metodo,
                        Browser = log.Browser,
                        Versao = log.Versao,
                        Pagina = log.Pagina,
                        Evento = log.Evento,
                        Acao = log.Acao,
                        Objeto = log.Objeto,
                        Teste = log.Teste,
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
                    };

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repLog.Create(_log);
                        repLog.SaveChanges();
                        scope.Complete();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LogDetalhado> ObterLogDetalhado(int arquivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<LogDetalhado>();
                var _logDetalhado = repCatalogo.Filter<LogDetalhado>(x => x.Lote.ArquivoId == arquivoId).ToList();

                if (_logDetalhado == null)
                    return null;

                var logsDetalhados = new List<LogDetalhado>();

                foreach (var logDetalhado in _logDetalhado)
                {
                    logsDetalhados.Add(new LogDetalhado()
                    {
                        Id = logDetalhado.Id,
                        DataInclusao = logDetalhado.DataInclusao,
                        Registro = logDetalhado.Registro,
                        TipoAcao = logDetalhado.TipoAcao,
                        Pontos = logDetalhado.Pontos,
                        StatusPontos = logDetalhado.StatusPontos,
                        TipoPrograma = logDetalhado.TipoPrograma,
                        Campanha = logDetalhado.Campanha,
                        NumeroLinha = logDetalhado.NumeroLinha
                    });
                }

                return logsDetalhados;
            }
        }

        public List<ImportacaoErro> ObterLogImportacaoErro(int arquivoId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repCatalogo = context.CreateRepository<ImportacaoErro>();
                var _logErro = repCatalogo.Filter<ImportacaoErro>(x => x.Lote.ArquivoId == arquivoId).ToList();

                if (_logErro == null)
                    return null;

                var logsErros = new List<ImportacaoErro>();

                foreach (var logErro in _logErro)
                {
                    logsErros.Add(new ImportacaoErro()
                    {
                        Id = logErro.Id,
                        DescricaoErro = logErro.DescricaoErro,
                        LinhaArquivo = logErro.LinhaArquivo,
                        LinhaConteudo = logErro.LinhaConteudo,
                        Lote = logErro.Lote
                    });
                }

                return logsErros;
            }
        }

        public void GerarArquivoLogDetalhado(int arquivoId)
        {
            int lote = 2000;

            var nomeArquivo = string.Format("LogErro/LOG_DETALHADO_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string linha = string.Empty;
            int cntLote = 0;

            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    foreach (var item in new LogService().ObterLogDetalhado(arquivoId))
                    {
                        cntLote++;
                        linha += string.Format("{0} - {1} (REGISTRO) {2} {3} pontos {4} programa de indicação {5}{6} Linha {7}", item.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss"), item.Registro, item.TipoAcao.ToLower(), item.Pontos, item.StatusPontos.ToLower(), item.Campanha, item.NumeroLinha, Environment.NewLine);

                        if (cntLote == lote)
                        {
                            sw.Write(linha);
                            linha = string.Empty;
                            cntLote = 0;
                        }
                    }

                    //grava restante do lote
                    if (cntLote > 0)
                        sw.Write(linha);

                    sw.Flush();

                    //Salva no BLOB
                    UploadFile.Upload(ms, nomeArquivo);
                }

                linha = string.Empty;
                cntLote = 0;
            }

            ArquivoService.AtualizaArquivo(arquivoId, nomeArquivo, null);
        }

        public void GerarArquivoLogRejeitado(int arquivoId)
        {
            int lote = 20000;

            var nomeArquivo = string.Format("LogErro/LOG_LINHA_REJEITADA_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string linha = string.Empty;
            int cntLote = 0;
            bool existeErros = false;

            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    foreach (var item in new LogService().ObterLogImportacaoErro(arquivoId))
                    {
                        cntLote++;
                        linha += string.Format("{0};{1};{2};{3};{4};{5}", item.Lote.Arquivo.Nome, item.Lote.Arquivo.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss"), item.LinhaArquivo, item.ParticipanteId, item.DescricaoErro, Environment.NewLine);

                        if (cntLote == lote)
                        {
                            sw.Write(linha);
                            linha = string.Empty;
                            cntLote = 0;
                        }
                    }

                    if (ms.Length > 0)
                        existeErros = true;

                    //grava restante do lote
                    if (cntLote > 0)
                        sw.Write(linha);

                    sw.Flush();

                    if (existeErros)
                    {
                        //Salva no BLOB
                        UploadFile.Upload(ms, nomeArquivo);
                    }
                }

                linha = string.Empty;
                cntLote = 0;

                if (existeErros)
                    ArquivoService.AtualizaArquivo(arquivoId, null, nomeArquivo);
            }
        }
    }
}