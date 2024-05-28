using GrupoLTM.WebSmart.Domain.DTO;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GrupoLTM.WebSmart.Domain.Repositories
{
    public class LogAccountRepository
    {
        private AvonDbContext Context = new AvonDbContext();

        public void Create(LogAccount logAccount)
        {
            Context.LogAccount.Add(logAccount);
            Context.SaveChanges();
        }

        public IEnumerable<LogAccountDTO> ListarRelatorio(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                var query = from log in Context.LogAccount
                            join cat in Context.Catalogo on log.CatalogoId equals cat.Codigo
                            where
                            log.DataInclusao >= dtInicio &&
                            log.DataInclusao <= dtFim &&
                            log.CatalogoId > 0 
                            select new LogAccountDTO
                            {
                                Id = log.Id,
                                AccountNumber = log.AccountNumber,
                                CatalogoId = log.CatalogoId,
                                DataInclusao = log.DataInclusao,
                                IP = log.IP,
                                LoginAdmin = log.LoginAdmin,
                                NameAccountNumber = log.NameAccountNumber,
                                Catalogo = cat.Nome
                            };                            
                
                if (catalogoId.HasValue && catalogoId > 0)
                    query = query.Where(l => l.CatalogoId == catalogoId.Value);
                
                return query;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}


