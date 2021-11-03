using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace Diograms
{
    public partial class Grafics : Form
    {
        public Grafics(string text, DataGridViewSelectedCellCollection selectedCells, DataTable data, int clmAmount)
        {
            InitializeComponent();
            SetGrafic(text, selectedCells, data, clmAmount);
        }
        /// <summary>
        /// Choose grafic type
        /// </summary>
        /// <param name="type">grafic type</param>
        /// <param name="selectedCells">selected data from dataGridView</param>
        /// <param name="data">all data</param>
        /// <param name="clmAmount">width of column</param>

        private void SetGrafic(string type, DataGridViewSelectedCellCollection selectedCells, DataTable data, int clmAmount)
        {
            if (type == "column")
            {
                ColumnGrafic(type, selectedCells, data, clmAmount);
            }
            if (type == "two")
            {
                TwoDimensionalGraphs(type, selectedCells, data, clmAmount);
            }

        }
        /// <summary>
        /// Graph by one column
        /// </summary>
        /// <param name="type">grafic type</param>
        /// <param name="selectedCells">selected data from dataGridView</param>
        /// <param name="data">all data</param>
        /// <param name="clmAmount">width of column</param>
        private void ColumnGrafic(string type, DataGridViewSelectedCellCollection selectedCells, DataTable data, int clmAmount)
        {
            try
            {
                SortedDictionary<string, double> pairs = SetDataForGrafics.SetData(type, selectedCells, data);
                ChartValues<double> valueClm = new ChartValues<double>();
                string name;
                if (data.Columns[selectedCells[0].ColumnIndex].ColumnName != null)
                    name = data.Columns[selectedCells[0].ColumnIndex].ColumnName;
                else
                    name = "none";
                if (clmAmount > pairs.Keys.Count) //проверка на кол-во данных
                {
                    clmAmount = pairs.Keys.Count;
                    MessageBox.Show($"Недостаточно данных для такого объединения." + Environment.NewLine +
                        $"Максимально возможное: {clmAmount}. Щас, покажу как выглядит)", "Error", MessageBoxButtons.OK);
                }

                string[] labels = SortLabels(pairs.Keys.ToArray(), clmAmount);
                double[] values = SortValues(pairs.Values.ToArray(), clmAmount);

                for (int i = 0; i < values.Length; i++)
                {
                    valueClm.Add(values[i]);
                }

                cGrafic.Series = new SeriesCollection // построение графика
            {
                new ColumnSeries
                {
                    Title = $"{name}",
                    Values = valueClm,

                }
            };

                cGrafic.AxisX.Add(new Axis
                {
                    Title = name,
                    Labels = labels,
                });

                cGrafic.AxisY.Add(new Axis
                {
                    Title = "Amount",
                    LabelFormatter = value => value.ToString("G")
                });
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так( Возможно ошибка в представленных данных", "Error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Graph by two column
        /// </summary>
        /// <param name="type">grafic type</param>
        /// <param name="selectedCells">selected data from dataGridView</param>
        /// <param name="data">all data</param>
        /// <param name="clmAmount">width of column</param>
        private void TwoDimensionalGraphs(string type, DataGridViewSelectedCellCollection selectedCells, DataTable data, int clmAmount)
        {
            try
            {
                SortedDictionary<string, double> pairs = SetDataForGrafics.SetData(type, selectedCells, data);

                string[] labels = pairs.Keys.ToArray();
                string nameX = labels[0];
                string[] textRow = new string[labels.Length - 1];
                for (int i = 1; i < labels.Length; i++)
                {
                    textRow[i - 1] = labels[i];
                }
                if (clmAmount > pairs.Keys.Count) //проверка на кол-во данных
                {
                    clmAmount = pairs.Keys.Count;
                    MessageBox.Show($"Недостаточно данных для такого объединения." + Environment.NewLine +
                        $"Максимально возможное: {clmAmount}. Щас, покажу как выглядит)", "Error", MessageBoxButtons.OK);
                }
                string[] text = SortLabels(textRow, clmAmount);
                string nameY;
                if (nameX != data.Columns[selectedCells[0].ColumnIndex].ColumnName)
                    nameY = data.Columns[selectedCells[0].ColumnIndex].ColumnName;
                else
                    nameY = data.Columns[selectedCells[1].ColumnIndex].ColumnName;
                double[] values = SortValues(pairs.Values.ToArray(), clmAmount);
                ChartValues<double> vs = new ChartValues<double>();
                for (int i = 1; i < values.Length; i++)
                {
                    vs.Add(values[i]);
                }
                cGrafic.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = nameY,
                    Values = vs
                }
            };
                cGrafic.Series[0].Values.Add(48d);

                cGrafic.AxisX.Add(new Axis
                {
                    Title = nameX,
                    Labels = text,
                });

                cGrafic.AxisY.Add(new Axis
                {
                    Title = nameY,
                    LabelFormatter = value => value.ToString("G")
                });
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так( Возможно ошибка в представленных данных", "Error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Sort Values according to wight column
        /// </summary>
        /// <param name="values">data about values</param>
        /// <param name="clmAmount">wight column</param>
        /// <returns></returns>
        private double[] SortValues(double[] values, int clmAmount)
        {
            int countClm = 0;
            int integer = 0;
            int surplus = 0;
            if (clmAmount == 1)
            {
                return values;
            }
            if (values.Length % clmAmount == 0) //расчёт кол-во столбцов
            {
                countClm = values.Length / clmAmount;
                integer = countClm;
            }
            else
            {
                integer = values.Length / clmAmount;
                surplus = values.Length - (integer * clmAmount);
                countClm = integer + surplus;
            }
            double[] value = new double[countClm];
            int index = 0;

            for (int i = 0; i < values.Length; i += clmAmount)
            {
                if (i + (clmAmount - 1) < integer * clmAmount)
                {
                    for (int l = i; l <= i + (clmAmount - 1); l++)
                    {
                        value[index] += values[l];
                    }
                    index++;
                }
            }

            for (int i = 0; i < surplus; i++)
                value[countClm - 1] += values[integer * clmAmount + i];

            return value;
        }
        /// <summary>
        /// Sort labels according to wight column
        /// </summary>
        /// <param name="labels">all labels</param>
        /// <param name="clmAmount"> wight column</param>
        /// <returns></returns>

        private string[] SortLabels(string[] labels, int clmAmount)
        {
            int countClm = 0;
            int integer = 0;
            int surplus = 0;
            if (clmAmount == 1)
            {
                return labels;
            }
            if (labels.Length % clmAmount == 0)
            {
                countClm = labels.Length / clmAmount;
                integer = countClm;
            }
            else
            {
                integer = labels.Length / clmAmount;
                surplus = labels.Length - (integer * clmAmount);
                countClm = integer + surplus;
            }
            string[] lbl = new string[countClm];
            int index = 0;

            for (int i = 0; i < labels.Length; i += clmAmount)
            {
                if (i + (clmAmount - 1) < integer * clmAmount)
                {
                    for (int l = i; l <= i + (clmAmount - 1); l++)
                    {
                        lbl[index] += labels[l] + "/";

                    }
                    index++;
                }
            }
            for (int i = 0; i < surplus; i++)
                lbl[countClm - 1] += labels[integer * clmAmount + i] + "/";
            return lbl;
        }
        /// <summary>
        /// Size of Grafics form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grafics_Load(object sender, EventArgs e)
        {
            this.Height = 1110;
            this.Width = 1688;
        }
        /// <summary>
        /// Save grafics
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "png files (*.png)|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int width, height;
                width = cGrafic.Width;
                height = cGrafic.Height;

                Bitmap bmb = new Bitmap(width, height);
                try
                {
                    cGrafic.DrawToBitmap(bmb, cGrafic.Bounds);
                    bmb.Save(sfd.FileName);
                }
                catch
                {
                    MessageBox.Show("Не получилось сохранить картинку", "Error", MessageBoxButtons.OK);
                }
            }
        }
    }
}
