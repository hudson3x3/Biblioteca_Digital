using System;

namespace GrupoLTM.WebSmart.Domain.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<TEntity> CreateRepository<TEntity>();
        ConteudoRepository ConteudoRepository();
        QuestionarioRepository QuestionarioRepository();
        ArquivoRepository ArquivoRepository();
        ProgramaIncentivoRepository ProgramaIncentivoRepository();
    }
}
