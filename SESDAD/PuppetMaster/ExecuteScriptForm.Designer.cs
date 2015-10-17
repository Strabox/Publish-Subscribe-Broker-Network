namespace PuppetMaster
{
    partial class ExecuteScriptForm
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
            this.progressBarScript = new System.Windows.Forms.ProgressBar();
            this.labelScript = new System.Windows.Forms.Label();
            this.backgroundWorkerScript = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // progressBarScript
            // 
            this.progressBarScript.Location = new System.Drawing.Point(40, 83);
            this.progressBarScript.Name = "progressBarScript";
            this.progressBarScript.Size = new System.Drawing.Size(324, 23);
            this.progressBarScript.TabIndex = 0;
            // 
            // labelScript
            // 
            this.labelScript.AutoSize = true;
            this.labelScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScript.Location = new System.Drawing.Point(127, 43);
            this.labelScript.Name = "labelScript";
            this.labelScript.Size = new System.Drawing.Size(147, 24);
            this.labelScript.TabIndex = 1;
            this.labelScript.Text = "Executing Script";
            // 
            // backgroundWorkerScript
            // 
            this.backgroundWorkerScript.WorkerReportsProgress = true;
            this.backgroundWorkerScript.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerScript_DoWork);
            this.backgroundWorkerScript.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerScript_ProgressChanged);
            this.backgroundWorkerScript.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerScript_RunWorkerCompleted);
            // 
            // ExecuteScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 174);
            this.Controls.Add(this.labelScript);
            this.Controls.Add(this.progressBarScript);
            this.Name = "ExecuteScriptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Execute Script";
            this.Load += new System.EventHandler(this.ExecuteScriptForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarScript;
        private System.Windows.Forms.Label labelScript;
        private System.ComponentModel.BackgroundWorker backgroundWorkerScript;
    }
}