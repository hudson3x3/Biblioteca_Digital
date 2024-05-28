using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GrupoLTM.WebSmart.Domain.Repository
{
    public class ProgramaIncentivoRepository : Repository<ProgramaIncentivo>
    {
        private readonly DbContext context;
        private readonly DbSet<ProgramaIncentivo> dbSet;

        public ProgramaIncentivoRepository(DbContext _context) : base (_context)
        {
            context = _context;
            dbSet = context.Set<ProgramaIncentivo>();
        }

        public List<ProgramaIncentivo> ObterPorArquivoId(int arquivoId, EnumDomain.TipoArquivo tipoArquivo)
        {
            var query = from arq in context.Set<Arquivo>()
                        join lote in context.Set<Lote>() on arq.Id equals lote.ArquivoId
                        where arq.Id == arquivoId
                        select lote;

            IQueryable<ProgramaIncentivo> incentivo = null;

            switch (tipoArquivo)
            {
                case EnumDomain.TipoArquivo.Indicacao:
                    incentivo = from lote in query
                                join import in context.Set<IndicacaoHeader>() on lote.Id equals import.LoteId
                                join incent in context.Set<ProgramaIncentivo>() on import.IncentiveProgramDescriptionHeader equals incent.Nome
                                select incent;
                    break;

                case EnumDomain.TipoArquivo.Apoio:
                    incentivo = from lote in query
                                join import in context.Set<ApoioDetail>() on lote.Id equals import.LoteId
                                join incent in context.Set<ProgramaIncentivo>() on import.IncentiveProgramDescriptionDetail equals incent.Nome
                                select incent;
                    break;

                case EnumDomain.TipoArquivo.Consecutividade:
                    incentivo = from lote in query
                                join import in context.Set<ConsecutividadeHeader>() on lote.Id equals import.LoteId
                                join incent in context.Set<ProgramaIncentivo>() on import.ProgramDescriptionHeader equals incent.Nome
                                select incent;
                    break;

                case EnumDomain.TipoArquivo.ClubeDasEstrelas:
                    incentivo = from lote in query
                                join import in context.Set<ArquivoClubeEstrelasRegister>() on lote.Id equals import.LoteId
                                join incent in context.Set<ProgramaIncentivo>() on import.ProgramName equals incent.Nome
                                select incent;
                    break;

                case EnumDomain.TipoArquivo.Migracao:
                    incentivo = from lote in query
                                join import in context.Set<MigracaoRegister1>() on lote.Id equals import.LoteId
                                join incent in context.Set<ProgramaIncentivo>() on import.ProgramName equals incent.Nome
                                select incent;
                    break;
            }

            return incentivo.Distinct().ToList();
        }

        public void AtualizarIdOrigem(List<ProgramaIncentivo> programaIncentivos)
        {
            var ids = programaIncentivos.Select(x => x.Id).ToList();

            var programasDb = dbSet.Where(x => ids.Any(y => y == x.Id)).ToList();

            foreach (var item in programaIncentivos)            
                programasDb.First(x => x.Id == item.Id).IdOrigem = item.IdOrigem;

            context.SaveChanges();
        }
    }
}
