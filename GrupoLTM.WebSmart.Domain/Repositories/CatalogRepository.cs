using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System.Linq;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class CatalogRepository
    {
        private AvonDbContext Context = new AvonDbContext();

        public Catalogo GetCatalog(long id)
        {
            return (from C in Context.Catalogo
                    where C.Codigo == id
                    select C).FirstOrDefault();
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
