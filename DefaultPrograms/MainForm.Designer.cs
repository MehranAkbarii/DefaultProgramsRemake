namespace DefaultPrograms {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            linkDefaultUWPAppsEditor = new LinkLabel();
            linkDefaultProgramsEditor = new LinkLabel();
            label1 = new Label();
            buttonClose = new Button();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // linkDefaultUWPAppsEditor
            // 
            linkDefaultUWPAppsEditor.ActiveLinkColor = Color.DodgerBlue;
            linkDefaultUWPAppsEditor.ImageAlign = ContentAlignment.MiddleLeft;
            linkDefaultUWPAppsEditor.LinkBehavior = LinkBehavior.HoverUnderline;
            linkDefaultUWPAppsEditor.LinkColor = Color.Blue;
            linkDefaultUWPAppsEditor.Location = new Point(14, 35);
            linkDefaultUWPAppsEditor.Name = "linkDefaultUWPAppsEditor";
            linkDefaultUWPAppsEditor.Padding = new Padding(0, 10, 0, 0);
            linkDefaultUWPAppsEditor.Size = new Size(121, 25);
            linkDefaultUWPAppsEditor.TabIndex = 0;
            linkDefaultUWPAppsEditor.TabStop = true;
            linkDefaultUWPAppsEditor.Text = "Set Default UWP apps";
            linkDefaultUWPAppsEditor.LinkClicked += linkDefaultUWPAppsEditor_LinkClicked;
            // 
            // linkDefaultProgramsEditor
            // 
            linkDefaultProgramsEditor.ActiveLinkColor = Color.DodgerBlue;
            linkDefaultProgramsEditor.AutoSize = true;
            linkDefaultProgramsEditor.LinkBehavior = LinkBehavior.HoverUnderline;
            linkDefaultProgramsEditor.Location = new Point(14, 92);
            linkDefaultProgramsEditor.Name = "linkDefaultProgramsEditor";
            linkDefaultProgramsEditor.Size = new Size(118, 15);
            linkDefaultProgramsEditor.TabIndex = 1;
            linkDefaultProgramsEditor.TabStop = true;
            linkDefaultProgramsEditor.Text = "Set Default Programs";
            linkDefaultProgramsEditor.LinkClicked += linkDefaultProgramsEditor_LinkClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.MidnightBlue;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(368, 21);
            label1.TabIndex = 3;
            label1.Text = "Choose the programs that Windows uses by default";
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(409, 141);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(75, 23);
            buttonClose.TabIndex = 4;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 62);
            label2.Name = "label2";
            label2.Size = new Size(283, 15);
            label2.TabIndex = 5;
            label2.Text = "Make an app the default for all file types it can open.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 110);
            label3.Name = "label3";
            label3.Size = new Size(302, 15);
            label3.TabIndex = 6;
            label3.Text = "Make a program the default for all file types it can open.";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(496, 176);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(buttonClose);
            Controls.Add(label1);
            Controls.Add(linkDefaultProgramsEditor);
            Controls.Add(linkDefaultUWPAppsEditor);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Set Default Programs";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private LinkLabel linkDefaultUWPAppsEditor;
        private LinkLabel linkDefaultProgramsEditor;
        private Label label1;
        private Button buttonClose;
        private Label label2;
        private Label label3;
    }
}