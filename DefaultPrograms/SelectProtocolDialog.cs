using FileAssociationLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DefaultPrograms {
    public partial class SelectProtocolDialog : Form {

        private Dictionary<string, string> appnames;
        private string protocol;
        public SelectProtocolDialog(Dictionary<string, string> appnames, string protocol) {
            this.appnames = appnames;
            this.protocol = protocol;
            InitializeComponent();
            foreach (var appname in this.appnames) {
                ListViewItem item = new ListViewItem(appname.Key);
                item.SubItems.Add(appname.Key);
                progslistView.Items.Add(item);
            }
        }


        private string getProtocolsDefaultHandlers() {
            //I have no idea how to get this.
            return string.Empty;
        }

        private void okButton_Click(object sender, EventArgs e) {
            if (progslistView.SelectedItems.Count > 0)
                AppAssociation.changeProtocolAssociation(appnames[progslistView.SelectedItems[0].Text], protocol);
            this.Close();
        }

        private void SelectProtocolDialog_Deactivate(object sender, EventArgs e) {
            this.Close();
        }
    }
}
