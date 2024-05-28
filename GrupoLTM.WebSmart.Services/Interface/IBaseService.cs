using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.Services.Interface
{
    public interface IBaseService<T>
    {
        IEnumerable<T> ListarTodos();
        T ListarPorId(int id);
    }
}
