namespace PuppetMaster
{
    partial class FormPuppetMaster
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
            this.treeViewConfigFiles = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeViewConfigFiles
            // 
            this.treeViewConfigFiles.Location = new System.Drawing.Point(531, 27);
            this.treeViewConfigFiles.Name = "treeViewConfigFiles";
            this.treeViewConfigFiles.Size = new System.Drawing.Size(220, 255);
            this.treeViewConfigFiles.TabIndex = 0;
            this.treeViewConfigFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewConfigFiles_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(528, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Configuration Files";
            // 
            // FormPuppetMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 311);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewConfigFiles);
            this.Name = "FormPuppetMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PuppetMaster";
            this.Load += new System.EventHandler(this.FormPuppetMaster_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewConfigFiles;
        private System.Windows.Forms.Label label1;
    }
}

