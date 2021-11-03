using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace Diograms
{
    class SetDataForGrafics
    {
        /// <summary>
        /// Redirects data for sorting
        /// </summary>
        /// <param name="type">grafic type</param>
        /// <param name="selectedCells">selected cells from datagridview</param>
        /// <param name="data">all info from DataGridView</param>
        /// <returns>Sorted Dictionary with all information</returns>
        public static SortedDictionary<string, double> SetData(string type, DataGridViewSelectedCellCollection selectedCells, DataTable data)
        {
            SortedDictionary<string, double> valuePairs = new SortedDictionary<string, double>();
            if (type == "column")
            {
                int colIndex = selectedCells[0].ColumnIndex;
                string name = data.Columns[colIndex].ColumnName;
                valuePairs = Statistics(colIndex, data);
            }
            if (type == "two")
            {
                int[] cIndexes = new int[] { selectedCells[0].ColumnIndex, selectedCells[1].ColumnIndex };
                valuePairs = Dependence(cIndexes, data);
            }
            return valuePairs;
        }
        /// <summary>
        /// Sarted data to dictionary
        /// </summary>
        /// <param name="columnIndex">index of column</param>
        /// <param name="data">all data from DataGridView</param>
        /// <returns>sorted dictionary</returns>
        private static SortedDictionary<string, double> Statistics(int columnIndex, DataTable data)
        {
            Dictionary<string, double> pairs = new Dictionary<string, double>();
            try
            {

                List<string> dr = new List<string>();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    dr.Add(data.Rows[i][columnIndex].ToString());
                }
                dr.Sort();

                for (int i = 0; i < dr.Count; i++)
                {
                    if (pairs.ContainsKey(dr[i].ToString()))
                    {
                        pairs[dr[i].ToString()] += 1;
                    }
                    else
                    {
                        pairs.Add(dr[i].ToString(), 1);
                    }
                }

            }
            catch
            {
                MessageBox.Show("Данные не корректны(", "Error", MessageBoxButtons.OK);
            }
            var dict = pairs;

            var sortedDict = new SortedDictionary<string, double>(dict);
            return sortedDict;


        }
        /// <summary>
        /// Finds the dependency in the columns and writes it to the dictionary
        /// </summary>
        /// <param name="columnIndexes">indexes of columns</param>
        /// <param name="data">all information from DataGridView</param>
        /// <returns>sorted dictionary</returns>
        private static SortedDictionary<string, double> Dependence(int[] columnIndexes, DataTable data)
        {
            SortedDictionary<string, double> pairs = new SortedDictionary<string, double>();
            int indexForValue;
            int indexForKey;
            try
            {
                // проверка данных столбца
                if (int.TryParse(data.Rows[1][columnIndexes[0]].ToString(), out _)
                            && int.TryParse(data.Rows[1][columnIndexes[1]].ToString(), out _))
                {
                    List<double> first = new List<double>();
                    List<double> second = new List<double>();
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        if (double.TryParse(data.Rows[i][columnIndexes[0]].ToString(), out double numberOne)
                            && double.TryParse(data.Rows[i][columnIndexes[1]].ToString(), out double numberTwo))
                        {
                            if (!first.Contains(numberOne))
                                first.Add(numberOne);

                            if (!second.Contains(numberTwo))
                                second.Add(int.Parse(data.Rows[i][columnIndexes[1]].ToString()));
                        }
                    }
                    indexForKey = first.Count > second.Count ? columnIndexes[1] : columnIndexes[0];
                    indexForValue = first.Count > second.Count ? columnIndexes[0] : columnIndexes[1];
                    CreateDataDictionary(data, pairs, indexForValue, indexForKey);
                }
                else
                {
                    if (double.TryParse(data.Rows[1][columnIndexes[0]].ToString(), out double _))
                    {
                        indexForValue = columnIndexes[0];
                        indexForKey = columnIndexes[1];
                    }
                    else
                    {
                        indexForValue = columnIndexes[1];
                        indexForKey = columnIndexes[0];
                    }
                    CreateDataDictionary(data, pairs, indexForValue, indexForKey);
                }
            }
            catch
            {
                MessageBox.Show("что-то пошло не так. Неверный формат данных", "упс", MessageBoxButtons.OK);
            }
            var dict = pairs; //попытка отсортировать словарь
            var sortedDict = new SortedDictionary<string, double>(dict);
            return sortedDict;

        }
        /// <summary>
        /// Цriting to the dictionary
        /// </summary>
        /// <param name="data">all info frim DataGridView</param>
        /// <param name="pairs">sorted dictionary</param>
        /// <param name="indexForValue">index of column for value in dictionary</param>
        /// <param name="indexForKey">index of column for key in dictionary</param>
        private static void CreateDataDictionary(DataTable data, SortedDictionary<string, double> pairs, int indexForValue, int indexForKey)
        {
            if (data.Columns[indexForKey].ColumnName != null)
                pairs.Add(data.Columns[indexForKey].ColumnName, 0);
            else
                pairs.Add("none", 0);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (pairs.ContainsKey(data.Rows[i][indexForKey].ToString()))
                {
                    pairs[data.Rows[i][indexForKey].ToString()] += double.Parse(data.Rows[i][indexForValue].ToString()) / 2;
                }
                else
                {
                    if (double.TryParse(data.Rows[i][indexForValue].ToString(), out double number))
                    {
                        pairs.Add(data.Rows[i][indexForKey].ToString(), number);

                    }
                }
            }
        }
    }
}
