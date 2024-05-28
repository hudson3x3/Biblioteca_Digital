using GrupoLTM.WebSmart.Domain.Models.Live;
using GrupoLTM.WebSmart.Domain.Repository.Live;
using System;
using System.Linq;

namespace GrupoLTM.WebSmart.Services.LiveServiceMkt
{
    public  class TransactionTempService
    {
        public static TransactionTemp ObterArquivoPorNomeTransactionTemp(string nomeArquivo)
        {
            try
            {
                using (IUnitOfWorkProcess context = UnitOfWorkFactoryLive.Create())
                {
                    IRepositoryLive repArquivo = context.CreateRepository<TransactionTemp>();
                    return repArquivo.Filter<TransactionTemp>(x => x.FileTitle.ToUpper() == nomeArquivo.ToUpper()
                                                                   && x.Active == true).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public static StatusTransactionTemp ObterStatusTransactionTemp(int StatusId)
        {
            try
            {
                using (IUnitOfWorkProcess contextStatus = UnitOfWorkFactoryLive.Create())
                {
                    IRepositoryLive repStatus = contextStatus.CreateRepository<StatusTransactionTemp>();
                    return repStatus.Filter<StatusTransactionTemp>(x => x.Id == StatusId && x.Active == true)
                        .FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static TransactionTemp SalvarArquivoTransactionTemp(TransactionTemp arquivo)
        {
            try
            {
                using (IUnitOfWorkProcess contextStatus = UnitOfWorkFactoryLive.Create())
                {
                    IRepositoryLive _repository = contextStatus.CreateRepository<TransactionTemp>();
                    if (arquivo.Id > 0)
                        _repository.Update<TransactionTemp>(arquivo);
                    else
                        _repository.Create<TransactionTemp>(arquivo);
                    _repository.SaveChanges();

                    return _repository.Filter<TransactionTemp>(x => x.FileTitle == arquivo.FileTitle && x.ProjectId == arquivo.ProjectId).FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
