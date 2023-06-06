// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright The LKMap Desktop Project

namespace LKMAPS_Desktop
{
    partial class FormHelpUtilities
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHelpUtilities));
            this.DeleteMapDataFilesButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.AppDataDeleteButton = new System.Windows.Forms.Button();
            this.ReturnToForm1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DeleteMapDataFilesButton
            // 
            this.DeleteMapDataFilesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.DeleteMapDataFilesButton.Location = new System.Drawing.Point(0, 180);
            this.DeleteMapDataFilesButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeleteMapDataFilesButton.Name = "DeleteMapDataFilesButton";
            this.DeleteMapDataFilesButton.Size = new System.Drawing.Size(248, 108);
            this.DeleteMapDataFilesButton.TabIndex = 3;
            this.DeleteMapDataFilesButton.Text = "Delete  Redundant .OSM and .PBF Files from Map Output Folder";
            this.DeleteMapDataFilesButton.UseVisualStyleBackColor = false;
            this.DeleteMapDataFilesButton.Visible = false;
            this.DeleteMapDataFilesButton.Click += new System.EventHandler(this.DeleteMapDataFilesButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.HelpButton.Location = new System.Drawing.Point(216, 36);
            this.HelpButton.Margin = new System.Windows.Forms.Padding(4);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(248, 108);
            this.HelpButton.TabIndex = 2;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = false;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // AppDataDeleteButton
            // 
            this.AppDataDeleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.AppDataDeleteButton.Location = new System.Drawing.Point(216, 180);
            this.AppDataDeleteButton.Margin = new System.Windows.Forms.Padding(4);
            this.AppDataDeleteButton.Name = "AppDataDeleteButton";
            this.AppDataDeleteButton.Size = new System.Drawing.Size(248, 108);
            this.AppDataDeleteButton.TabIndex = 4;
            this.AppDataDeleteButton.Text = "Purge App Data Folders";
            this.AppDataDeleteButton.UseVisualStyleBackColor = false;
            this.AppDataDeleteButton.Click += new System.EventHandler(this.AppDataDeleteButton_Click);
            // 
            // ReturnToForm1
            // 
            this.ReturnToForm1.BackColor = System.Drawing.Color.Red;
            this.ReturnToForm1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReturnToForm1.ForeColor = System.Drawing.Color.White;
            this.ReturnToForm1.Location = new System.Drawing.Point(216, 336);
            this.ReturnToForm1.Margin = new System.Windows.Forms.Padding(4);
            this.ReturnToForm1.Name = "ReturnToForm1";
            this.ReturnToForm1.Size = new System.Drawing.Size(248, 108);
            this.ReturnToForm1.TabIndex = 5;
            this.ReturnToForm1.Text = "Exit";
            this.ReturnToForm1.UseVisualStyleBackColor = false;
            this.ReturnToForm1.Click += new System.EventHandler(this.ReturnToForm1_Click);
            // 
            // FormHelpUtilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 482);
            this.Controls.Add(this.ReturnToForm1);
            this.Controls.Add(this.AppDataDeleteButton);
            this.Controls.Add(this.DeleteMapDataFilesButton);
            this.Controls.Add(this.HelpButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormHelpUtilities";
            this.Text = "LKMaps Desktop:- Help and Utilities";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DeleteMapDataFilesButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button AppDataDeleteButton;
        private System.Windows.Forms.Button ReturnToForm1;
    }
}