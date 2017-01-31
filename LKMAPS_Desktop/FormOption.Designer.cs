namespace LKMAPS_Desktop
{
    partial class FormOption
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxSimplification = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancell = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxuseOSMRoads = new System.Windows.Forms.CheckBox();
            this.checkBoxuseOSMRail = new System.Windows.Forms.CheckBox();
            this.checkBoxuseOSMRivers = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxuseOSMRivers = new System.Windows.Forms.ComboBox();
            this.checkBoxuseOSMRLakes = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxninLakesSize = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxuseOSMRCity = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxOSMRCity = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxCitySize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxuseOSMRResidential = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBoxSimplification);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(9, 266);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(356, 74);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Common";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(18, 54);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(264, 9);
            this.label10.TabIndex = 5;
            this.label10.Text = "Use higher value to reduce map complexity and indrese LK8000 performance";
            // 
            // textBoxSimplification
            // 
            this.textBoxSimplification.Location = new System.Drawing.Point(153, 25);
            this.textBoxSimplification.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSimplification.Name = "textBoxSimplification";
            this.textBoxSimplification.Size = new System.Drawing.Size(83, 20);
            this.textBoxSimplification.TabIndex = 4;
            this.textBoxSimplification.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Geometry semplification";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "meters";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(308, 370);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(77, 28);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancell
            // 
            this.buttonCancell.Location = new System.Drawing.Point(9, 370);
            this.buttonCancell.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancell.Name = "buttonCancell";
            this.buttonCancell.Size = new System.Drawing.Size(77, 28);
            this.buttonCancell.TabIndex = 7;
            this.buttonCancell.Text = "CANCEL";
            this.buttonCancell.UseVisualStyleBackColor = true;
            this.buttonCancell.Click += new System.EventHandler(this.buttonCancell_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "VeyHigh",
            "High",
            "Medium",
            "Low"});
            this.comboBox1.Location = new System.Drawing.Point(153, 37);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(83, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Detail";
            // 
            // checkBoxuseOSMRoads
            // 
            this.checkBoxuseOSMRoads.AutoSize = true;
            this.checkBoxuseOSMRoads.Location = new System.Drawing.Point(20, 37);
            this.checkBoxuseOSMRoads.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxuseOSMRoads.Name = "checkBoxuseOSMRoads";
            this.checkBoxuseOSMRoads.Size = new System.Drawing.Size(57, 17);
            this.checkBoxuseOSMRoads.TabIndex = 8;
            this.checkBoxuseOSMRoads.Text = "Roads";
            this.checkBoxuseOSMRoads.UseVisualStyleBackColor = true;
            // 
            // checkBoxuseOSMRail
            // 
            this.checkBoxuseOSMRail.AutoSize = true;
            this.checkBoxuseOSMRail.Location = new System.Drawing.Point(20, 72);
            this.checkBoxuseOSMRail.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxuseOSMRail.Name = "checkBoxuseOSMRail";
            this.checkBoxuseOSMRail.Size = new System.Drawing.Size(73, 17);
            this.checkBoxuseOSMRail.TabIndex = 9;
            this.checkBoxuseOSMRail.Text = "Rail roads";
            this.checkBoxuseOSMRail.UseVisualStyleBackColor = true;
            // 
            // checkBoxuseOSMRivers
            // 
            this.checkBoxuseOSMRivers.AutoSize = true;
            this.checkBoxuseOSMRivers.Location = new System.Drawing.Point(20, 104);
            this.checkBoxuseOSMRivers.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxuseOSMRivers.Name = "checkBoxuseOSMRivers";
            this.checkBoxuseOSMRivers.Size = new System.Drawing.Size(56, 17);
            this.checkBoxuseOSMRivers.TabIndex = 12;
            this.checkBoxuseOSMRivers.Text = "Rivers";
            this.checkBoxuseOSMRivers.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Detail";
            // 
            // comboBoxuseOSMRivers
            // 
            this.comboBoxuseOSMRivers.Enabled = false;
            this.comboBoxuseOSMRivers.FormattingEnabled = true;
            this.comboBoxuseOSMRivers.Items.AddRange(new object[] {
            "High",
            "Medium",
            "Low"});
            this.comboBoxuseOSMRivers.Location = new System.Drawing.Point(153, 103);
            this.comboBoxuseOSMRivers.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxuseOSMRivers.Name = "comboBoxuseOSMRivers";
            this.comboBoxuseOSMRivers.Size = new System.Drawing.Size(83, 21);
            this.comboBoxuseOSMRivers.TabIndex = 10;
            this.comboBoxuseOSMRivers.SelectedIndexChanged += new System.EventHandler(this.comboBoxuseOSMRivers_SelectedIndexChanged);
            // 
            // checkBoxuseOSMRLakes
            // 
            this.checkBoxuseOSMRLakes.AutoSize = true;
            this.checkBoxuseOSMRLakes.Location = new System.Drawing.Point(20, 136);
            this.checkBoxuseOSMRLakes.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxuseOSMRLakes.Name = "checkBoxuseOSMRLakes";
            this.checkBoxuseOSMRLakes.Size = new System.Drawing.Size(55, 17);
            this.checkBoxuseOSMRLakes.TabIndex = 13;
            this.checkBoxuseOSMRLakes.Text = "Lakes";
            this.checkBoxuseOSMRLakes.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(103, 138);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Min area:";
            // 
            // textBoxninLakesSize
            // 
            this.textBoxninLakesSize.Location = new System.Drawing.Point(153, 133);
            this.textBoxninLakesSize.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxninLakesSize.Name = "textBoxninLakesSize";
            this.textBoxninLakesSize.Size = new System.Drawing.Size(83, 20);
            this.textBoxninLakesSize.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(240, 136);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Square kilometers";
            // 
            // checkBoxuseOSMRCity
            // 
            this.checkBoxuseOSMRCity.AutoSize = true;
            this.checkBoxuseOSMRCity.Location = new System.Drawing.Point(20, 167);
            this.checkBoxuseOSMRCity.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxuseOSMRCity.Name = "checkBoxuseOSMRCity";
            this.checkBoxuseOSMRCity.Size = new System.Drawing.Size(48, 17);
            this.checkBoxuseOSMRCity.TabIndex = 19;
            this.checkBoxuseOSMRCity.Text = "Citys";
            this.checkBoxuseOSMRCity.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(104, 169);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Detail";
            // 
            // comboBoxOSMRCity
            // 
            this.comboBoxOSMRCity.FormattingEnabled = true;
            this.comboBoxOSMRCity.Items.AddRange(new object[] {
            "VeyHigh",
            "High",
            "Medium",
            "Low"});
            this.comboBoxOSMRCity.Location = new System.Drawing.Point(153, 167);
            this.comboBoxOSMRCity.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxOSMRCity.Name = "comboBoxOSMRCity";
            this.comboBoxOSMRCity.Size = new System.Drawing.Size(83, 21);
            this.comboBoxOSMRCity.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(240, 202);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Square kilometers";
            // 
            // textBoxCitySize
            // 
            this.textBoxCitySize.Location = new System.Drawing.Point(153, 199);
            this.textBoxCitySize.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCitySize.Name = "textBoxCitySize";
            this.textBoxCitySize.Size = new System.Drawing.Size(83, 20);
            this.textBoxCitySize.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(103, 204);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Min area:";
            // 
            // checkBoxuseOSMRResidential
            // 
            this.checkBoxuseOSMRResidential.AutoSize = true;
            this.checkBoxuseOSMRResidential.Location = new System.Drawing.Point(20, 202);
            this.checkBoxuseOSMRResidential.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxuseOSMRResidential.Name = "checkBoxuseOSMRResidential";
            this.checkBoxuseOSMRResidential.Size = new System.Drawing.Size(84, 17);
            this.checkBoxuseOSMRResidential.TabIndex = 20;
            this.checkBoxuseOSMRResidential.Text = "Urban areas";
            this.checkBoxuseOSMRResidential.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxCitySize);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.checkBoxuseOSMRResidential);
            this.groupBox1.Controls.Add(this.checkBoxuseOSMRCity);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBoxOSMRCity);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxninLakesSize);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.checkBoxuseOSMRLakes);
            this.groupBox1.Controls.Add(this.checkBoxuseOSMRivers);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxuseOSMRivers);
            this.groupBox1.Controls.Add(this.checkBoxuseOSMRail);
            this.groupBox1.Controls.Add(this.checkBoxuseOSMRoads);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(355, 246);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OpenStreetMap";
            // 
            // FormOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(385, 409);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancell);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormOption";
            this.Text = "Option";
            this.Load += new System.EventHandler(this.FormOption_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSimplification;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancell;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxuseOSMRoads;
        private System.Windows.Forms.CheckBox checkBoxuseOSMRail;
        private System.Windows.Forms.CheckBox checkBoxuseOSMRivers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxuseOSMRivers;
        private System.Windows.Forms.CheckBox checkBoxuseOSMRLakes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxninLakesSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxuseOSMRCity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxOSMRCity;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxCitySize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxuseOSMRResidential;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}