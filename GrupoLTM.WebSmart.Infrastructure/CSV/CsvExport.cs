using ExcelLibrary.SpreadSheet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace GrupoLTM.WebSmart.Infrastructure.CSV
{
    public class CsvExport
    {
        public static bool WriteCSVFile(DataTable dt, string dir, string nomeArquivo, out string nomeArquivoGerado)
        {
            try
            {
                var storage = new Storage.Azure.Blob.Storage();

                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(";", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(";", fields));
                }

                //salva arquivo
                if (string.IsNullOrEmpty(nomeArquivo))
                    nomeArquivo = DateTime.Now.ToString("ddMMyyy_HHmmss") + ".csv";

                using (var ms = new MemoryStream())
                {
                    TextWriter tw = new StreamWriter(ms);
                    tw.WriteCsv(sb.ToString());

                    storage.UploadBlobAsync(ms, dir + nomeArquivo);
                }

                nomeArquivoGerado = nomeArquivo;

                return true;
            }
            catch (Exception exc)
            {
                nomeArquivoGerado = "";
                return false;
            }
        }
    }
}
