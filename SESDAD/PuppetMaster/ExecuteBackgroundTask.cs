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

        private Object callObject;

        private string executeFunction;

        public ExecuteBackgroundTask(string textForm,string labelScript,string executeFunction,Object argument,Object callObject)
        {
            InitializeComponent();
            this.Text = textForm;
            this.labelScript.Text = labelScript;
            this.executeFunction = executeFunction;
            this.callObject = callObject;
            backgroundWorkerScript.RunWorkerAsync(argument);
        }

        private void ExecuteScriptForm_Load(object sender, EventArgs e)
        {
            progressBarScript.Maximum = 100;
        }

        //BackgroundWorker events

        private void backgroundWorkerScript_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] param = new object[2];
            param[0] = e.Argument;
            param[1] = sender as BackgroundWorker;
            callObject.GetType().GetMethod(executeFunction).Invoke(callObject,param);
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
            {
                labelScript.Text = "Execution Completed";
                this.Close();
            }
        }
    }
}
