using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class LogAcessoHotSiteRepository
    {
        private AvonDbContext Context = new AvonDbContext();

        public void Create(LogAcessoHotSite logAcessoHotSite)
        {
            Context.LogAcessoHotSite.Add(logAcessoHotSite);
            Context.SaveChanges();
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }

    }
}


