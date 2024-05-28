namespace GrupoLTM.WebSmart.Domain.Repository
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork Create()
        {            
            return new AvonDbContext();
        }
    }
}
