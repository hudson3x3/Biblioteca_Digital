using Excel;
using System.Data;
using System.IO;

namespace GrupoLTM.WebSmart.Infrastructure.Excel
{
    public class ExcelDataReader
    {
        //public static DataSet LerExcel(string filePath)
        //{
        //    FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        //    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //    excelReader.IsFirstRowAsColumnNames = true;
        //    DataSet result = excelReader.AsDataSet();
        //    return result;
        //}

        public static DataSet OpenExcel(string file)
        {

            var storage = new Storage.Azure.Blob.Storage();

            var blob = storage.GetFile(file);

            //IExcelDataReader excelDataReader = ExcelReaderFactory.CreateBinaryReader(blob);
            var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(blob);

            excelDataReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelDataReader.AsDataSet();
            return result;
        }
    }
}
