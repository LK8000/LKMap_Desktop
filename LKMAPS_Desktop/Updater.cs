// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright The LKMap Desktop Project

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;

namespace LKMAPS_Desktop
{
    public    class Updater
    {
        private LKMAPS_Desktop form;

        public Updater(LKMAPS_Desktop form)
        {
            this.form = form;
        }

        public  bool checkUpdate()
        {
            
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;

                var webRequest = WebRequest.Create(@"http://www.vololiberomontecucco.it/LKMAPS_Desktop/version.txt");
                string strContent;
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    strContent = reader.ReadToEnd();
                }
                if (strContent == version)
                    return false;

                DialogResult dialogResult = MessageBox.Show("An updated version is available.\nDo you want to download it?", "Updater", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    form.setStatus("Updating ... ");
                    form.enableControls(false);
                    WebClient client = new WebClient();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    string outfile = Path.GetTempPath() + "/LKMAPS_Desktop.exe";
                    client.DownloadFileAsync(new Uri("http://www.vololiberomontecucco.it/LKMAPS_Desktop/LKMAPS_Desktop_upd.exe"), outfile);
                }
                else if (dialogResult == DialogResult.No)
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }




            return true;
        }


        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            int percentage = (int) ( bytesIn / totalBytes * 100 );

            form.SetProgressDownload(percentage);


        }
        void  client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string outfile = Path.GetTempPath() + "/LKMAPS_Desktop.exe";
                System.Diagnostics.Process.Start(outfile);
                System.Windows.Forms.Application.Exit();
            }
            catch
            {
                MessageBox.Show("Error updating .... sorry" );
                form.enableControls(true);
            }
        }




    }
}
