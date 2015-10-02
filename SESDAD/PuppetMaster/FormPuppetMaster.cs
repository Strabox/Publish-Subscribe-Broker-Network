using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuppetMaster
{
    public partial class FormPuppetMaster : Form
    {

        public FormPuppetMaster()
        {
            InitializeComponent();
        }

        private void FormPuppetMaster_Load(object sender, EventArgs e)
        {
            IEnumerable<string> files = Directory.GetFiles(
                ConfigurationManager.CONFIG_FILES_DIRECTORY);
            foreach(string file in files)
                treeViewConfigFiles.Nodes.Add(Path.GetFileName(file));
        }

        private void treeViewConfigFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeView tree = sender as TreeView;
            ConfigurationManager.ReadConfigurationFile(
                ConfigurationManager.CONFIG_FILES_DIRECTORY + tree.SelectedNode.Text);
        }
    }
}
