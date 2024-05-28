using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;

namespace GrupoLTM.WebSmart.Services
{
    public class DisparoEmailService : BaseService<DisparoEmail>
    {
        public List<DisparoEmail> ListarDisparosPendentes()
        {
            return ListarTodos().Where(disparoEmail => !disparoEmail.Enviado && disparoEmail.Ativo).ToList();
        }

        public bool Update(DisparoEmail disparoEmail)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repository = context.CreateRepository<DisparoEmail>();

                if (disparoEmail != null)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        repository.Update(disparoEmail);
                        repository.SaveChanges();
                        scope.Complete();
                    }

                    return true;
                }

                return false;
            }
        }
    }
}
