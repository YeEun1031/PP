namespace FileSenderApp
{
    partial class RS232Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RS232Form));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.PortCbx = new MetroFramework.Controls.MetroComboBox();
            this.BaudrateCbx = new MetroFramework.Controls.MetroComboBox();
            this.ParityCbx = new MetroFramework.Controls.MetroComboBox();
            this.DataBitsCbx = new MetroFramework.Controls.MetroComboBox();
            this.StopBitsCbx = new MetroFramework.Controls.MetroComboBox();
            this.SaveBtn = new MetroFramework.Controls.MetroButton();
            this.CancelBtn = new MetroFramework.Controls.MetroButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(70, 63);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(41, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Port :";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(44, 98);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(69, 19);
            this.metroLabel2.TabIndex = 0;
            this.metroLabel2.Text = "Baudrate :";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(63, 133);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(48, 19);
            this.metroLabel3.TabIndex = 0;
            this.metroLabel3.Text = "Parity :";
            this.metroLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(44, 168);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(67, 19);
            this.metroLabel4.TabIndex = 0;
            this.metroLabel4.Text = "Data Bits :";
            this.metroLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(44, 203);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(67, 19);
            this.metroLabel5.TabIndex = 0;
            this.metroLabel5.Text = "Stop Bits :";
            this.metroLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PortCbx
            // 
            this.PortCbx.FormattingEnabled = true;
            this.PortCbx.ItemHeight = 23;
            this.PortCbx.Location = new System.Drawing.Point(131, 63);
            this.PortCbx.Name = "PortCbx";
            this.PortCbx.Size = new System.Drawing.Size(160, 29);
            this.PortCbx.TabIndex = 1;
            this.PortCbx.UseSelectable = true;
            // 
            // BaudrateCbx
            // 
            this.BaudrateCbx.FormattingEnabled = true;
            this.BaudrateCbx.ItemHeight = 23;
            this.BaudrateCbx.Location = new System.Drawing.Point(131, 98);
            this.BaudrateCbx.Name = "BaudrateCbx";
            this.BaudrateCbx.Size = new System.Drawing.Size(160, 29);
            this.BaudrateCbx.TabIndex = 1;
            this.BaudrateCbx.UseSelectable = true;
            // 
            // ParityCbx
            // 
            this.ParityCbx.FormattingEnabled = true;
            this.ParityCbx.ItemHeight = 23;
            this.ParityCbx.Location = new System.Drawing.Point(131, 133);
            this.ParityCbx.Name = "ParityCbx";
            this.ParityCbx.Size = new System.Drawing.Size(160, 29);
            this.ParityCbx.TabIndex = 1;
            this.ParityCbx.UseSelectable = true;
            // 
            // DataBitsCbx
            // 
            this.DataBitsCbx.FormattingEnabled = true;
            this.DataBitsCbx.ItemHeight = 23;
            this.DataBitsCbx.Location = new System.Drawing.Point(131, 168);
            this.DataBitsCbx.Name = "DataBitsCbx";
            this.DataBitsCbx.Size = new System.Drawing.Size(160, 29);
            this.DataBitsCbx.TabIndex = 1;
            this.DataBitsCbx.UseSelectable = true;
            // 
            // StopBitsCbx
            // 
            this.StopBitsCbx.FormattingEnabled = true;
            this.StopBitsCbx.ItemHeight = 23;
            this.StopBitsCbx.Location = new System.Drawing.Point(131, 203);
            this.StopBitsCbx.Name = "StopBitsCbx";
            this.StopBitsCbx.Size = new System.Drawing.Size(160, 29);
            this.StopBitsCbx.TabIndex = 1;
            this.StopBitsCbx.UseSelectable = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(317, 63);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(86, 64);
            this.SaveBtn.TabIndex = 1;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseSelectable = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(317, 133);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(86, 64);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseSelectable = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // RS232Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 300);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.StopBitsCbx);
            this.Controls.Add(this.DataBitsCbx);
            this.Controls.Add(this.ParityCbx);
            this.Controls.Add(this.BaudrateCbx);
            this.Controls.Add(this.PortCbx);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RS232Form";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Purple;
            this.Load += new System.EventHandler(this.RS232Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroComboBox PortCbx;
        private MetroFramework.Controls.MetroComboBox BaudrateCbx;
        private MetroFramework.Controls.MetroComboBox ParityCbx;
        private MetroFramework.Controls.MetroComboBox DataBitsCbx;
        private MetroFramework.Controls.MetroComboBox StopBitsCbx;
        private MetroFramework.Controls.MetroButton SaveBtn;
        private MetroFramework.Controls.MetroButton CancelBtn;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer timer1;
    }
}