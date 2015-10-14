using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuppetMaster
{
    public delegate void DoSomething();

    public partial class ExecuteScriptForm : Form
    {

        private int barMax;

        public ExecuteScriptForm(int barMax)
        {
            InitializeComponent();
            this.barMax = barMax;
        }

        public void IncreaseBar()
        {
            progressBarScript.PerformStep();
        }

        private void ExecuteScriptForm_Load(object sender, EventArgs e)
        {
            progressBarScript.Maximum = barMax;
            progressBarScript.Step = 1;
        }
    }
}
