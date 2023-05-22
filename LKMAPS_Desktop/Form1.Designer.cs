using OSGeo.GDAL;
using OSGeo.OSR;


namespace LKMAPS_Desktop
{
    partial class LKMAPS_Desktop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LKMAPS_Desktop));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.richTextBoxHelp = new System.Windows.Forms.RichTextBox();
            this.buttonZoomDown = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.progressBarPartial = new System.Windows.Forms.ProgressBar();
            this.l = new System.Windows.Forms.Label();
            this.textBoxMapName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Longitude2Label = new System.Windows.Forms.Label();
            this.Logitude1Label = new System.Windows.Forms.Label();
            this.Latitude2Label = new System.Windows.Forms.Label();
            this.Latitude1label = new System.Windows.Forms.Label();
            this.textBoxLatMin = new System.Windows.Forms.TextBox();
            this.textBoxLatMax = new System.Windows.Forms.TextBox();
            this.textBoxLonMin = new System.Windows.Forms.TextBox();
            this.textBoxLonMax = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HelpUtilitiesButton = new System.Windows.Forms.Button();
            this.buttonOfflineTopology = new System.Windows.Forms.Button();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelPixelSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarPixelSize = new System.Windows.Forms.TrackBar();
            this.buttonCreateTopology = new System.Windows.Forms.Button();
            this.buttonZoomUp = new System.Windows.Forms.Button();
            this.checkBoxShowExistingMaps = new System.Windows.Forms.CheckBox();
            this.buttonCreateTerrain = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxOutFolder = new System.Windows.Forms.TextBox();
            this.buttonSelectOutFolder = new System.Windows.Forms.Button();
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerLKM = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerFakeProgress = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerOSM = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerOffline = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPixelSize)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxHelp);
            this.splitContainer1.Panel1.Controls.Add(this.buttonZoomDown);
            this.splitContainer1.Panel1.Controls.Add(this.trackBar1);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarTotal);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarPartial);
            this.splitContainer1.Panel1.Controls.Add(this.l);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxMapName);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Resize += new System.EventHandler(this.splitContainer1_Panel1_Resize);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gmap);
            this.splitContainer1.Size = new System.Drawing.Size(1509, 1054);
            this.splitContainer1.SplitterDistance = 579;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 954);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(579, 100);
            this.panel2.TabIndex = 32;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(0, 70);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(226, 28);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.lk8000.it";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // richTextBoxHelp
            // 
            this.richTextBoxHelp.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.richTextBoxHelp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxHelp.Location = new System.Drawing.Point(7, 1039);
            this.richTextBoxHelp.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.richTextBoxHelp.Name = "richTextBoxHelp";
            this.richTextBoxHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxHelp.Size = new System.Drawing.Size(572, 632);
            this.richTextBoxHelp.TabIndex = 0;
            this.richTextBoxHelp.Text = "";
            // 
            // buttonZoomDown
            // 
            this.buttonZoomDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonZoomDown.Location = new System.Drawing.Point(440, 882);
            this.buttonZoomDown.Margin = new System.Windows.Forms.Padding(0);
            this.buttonZoomDown.Name = "buttonZoomDown";
            this.buttonZoomDown.Size = new System.Drawing.Size(108, 50);
            this.buttonZoomDown.TabIndex = 31;
            this.buttonZoomDown.Text = "-";
            this.buttonZoomDown.UseVisualStyleBackColor = true;
            this.buttonZoomDown.Click += new System.EventHandler(this.buttonZoomDown_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(456, 273);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(0);
            this.trackBar1.Maximum = 1700;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(80, 598);
            this.trackBar1.TabIndex = 30;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 12;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // progressBarTotal
            // 
            this.progressBarTotal.Location = new System.Drawing.Point(35, 825);
            this.progressBarTotal.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.progressBarTotal.Name = "progressBarTotal";
            this.progressBarTotal.Size = new System.Drawing.Size(389, 42);
            this.progressBarTotal.TabIndex = 15;
            // 
            // progressBarPartial
            // 
            this.progressBarPartial.Location = new System.Drawing.Point(35, 777);
            this.progressBarPartial.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.progressBarPartial.Name = "progressBarPartial";
            this.progressBarPartial.Size = new System.Drawing.Size(389, 42);
            this.progressBarPartial.TabIndex = 14;
            // 
            // l
            // 
            this.l.AutoSize = true;
            this.l.Location = new System.Drawing.Point(33, 216);
            this.l.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.l.Name = "l";
            this.l.Size = new System.Drawing.Size(105, 25);
            this.l.TabIndex = 7;
            this.l.Text = "Map name";
            // 
            // textBoxMapName
            // 
            this.textBoxMapName.Location = new System.Drawing.Point(143, 210);
            this.textBoxMapName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxMapName.Name = "textBoxMapName";
            this.textBoxMapName.Size = new System.Drawing.Size(272, 29);
            this.textBoxMapName.TabIndex = 6;
            this.textBoxMapName.TextChanged += new System.EventHandler(this.textBoxMapName_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(543, 153);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Longitude2Label);
            this.groupBox1.Controls.Add(this.Logitude1Label);
            this.groupBox1.Controls.Add(this.Latitude2Label);
            this.groupBox1.Controls.Add(this.Latitude1label);
            this.groupBox1.Controls.Add(this.textBoxLatMin);
            this.groupBox1.Controls.Add(this.textBoxLatMax);
            this.groupBox1.Controls.Add(this.textBoxLonMin);
            this.groupBox1.Controls.Add(this.textBoxLonMax);
            this.groupBox1.Location = new System.Drawing.Point(31, 369);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(387, 207);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Area";
            // 
            // Longitude2Label
            // 
            this.Longitude2Label.AutoSize = true;
            this.Longitude2Label.Location = new System.Drawing.Point(264, 72);
            this.Longitude2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Longitude2Label.Name = "Longitude2Label";
            this.Longitude2Label.Size = new System.Drawing.Size(114, 25);
            this.Longitude2Label.TabIndex = 37;
            this.Longitude2Label.Text = "Longitude 2";
            // 
            // Logitude1Label
            // 
            this.Logitude1Label.AutoSize = true;
            this.Logitude1Label.Location = new System.Drawing.Point(0, 72);
            this.Logitude1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Logitude1Label.Name = "Logitude1Label";
            this.Logitude1Label.Size = new System.Drawing.Size(114, 25);
            this.Logitude1Label.TabIndex = 34;
            this.Logitude1Label.Text = "Longitude 1";
            // 
            // Latitude2Label
            // 
            this.Latitude2Label.AutoSize = true;
            this.Latitude2Label.Location = new System.Drawing.Point(145, 133);
            this.Latitude2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Latitude2Label.Name = "Latitude2Label";
            this.Latitude2Label.Size = new System.Drawing.Size(97, 25);
            this.Latitude2Label.TabIndex = 37;
            this.Latitude2Label.Text = "Latitude 2";
            // 
            // Latitude1label
            // 
            this.Latitude1label.AutoSize = true;
            this.Latitude1label.Location = new System.Drawing.Point(145, 11);
            this.Latitude1label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Latitude1label.Name = "Latitude1label";
            this.Latitude1label.Size = new System.Drawing.Size(97, 25);
            this.Latitude1label.TabIndex = 33;
            this.Latitude1label.Text = "Latitude 1";
            // 
            // textBoxLatMin
            // 
            this.textBoxLatMin.Location = new System.Drawing.Point(125, 155);
            this.textBoxLatMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLatMin.Name = "textBoxLatMin";
            this.textBoxLatMin.Size = new System.Drawing.Size(136, 29);
            this.textBoxLatMin.TabIndex = 3;
            this.textBoxLatMin.TextChanged += new System.EventHandler(this.textBoxLatMin_TextChanged);
            // 
            // textBoxLatMax
            // 
            this.textBoxLatMax.Location = new System.Drawing.Point(125, 42);
            this.textBoxLatMax.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLatMax.Name = "textBoxLatMax";
            this.textBoxLatMax.Size = new System.Drawing.Size(136, 29);
            this.textBoxLatMax.TabIndex = 2;
            this.textBoxLatMax.TextChanged += new System.EventHandler(this.textBoxLatMax_TextChanged);
            // 
            // textBoxLonMin
            // 
            this.textBoxLonMin.Location = new System.Drawing.Point(7, 96);
            this.textBoxLonMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLonMin.Name = "textBoxLonMin";
            this.textBoxLonMin.Size = new System.Drawing.Size(136, 29);
            this.textBoxLonMin.TabIndex = 1;
            this.textBoxLonMin.TextChanged += new System.EventHandler(this.textBoxLonMin_TextChanged);
            // 
            // textBoxLonMax
            // 
            this.textBoxLonMax.Location = new System.Drawing.Point(240, 96);
            this.textBoxLonMax.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLonMax.Name = "textBoxLonMax";
            this.textBoxLonMax.Size = new System.Drawing.Size(136, 29);
            this.textBoxLonMax.TabIndex = 0;
            this.textBoxLonMax.TextChanged += new System.EventHandler(this.textBoxLonMax_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.HelpUtilitiesButton);
            this.panel1.Controls.Add(this.buttonOfflineTopology);
            this.panel1.Controls.Add(this.buttonOptions);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.buttonCreateTopology);
            this.panel1.Controls.Add(this.buttonZoomUp);
            this.panel1.Controls.Add(this.checkBoxShowExistingMaps);
            this.panel1.Controls.Add(this.buttonCreateTerrain);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.textBoxOutFolder);
            this.panel1.Controls.Add(this.buttonSelectOutFolder);
            this.panel1.Location = new System.Drawing.Point(15, 175);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(545, 833);
            this.panel1.TabIndex = 17;
            // 
            // HelpUtilitiesButton
            // 
            this.HelpUtilitiesButton.Location = new System.Drawing.Point(216, 702);
            this.HelpUtilitiesButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.HelpUtilitiesButton.Name = "HelpUtilitiesButton";
            this.HelpUtilitiesButton.Size = new System.Drawing.Size(191, 52);
            this.HelpUtilitiesButton.TabIndex = 33;
            this.HelpUtilitiesButton.Text = "Help + Utilities";
            this.HelpUtilitiesButton.UseVisualStyleBackColor = true;
            this.HelpUtilitiesButton.Click += new System.EventHandler(this.CreatePbfButton_Click);
            // 
            // buttonOfflineTopology
            // 
            this.buttonOfflineTopology.Location = new System.Drawing.Point(216, 764);
            this.buttonOfflineTopology.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOfflineTopology.Name = "buttonOfflineTopology";
            this.buttonOfflineTopology.Size = new System.Drawing.Size(191, 52);
            this.buttonOfflineTopology.TabIndex = 36;
            this.buttonOfflineTopology.Text = "Offline Topology";
            this.buttonOfflineTopology.UseVisualStyleBackColor = true;
            this.buttonOfflineTopology.Click += new System.EventHandler(this.buttonOfflineTopology_Click);
            // 
            // buttonOptions
            // 
            this.buttonOptions.Location = new System.Drawing.Point(424, 764);
            this.buttonOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(108, 52);
            this.buttonOptions.TabIndex = 35;
            this.buttonOptions.Text = "Options";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelPixelSize);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.trackBarPixelSize);
            this.groupBox2.Location = new System.Drawing.Point(15, 406);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(392, 146);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Terrain Pixel Size";
            // 
            // labelPixelSize
            // 
            this.labelPixelSize.AutoSize = true;
            this.labelPixelSize.Location = new System.Drawing.Point(126, 98);
            this.labelPixelSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPixelSize.Name = "labelPixelSize";
            this.labelPixelSize.Size = new System.Drawing.Size(45, 25);
            this.labelPixelSize.TabIndex = 33;
            this.labelPixelSize.Text = "500";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 25);
            this.label3.TabIndex = 33;
            this.label3.Text = "meters";
            // 
            // trackBarPixelSize
            // 
            this.trackBarPixelSize.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.trackBarPixelSize.LargeChange = 1;
            this.trackBarPixelSize.Location = new System.Drawing.Point(7, 39);
            this.trackBarPixelSize.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarPixelSize.Maximum = 50000;
            this.trackBarPixelSize.Minimum = 9000;
            this.trackBarPixelSize.Name = "trackBarPixelSize";
            this.trackBarPixelSize.Size = new System.Drawing.Size(368, 80);
            this.trackBarPixelSize.TabIndex = 33;
            this.trackBarPixelSize.TickFrequency = 100;
            this.trackBarPixelSize.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarPixelSize.Value = 9000;
            this.trackBarPixelSize.ValueChanged += new System.EventHandler(this.trackBarPixelSize_ValueChanged_1);
            // 
            // buttonCreateTopology
            // 
            this.buttonCreateTopology.Location = new System.Drawing.Point(15, 764);
            this.buttonCreateTopology.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCreateTopology.Name = "buttonCreateTopology";
            this.buttonCreateTopology.Size = new System.Drawing.Size(204, 52);
            this.buttonCreateTopology.TabIndex = 15;
            this.buttonCreateTopology.Text = "Online Topology";
            this.buttonCreateTopology.UseVisualStyleBackColor = true;
            this.buttonCreateTopology.Click += new System.EventHandler(this.buttonCreateTopology_Click);
            // 
            // buttonZoomUp
            // 
            this.buttonZoomUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonZoomUp.Location = new System.Drawing.Point(424, 33);
            this.buttonZoomUp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonZoomUp.Name = "buttonZoomUp";
            this.buttonZoomUp.Size = new System.Drawing.Size(108, 57);
            this.buttonZoomUp.TabIndex = 18;
            this.buttonZoomUp.Text = "+";
            this.buttonZoomUp.UseVisualStyleBackColor = true;
            this.buttonZoomUp.Click += new System.EventHandler(this.buttonZoomUp_Click);
            // 
            // checkBoxShowExistingMaps
            // 
            this.checkBoxShowExistingMaps.AutoSize = true;
            this.checkBoxShowExistingMaps.Location = new System.Drawing.Point(24, 144);
            this.checkBoxShowExistingMaps.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxShowExistingMaps.Name = "checkBoxShowExistingMaps";
            this.checkBoxShowExistingMaps.Size = new System.Drawing.Size(212, 29);
            this.checkBoxShowExistingMaps.TabIndex = 14;
            this.checkBoxShowExistingMaps.Text = "Show existing maps";
            this.checkBoxShowExistingMaps.UseVisualStyleBackColor = true;
            this.checkBoxShowExistingMaps.CheckedChanged += new System.EventHandler(this.checkBoxShowExistingMaps_CheckedChanged);
            // 
            // buttonCreateTerrain
            // 
            this.buttonCreateTerrain.Location = new System.Drawing.Point(18, 702);
            this.buttonCreateTerrain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCreateTerrain.Name = "buttonCreateTerrain";
            this.buttonCreateTerrain.Size = new System.Drawing.Size(198, 52);
            this.buttonCreateTerrain.TabIndex = 13;
            this.buttonCreateTerrain.Text = "Create Terrain";
            this.buttonCreateTerrain.UseVisualStyleBackColor = true;
            this.buttonCreateTerrain.Click += new System.EventHandler(this.buttonCreateTerrain_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "Out Folder";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(24, 567);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 25);
            this.labelStatus.TabIndex = 16;
            // 
            // textBoxOutFolder
            // 
            this.textBoxOutFolder.Location = new System.Drawing.Point(130, 87);
            this.textBoxOutFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxOutFolder.Name = "textBoxOutFolder";
            this.textBoxOutFolder.Size = new System.Drawing.Size(220, 29);
            this.textBoxOutFolder.TabIndex = 10;
            this.textBoxOutFolder.TextChanged += new System.EventHandler(this.textBoxOutFolder_TextChanged);
            // 
            // buttonSelectOutFolder
            // 
            this.buttonSelectOutFolder.Location = new System.Drawing.Point(361, 87);
            this.buttonSelectOutFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSelectOutFolder.Name = "buttonSelectOutFolder";
            this.buttonSelectOutFolder.Size = new System.Drawing.Size(46, 33);
            this.buttonSelectOutFolder.TabIndex = 12;
            this.buttonSelectOutFolder.Text = "...";
            this.buttonSelectOutFolder.UseVisualStyleBackColor = true;
            this.buttonSelectOutFolder.Click += new System.EventHandler(this.buttonSelectOutFolder_Click);
            // 
            // gmap
            // 
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemory = 5;
            this.gmap.Location = new System.Drawing.Point(4, 0);
            this.gmap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 17;
            this.gmap.MinZoom = 1;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(458, 945);
            this.gmap.TabIndex = 1;
            this.gmap.Zoom = 6D;
            this.gmap.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gmap_OnMarkerClick);
            this.gmap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gmap_MouseDown);
            this.gmap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gmap_MouseMove);
            this.gmap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gmap_MouseUp);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorkerLKM
            // 
            this.backgroundWorkerLKM.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLKM_DoWork);
            this.backgroundWorkerLKM.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerLKM_ProgressChanged);
            this.backgroundWorkerLKM.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerLKM_RunWorkerCompleted);
            // 
            // backgroundWorkerFakeProgress
            // 
            this.backgroundWorkerFakeProgress.WorkerReportsProgress = true;
            this.backgroundWorkerFakeProgress.WorkerSupportsCancellation = true;
            this.backgroundWorkerFakeProgress.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerFakeProgress_DoWork);
            // 
            // backgroundWorkerOSM
            // 
            this.backgroundWorkerOSM.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerOSM_DoWork);
            // 
            // backgroundWorkerOffline
            // 
            this.backgroundWorkerOffline.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerOffline_DoWork);
            // 
            // LKMAPS_Desktop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1509, 1054);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LKMAPS_Desktop";
            this.Text = "LKMAPS Desktop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LKMAPS_Desktop_FormClosing);
            this.Load += new System.EventHandler(this.LKMAPS_Desktop_Load);
            this.Resize += new System.EventHandler(this.LKMAPS_Desktop_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPixelSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.TextBox textBoxLatMin;
        private System.Windows.Forms.TextBox textBoxLatMax;
        private System.Windows.Forms.TextBox textBoxLonMin;
        private System.Windows.Forms.TextBox textBoxLonMax;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label l;
        private System.Windows.Forms.TextBox textBoxMapName;
        private System.Windows.Forms.Button buttonSelectOutFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOutFolder;
        private System.Windows.Forms.Button buttonCreateTerrain;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.ProgressBar progressBarPartial;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonZoomDown;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button buttonZoomUp;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBoxHelp;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBoxShowExistingMaps;
        private System.Windows.Forms.Button buttonCreateTopology;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLKM;
        private System.ComponentModel.BackgroundWorker backgroundWorkerFakeProgress;
        private System.Windows.Forms.Label labelPixelSize;
        private System.Windows.Forms.TrackBar trackBarPixelSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonOptions;
        private System.ComponentModel.BackgroundWorker backgroundWorkerOSM;
        private System.Windows.Forms.Button buttonOfflineTopology;
        private System.ComponentModel.BackgroundWorker backgroundWorkerOffline;
        private System.Windows.Forms.Button HelpUtilitiesButton;
        private System.Windows.Forms.Label Longitude2Label;
        private System.Windows.Forms.Label Logitude1Label;
        private System.Windows.Forms.Label Latitude2Label;
        private System.Windows.Forms.Label Latitude1label;
    }
}

