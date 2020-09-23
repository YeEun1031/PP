using MetroFramework.Forms;
using System;
using System.Windows.Forms;

namespace FileSenderApp
{
    public partial class EthernetForm : MetroForm
    {
        public EthernetForm()
        {
            InitializeComponent();
        }

        private void EthernetForm_Load(object sender, EventArgs e)
        {
            // Master 모드일 때는 IP주소를 입력할 필요없음
            if (MainForm.Master == true)
                ServerIPTbx.Enabled = false;
            else if (MainForm.Slave == true)
                ServerIPTbx.Enabled = true;
            LoadiniFile();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Config.SaveiniFile(ServerIPTbx.Text, PortTbx.Text);
            Application.OpenForms["EthernetForm"].Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Application.OpenForms["EthernetForm"].Close();
        }

        private void LoadiniFile()
        {
            // ServerIP
            if(MainForm.Master == true)
                ServerIPTbx.Text = String.Empty;
            else if (!Config.GetInformation("SERVER", "ServerIP").ToUpper().Equals("NONE"))
                ServerIPTbx.Text = Config.GetInformation("SERVER", "ServerIP");
            else
                ServerIPTbx.Text = String.Empty;
            // Port
            if (!Config.GetInformation("SERVER", "Ethernet_Port").ToUpper().Equals("NONE"))
                PortTbx.Text = Config.GetInformation("SERVER", "Ethernet_Port");
            else
                PortTbx.Text = String.Empty;
        }
    }
}
