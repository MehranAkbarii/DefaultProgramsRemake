﻿namespace DefaultPrograms {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            listViewUWPApps = new ListView();
            listViewFileExtensions = new ListView();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // listViewUWPApps
            // 
            listViewUWPApps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listViewUWPApps.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listViewUWPApps.Location = new Point(12, 60);
            listViewUWPApps.MultiSelect = false;
            listViewUWPApps.Name = "listViewUWPApps";
            listViewUWPApps.Size = new Size(377, 490);
            listViewUWPApps.Sorting = SortOrder.Ascending;
            listViewUWPApps.TabIndex = 0;
            listViewUWPApps.UseCompatibleStateImageBehavior = false;
            listViewUWPApps.View = View.Details;
            listViewUWPApps.ItemSelectionChanged += listViewUWPApps_ItemSelectionChanged;
            listViewUWPApps.MouseClick += listViewUWPApps_MouseClick;
            // 
            // listViewFileExtensions
            // 
            listViewFileExtensions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewFileExtensions.Location = new Point(395, 60);
            listViewFileExtensions.MultiSelect = false;
            listViewFileExtensions.Name = "listViewFileExtensions";
            listViewFileExtensions.Size = new Size(377, 490);
            listViewFileExtensions.Sorting = SortOrder.Ascending;
            listViewFileExtensions.TabIndex = 1;
            listViewFileExtensions.UseCompatibleStateImageBehavior = false;
            listViewFileExtensions.View = View.Details;
            listViewFileExtensions.MouseDoubleClick += listViewFileExtensions_MouseDoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.MidnightBlue;
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(142, 21);
            label1.TabIndex = 2;
            label1.Text = "Set defaults by app";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 33);
            label2.Name = "label2";
            label2.Size = new Size(569, 15);
            label2.TabIndex = 3;
            label2.Text = "Select an app to view apps's availible associations, to change the default app, double click on an extension.";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(784, 561);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listViewFileExtensions);
            Controls.Add(listViewUWPApps);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 600);
            Name = "Form1";
            Text = "Set Defaults By App";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listViewUWPApps;
        private ListView listViewFileExtensions;
        private Label label1;
        private Label label2;
    }
}