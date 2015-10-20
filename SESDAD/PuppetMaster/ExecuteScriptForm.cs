using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuppetMaster
{

    public partial class ExecuteScriptForm : Form
    {

        private ProcessesManager manager;

        public ExecuteScriptForm(string scriptFile,ProcessesManager form)
        {
            InitializeComponent();
            manager = form;
            backgroundWorkerScript.RunWorkerAsync(scriptFile);
        }

        private void ExecuteScriptForm_Load(object sender, EventArgs e)
        {
            progressBarScript.Maximum = 100;
        }

        //BackgroundWorker events

        private void backgroundWorkerScript_DoWork(object sender, DoWorkEventArgs e)
        {
            manager.ExecuteScriptFile((string) e.Argument,sender as BackgroundWorker);
        }

        private void backgroundWorkerScript_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Debug.WriteLineIf(FormPuppetMaster.Debug,e.ProgressPercentage,"[Script File]");
            progressBarScript.Value = e.ProgressPercentage;
            labelScriptLine.Text = e.UserState as string;
        }

        private void backgroundWorkerScript_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            labelScript.Text = "Execution Completed";
        }
    }
}
