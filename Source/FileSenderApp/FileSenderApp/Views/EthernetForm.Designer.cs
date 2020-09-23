namespace FileSenderApp
{
    partial class EthernetForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EthernetForm));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.ServerIPTbx = new MetroFramework.Controls.MetroTextBox();
            this.PortTbx = new MetroFramework.Controls.MetroTextBox();
            this.SaveBtn = new MetroFramework.Controls.MetroButton();
            this.CancelBtn = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(47, 63);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(69, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Server IP :";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(75, 112);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(41, 19);
            this.metroLabel2.TabIndex = 0;
            this.metroLabel2.Text = "Port :";
            // 
            // ServerIPTbx
            // 
            // 
            // 
            // 
            this.ServerIPTbx.CustomButton.Image = null;
            this.ServerIPTbx.CustomButton.Location = new System.Drawing.Point(166, 1);
            this.ServerIPTbx.CustomButton.Name = "";
            this.ServerIPTbx.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.ServerIPTbx.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.ServerIPTbx.CustomButton.TabIndex = 1;
            this.ServerIPTbx.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ServerIPTbx.CustomButton.UseSelectable = true;
            this.ServerIPTbx.CustomButton.Visible = false;
            this.ServerIPTbx.Lines = new string[0];
            this.ServerIPTbx.Location = new System.Drawing.Point(122, 63);
            this.ServerIPTbx.MaxLength = 32767;
            this.ServerIPTbx.Name = "ServerIPTbx";
            this.ServerIPTbx.PasswordChar = '\0';
            this.ServerIPTbx.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ServerIPTbx.SelectedText = "";
            this.ServerIPTbx.SelectionLength = 0;
            this.ServerIPTbx.SelectionStart = 0;
            this.ServerIPTbx.ShortcutsEnabled = true;
            this.ServerIPTbx.Size = new System.Drawing.Size(188, 23);
            this.ServerIPTbx.TabIndex = 1;
            this.ServerIPTbx.UseSelectable = true;
            this.ServerIPTbx.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.ServerIPTbx.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // PortTbx
            // 
            // 
            // 
            // 
            this.PortTbx.CustomButton.Image = null;
            this.PortTbx.CustomButton.Location = new System.Drawing.Point(166, 1);
            this.PortTbx.CustomButton.Name = "";
            this.PortTbx.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.PortTbx.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.PortTbx.CustomButton.TabIndex = 1;
            this.PortTbx.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.PortTbx.CustomButton.UseSelectable = true;
            this.PortTbx.CustomButton.Visible = false;
            this.PortTbx.Lines = new string[0];
            this.PortTbx.Location = new System.Drawing.Point(122, 112);
            this.PortTbx.MaxLength = 32767;
            this.PortTbx.Name = "PortTbx";
            this.PortTbx.PasswordChar = '\0';
            this.PortTbx.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.PortTbx.SelectedText = "";
            this.PortTbx.SelectionLength = 0;
            this.PortTbx.SelectionStart = 0;
            this.PortTbx.ShortcutsEnabled = true;
            this.PortTbx.Size = new System.Drawing.Size(188, 23);
            this.PortTbx.TabIndex = 2;
            this.PortTbx.UseSelectable = true;
            this.PortTbx.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.PortTbx.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(316, 63);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 3;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseSelectable = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(316, 112);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseSelectable = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // EthernetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 200);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.PortTbx);
            this.Controls.Add(this.ServerIPTbx);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EthernetForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Purple;
            this.Load += new System.EventHandler(this.EthernetForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox ServerIPTbx;
        private MetroFramework.Controls.MetroTextBox PortTbx;
        private MetroFramework.Controls.MetroButton SaveBtn;
        private MetroFramework.Controls.MetroButton CancelBtn;
    }
}