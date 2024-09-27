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
            progslistView.Columns.Add("").Width = 400;
            foreach (var appname in this.appnames) {
                //add and handle customized names for apps 
                if (appname.Key == "Windows Media Player") {
                    ListViewItem item = new ListViewItem("Media Player");
                    item.SubItems.Add("Media Player");
                    progslistView.Items.Add(item);
                } else {
                    ListViewItem item = new ListViewItem(appname.Key);
                    item.SubItems.Add(appname.Key);
                    progslistView.Items.Add(item);
                }
            }
        }
        private void okButton_Click(object sender, EventArgs e) {
            if (progslistView.SelectedItems.Count > 0) {
                //add and handle customized names for apps 
                if (progslistView.SelectedItems[0].Text == "Media Player") {
                    AppAssociation.changeProtocolAssociation(appnames["Windows Media Player"], protocol);
                } else
                    AppAssociation.changeProtocolAssociation(appnames[progslistView.SelectedItems[0].Text], protocol);
            }
            this.Close();
        }

        private void SelectProtocolDialog_Deactivate(object sender, EventArgs e) {
            this.Close();
        }
    }
}
