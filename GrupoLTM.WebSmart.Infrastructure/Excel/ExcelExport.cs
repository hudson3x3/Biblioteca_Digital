using OfficeOpenXml;
using System;
using System.Data;
using System.IO;

namespace GrupoLTM.WebSmart.Infrastructure.Excel
{
    public class ExcelExport
    {
        public static void WriteXLSFile(DataSet dataset, string dir, out string nomeArquivoGerado)
        {
            var storage = new Storage.Azure.Blob.Storage();
            var excel = new ExcelPackage();
            var worksheet = excel.Workbook.Worksheets.Add("Plan1");
            worksheet.Cells["A1"].LoadFromDataTable(dataset.Tables[0], true);
            nomeArquivoGerado = DateTime.Now.ToString("ddMMyyy_HHmmss") + ".xls";

            using (var ms = new MemoryStream())
            {
                excel.SaveAs(ms);
                storage.UploadBlobAsync(ms, dir + nomeArquivoGerado);
            }
        }

        public static void ToPdf(Stream xxx)
        {
            var storage = new Storage.Azure.Blob.Storage();
            storage.UploadBlobAsync(xxx, "relatorio/teste.pdf");
        }
    }
}
