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
    public delegate void AddProcess(string processName);

    public partial class FormPuppetMaster : Form
    {

        private ProcessesManager manager;


        public FormPuppetMaster()
        {
            InitializeComponent();
        }

        public void AddToSubscribersProcesses(string processName)
        {
            comboBoxSubUnsub.Items.Add(processName);
        }

        public void AddToPublishersProcesses(string processName)
        {
            comboBoxPublish.Items.Add(processName);
        }

        public void AddToGenericProcesses(string processName)
        {
            comboBoxProcesses.Items.Add(processName);
        }

        public void ReloadLogFiles()
        {
            treeViewLogFiles.Nodes.Clear();
            IEnumerable<string> files = Directory.GetFiles(
                LogManager.LOG_FILES_DIRECTORY);
            foreach (string file in files)
                treeViewLogFiles.Nodes.Add(Path.GetFileName(file));
        }

        /* ########################## Events ########################### */

        private void FormPuppetMaster_Load(object sender, EventArgs e)
        {
            manager = new ProcessesManager(this);
            IEnumerable<string> files = Directory.GetFiles(
                ProcessesManager.CONFIG_FILES_DIRECTORY);
            foreach(string file in files)
                treeViewConfigFiles.Nodes.Add(Path.GetFileName(file));
            ReloadLogFiles();
        }

        private void treeViewLogFiles_KeyUp(object sender, KeyEventArgs e)
        {
            TreeView tree = sender as TreeView;
            if (null != tree.SelectedNode && e.KeyCode == Keys.Delete)
            {
                File.Delete(LogManager.LOG_FILES_DIRECTORY + tree.SelectedNode.Text);
            }
            ReloadLogFiles();
        }

        private void treeViewConfigFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            manager.ReadConfigurationFile(
                ProcessesManager.CONFIG_FILES_DIRECTORY + tree.SelectedNode.Text);
        }

        private void treeViewLogFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            Process.Start(LogManager.LOG_FILES_DIRECTORY+tree.SelectedNode.Text);
        }


        private void buttonCrashAll_Click(object sender, EventArgs e)
        {
            manager.CrashAll();
            comboBoxProcesses.Items.Clear();
            comboBoxPublish.Items.Clear();
            comboBoxSubUnsub.Items.Clear();
        }

        private void buttonCrashProcess_Click(object sender, EventArgs e)
        {
            string deletedProcess = comboBoxProcesses.Text;
            manager.Crash(comboBoxProcesses.Text);
            comboBoxProcesses.Items.Remove(deletedProcess);
            comboBoxPublish.Items.Remove(deletedProcess);
            comboBoxSubUnsub.Items.Remove(deletedProcess);
        }

        private void buttonFreeze_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void buttonUnsubscribe_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void buttonUnfreeze_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            manager.Status();
        }

        private void buttonSubscribe_Click(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
