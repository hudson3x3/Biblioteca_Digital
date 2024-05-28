using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Infrastructure.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class PerfilService : BaseService<Perfil>
    {
        public List<WebSmart.DTO.PerfilModel> ListarPerfisAtivosSite()
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();
                List<WebSmart.DTO.PerfilModel> model =  new List<WebSmart.DTO.PerfilModel>();

                var perfis = repPerfil.Filter<Perfil>(x => x.Ativo && x.Adm == false);

                foreach (var perfil in perfis)
                {
                    model.Add(new WebSmart.DTO.PerfilModel{
                        Ativo = perfil.Ativo,
                        DataAlteracao = perfil.DataAlteracao.HasValue ? perfil.DataAlteracao.Value : DateTime.MinValue,
                        Adm = perfil.Adm,
                        DataInclusao = perfil.DataInclusao,
                        Id = perfil.Id,
                        NivelHierarquia = perfil.NivelHierarquia.HasValue? perfil.NivelHierarquia.Value : 0,
                        Nome = perfil.Nome,
                        PaiId = perfil.PaiId.HasValue? perfil.PaiId.Value : 0
                    });
                }

                return model;
            }
        }

        public WebSmart.DTO.PerfilModel ListarPerfilPorNivel(int nivelPerfil)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();
                List<WebSmart.DTO.PerfilModel> model = new List<WebSmart.DTO.PerfilModel>();

                WebSmart.DTO.PerfilModel Perfil = new DTO.PerfilModel();

                var perfil = repPerfil.Single<Perfil>(x => x.Ativo && x.Adm == false && x.NivelHierarquia == nivelPerfil);

                        Perfil.Ativo = perfil.Ativo;
                        Perfil.DataAlteracao = perfil.DataAlteracao.HasValue ? perfil.DataAlteracao.Value : DateTime.MinValue;
                        Perfil.Adm = perfil.Adm;
                        Perfil.DataInclusao = perfil.DataInclusao;
                        Perfil.Id = perfil.Id;
                        Perfil.NivelHierarquia = perfil.NivelHierarquia.HasValue ? perfil.NivelHierarquia.Value : 0;
                        Perfil.Nome = perfil.Nome;
                        Perfil.PaiId = perfil.PaiId.HasValue ? perfil.PaiId.Value : 0;

                return Perfil;
            }
        }

        public void DesabilitarPerfil(int PerfilId)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repPerfil = context.CreateRepository<Perfil>();

                var perfil = repPerfil.Find<Perfil>(PerfilId);

                if (perfil != null)
                {
                    perfil.DataAlteracao = DateTime.Now;
                    perfil.Ativo = false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        repPerfil.Update(perfil);
                        repPerfil.SaveChanges();
                        scope.Complete();
                    }
                    
                }
                else
                {
                    throw new Exception("Perfil não encontrado");
                }
            }
        }

        public bool InativarPerfil(int PerfilId)
        {
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_DEL_Perfil";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@PerfilId", Value = PerfilId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ProcessadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[1].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }

            return blnSucesso;
        }
    }
}
