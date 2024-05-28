using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.Infrastructure.Excel
{
    public class ExcelManagerServices
    {
        /*
         Implementação
         * var list = ExcelManagerServices.Listar<ParticipanteImportarModel>(@"C:\Users\rcoelho\Desktop\Modelo.xls"); 
         */
        private static ExcelManager _excelManager;

        /// <summary>
        /// Método que lista todos os dados de uma planilha do excel e retorna suas colunas por referência
        /// </summary>
        /// <typeparam name="T">Objeto que representa a planilha</typeparam>
        /// <param name="fileName">Caminho do arquivo excel</param>
        /// <param name="camposTamanhoFixo">KeyValuePair com 'Nome do Campo (Númerico)' e tamanho fixo (com zeros à frente)</param>
        /// <returns>Lista dos dados da planilha</returns>
        public static List<T> Listar<T>(string fileName, ref Dictionary<string, int> columnNames,
                                        List<KeyValuePair<string, int>> camposTamanhoFixo = null)
        {
            try
            {
                var pColumnNames = columnNames;

                _excelManager = new ExcelManager(fileName, camposTamanhoFixo);

                return _excelManager.Listar<T>(ref pColumnNames);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que lista todos os dados de uma planilha do excel
        /// </summary>
        /// <typeparam name="T">Objeto que representa a planilha</typeparam>
        /// <param name="fileName">Caminho do arquivo excel</param>
        /// <param name="camposTamanhoFixo">KeyValuePair com 'Nome do Campo (Númerico)' e tamanho fixo (com zeros à frente)</param>
        /// <returns>Lista dos dados da planilha</returns>
        public static List<T> Listar<T>(string fileName, List<KeyValuePair<string, int>> camposTamanhoFixo = null)
        {
            try
            {
                _excelManager = new ExcelManager(fileName, camposTamanhoFixo);

                return _excelManager.Listar<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que altera uma seria de itens no arquivo excel
        /// </summary>
        /// <typeparam name="T">Obejto que representa a planilha excel</typeparam>
        /// <param name="itens">Lista de itens a serem inseridos</param>
        /// <param name="key">Nome da coluna chave</param>
        /// <param name="fileName">Caminho do arquivo excel</param>
        /// <param name="fildToUpdate">Índice do campo a ser alterado</param>
        /// <param name="camposTamanhoFixo">KeyValuePair com 'Nome do Campo (Númerico)' e tamanho fixo (com zeros à frente)</param>
        /// <returns>Boolean</returns>
        public static bool UpdateRange<T>(List<T> itens,
                                          KeyValuePair<string, int> key,
                                          string fileName,
                                          KeyValuePair<string, int>? fieldToUpdate,
                                          List<KeyValuePair<string, int>> camposTamanhoFixo = null)
        {
            _excelManager = new ExcelManager(fileName, camposTamanhoFixo);

            return _excelManager.UpdateRange<T>(itens, key, fieldToUpdate);
        }
    }
}
