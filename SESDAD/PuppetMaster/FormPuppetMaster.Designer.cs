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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxSubUnsub = new System.Windows.Forms.ComboBox();
            this.comboBoxTopicSub = new System.Windows.Forms.ComboBox();
            this.buttonUnsubscribe = new System.Windows.Forms.Button();
            this.buttonSubscribe = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxPublish = new System.Windows.Forms.ComboBox();
            this.comboBoxTopicPublish = new System.Windows.Forms.ComboBox();
            this.textBoxInterval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNrEvents = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.buttonStatus = new System.Windows.Forms.Button();
            this.buttonCrashAll = new System.Windows.Forms.Button();
            this.buttonCrashProcess = new System.Windows.Forms.Button();
            this.buttonFreeze = new System.Windows.Forms.Button();
            this.buttonUnfreeze = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxProcesses = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.treeViewLogFiles = new System.Windows.Forms.TreeView();
            this.label10 = new System.Windows.Forms.Label();
            this.treeViewScriptFiles = new System.Windows.Forms.TreeView();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBoxDebug = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewConfigFiles
            // 
            this.treeViewConfigFiles.Location = new System.Drawing.Point(760, 50);
            this.treeViewConfigFiles.Name = "treeViewConfigFiles";
            this.treeViewConfigFiles.Size = new System.Drawing.Size(192, 99);
            this.treeViewConfigFiles.TabIndex = 0;
            this.treeViewConfigFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewConfigFiles_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(760, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Configuration Files";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.comboBoxSubUnsub);
            this.groupBox1.Controls.Add(this.comboBoxTopicSub);
            this.groupBox1.Controls.Add(this.buttonUnsubscribe);
            this.groupBox1.Controls.Add(this.buttonSubscribe);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 163);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Subscribe/Unsubscribe";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Process Name";
            // 
            // comboBoxSubUnsub
            // 
            this.comboBoxSubUnsub.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSubUnsub.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSubUnsub.FormattingEnabled = true;
            this.comboBoxSubUnsub.Location = new System.Drawing.Point(7, 45);
            this.comboBoxSubUnsub.Name = "comboBoxSubUnsub";
            this.comboBoxSubUnsub.Size = new System.Drawing.Size(354, 21);
            this.comboBoxSubUnsub.TabIndex = 17;
            // 
            // comboBoxTopicSub
            // 
            this.comboBoxTopicSub.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTopicSub.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTopicSub.FormattingEnabled = true;
            this.comboBoxTopicSub.Location = new System.Drawing.Point(7, 94);
            this.comboBoxTopicSub.Name = "comboBoxTopicSub";
            this.comboBoxTopicSub.Size = new System.Drawing.Size(354, 21);
            this.comboBoxTopicSub.TabIndex = 7;
            // 
            // buttonUnsubscribe
            // 
            this.buttonUnsubscribe.Location = new System.Drawing.Point(177, 121);
            this.buttonUnsubscribe.Name = "buttonUnsubscribe";
            this.buttonUnsubscribe.Size = new System.Drawing.Size(184, 21);
            this.buttonUnsubscribe.TabIndex = 6;
            this.buttonUnsubscribe.Text = "Unsubscribe";
            this.buttonUnsubscribe.UseVisualStyleBackColor = true;
            this.buttonUnsubscribe.Click += new System.EventHandler(this.buttonUnsubscribe_Click);
            // 
            // buttonSubscribe
            // 
            this.buttonSubscribe.Location = new System.Drawing.Point(7, 121);
            this.buttonSubscribe.Name = "buttonSubscribe";
            this.buttonSubscribe.Size = new System.Drawing.Size(165, 23);
            this.buttonSubscribe.TabIndex = 6;
            this.buttonSubscribe.Text = "Subscribe";
            this.buttonSubscribe.UseVisualStyleBackColor = true;
            this.buttonSubscribe.Click += new System.EventHandler(this.buttonSubscribe_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Topic";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.comboBoxPublish);
            this.groupBox2.Controls.Add(this.comboBoxTopicPublish);
            this.groupBox2.Controls.Add(this.textBoxInterval);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxNrEvents);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonPublish);
            this.groupBox2.Location = new System.Drawing.Point(385, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 204);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Publish";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Process Name";
            // 
            // comboBoxPublish
            // 
            this.comboBoxPublish.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxPublish.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPublish.FormattingEnabled = true;
            this.comboBoxPublish.Location = new System.Drawing.Point(11, 45);
            this.comboBoxPublish.Name = "comboBoxPublish";
            this.comboBoxPublish.Size = new System.Drawing.Size(349, 21);
            this.comboBoxPublish.TabIndex = 18;
            // 
            // comboBoxTopicPublish
            // 
            this.comboBoxTopicPublish.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTopicPublish.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTopicPublish.FormattingEnabled = true;
            this.comboBoxTopicPublish.Location = new System.Drawing.Point(11, 94);
            this.comboBoxTopicPublish.Name = "comboBoxTopicPublish";
            this.comboBoxTopicPublish.Size = new System.Drawing.Size(349, 21);
            this.comboBoxTopicPublish.TabIndex = 8;
            // 
            // textBoxInterval
            // 
            this.textBoxInterval.Location = new System.Drawing.Point(182, 149);
            this.textBoxInterval.Name = "textBoxInterval";
            this.textBoxInterval.Size = new System.Drawing.Size(181, 20);
            this.textBoxInterval.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(180, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Interval";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Number of Events";
            // 
            // textBoxNrEvents
            // 
            this.textBoxNrEvents.Location = new System.Drawing.Point(9, 149);
            this.textBoxNrEvents.Name = "textBoxNrEvents";
            this.textBoxNrEvents.Size = new System.Drawing.Size(167, 20);
            this.textBoxNrEvents.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Topic";
            // 
            // buttonPublish
            // 
            this.buttonPublish.Location = new System.Drawing.Point(8, 175);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(355, 23);
            this.buttonPublish.TabIndex = 8;
            this.buttonPublish.Text = "Publish";
            this.buttonPublish.UseVisualStyleBackColor = true;
            this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
            // 
            // buttonStatus
            // 
            this.buttonStatus.Location = new System.Drawing.Point(6, 56);
            this.buttonStatus.Name = "buttonStatus";
            this.buttonStatus.Size = new System.Drawing.Size(357, 23);
            this.buttonStatus.TabIndex = 12;
            this.buttonStatus.Text = "Status";
            this.buttonStatus.UseVisualStyleBackColor = true;
            this.buttonStatus.Click += new System.EventHandler(this.buttonStatus_Click);
            // 
            // buttonCrashAll
            // 
            this.buttonCrashAll.Location = new System.Drawing.Point(6, 27);
            this.buttonCrashAll.Name = "buttonCrashAll";
            this.buttonCrashAll.Size = new System.Drawing.Size(357, 23);
            this.buttonCrashAll.TabIndex = 13;
            this.buttonCrashAll.Text = "Crash All Processes";
            this.buttonCrashAll.UseVisualStyleBackColor = true;
            this.buttonCrashAll.Click += new System.EventHandler(this.buttonCrashAll_Click);
            // 
            // buttonCrashProcess
            // 
            this.buttonCrashProcess.Location = new System.Drawing.Point(11, 71);
            this.buttonCrashProcess.Name = "buttonCrashProcess";
            this.buttonCrashProcess.Size = new System.Drawing.Size(352, 23);
            this.buttonCrashProcess.TabIndex = 14;
            this.buttonCrashProcess.Text = "Crash Process";
            this.buttonCrashProcess.UseVisualStyleBackColor = true;
            this.buttonCrashProcess.Click += new System.EventHandler(this.buttonCrashProcess_Click);
            // 
            // buttonFreeze
            // 
            this.buttonFreeze.Location = new System.Drawing.Point(12, 100);
            this.buttonFreeze.Name = "buttonFreeze";
            this.buttonFreeze.Size = new System.Drawing.Size(351, 23);
            this.buttonFreeze.TabIndex = 15;
            this.buttonFreeze.Text = "Freeze";
            this.buttonFreeze.UseVisualStyleBackColor = true;
            this.buttonFreeze.Click += new System.EventHandler(this.buttonFreeze_Click);
            // 
            // buttonUnfreeze
            // 
            this.buttonUnfreeze.Location = new System.Drawing.Point(12, 129);
            this.buttonUnfreeze.Name = "buttonUnfreeze";
            this.buttonUnfreeze.Size = new System.Drawing.Size(351, 23);
            this.buttonUnfreeze.TabIndex = 16;
            this.buttonUnfreeze.Text = "Unfreeze";
            this.buttonUnfreeze.UseVisualStyleBackColor = true;
            this.buttonUnfreeze.Click += new System.EventHandler(this.buttonUnfreeze_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.buttonUnfreeze);
            this.groupBox3.Controls.Add(this.comboBoxProcesses);
            this.groupBox3.Controls.Add(this.buttonFreeze);
            this.groupBox3.Controls.Add(this.buttonCrashProcess);
            this.groupBox3.Location = new System.Drawing.Point(10, 209);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(369, 165);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Generic Operations";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Process Name";
            // 
            // comboBoxProcesses
            // 
            this.comboBoxProcesses.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxProcesses.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxProcesses.FormattingEnabled = true;
            this.comboBoxProcesses.Location = new System.Drawing.Point(11, 44);
            this.comboBoxProcesses.Name = "comboBoxProcesses";
            this.comboBoxProcesses.Size = new System.Drawing.Size(352, 21);
            this.comboBoxProcesses.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(244, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(265, 29);
            this.label9.TabIndex = 18;
            this.label9.Text = "Puppet Master Console";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonCrashAll);
            this.groupBox4.Controls.Add(this.buttonStatus);
            this.groupBox4.Location = new System.Drawing.Point(385, 253);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(369, 94);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Broadcast Operations";
            // 
            // treeViewLogFiles
            // 
            this.treeViewLogFiles.Location = new System.Drawing.Point(760, 168);
            this.treeViewLogFiles.Name = "treeViewLogFiles";
            this.treeViewLogFiles.Size = new System.Drawing.Size(192, 86);
            this.treeViewLogFiles.TabIndex = 20;
            this.treeViewLogFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewLogFiles_KeyUp);
            this.treeViewLogFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewLogFiles_MouseDoubleClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(760, 152);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Log Files";
            // 
            // treeViewScriptFiles
            // 
            this.treeViewScriptFiles.Enabled = false;
            this.treeViewScriptFiles.Location = new System.Drawing.Point(763, 280);
            this.treeViewScriptFiles.Name = "treeViewScriptFiles";
            this.treeViewScriptFiles.Size = new System.Drawing.Size(192, 94);
            this.treeViewScriptFiles.TabIndex = 22;
            this.treeViewScriptFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewScriptFiles_MouseDoubleClick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(760, 261);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Script Files";
            // 
            // checkBoxDebug
            // 
            this.checkBoxDebug.AutoSize = true;
            this.checkBoxDebug.Checked = true;
            this.checkBoxDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDebug.Location = new System.Drawing.Point(760, 12);
            this.checkBoxDebug.Name = "checkBoxDebug";
            this.checkBoxDebug.Size = new System.Drawing.Size(58, 17);
            this.checkBoxDebug.TabIndex = 25;
            this.checkBoxDebug.Text = "Debug";
            this.checkBoxDebug.UseVisualStyleBackColor = true;
            this.checkBoxDebug.CheckedChanged += new System.EventHandler(this.checkBoxDebug_CheckedChanged);
            // 
            // FormPuppetMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 382);
            this.Controls.Add(this.checkBoxDebug);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.treeViewScriptFiles);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.treeViewLogFiles);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewConfigFiles);
            this.Name = "FormPuppetMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PuppetMaster";
            this.Load += new System.EventHandler(this.FormPuppetMaster_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewConfigFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSubscribe;
        private System.Windows.Forms.Button buttonPublish;
        private System.Windows.Forms.Button buttonUnsubscribe;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNrEvents;
        private System.Windows.Forms.Button buttonStatus;
        private System.Windows.Forms.Button buttonCrashAll;
        private System.Windows.Forms.Button buttonCrashProcess;
        private System.Windows.Forms.Button buttonFreeze;
        private System.Windows.Forms.Button buttonUnfreeze;
        private System.Windows.Forms.ComboBox comboBoxTopicSub;
        private System.Windows.Forms.ComboBox comboBoxTopicPublish;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxProcesses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxSubUnsub;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxPublish;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TreeView treeViewLogFiles;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TreeView treeViewScriptFiles;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxDebug;
    }
}

