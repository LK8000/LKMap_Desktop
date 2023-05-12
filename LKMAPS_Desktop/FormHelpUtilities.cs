using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKMAPS_Desktop
{
    public partial class FormHelpUtilities : Form
    {
        public FormHelpUtilities()
        {
            InitializeComponent();
        }

        private void AppDataDeleteButton_Click(object sender, EventArgs e)
        {
            var _userFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\LKMAPS\\";
            bool exists = System.IO.Directory.Exists(_userFolder);
            if (exists)
            {
                Directory.Delete(_userFolder, recursive: true);
                exists = System.IO.Directory.Exists(_userFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(_userFolder);
                var _srtmFolder = _userFolder + "srtm\\";
                exists = System.IO.Directory.Exists(_srtmFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(_srtmFolder);
                var _osmFolder = _userFolder + "osm\\";
                exists = System.IO.Directory.Exists(_osmFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(_osmFolder);

                var _vmapFolder = _userFolder + "vmap\\";
                exists = System.IO.Directory.Exists(_vmapFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(_vmapFolder);

                var _tmpFolder = _userFolder + "tmp\\";
                exists = System.IO.Directory.Exists(_tmpFolder);
                if (!exists)
                    System.IO.Directory.CreateDirectory(_tmpFolder);
            }
            MessageBox.Show("App Data Folders Purged Successfully");
        }

        private void ReturnToForm1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void DeleteMapDataFilesButton_Click(object sender, EventArgs e)
        {
            string MapFolder = Properties.Settings.Default.OutFolder;
            //MessageBox.Show(MapFolder);
                //StreamReader sr = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\MapFolder.txt");
            //string MapFolder=sr.ReadLine();
            bool exists = System.IO.Directory.Exists(MapFolder);
            if (exists)
            {
                string[] files = Directory.GetFiles(MapFolder);
                int n = 0;
                foreach (string file in files)
                {
                    var b = file.ToUpper();
                    bool a = b.EndsWith(".PBF");
                    if (a)
                    {
                        File.Delete(file);
                        //MessageBox.Show($"{file} is deleted.");
                        n++;
                    }
                    a = b.EndsWith(".OSM");
                    if (a)
                    {
                        File.Delete(file);
                        //MessageBox.Show($"{file} is deleted.");
                        n++;
                    }

                }
                switch (n)
                {
                    case 0:
                    MessageBox.Show("No Matching Files Found!");
                        break;
                    case 1:
                    MessageBox.Show(n + " File Deleted");
                        break;
                    case int t when n > 1:
                    MessageBox.Show(n + " Files Deleted");
                        break;
                }

            }
        }
    }
}
