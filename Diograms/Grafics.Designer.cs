
namespace Diograms
{
    partial class Grafics
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cGrafic = new LiveCharts.WinForms.CartesianChart();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cGrafic
            // 
            this.cGrafic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cGrafic.Location = new System.Drawing.Point(21, 12);
            this.cGrafic.Name = "cGrafic";
            this.cGrafic.Size = new System.Drawing.Size(1571, 814);
            this.cGrafic.TabIndex = 0;
            this.cGrafic.Text = "cartesianChart1";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(92, 889);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(239, 87);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Grafics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1662, 1039);
            this.Controls.Add(this.cGrafic);
            this.Controls.Add(this.btnSave);
            this.Name = "Grafics";
            this.Text = "Grafics";
            this.Load += new System.EventHandler(this.Grafics_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private LiveCharts.WinForms.CartesianChart cGrafic;
        private System.Windows.Forms.Button btnSave;
    }
}