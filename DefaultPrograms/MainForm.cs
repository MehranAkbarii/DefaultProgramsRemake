using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DefaultPrograms {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void linkDefaultUWPAppsEditor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            new Form1().Show();
        }

        private void buttonClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void linkDefaultProgramsEditor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = AppContext.BaseDirectory +"DefaultProgramsEditor.exe";
            p.StartInfo.Arguments = "-Task DefaultPrograms";
            p.Start();
        }
    }
}
