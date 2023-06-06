// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright The LKMap Desktop Project

using System;
using System.Windows.Forms;

namespace LKMAPS_Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LKMAPS_Desktop());
        }
    }
}
