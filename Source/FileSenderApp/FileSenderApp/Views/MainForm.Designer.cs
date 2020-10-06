namespace FileSenderApp
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ConfigBtn = new MetroFramework.Controls.MetroButton();
            this.RS232Rbtn = new MetroFramework.Controls.MetroRadioButton();
            this.EthernetRBtn = new MetroFramework.Controls.MetroRadioButton();
            this.SendRateBar = new MetroFramework.Controls.MetroProgressBar();
            this.SendRateTbx = new MetroFramework.Controls.MetroTextBox();
            this.FileNameTbx = new MetroFramework.Controls.MetroTextBox();
            this.FileLocTbx = new MetroFramework.Controls.MetroTextBox();
            this.ComLinkGbx = new System.Windows.Forms.GroupBox();
            this.MOrSGbx = new System.Windows.Forms.GroupBox();
            this.MasterRBtn = new MetroFramework.Controls.MetroRadioButton();
            this.SlaveRBtn = new MetroFramework.Controls.MetroRadioButton();
            this.ConStateLabel = new MetroFramework.Controls.MetroLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ConnectStateBtn = new System.Windows.Forms.Button();
            this.RecvSendBtn = new MetroFramework.Controls.MetroButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.OpenFileBtn = new MetroFramework.Controls.MetroButton();
            this.HexaCodeTbx = new System.Windows.Forms.RichTextBox();
            this.SaveFileBtn = new MetroFramework.Controls.MetroButton();
            this.ComLinkGbx.SuspendLayout();
            this.MOrSGbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConfigBtn
            // 
            this.ConfigBtn.BackColor = System.Drawing.Color.White;
            this.ConfigBtn.Location = new System.Drawing.Point(324, 76);
            this.ConfigBtn.Name = "ConfigBtn";
            this.ConfigBtn.Size = new System.Drawing.Size(92, 23);
            this.ConfigBtn.Style = MetroFramework.MetroColorStyle.Pink;
            this.ConfigBtn.TabIndex = 1;
            this.ConfigBtn.Text = "Config";
            this.ConfigBtn.UseSelectable = true;
            // 
            // RS232Rbtn
            // 
            this.RS232Rbtn.AutoSize = true;
            this.RS232Rbtn.Location = new System.Drawing.Point(20, 63);
            this.RS232Rbtn.Name = "RS232Rbtn";
            this.RS232Rbtn.Size = new System.Drawing.Size(54, 15);
            this.RS232Rbtn.TabIndex = 2;
            this.RS232Rbtn.Tag = "RS232";
            this.RS232Rbtn.Text = "RS232";
            this.RS232Rbtn.UseSelectable = true;
            // 
            // EthernetRBtn
            // 
            this.EthernetRBtn.AutoSize = true;
            this.EthernetRBtn.Location = new System.Drawing.Point(20, 103);
            this.EthernetRBtn.Name = "EthernetRBtn";
            this.EthernetRBtn.Size = new System.Drawing.Size(67, 15);
            this.EthernetRBtn.TabIndex = 2;
            this.EthernetRBtn.Tag = "Ethernet";
            this.EthernetRBtn.Text = "Ethernet";
            this.EthernetRBtn.UseSelectable = true;
            // 
            // SendRateBar
            // 
            this.SendRateBar.Location = new System.Drawing.Point(34, 341);
            this.SendRateBar.Name = "SendRateBar";
            this.SendRateBar.Size = new System.Drawing.Size(382, 23);
            this.SendRateBar.Style = MetroFramework.MetroColorStyle.Purple;
            this.SendRateBar.TabIndex = 3;
            // 
            // SendRateTbx
            // 
            // 
            // 
            // 
            this.SendRateTbx.CustomButton.Image = null;
            this.SendRateTbx.CustomButton.Location = new System.Drawing.Point(121, 1);
            this.SendRateTbx.CustomButton.Name = "";
            this.SendRateTbx.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.SendRateTbx.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.SendRateTbx.CustomButton.TabIndex = 1;
            this.SendRateTbx.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.SendRateTbx.CustomButton.UseSelectable = true;
            this.SendRateTbx.CustomButton.Visible = false;
            this.SendRateTbx.Lines = new string[] {
        "파일전송진행률"};
            this.SendRateTbx.Location = new System.Drawing.Point(34, 312);
            this.SendRateTbx.MaxLength = 32767;
            this.SendRateTbx.Name = "SendRateTbx";
            this.SendRateTbx.PasswordChar = '\0';
            this.SendRateTbx.ReadOnly = true;
            this.SendRateTbx.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.SendRateTbx.SelectedText = "";
            this.SendRateTbx.SelectionLength = 0;
            this.SendRateTbx.SelectionStart = 0;
            this.SendRateTbx.ShortcutsEnabled = true;
            this.SendRateTbx.Size = new System.Drawing.Size(143, 23);
            this.SendRateTbx.TabIndex = 4;
            this.SendRateTbx.Text = "파일전송진행률";
            this.SendRateTbx.UseSelectable = true;
            this.SendRateTbx.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.SendRateTbx.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // FileNameTbx
            // 
            // 
            // 
            // 
            this.FileNameTbx.CustomButton.Image = null;
            this.FileNameTbx.CustomButton.Location = new System.Drawing.Point(121, 1);
            this.FileNameTbx.CustomButton.Name = "";
            this.FileNameTbx.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.FileNameTbx.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.FileNameTbx.CustomButton.TabIndex = 1;
            this.FileNameTbx.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.FileNameTbx.CustomButton.UseSelectable = true;
            this.FileNameTbx.CustomButton.Visible = false;
            this.FileNameTbx.Lines = new string[] {
        "파일명"};
            this.FileNameTbx.Location = new System.Drawing.Point(34, 234);
            this.FileNameTbx.MaxLength = 32767;
            this.FileNameTbx.Name = "FileNameTbx";
            this.FileNameTbx.PasswordChar = '\0';
            this.FileNameTbx.ReadOnly = true;
            this.FileNameTbx.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FileNameTbx.SelectedText = "";
            this.FileNameTbx.SelectionLength = 0;
            this.FileNameTbx.SelectionStart = 0;
            this.FileNameTbx.ShortcutsEnabled = true;
            this.FileNameTbx.Size = new System.Drawing.Size(143, 23);
            this.FileNameTbx.TabIndex = 6;
            this.FileNameTbx.Text = "파일명";
            this.FileNameTbx.UseSelectable = true;
            this.FileNameTbx.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.FileNameTbx.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // FileLocTbx
            // 
            // 
            // 
            // 
            this.FileLocTbx.CustomButton.Image = null;
            this.FileLocTbx.CustomButton.Location = new System.Drawing.Point(360, 1);
            this.FileLocTbx.CustomButton.Name = "";
            this.FileLocTbx.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.FileLocTbx.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.FileLocTbx.CustomButton.TabIndex = 1;
            this.FileLocTbx.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.FileLocTbx.CustomButton.UseSelectable = true;
            this.FileLocTbx.CustomButton.Visible = false;
            this.FileLocTbx.Lines = new string[0];
            this.FileLocTbx.Location = new System.Drawing.Point(34, 263);
            this.FileLocTbx.MaxLength = 32767;
            this.FileLocTbx.Name = "FileLocTbx";
            this.FileLocTbx.PasswordChar = '\0';
            this.FileLocTbx.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FileLocTbx.SelectedText = "";
            this.FileLocTbx.SelectionLength = 0;
            this.FileLocTbx.SelectionStart = 0;
            this.FileLocTbx.ShortcutsEnabled = true;
            this.FileLocTbx.Size = new System.Drawing.Size(382, 23);
            this.FileLocTbx.TabIndex = 9;
            this.FileLocTbx.UseSelectable = true;
            this.FileLocTbx.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.FileLocTbx.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // ComLinkGbx
            // 
            this.ComLinkGbx.Controls.Add(this.RS232Rbtn);
            this.ComLinkGbx.Controls.Add(this.EthernetRBtn);
            this.ComLinkGbx.Location = new System.Drawing.Point(183, 70);
            this.ComLinkGbx.Name = "ComLinkGbx";
            this.ComLinkGbx.Size = new System.Drawing.Size(121, 137);
            this.ComLinkGbx.TabIndex = 10;
            this.ComLinkGbx.TabStop = false;
            this.ComLinkGbx.Text = "Com Link";
            // 
            // MOrSGbx
            // 
            this.MOrSGbx.Controls.Add(this.MasterRBtn);
            this.MOrSGbx.Controls.Add(this.SlaveRBtn);
            this.MOrSGbx.Location = new System.Drawing.Point(34, 70);
            this.MOrSGbx.Name = "MOrSGbx";
            this.MOrSGbx.Size = new System.Drawing.Size(121, 137);
            this.MOrSGbx.TabIndex = 11;
            this.MOrSGbx.TabStop = false;
            this.MOrSGbx.Text = "M/S";
            // 
            // MasterRBtn
            // 
            this.MasterRBtn.AutoSize = true;
            this.MasterRBtn.Location = new System.Drawing.Point(19, 63);
            this.MasterRBtn.Name = "MasterRBtn";
            this.MasterRBtn.Size = new System.Drawing.Size(59, 15);
            this.MasterRBtn.TabIndex = 2;
            this.MasterRBtn.Tag = "Master";
            this.MasterRBtn.Text = "Master";
            this.MasterRBtn.UseSelectable = true;
            // 
            // SlaveRBtn
            // 
            this.SlaveRBtn.AutoSize = true;
            this.SlaveRBtn.Location = new System.Drawing.Point(19, 103);
            this.SlaveRBtn.Name = "SlaveRBtn";
            this.SlaveRBtn.Size = new System.Drawing.Size(50, 15);
            this.SlaveRBtn.TabIndex = 2;
            this.SlaveRBtn.Tag = "Slave";
            this.SlaveRBtn.Text = "Slave";
            this.SlaveRBtn.UseSelectable = true;
            // 
            // ConStateLabel
            // 
            this.ConStateLabel.AutoSize = true;
            this.ConStateLabel.Location = new System.Drawing.Point(337, 135);
            this.ConStateLabel.Name = "ConStateLabel";
            this.ConStateLabel.Size = new System.Drawing.Size(65, 19);
            this.ConStateLabel.TabIndex = 12;
            this.ConStateLabel.Text = "연결상태";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "모든 파일 (*.*)|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "모든 파일 (*.*)|*.*";
            // 
            // ConnectStateBtn
            // 
            this.ConnectStateBtn.BackColor = System.Drawing.Color.Transparent;
            this.ConnectStateBtn.Location = new System.Drawing.Point(345, 165);
            this.ConnectStateBtn.Name = "ConnectStateBtn";
            this.ConnectStateBtn.Size = new System.Drawing.Size(49, 42);
            this.ConnectStateBtn.TabIndex = 1;
            this.ConnectStateBtn.UseVisualStyleBackColor = false;
            // 
            // RecvSendBtn
            // 
            this.RecvSendBtn.Location = new System.Drawing.Point(324, 234);
            this.RecvSendBtn.Name = "RecvSendBtn";
            this.RecvSendBtn.Size = new System.Drawing.Size(92, 23);
            this.RecvSendBtn.TabIndex = 13;
            this.RecvSendBtn.Text = "Receive/Send";
            this.RecvSendBtn.UseSelectable = true;
            this.RecvSendBtn.Click += new System.EventHandler(this.RecvSendBtn_Click);
            // 
            // OpenFileBtn
            // 
            this.OpenFileBtn.Location = new System.Drawing.Point(183, 234);
            this.OpenFileBtn.Name = "OpenFileBtn";
            this.OpenFileBtn.Size = new System.Drawing.Size(91, 23);
            this.OpenFileBtn.TabIndex = 14;
            this.OpenFileBtn.Text = "Open";
            this.OpenFileBtn.UseSelectable = true;
            this.OpenFileBtn.Click += new System.EventHandler(this.OpenFileBtn_Click);
            // 
            // HexaCodeTbx
            // 
            this.HexaCodeTbx.Location = new System.Drawing.Point(34, 392);
            this.HexaCodeTbx.Name = "HexaCodeTbx";
            this.HexaCodeTbx.Size = new System.Drawing.Size(382, 122);
            this.HexaCodeTbx.TabIndex = 15;
            this.HexaCodeTbx.Text = "";
            // 
            // SaveFileBtn
            // 
            this.SaveFileBtn.Location = new System.Drawing.Point(324, 312);
            this.SaveFileBtn.Name = "SaveFileBtn";
            this.SaveFileBtn.Size = new System.Drawing.Size(92, 23);
            this.SaveFileBtn.TabIndex = 16;
            this.SaveFileBtn.Text = "SaveTest";
            this.SaveFileBtn.UseSelectable = true;
            this.SaveFileBtn.Click += new System.EventHandler(this.SaveFileBtn_Click);
            // 
            // MainForm
            // 
            this.AccessibleDescription = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 537);
            this.Controls.Add(this.SaveFileBtn);
            this.Controls.Add(this.HexaCodeTbx);
            this.Controls.Add(this.OpenFileBtn);
            this.Controls.Add(this.RecvSendBtn);
            this.Controls.Add(this.ConnectStateBtn);
            this.Controls.Add(this.ConStateLabel);
            this.Controls.Add(this.MOrSGbx);
            this.Controls.Add(this.ComLinkGbx);
            this.Controls.Add(this.FileLocTbx);
            this.Controls.Add(this.FileNameTbx);
            this.Controls.Add(this.SendRateTbx);
            this.Controls.Add(this.SendRateBar);
            this.Controls.Add(this.ConfigBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Purple;
            this.Tag = "";
            this.Text = "FileSender";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ComLinkGbx.ResumeLayout(false);
            this.ComLinkGbx.PerformLayout();
            this.MOrSGbx.ResumeLayout(false);
            this.MOrSGbx.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroButton ConfigBtn;
        private MetroFramework.Controls.MetroRadioButton RS232Rbtn;
        private MetroFramework.Controls.MetroRadioButton EthernetRBtn;
        private MetroFramework.Controls.MetroProgressBar SendRateBar;
        private MetroFramework.Controls.MetroTextBox SendRateTbx;
        private MetroFramework.Controls.MetroTextBox FileNameTbx;
        private MetroFramework.Controls.MetroTextBox FileLocTbx;
        private System.Windows.Forms.GroupBox ComLinkGbx;
        private System.Windows.Forms.GroupBox MOrSGbx;
        private MetroFramework.Controls.MetroLabel ConStateLabel;
        private MetroFramework.Controls.MetroRadioButton MasterRBtn;
        private MetroFramework.Controls.MetroRadioButton SlaveRBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button ConnectStateBtn;
        private MetroFramework.Controls.MetroButton RecvSendBtn;
        private System.IO.Ports.SerialPort serialPort1;
        private MetroFramework.Controls.MetroButton OpenFileBtn;
        private System.Windows.Forms.RichTextBox HexaCodeTbx;
        private MetroFramework.Controls.MetroButton SaveFileBtn;
    }
}

