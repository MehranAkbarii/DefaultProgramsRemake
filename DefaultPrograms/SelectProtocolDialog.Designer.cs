namespace DefaultPrograms {
    partial class SelectProtocolDialog {
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
            progslistView = new ListView();
            okButton = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // progslistView
            // 
            progslistView.BackColor = SystemColors.Window;
            progslistView.BorderStyle = BorderStyle.None;
            progslistView.FullRowSelect = true;
            progslistView.Location = new Point(12, 57);
            progslistView.MultiSelect = false;
            progslistView.Name = "progslistView";
            progslistView.Size = new Size(400, 341);
            progslistView.TabIndex = 0;
            progslistView.UseCompatibleStateImageBehavior = false;
            progslistView.View = View.List;
            // 
            // okButton
            // 
            okButton.Location = new Point(338, 404);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(10, 10);
            label1.Name = "label1";
            label1.Size = new Size(207, 21);
            label1.TabIndex = 3;
            label1.Text = "How do you want open this?";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ActiveBorder;
            label2.Location = new Point(10, 32);
            label2.Name = "label2";
            label2.Size = new Size(402, 13);
            label2.TabIndex = 4;
            label2.Text = "_______________________________________________________________________________";
            // 
            // SelectProtocolDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(425, 435);
            ControlBox = false;
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(okButton);
            Controls.Add(progslistView);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectProtocolDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Deactivate += SelectProtocolDialog_Deactivate;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView progslistView;
        private Button okButton;
        private Label label1;
        private Label label2;
    }
}