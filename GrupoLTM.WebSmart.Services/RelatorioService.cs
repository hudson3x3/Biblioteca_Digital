using GrupoLTM.WebSmart.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.Services
{
    public class RelatorioService
    {
        public static DataTable RelParticipantePF()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_ParticipantePF";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelPontuacao()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_Pontuacao";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelParticipantePJ()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_ParticipantePJ";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelParticipanteQuizPF()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_ParticipanteQuizPF";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelParticipanteQuizPJ()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_ParticipanteQuizPJ";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelParticipantePesquisaPF()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_ParticipantePesquisaPF";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelParticipantePesquisaPJ()
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_ParticipantePesquisaPJ";
            DataTable table = DataProvider.SelectStoreProcedure(proc);
            return table;
        }

        public static DataTable RelLog(DateTime DataInicio, DateTime DataFim, int? ParticipanteId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Rel_Log";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@DataInicio", Value = DataInicio, SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@DataFim", Value = DataFim, SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input });

            if (ParticipanteId != null) {
                listParam.Add(new SqlParameter { ParameterName = "@ParticipanteId", Value = ParticipanteId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            }

            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }
    }
}
