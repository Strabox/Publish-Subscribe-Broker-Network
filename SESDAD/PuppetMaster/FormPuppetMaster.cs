using CommonTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuppetMaster
{
    public delegate void AddProcess(string processName);

    public delegate void Enable(bool enable);

    public partial class FormPuppetMaster : Form
    {

        private ProcessesManager manager;

        private static bool debug;

        //Used do activate de Debug Messages to Debug Console.
        public static bool Debug
        {
            get
            {
                return debug;
            }
        }

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

        public void EnableScriptFiles(bool enable)
        {
            treeViewScriptFiles.Enabled = enable;
        }

        public void EnableConfigFiles(bool enable)
        {
            treeViewConfigFiles.Enabled = enable;
        }

        public void ReloadLogFiles()
        {
            treeViewLogFiles.Nodes.Clear();
            IEnumerable<string> files = Directory.GetFiles(
                LogServer.LOG_FILES_DIRECTORY);
            foreach (string file in files)
            {
                if(!Path.GetFileName(file).Equals(".gitkeep"))
                    treeViewLogFiles.Nodes.Add(Path.GetFileName(file));
            }
        }

        /* ########################## Events ########################### */

        private void FormPuppetMaster_Load(object sender, EventArgs e)
        {
            TcpChannel channel = new TcpChannel(CommonUtil.PUPPET_MASTER_PORT);
            ChannelServices.RegisterChannel(channel, false);
            manager = new ProcessesManager(this);
            IEnumerable<string> files = Directory.GetFiles(
                ProcessesManager.CONFIG_FILES_DIRECTORY);
            foreach(string file in files)
                treeViewConfigFiles.Nodes.Add(Path.GetFileName(file));
            files = Directory.GetFiles(
                ProcessesManager.SCRIPT_FILES_DIRECTORY);
            foreach (string file in files)
                treeViewScriptFiles.Nodes.Add(Path.GetFileName(file));
            ReloadLogFiles();
        }

        private void treeViewLogFiles_KeyUp(object sender, KeyEventArgs e)
        {
            TreeView tree = sender as TreeView;
            if (null != tree.SelectedNode && e.KeyCode == Keys.Delete)
            {
                File.Delete(LogServer.LOG_FILES_DIRECTORY + tree.SelectedNode.Text);
            }
            ReloadLogFiles();
        }

        private void treeViewScriptFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            ExecuteBackgroundTask form = new ExecuteBackgroundTask("Script Execution","ExecuteScriptFile",
                ProcessesManager.SCRIPT_FILES_DIRECTORY + tree.SelectedNode.Text,manager);
            form.ShowDialog();
        }

        private void treeViewConfigFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            manager.LaunchConfigurationFile(
                ProcessesManager.CONFIG_FILES_DIRECTORY + tree.SelectedNode.Text);
            tree.Enabled = false;
            treeViewScriptFiles.Enabled = true;
        }

        private void treeViewLogFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            Process.Start(LogServer.LOG_FILES_DIRECTORY + tree.SelectedNode.Text);
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
            manager.Freeze(comboBoxProcesses.Text);
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            manager.Publish(comboBoxPublish.Text, int.Parse(textBoxInterval.Text),
                int.Parse(textBoxNrEvents.Text), comboBoxTopicPublish.Text);
        }

        private void buttonUnsubscribe_Click(object sender, EventArgs e)
        {
            manager.Unsubscribe(comboBoxSubUnsub.Text, comboBoxTopicSub.Text);
        }

        private void buttonUnfreeze_Click(object sender, EventArgs e)
        {
            manager.Unfreeze(comboBoxProcesses.Text);
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            manager.Status();
        }

        private void buttonSubscribe_Click(object sender, EventArgs e)
        {
            manager.Subscribe(comboBoxSubUnsub.Text, comboBoxTopicSub.Text);
        }

        private void checkBoxDebug_CheckedChanged(object sender, EventArgs e)
        {
            debug = (sender as CheckBox).Checked;
        }
    }
}
