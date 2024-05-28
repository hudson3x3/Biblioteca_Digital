using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class ParticipantRepository
    {

        private AvonDbContext Context = new AvonDbContext();

        private IQueryable<Participante> GetParticipantQuery()
        {
            return (from P in Context.Participante
                    join PP in Context.ParticipantePerfil on P.Id equals PP.ParticipanteId
                    where PP.Ativo
                    select P);
        }

        public GrupoLTM.WebSmart.Domain.Models.Participante GetById(int participantId)
        {
            return (from P in GetParticipantQuery()
                    where P.Id == participantId
                    select P).AsNoTracking().FirstOrDefault();
        }

        public GrupoLTM.WebSmart.Domain.Models.Participante GetByLogin(string participantLogin)
        {
            return (from P in GetParticipantQuery()
                    where P.Login == participantLogin
                    select P).AsNoTracking().FirstOrDefault();
        }

        public DTO.ParticipanteDTO GetById(long participantId, long catalogId)
        {
            var sqlParams = new[] {
                new SqlParameter("p0", System.Data.SqlDbType.Int) { Value = (int)participantId },
                new SqlParameter("p1", System.Data.SqlDbType.BigInt) { Value = catalogId }
            };
            var proc = Context.Database.SqlQuery<DTO.ParticipanteDTO>("EXEC [JP_SEL_ParticipantePorId] @p0, @p1", sqlParams);
            
            return proc.FirstOrDefault();
        }

        public DTO.ParticipanteDTO GetByLogin(string participantLogin, long catalogId)
        {
            var sqlParams = new[] {
                new SqlParameter("p0", participantLogin),
                new SqlParameter("p1", System.Data.SqlDbType.BigInt) { Value = catalogId }
            };
            var proc = Context.Database.SqlQuery<DTO.ParticipanteDTO>("EXEC [JP_SEL_Participante] @p0, @p1", sqlParams);
            
            return proc.FirstOrDefault();
        }

        public DTO.ParticipanteDTO GetParticipanteCatalogoByLogin(string participantLogin, long catalogId)
        {
            var sqlParams = new[] {
                new SqlParameter("p0", participantLogin),
                new SqlParameter("p1", System.Data.SqlDbType.BigInt) { Value = catalogId }
            };
            var proc = Context.Database.SqlQuery<DTO.ParticipanteDTO>("EXEC [JP_SEL_Participante_Catalogo] @p0, @p1", sqlParams);
            return proc.FirstOrDefault(); 
        }

        public GrupoLTM.WebSmart.Domain.Models.Participante GetAllByLogin(string participantLogin)
        {
            return (from P in Context.Participante
                    where P.Login == participantLogin
                    select P).FirstOrDefault();
        }

        public Models.Participante GetByLogin(string participantLogin, string password)
        {
            int statusAtivo = (int)EnumDomain.StatusParticipante.Ativo;
            return (from P in Context.Participante
                    join PP in Context.ParticipantePerfil on P.Id equals PP.ParticipanteId
                    join PE in Context.ParticipanteEstrutura on P.Id equals PE.ParticipanteId
                    where
                        P.Login == participantLogin &&
                        P.Senha == password &&
                        P.StatusId == statusAtivo &&
                        PP.Ativo &&
                        PE.Ativo
                    select P).FirstOrDefault();
        }

        public Models.Estrutura GetStructure(string structureName)
        {
            return (from E in Context.Estrutura
                    where E.Nome == structureName.ToUpper()
                    select E).FirstOrDefault();
        }

        public int Update(Models.Participante participant)
        {
            Context.Entry<Models.Participante>(participant).State = System.Data.Entity.EntityState.Modified;
            return Context.SaveChanges();
        }

        public void Create(Models.Participante participant)
        {
            Context.Participante.Add(participant);
            Context.SaveChanges();
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
