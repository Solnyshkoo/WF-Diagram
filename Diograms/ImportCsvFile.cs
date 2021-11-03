using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Windows;
using System.Windows.Forms;



namespace Diograms
{
    class ImportCsvFile
    {
        /// <summary>
        /// Import CSV-file
        /// </summary>
        /// <param name="filePath">path</param>
        /// <returns>DataTable with all data</returns>
        public DataTable CsvBindData(string filePath)
        {
            DataTable data = new DataTable();
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length > 0)
                {
                    //first line to create header
                    string firstLine = " " + lines[0];
                    if (firstLine.Contains(",,"))
                    {
                        firstLine = firstLine.Replace(",,", ",none,");
                    }
                    string[] headerLines = firstLine.Split(',');
                    foreach (var header in headerLines)
                    {
                        data.Columns.Add(new DataColumn(header));
                    }
                    // for data
                    for (int i = 1; i < lines.Length; i++)
                    {
                        lines[i] = lines[i].Replace(", ", ". ");
                        lines[i] = lines[i].Replace(", ,", ",0,");
                        lines[i] = lines[i].Replace(",,", ",0,");
                        string[] dataWords = lines[i].Split(',');
                        DataRow dataRow = data.NewRow();
                        int colmnIndex = 0;
                        foreach (var item in headerLines)
                        {
                            dataRow[item] = dataWords[colmnIndex++];
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                if (data.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Файл пуст", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return data;
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Операция прервана.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }
            catch
            {
                throw new ArgumentException();
            }

        }
    }
}
