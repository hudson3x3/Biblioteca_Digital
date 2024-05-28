using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Infrastructure.ExtensionMethods;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace GrupoLTM.WebSmart.Services
{
    public class GoalService
    {
        public IEnumerable<MetaRA> ObterMetasRA(string ra)
        {
            using (SqlConnection cn = new SqlConnection(ConfiguracaoService.GetDatabaseConnection()))
            {
                var cmd = new SqlCommand("JP_PRC_ObterMetasDoParticipante", cn);
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@RA", Value = ra, SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input });
                cmd.CommandTimeout = 300; //5 minutos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                IEnumerable<MetaRA> goals;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    goals = reader.AutoMap<MetaRA>();
                }

                return goals;
            }
        }
    }
}
