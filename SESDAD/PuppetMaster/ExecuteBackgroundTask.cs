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

    public partial class ExecuteBackgroundTask : Form
    {

        private ProcessesManager manager;

        private string executeFunction;

        public ExecuteBackgroundTask(string textForm,
            string executeFunction,string scriptFile,ProcessesManager form)
        {
            InitializeComponent();
            this.Text = textForm;
            this.executeFunction = executeFunction;
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
            object[] param = new object[2];
            param[0] = e.Argument as string;
            param[1] = sender as BackgroundWorker;
            manager.GetType().GetMethod(executeFunction).Invoke(manager,param);
        }

        private void backgroundWorkerScript_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
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
            else if (e.Cancelled)
            {
                labelScript.Text = "Error in Execution";
            }
            else
                labelScript.Text = "Execution Completed";
        }
    }
}
