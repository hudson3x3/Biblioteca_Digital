using GrupoLTM.WebSmart.Domain.DTO;
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
    public class LogAccountService
    {
        private readonly LogAccountRepository _LogAccountRepository;

        public LogAccountService()
        {
            _LogAccountRepository = new LogAccountRepository();
        }

        public void GravarLogAccount(LogAccount logAccount)
        {
            _LogAccountRepository.Create(logAccount);
        }

        // Refatorar para paginação
        public List<LogAccountDTO> ListarRelatorio(int? catalogoId, DateTime dtInicio, DateTime dtFim, int startExibir, int regExibir, out int total)
        {
            var lstRetorno = _LogAccountRepository.ListarRelatorio(catalogoId, dtInicio, dtFim.AddDays(1));
            _LogAccountRepository.Dispose();
            total = lstRetorno.Count();

            return lstRetorno.Skip(startExibir).Take(regExibir).OrderByDescending(x => x.DataInclusao).ToList();
        }

        public List<LogAccountDTO> ListarRelatorio(int? catalogoId, DateTime dtInicio, DateTime dtFim)
        {
            var relatorio = _LogAccountRepository.ListarRelatorio(catalogoId, dtInicio, dtFim.AddDays(1)).ToList();
            _LogAccountRepository.Dispose();
            return relatorio;
        }

        public List<RelatorioPermissaoModel> ObterPontosCancelamento()
        {
            try
            {
                List<RelatorioPermissaoModel> retorno = new List<RelatorioPermissaoModel>();

                using (SqlConnection conn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
                {
                    SqlCommand cmd = new SqlCommand("JP_REL_UsuarioPermissao", conn);
                    cmd.CommandTimeout = 2400; // 40 minuitos
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno.Add(new RelatorioPermissaoModel()
                            {
                                Login = reader["Login"] != DBNull.Value ? (string)reader["Login"] : null,
                                Usuario = reader["Usuario"] != DBNull.Value ? (string)reader["Usuario"] : null,
                                Perfil = reader["Perfil"] != DBNull.Value ? (string)reader["Perfil"] : null,
                                Menu = reader["Menu"] != DBNull.Value ? (string)reader["Menu"] : null
                            });
                        }

                        conn.Close();
                        cmd.Dispose();
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
