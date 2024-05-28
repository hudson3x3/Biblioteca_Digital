namespace GrupoLTM.WebSmart.Domain.Repository.Live
{
    public class UnitOfWorkFactoryLive
    {
        public static IUnitOfWorkProcess Create()
        {
            return new AvonProcessDBContext();
        }
    }
}
