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
    public class HierarquiaService
    {
        public static bool SalvarHierarquiaIndividual(int ID, int periodoID, int nivel1, int nivel2, int nivel3, int nivel4, int nivel5, int nivel6, int nivel7, int nivel8, int nivel9, int nivel10)
        {
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Ins_ParticipanteHierarquia";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ParticipanteID", Value = ID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoID", Value = periodoID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel1", Value = nivel1, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel2", Value = nivel2, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel3", Value = nivel3, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel4", Value = nivel4, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel5", Value = nivel5, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel6", Value = nivel6, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel7", Value = nivel7, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel8", Value = nivel8, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel9", Value = nivel9, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@nivel10", Value = nivel10, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[12].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            return blnSucesso;
        }


        public static DataTable ObterNivelHierarquia(int participanteID, int periodoID)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_HierarquiaNivel";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ID", Value = participanteID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoID", Value = periodoID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ObterHierarquiaPorNivelParticipante(int nivelHierarquia, int periodoID, int participanteID)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_HierarquiaPorNivel";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@NivelHierarquia", Value = nivelHierarquia, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoID", Value = periodoID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ParticipanteID", Value = participanteID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ObterHierarquiaPorParticipante(int participanteID, int PeriodoID)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_HierarquiaPorParticipante";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ParticipanteID", Value = participanteID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoID", Value = PeriodoID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }
    }
}
