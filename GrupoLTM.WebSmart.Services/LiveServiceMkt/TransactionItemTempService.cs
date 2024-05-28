using GrupoLTM.WebSmart.Domain.Models.Live;
using GrupoLTM.WebSmart.Domain.Repository.Live;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Services.LiveServiceMkt
{
    public class TransactionItemTempService
    {

        private static string ConnectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmartProcess"].ToString();
        private const string TableName = "TB_TransactionItemTemp";
        public static bool SalvarArquivoTransactionItemTemp(DataTable dt)
        {
            try
            {
                //using (IUnitOfWorkProcess contextStatus = UnitOfWorkFactoryLive.Create())
                //{
                //    IRepositoryLive _repository = contextStatus.CreateRepository<TransactionTemp>();
                //    foreach (var item in Creditos)
                //    {
                //        item.InsertDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
                //        _repository.Create<TransactionItemTemp>(item);
                //    }
                //    _repository.SaveChanges();
                //    return true;
                //}

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    // make sure to enable triggers
                    // more on triggers in next post
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = TableName;
                        connection.Open();

                        dt.Columns.Cast<DataColumn>().ToList().ForEach(f =>
                        {
                            SqlBulkCopyColumnMapping bccm = new SqlBulkCopyColumnMapping();
                            bccm.DestinationColumn = f.ColumnName;
                            bccm.SourceColumn = f.ColumnName;
                            bulkCopy.ColumnMappings.Add(bccm);
                        });

                        // write the data in the "dataTable"
                        bulkCopy.WriteToServer(dt);
                        connection.Close();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }



    }

}
