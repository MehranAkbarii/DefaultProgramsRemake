namespace DefaultPrograms {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            listViewUWPApps = new ListView();
            listViewFileExtensions = new ListView();
            SuspendLayout();
            // 
            // listViewUWPApps
            // 
            listViewUWPApps.Location = new Point(12, 12);
            listViewUWPApps.Name = "listViewUWPApps";
            listViewUWPApps.Size = new Size(687, 664);
            listViewUWPApps.TabIndex = 0;
            listViewUWPApps.UseCompatibleStateImageBehavior = false;
            listViewUWPApps.View = View.List;
            listViewUWPApps.ItemSelectionChanged += listViewUWPApps_ItemSelectionChanged;
            // 
            // listViewFileExtensions
            // 
            listViewFileExtensions.Location = new Point(705, 12);
            listViewFileExtensions.MultiSelect = false;
            listViewFileExtensions.Name = "listViewFileExtensions";
            listViewFileExtensions.Size = new Size(541, 664);
            listViewFileExtensions.TabIndex = 1;
            listViewFileExtensions.UseCompatibleStateImageBehavior = false;
            listViewFileExtensions.View = View.List;
            listViewFileExtensions.SelectedIndexChanged += listViewFileExtensions_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 677);
            Controls.Add(listViewFileExtensions);
            Controls.Add(listViewUWPApps);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ListView listViewUWPApps;
        private ListView listViewFileExtensions;
    }
}
