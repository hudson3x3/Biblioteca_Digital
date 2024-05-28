using GrupoLTM.WebSmart.Domain.Repository;
using System.Linq;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class ParticipanteCPRepository
    {
        private AvonDbContext Context = new AvonDbContext();

        public bool CheckParticipanteCP(int participanteId, string catalogoCP)
        {            
            return (from P in Context.ParticipanteCP
                    join C in Context.CatalogoCP on P.CatalogoCPId equals C.Id
                    where P.ParticipanteId == participanteId && C.CP == catalogoCP
                    select C).Any();
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
