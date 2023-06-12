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

namespace LKMAPS_Desktop
{
    public partial class Splash : Form
    {
        public int timerleft { get; set; }

        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timerleft = 2;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timerleft > 0)
            {
                timerleft -= 1;
            }
            else
            {
                timer1.Stop();
                this.Hide();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }



    }
}
