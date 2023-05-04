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
            this.SuspendLayout();
            // 
            // DeleteMapDataFilesButton
            // 
            this.DeleteMapDataFilesButton.Location = new System.Drawing.Point(48, 108);
            this.DeleteMapDataFilesButton.Name = "DeleteMapDataFilesButton";
            this.DeleteMapDataFilesButton.Size = new System.Drawing.Size(180, 84);
            this.DeleteMapDataFilesButton.TabIndex = 3;
            this.DeleteMapDataFilesButton.Text = "Delete .OSM and .PBF Files from Map Folder";
            this.DeleteMapDataFilesButton.UseVisualStyleBackColor = true;
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(48, 36);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(180, 36);
            this.HelpButton.TabIndex = 2;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = true;
            // 
            // AppDataDeleteButton
            // 
            this.AppDataDeleteButton.Location = new System.Drawing.Point(48, 216);
            this.AppDataDeleteButton.Name = "AppDataDeleteButton";
            this.AppDataDeleteButton.Size = new System.Drawing.Size(180, 60);
            this.AppDataDeleteButton.TabIndex = 4;
            this.AppDataDeleteButton.Text = "Delete App Data Folders";
            this.AppDataDeleteButton.UseVisualStyleBackColor = true;
            // 
            // HelpUtilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AppDataDeleteButton);
            this.Controls.Add(this.DeleteMapDataFilesButton);
            this.Controls.Add(this.HelpButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelpUtilities";
            this.Text = "LKMaps Desktop:- Help and Utilities";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DeleteMapDataFilesButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button AppDataDeleteButton;
    }
}