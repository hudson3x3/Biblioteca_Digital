using GrupoLTM.WebSmart.Domain.DTO;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Models.Importacao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Repository
{
    public class ArquivoRepository : Repository<Arquivo>
    {
        private readonly DbContext Context;

        public ArquivoRepository(DbContext context) : base (context)
        {
            Context = context;
        }

        public Arquivo ObterArquivoPorNome(string nome, EnumDomain.TipoArquivo tipoArquivo)
        {
            return Context.Set<Arquivo>().Include(x => x.TipoArquivo).Include(x => x.CatalogosCP).FirstOrDefault(x => x.Nome == nome && x.TipoArquivoId == (int)tipoArquivo);
        }

        public List<ImportacaoErro> ObterLogErroArquivo(int arquivoId)
        {
            Context.Database.CommandTimeout = 300;

            var loteId = Context.Set<Lote>().First(x => x.ArquivoId == arquivoId).Id;

            var erros = Context.Set<ImportacaoErro>().Where(x => x.LoteId == loteId).ToList();

            var importacao = Context.Set<IndicacaoImportacao>().Where(x => x.LoteId == loteId && x.Erro != null).ToList()
                .Select(x => new ImportacaoErro
                {
                    IdOrigemNormalizada = x.Id,
                    DataInclusao = x.DataInclusao,
                    LoteId = x.LoteId,
                    DescricaoErro = x.Erro,
                    LinhaArquivo = x.NumeroLinha,
                    LinhaConteudo = x.LinhaConteudo,
                    TipoArquivoId = (int)EnumDomain.TipoArquivo.Indicacao,
                });

            var result = erros.Union(importacao);

            return result.ToList();
        }

        public void AtualizarPontosConquistados(int arquivoCreditoId)
        {
            Context.Database.CommandTimeout = 600;
            Context.Database.ExecuteSqlCommand("EXEC JP_PRC_ProcessarRetornoPontos " + arquivoCreditoId);
        }

        public List<ExtratoPunch> ObterExtrato(int arquivoId, EnumDomain.TipoArquivo tipoArquivo)
        {
            var extrato = new List<ExtratoPunch>();

            switch (tipoArquivo)
            {
                case EnumDomain.TipoArquivo.Indicacao:
                    var extratoIndicacao = ObterExtratoIndicacao(arquivoId);
                    extrato.AddRange(extratoIndicacao);
                    break;

                case EnumDomain.TipoArquivo.Apoio:
                    var extratoApoio = ObterExtratoApoio(arquivoId);
                    extrato.AddRange(extratoApoio);
                    break;

                case EnumDomain.TipoArquivo.Consecutividade:
                    var extratoConsecutividade = ObterExtratoConsecutividade(arquivoId);
                    extrato.AddRange(extratoConsecutividade);
                    break;

                case EnumDomain.TipoArquivo.ClubeDasEstrelas:
                    var extratoClube = ObterExtratoClube(arquivoId);
                    extrato.AddRange(extratoClube);
                    break;

                case EnumDomain.TipoArquivo.Migracao:
                    var extratoMigracao = ObterExtratoMigracao(arquivoId);
                    extrato.AddRange(extratoMigracao);
                    break;
            }

            return extrato;
        }

        public List<ExtratoPunch> ObterExtratoApoio(int arquivoId)
        {
            var query = from arq in Context.Set<Arquivo>()
                        join lote in Context.Set<Lote>() on arq.Id equals lote.ArquivoId
                        join detail in Context.Set<ApoioDetail>() on lote.Id equals detail.LoteId
                        where arq.Id == arquivoId
                        select detail;

            var conquistados = query.Where(x => x.StatusPoints == "S").Take(1000).ToList();
            var pendentes = query.Where(x => x.StatusPoints == "N").Take(1000).ToList();
            var cancelados = query.Where(x => x.StatusPoints == "C").Take(1000).ToList();

            var union = conquistados.Union(pendentes).Union(cancelados);

            var resultado = union.Select(x => new ExtratoPunch
            {
                Points = x.Points,
                StatusPoints = x.StatusPoints,
                AccountNumber = x.AccountNumberDetail,
                IncentiveProgram = x.IncentiveProgramDescriptionDetail,
                TipoArquivo = EnumDomain.TipoArquivo.Apoio
            });

            return resultado.ToList();
        }

        public List<ExtratoPunch> ObterExtratoIndicacao(int arquivoId)
        {
            var query = from arq in Context.Set<Arquivo>()
                        join lote in Context.Set<Lote>() on arq.Id equals lote.ArquivoId
                        join header in Context.Set<IndicacaoHeader>() on lote.Id equals header.LoteId
                        where arq.Id == arquivoId
                        select header;

            var conquistados = query.Where(x => x.FezJusIndicatorHeader == "S").Take(1000).ToList();
            var pendentes = query.Where(x => x.FezJusIndicatorHeader == "N").Take(1000).ToList();
            var cancelados = query.Where(x => x.FezJusIndicatorHeader == "C").Take(1000).ToList();

            var union = conquistados.Union(pendentes).Union(cancelados);

            var resultado = union.Select(x => new ExtratoPunch
            {
                Points = x.TotalPointsAmountHeader,
                StatusPoints = x.FezJusIndicatorHeader,
                AccountNumber = x.ReferralRepresentativeNumberHeader,
                IncentiveProgram = x.IncentiveProgramDescriptionHeader,
                TipoArquivo = EnumDomain.TipoArquivo.Indicacao
            });

            return resultado.ToList();
        }

        public List<ExtratoPunch> ObterExtratoConsecutividade(int arquivoId)
        {
            var query = from arq in Context.Set<Arquivo>()
                        join lote in Context.Set<Lote>() on arq.Id equals lote.ArquivoId
                        join header in Context.Set<ConsecutividadeHeader>() on lote.Id equals header.LoteId
                        where arq.Id == arquivoId
                        select header;

            var conquistados = query.Where(x => x.StatusPoints == "S").Take(1000).ToList();
            var pendentes = query.Where(x => x.StatusPoints == "N").Take(1000).ToList();
            var cancelados = query.Where(x => x.StatusPoints == "C").Take(1000).ToList();

            var union = conquistados.Union(pendentes).Union(cancelados);

            var resultado = union.Select(x => new ExtratoPunch
            {
                Points = x.Points,
                StatusPoints = x.StatusPoints,
                AccountNumber = x.RepresentativeNumberHeader,
                IncentiveProgram = x.ProgramDescriptionHeader,
                TipoArquivo = EnumDomain.TipoArquivo.Consecutividade
            });

            return resultado.ToList();
        }

        public List<ExtratoPunch> ObterExtratoClube(int arquivoId)
        {
            Context.Database.CommandTimeout = 300;

            var query = from extrato1 in Context.Set<ClubeEstrelasRegister1Extrato>()
                        join arquivo in Context.Set<ArquivoClubeEstrelasRegister>() on extrato1.ArquivoClubeEstrelasRegisterId equals arquivo.Id
                        join extrato2 in Context.Set<ClubeEstrelasRegister2Extrato>() on extrato1.Id equals extrato2.ClubeEstrelasRegister1ExtratoId
                        where arquivo.ArquivoId == arquivoId
                        select new { extrato1, extrato2 };

            var conquistados = query.Where(x => x.extrato1.FezJusIndicator == "S").Take(1000).ToList();
            var pendentes = query.Where(x => x.extrato1.FezJusIndicator == "N").Take(1000).ToList();
            var cancelados = query.Where(x => x.extrato1.FezJusIndicator == "C").Take(1000).ToList();

            var union = conquistados.Union(pendentes).Union(cancelados);

            var resultado = union.Select(x => new ExtratoPunch
            {
                AvailablePoints = x.extrato1.TotalAvailablePoints,
                CanceledPoints = x.extrato1.TotalCanceledPoints,
                PendingPoints = x.extrato1.TotalPendingPoints,
                StatusPoints = x.extrato2.FezJusIndicator,
                AccountNumber = x.extrato1.RepresentativeAccountNumber.ToString(),
                IncentiveProgram = x.extrato1.ProgramName,
                CampaignYear = x.extrato1.CampaignYear,
                CampaignNumber = x.extrato1.CampaignNumber,
                TipoArquivo = EnumDomain.TipoArquivo.ClubeDasEstrelas,
                OrderId = x.extrato2.OrderID ?? 0
            });

            return resultado.ToList();
        }

        public List<ExtratoPunch> ObterExtratoMigracao(int arquivoId)
        {
            Context.Database.CommandTimeout = 300;

            var query = from extrato1 in Context.Set<MigracaoRegister1Extrato>()
                        join register1 in Context.Set<MigracaoRegister1>() on extrato1.MigracaoRegister1Id equals register1.Id
                        where register1.ArquivoId == arquivoId
                        select extrato1;

            var conquistados = query.Where(x => x.TotalAvailablePoints > 0).ToList();
            var pendentes = query.Where(x => x.TotalPendingPoints > 0).ToList();
            var cancelados = query.Where(x => x.TotalCanceledPoints > 0).ToList();

            conquistados.ForEach(x => x.Status = "S");
            cancelados.ForEach(x => x.Status = "C");
            pendentes.ForEach(x => x.Status = "N");

            var resultado = new List<ExtratoPunch>();

            resultado.AddRange(conquistados.Select(x => MigracaoToExtratoPunch(x)));
            resultado.AddRange(cancelados.Select(x => MigracaoToExtratoPunch(x)));
            resultado.AddRange(pendentes.Select(x => MigracaoToExtratoPunch(x)));

            return resultado.ToList();
        }

        private ExtratoPunch MigracaoToExtratoPunch(MigracaoRegister1Extrato extrato)
        {
            return new ExtratoPunch
            {
                AvailablePoints = extrato.TotalAvailablePoints,
                CanceledPoints = extrato.TotalCanceledPoints,
                PendingPoints = extrato.TotalPendingPoints,
                AccountNumber = extrato.RepresentativeAccountNumber.ToString(),
                IncentiveProgram = extrato.ProgramName,
                CampaignYear = extrato.CampaignYear,
                CampaignNumber = extrato.CampaignNumber,
                EndCampaignYear = extrato.EndCampaignYear,
                EndCampaignNumber = extrato.EndCampaignNumber,
                StatusPoints = extrato.Status,
                TipoArquivo = EnumDomain.TipoArquivo.Migracao,
            };
        }

        public int ObterArquivoDeCreditoId(int arquivoId)
        {
            var query = from pontuacao in Context.Set<Pontuacao>()
                        where pontuacao.ArquivoIdOrigem == arquivoId
                        select pontuacao;

            if (query.All(x => x.ArquivoId.HasValue && x.ArquivoId > 0))
                return query.First(x => x.ArquivoIdOrigem == arquivoId).ArquivoId.Value;
            else
                throw new ApplicationException("O arquivo de crédito não foi gerado");
        }
    }
}
