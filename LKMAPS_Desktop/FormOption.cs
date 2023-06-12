// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright The LKMap Desktop Project

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace LKMAPS_Desktop
{
    public partial class FormOption : Form
    {

        LKMAPS_Desktop mainForm;

        public FormOption()
        {
            InitializeComponent();
        }

        public FormOption(LKMAPS_Desktop main)
        {
            mainForm = main;
            InitializeComponent();
        }

        private void FormOption_Load(object sender, EventArgs e)
        {
            checkBoxuseOSMRoads.Checked = mainForm._useOSMRoads;
            comboBox1.Text = mainForm._roadsDetail;

            checkBoxuseOSMRail.Checked = mainForm._useOSMRail;

            checkBoxuseOSMRivers.Checked = mainForm._useOSMRivers;
            comboBoxuseOSMRivers.Text = mainForm._riverDetail;

            checkBoxuseOSMRLakes.Checked = mainForm._useOSMRLakes;
            textBoxninLakesSize.Text = mainForm._lakesSize.ToString("0.0",CultureInfo.InvariantCulture);

            checkBoxuseOSMRCity.Checked = mainForm._useOSMRCity;
            comboBoxOSMRCity.Text = mainForm._cityDetail;

            checkBoxuseOSMRResidential.Checked = mainForm._useOSMRResidential;
            textBoxCitySize.Text = mainForm._citySize.ToString("0.0", CultureInfo.InvariantCulture);

            textBoxSimplification.Text = mainForm._simplify.ToString("0.0", CultureInfo.InvariantCulture);

            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            mainForm._useOSMRoads = checkBoxuseOSMRoads.Checked;
            mainForm._roadsDetail = comboBox1.Text;

            mainForm._useOSMRail = checkBoxuseOSMRail.Checked;

            mainForm._useOSMRivers = checkBoxuseOSMRivers.Checked;
            mainForm._riverDetail = comboBoxuseOSMRivers.Text;

            mainForm._useOSMRLakes = checkBoxuseOSMRLakes.Checked;
            mainForm._lakesSize = double.Parse(textBoxninLakesSize.Text, CultureInfo.InvariantCulture);

            mainForm._useOSMRCity = checkBoxuseOSMRCity.Checked;
            mainForm._cityDetail = comboBoxOSMRCity.Text;

            mainForm._useOSMRResidential = checkBoxuseOSMRResidential.Checked  ;
            mainForm._citySize = double.Parse(textBoxCitySize.Text, CultureInfo.InvariantCulture);

            mainForm._simplify = double.Parse(textBoxSimplification.Text, CultureInfo.InvariantCulture);
            Close();

        }

        private void buttonCancell_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxuseOSMRivers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
