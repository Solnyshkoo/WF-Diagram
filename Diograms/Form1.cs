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
    public partial class Form1 : Form
    {
        /// <summary>
        /// All data from DataGridView
        /// </summary>
        public DataTable Data { get; set; }
        public Form1()
        {
            InitializeComponent();
            MessageFromAuthor();
        }
        /// <summary>
        /// Load data to DataGridView
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Text Files(*.CSV)|*.CSV|All files(*.*)|*.*";
                openFileDialog1.Title = "Открыть документ";
                openFileDialog1.ShowDialog();
                string path = openFileDialog1.FileName;
                ImportCsvFile importCsv = new ImportCsvFile();
                Data = importCsv.CsvBindData(path);
                if (Data.Rows.Count > 0)
                {
                    dvgData.DataSource = Data;
                }

                /* Прекрасный код, до которого я додумалась слишком поздно(
                 * for (int j = 0; j < Data.Columns.Count; j++)
                     dvgData.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                     dvgData.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Load form1 with set size
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 1270;
            this.Height = 781;
        }
        /// <summary>
        /// Create grafic
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection selectedCells = dvgData.SelectedCells;
            if (selectedCells.Count == 0)
            {
                MessageBox.Show("Данные не выбраны: невозможно построить график", "Error", MessageBoxButtons.OK);
                return;
            }
            if (selectedCells.Count == 1)
            {
                int widthColumb = int.Parse(nuUpDo.Value.ToString());
                Grafics grafics = new Grafics("column", selectedCells, Data, widthColumb);
                grafics.Show();
            }
            else if (selectedCells.Count == 2)
            {
                if (selectedCells[0].ColumnIndex == selectedCells[1].ColumnIndex)
                {
                    MessageBox.Show("Значения должны быть из разных столбцов", "Error", MessageBoxButtons.OK);
                    return;
                }
                if (Data.Rows.Count > 2)
                {
                    if (!(int.TryParse(Data.Rows[1][selectedCells[0].ColumnIndex].ToString(), out int first)
                        || int.TryParse(Data.Rows[1][selectedCells[1].ColumnIndex].ToString(), out int second)))
                    {
                        MessageBox.Show("Хотя бы один из столбцов должен иметь числовое значение.", "Error", MessageBoxButtons.OK);
                        return;
                    }
                }
                int widthColumb = int.Parse(nuUpDo.Value.ToString());
                Grafics grafics = new Grafics("two", selectedCells, Data, widthColumb);
                grafics.Show();
                this.Width = 1270;
                this.Height = 781;
            }
            else
            {
                MessageBox.Show("Этот график строится по значениям из всего столбца." + Environment.NewLine +
                   "Выбери одну ячейку.", "Error", MessageBoxButtons.OK);
                return;
            }
        }
        /// <summary>
        /// Characteristics of column
        /// </summary>
        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (dvgData.SelectedCells.Count == 0)
            {
                MessageBox.Show("Выбери одну ячейку из столбца и получишь его характеристику.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (dvgData.SelectedCells.Count != 1)
            {
                MessageBox.Show("Надо выбрать только одну ячейку.", "Error", MessageBoxButtons.OK);
                return;
            }
            List<double> numbers = new List<double>();
            for (int i = 0; i < Data.Rows.Count; i++)
            {
                if (double.TryParse(Data.Rows[i][dvgData.SelectedCells[0].ColumnIndex].ToString(), out double numb))
                {
                    numbers.Add(numb);
                }
                else
                {
                    MessageBox.Show("Нельзя преобразовать все значения в число", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            MessageBox.Show($"Медиана - {Medians(numbers)}" + Environment.NewLine +
                $"Сред. значение - {Average(numbers)}" + Environment.NewLine +
                $"Среднеквадратичное отклонение - {AverageSq(numbers)}" + Environment.NewLine +
                $"Дисперсия - {Dispersion(numbers)}");

        }
        /// <summary>
        /// Count mediana
        /// </summary>
        /// <param name="values">all data from column</param>
        /// <returns>mediana</returns>
        private double Medians(List<double> values)
        {
            values.Sort();
            double med = -1;
            if (values.Count % 2 != 0)
            {
                med = values[values.Count / 2];
            }
            else
            {
                med = (values[values.Count / 2] + values[values.Count / 2 - 1]) / 2;
            }
            return med;
        }
        /// <summary>
        /// Count average
        /// </summary>
        /// <param name="values">all data from column</param>
        /// <returns>average</returns>
        private double Average(List<double> values)
        {
            double sum = 0;
            foreach (var item in values)
            {
                sum += item;
            }
            return sum / values.Count;
        }
        /// <summary>
        /// Посчтитать среднеквадратичное отклонение
        /// </summary>
        /// <param name="values">all data from column</param>
        /// <returns>среднеквадратичное отклонение</returns>
        private double AverageSq(List<double> values)
        {
            double averSum = Average(values);
            double averSqSum = 0;
            foreach (var num in values)
            {
                averSqSum += Math.Pow(num - averSum, 2);
            }
            averSqSum /= values.Count - 1;
            averSqSum = Math.Sqrt(averSqSum);
            return averSqSum;
        }
        /// <summary>
        /// Count dispersion
        /// </summary>
        /// <param name="values">all data from column</param>
        /// <returns>dispersion</returns>
        private double Dispersion(List<double> values)
        {
            double averSum = Average(values);
            double desp = 0;
            foreach (var num in values)
            {
                desp += Math.Pow(num - averSum, 2);
            }
            desp /= values.Count;
            return desp;
        }
        /// <summary>
        /// Open message from author
        /// </summary>
        private void btnAuthor_Click(object sender, EventArgs e)
        {
            MessageFromAuthor();
        }
        /// <summary>
        /// Open message from author
        /// </summary>

        private void MessageFromAuthor()
        {
            MessageBox.Show("Привет) Представляю любимый пир)" + Environment.NewLine +
               "Касательно нугетов читай в README). А так ситуация такова:" + Environment.NewLine +
               "Оно рисует прикольные графики, не так красиво как хотелось бы, но всё таки)" + Environment.NewLine +
               "Чтобы построить график по одному столбцу, выбери одну ячейку в" +
               " этом стобце" +
               "и нажми кнопку Create Grafic. Я долго пыталась настроить выделение столбца," +
               "но не получилось((( и только потом поняла как правильно, где-то выше этот " +
               "код закомменчен. Можешь посмотреть если интересно))" + Environment.NewLine +
               "Для построение графика из двух столбцов выбери две ячеки." + Environment.NewLine +
               "Если значений слишком много, то LiveChart может не справляться с их отображением" +
               "попробуй увеличить окно и ширину столбца. Должно помочь, но все мы люди" +
               "зачем мучать бедный график) Характеристика работает так же, выбери любую ячейку " +
               "в столбце и получишь характеристику столбца." + Environment.NewLine +
               "Я правда старалась, надеюсь на хорошую проверку)" + Environment.NewLine +
               "И да, форму может шакалить... я не понимаю почему, если знаешь " + Environment.NewLine +
               "напиши плиз)" + Environment.NewLine +
               "Всё, всё закончила) не злись) " + Environment.NewLine +
               "Доброго вечера и удачной проверики, солнце)" + Environment.NewLine +
               "Кстати был на АП?)))", "HI", MessageBoxButtons.OK);
        }
    }
}
