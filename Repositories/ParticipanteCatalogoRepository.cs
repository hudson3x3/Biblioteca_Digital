using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System.Linq;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class ParticipanteCatalogoRepository
    {
        private AvonDbContext Context = new AvonDbContext();
        
        public Catalogo GetByCodigoMktPlace(int CodigoMktPlaceId)
        {
            return (from P in Context.ParticipanteCatalogo
                    join C in Context.Catalogo on P.CatalogoId equals C.Id
                    where P.CodigoMktPlace == CodigoMktPlaceId
                       && P.Ativo == true
                    select C).FirstOrDefault();
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
