using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.Importacao;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoLTM.WebSmart.Services
{
    public class LoteService
    {
        public static Lote CadastrarLote(int arquivoId, int linhaInicio, int linhaFim)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    var repLote = context.CreateRepository<Lote>();

                    var lote = new Lote
                    {
                        ArquivoId = arquivoId,
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
                throw new Exception("Não foi possível cadastrar o lote para o arquivo id: " + arquivoId, ex);
            }
        }

        public static EstornoLote CadastrarLoteEstorno(int estornoId, int linhaInicio, int linhaFim)
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

        public static bool InativarLote(int loteId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repArquivo = context.CreateRepository<Lote>();
                    var lote = repArquivo.Find<Lote>(loteId);
                    if (lote != null)
                    {
                        lote.Ativo = false;
                        lote.DataAlteracao = DateTime.Now;
                    }

                    repArquivo.Update<Lote>(lote);
                    repArquivo.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool LoteProcessado(int loteId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var repositorio = context.CreateRepository<Lote>();

                    var lote = repositorio.Find<Lote>(loteId);

                    if (lote != null)
                    {
                        lote.Processado = true;
                        lote.DataAlteracao = DateTime.Now;
                    }

                    repositorio.Update(lote);

                    repositorio.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                gravaLogErro("Erro na atualização do status dos lote", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("LoteProcessado(Loteid:{0})", loteId.ToString()), "jobCatalog");
                throw ex;
            }
        }

        public static int LoteTipoArquivo(int loteId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository rep = context.CreateRepository<Arquivo>();

                    return rep.Filter<Arquivo>(x => (x.Lotes.Any(y => y.Id == loteId)))
                        .FirstOrDefault().TipoArquivoId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Lote> ConsultarLotes(int arquivoId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<Lote>();

                    return rep.Filter<Lote>(x => x.ArquivoId == arquivoId && x.Ativo).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<EstornoLote> ConsultarLotesEstorno(int estornoId)
        {
            try
            {
                using (var context = UnitOfWorkFactory.Create())
                {
                    var rep = context.CreateRepository<EstornoLote>();

                    return rep.Filter<EstornoLote>(x => x.EstornoId == estornoId && x.Ativo).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível consultar o estorno lote id: " + estornoId, ex);
            }
        }

        private static void gravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "LoteService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }
    }
}
