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

        private ConfigurationManager manager;

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
                ConfigurationManager.LOG_FILES_DIRECTORY);
            foreach (string file in files)
                treeViewLogFiles.Nodes.Add(Path.GetFileName(file));
        }

        private void FormPuppetMaster_Load(object sender, EventArgs e)
        {
            manager = new ConfigurationManager(this);
            IEnumerable<string> files = Directory.GetFiles(
                ConfigurationManager.CONFIG_FILES_DIRECTORY);
            foreach(string file in files)
                treeViewConfigFiles.Nodes.Add(Path.GetFileName(file));
            ReloadLogFiles();
        }

        private void treeViewConfigFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            manager.ReadConfigurationFile(
                ConfigurationManager.CONFIG_FILES_DIRECTORY + tree.SelectedNode.Text);
        }

        private void treeViewLogFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            Process.Start(ConfigurationManager.LOG_FILES_DIRECTORY+tree.SelectedNode.Text);
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            //TODO
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

        private void treeViewLogFiles_KeyUp(object sender, KeyEventArgs e)
        {
            TreeView tree = sender as TreeView;
            if(null != tree.SelectedNode && e.KeyCode == Keys.Delete)
            {
                File.Delete(ConfigurationManager.LOG_FILES_DIRECTORY + tree.SelectedNode.Text);
            }
            ReloadLogFiles();
        }
    }
}
