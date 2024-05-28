using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repositories;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GrupoLTM.WebSmart.Services
{
    public class LogAcessoHotSiteService
    {
        private readonly LogAcessoHotSiteRepository _LogAcessoHotSiteRepository;

        public LogAcessoHotSiteService()
        {
            _LogAcessoHotSiteRepository = new LogAcessoHotSiteRepository();
        }

        public void GravarLogAcessoHotSite(LogAcessoHotSite logAcessoHotSite)
        {
            _LogAcessoHotSiteRepository.Create(logAcessoHotSite);
            _LogAcessoHotSiteRepository.Dispose();
        }
    }
}
