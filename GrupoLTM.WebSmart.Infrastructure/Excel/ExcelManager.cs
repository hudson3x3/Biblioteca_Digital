using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelLibrary.SpreadSheet;
using System.Reflection;
using System.IO;

namespace GrupoLTM.WebSmart.Infrastructure.Excel
{
    public class ExcelManager
    {
        private string _fileName;
        private List<KeyValuePair<string, int>> _camposTamanhoFixo;

        public ExcelManager(string fileName, List<KeyValuePair<string, int>> camposTamnhoFixo = null)
        {
            if (Path.GetExtension(fileName).ToUpper() != ".XLS")
                throw new Exception("Você deve enviar um arquivo no formato .xls");

            this._fileName = fileName;
            this._camposTamanhoFixo = camposTamnhoFixo;
        }

        #region "Métodos Públicos"

        /// <summary>
        /// Método que lista todos os dados do arquivo excel
        /// </summary>
        /// <typeparam name="T">Objeto que representa o arquivo excel</typeparam>
        /// <param name="columnNames">Variável por referência que carrega nome das colunas</param>
        /// <returns>Lista do objeto T</returns>
        public List<T> Listar<T>(ref Dictionary<string, int> columnNames)
        {
            var lstRetorno = new List<T>();
            var propriedadesAusentes = string.Empty;
            var cells = GetFirstSheet().Cells;
            var lstColunas = new Dictionary<int, string>();

            // itera por todas as linhas do excel
            for (int rowIndex = cells.FirstRowIndex; rowIndex <= cells.LastRowIndex; rowIndex++)
            {
                // cria a instância de um objeto do tipo T
                T item = (T)Activator.CreateInstance(typeof(T));
                var values = new Dictionary<int, string>();

                // verifica se esta nos cabeçalhos
                if (rowIndex == cells.FirstRowIndex)
                    continue;

                if (rowIndex == (cells.FirstRowIndex + 1))
                {
                    lstColunas = ListarDadosLinha(cells.GetRow(rowIndex));

                    foreach (var data in lstColunas)
                        columnNames.Add(data.Value, data.Key);

                    // itera por todas as colunas da linha
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (!lstColunas.Select(x => x.Value).Contains(property.Name))
                            propriedadesAusentes += string.Format("{0}\n", property.Name);
                    }
                }
                else
                {
                    if (propriedadesAusentes != string.Empty)
                        throw new Exception(string.Format("As colunas abaixo estão ausentes:\n\n{0}\n", propriedadesAusentes));

                    for (int i = cells.FirstRowIndex; i < cells.LastColIndex; i++)
                    {
                        string cellValue;

                        if (_camposTamanhoFixo != null && _camposTamanhoFixo.Select(x => x.Key.ToUpper()).Contains(lstColunas[i].ToUpper()))
                        {
                            int tamanhoCampo = _camposTamanhoFixo.Where(x => x.Key.ToUpper() == lstColunas[i].ToUpper())
                                                .Select(x => x.Value).FirstOrDefault();

                            cellValue = string.Format("{0," + tamanhoCampo + "}", cells.GetRow(rowIndex).GetCell(i).StringValue).Replace(" ", "0");
                        }
                        else
                            cellValue = cells.GetRow(rowIndex).GetCell(i).StringValue;

                        PropertyInfo prop = item.GetType().GetProperty(lstColunas[i]);
                        prop.SetValue(item, (cellValue != null) ? cellValue.ToString() : string.Empty, null);
                    }

                    lstRetorno.Add(item);
                }
            }

            return lstRetorno;
        }

        /// <summary>
        /// Método que lista todos os dados do arquivo excel
        /// </summary>
        /// <typeparam name="T">Objeto que representa o arquivo excel</typeparam>
        /// <returns>Lista do objeto T</returns>
        public List<T> Listar<T>()
        {
            var lstRetorno = new List<T>();
            var propriedadesAusentes = string.Empty;
            var cells = GetFirstSheet().Cells;
            var lstColunas = new Dictionary<int, string>();

            // itera por todas as linhas do excel
            for (int rowIndex = cells.FirstRowIndex; rowIndex <= cells.LastRowIndex; rowIndex++)
            {
                // cria a instância de um objeto do tipo T
                T item = (T)Activator.CreateInstance(typeof(T));
                var values = new Dictionary<int, string>();

                // verifica se esta nos cabeçalhos
                if (rowIndex == cells.FirstRowIndex)
                {
                    lstColunas = ListarDadosLinha(cells.GetRow(rowIndex));

                    // itera por todas as colunas da linha
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (!lstColunas.Select(x => x.Value).Contains(property.Name))
                            propriedadesAusentes += string.Format("{0}\n", property.Name);
                    }
                }
                else
                {
                    if (propriedadesAusentes != string.Empty)
                        throw new Exception(string.Format("As colunas abaixo estão ausentes:\n\n{0}\n", propriedadesAusentes));

                    for (int i = cells.FirstRowIndex; i <= cells.LastColIndex; i++)
                    {
                        string cellValue;

                        if (_camposTamanhoFixo != null && _camposTamanhoFixo.Select(x => x.Key.ToUpper()).Contains(lstColunas[i].ToUpper()))
                        {
                            int tamanhoCampo = _camposTamanhoFixo.Where(x => x.Key.ToUpper() == lstColunas[i].ToUpper())
                                                .Select(x => x.Value).FirstOrDefault();

                            cellValue = string.Format("{0," + tamanhoCampo + "}", cells.GetRow(rowIndex).GetCell(i).StringValue).Replace(" ", "0");
                        }
                        else
                            cellValue = cells.GetRow(rowIndex).GetCell(i).StringValue;

                        PropertyInfo prop = item.GetType().GetProperty(lstColunas[i]);
                        prop.SetValue(item, (cellValue != null) ? cellValue.ToString() : string.Empty, null);
                    }

                    lstRetorno.Add(item);
                }                
            }

            return lstRetorno;
        }

        /// <summary>
        /// Método que grava uma lista de novas linhas na planilha
        /// </summary>
        /// <typeparam name="T">Objeto que representa a planilha</typeparam>
        /// <param name="itens">Lista de itens</param>
        /// <param name="key">Nome da coluna chave</param>
        /// <param name="fieldToUpdate">Nome da coluna a ser alterada e seu valor</param>
        /// <param name="Ids">Lista de Ids para limpeza da coluna Observação</param>
        /// <returns>Boolean</returns>
        public bool UpdateRange<T>(List<T> itens, KeyValuePair<string, int> key, KeyValuePair<string, int>? fieldToUpdate)
        {
            var woorkBook = new Workbook();
            var worksheet = new Worksheet(GetFirstSheet().Name);

            var lstColunas = new Dictionary<int, string>();
            var cells = GetFirstSheet().Cells;
            var lstKeys = new List<string>();
            var getColumns = false;
            string colValue = string.Empty;

            try
            {
                itens.ForEach(x =>
                {
                    lstKeys.Add(x.GetType().GetProperty(key.Key).GetValue(x, null).ToString().ToUpper());
                });

                for (int rowIndex = cells.FirstRowIndex; rowIndex <= cells.LastRowIndex; rowIndex++)
                {
                    getColumns = false;

                    if (rowIndex == cells.FirstRowIndex)
                        continue;

                    string cellValue;

                    if(cells.GetRow(rowIndex).GetCell(key.Value).Value != null)
                        cellValue = cells.GetRow(rowIndex).GetCell(key.Value).Value.ToString().ToUpper();
                    else
                        continue;

                    if (lstKeys.Contains(cellValue))
                    {
                        // Obétm objeto de 'itens' a ser alterado
                        var objToUpdate = itens.Where(x => x.GetType().GetProperty(key.Key).GetValue(x, null).ToString().ToUpper()
                                                      == cellValue).FirstOrDefault();

                        cells.GetRow(rowIndex).SetCell(fieldToUpdate.Value.Value, new Cell(objToUpdate.GetType().GetProperty(fieldToUpdate.Value.Key).GetValue(objToUpdate, null)));
                    }                   

                    for(int i = cells.GetRow(rowIndex).FirstColIndex; i <= cells.GetRow(rowIndex).LastColIndex; i++) 
                    {
                        if (lstColunas.Count == 0 && _camposTamanhoFixo != null)
                        {
                            // Obtém colunas
                            if (_camposTamanhoFixo.Select(x => x.Key.ToUpper()).Contains(cells.GetRow(rowIndex).GetCell(i).StringValue.ToUpper()))
                                lstColunas = ListarDadosLinha(cells.GetRow(rowIndex));

                            getColumns = true;
                        }

                        if (!getColumns && _camposTamanhoFixo.Select(x => x.Key.ToUpper()).Contains(lstColunas[i].ToUpper()))
                        {
                            int tamanhoCampo = _camposTamanhoFixo.Where(x => x.Key.ToUpper() == lstColunas[i].ToUpper())
                                                .Select(x => x.Value).FirstOrDefault();

                            colValue = string.Format("{0," + tamanhoCampo + "}", cells.GetRow(rowIndex).GetCell(i).StringValue).Replace(" ", "0");
                        }
                        else
                            colValue = cells.GetRow(rowIndex).GetCell(i).StringValue;

                        worksheet.Cells[rowIndex, i] = new Cell(colValue);
                        worksheet.Cells[rowIndex, i].Style = cells.GetRow(rowIndex).GetCell(i).Style;
                    }                    
                }

                DeletaArquivo();                

                woorkBook.Worksheets.Add(worksheet);
                woorkBook.Save(_fileName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region "Métodos Privados"

        private Worksheet GetFirstSheet()
        {
            var workbook = Workbook.Load(_fileName);

            return workbook.Worksheets[0];
        }

        private Dictionary<int, string> ListarDadosLinha(Row linha)
        {
            var lstRetorno = new Dictionary<int, string>();
            int count = 0;

            // joga valores da coluna em 'values'
            foreach (var cell in linha)
                lstRetorno.Add(count++, cell.Value.StringValue);

            return lstRetorno;
        }

        private void DeletaArquivo()
        {
            File.Delete(_fileName);
        }

        #endregion

        #region "Propriedades"

        public List<KeyValuePair<string, int>> CamposTamanhoFixo
        {
            get { return _camposTamanhoFixo; }
            set { _camposTamanhoFixo = value; }
        }

        #endregion
    }
}
