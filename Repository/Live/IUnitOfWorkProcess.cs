using System;

namespace GrupoLTM.WebSmart.Domain.Repository.Live
{
    public interface IUnitOfWorkProcess : IDisposable
    {
        RepositoryLive<TEntity> CreateRepository<TEntity>();
        ConteudoRepositoryLive ConteudoRepository();
    }
}
