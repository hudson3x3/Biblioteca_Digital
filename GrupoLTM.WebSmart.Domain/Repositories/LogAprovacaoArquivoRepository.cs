using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class LogAprovacaoArquivoRepository
    {
        private AvonDbContext Context = new AvonDbContext();

        public void Create(LogAprovacaoArquivo logAprovacaoArquivo)
        {
            Context.LogAprovacaoArquivo.Add(logAprovacaoArquivo);
            Context.SaveChanges();
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
