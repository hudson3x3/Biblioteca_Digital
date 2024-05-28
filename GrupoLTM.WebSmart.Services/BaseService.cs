using GrupoLTM.WebSmart.Services.Interface;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        public virtual IEnumerable<T> ListarTodos()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<T>();
                var _retorno = rep.All<T>().ToList();

                return _retorno;
            }
        }

        public virtual T ListarPorId(int id)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository rep = context.CreateRepository<T>();
                var _retorno = rep.Find<T>(id);

                return _retorno;
            }
        }

    }
}
