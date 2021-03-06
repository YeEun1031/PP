﻿using MetroFramework.Forms;
using System;
using System.Windows.Forms;
using System.IO.Ports;          // 시리얼통신

namespace FileSenderApp
{
    public partial class RS232Form : MetroForm
    {
        // 시리얼포트 생성
        private SerialPort serialPort = new SerialPort();

        public string PortName { get; set; }

        public string BaudrateName { get; set; }

        public string ParityName { get; set; }

        public string DataBitName { get; set; }

        public string StopBitName { get; set; }

        public enum BaudRates
        { 
            Baudrate75 = 75
            , Baudrate150 = 150
            , Baudrate300 = 300
            , Baudrate600 = 600
            , Baudrate1200 = 1200
            , Baudrate2400 = 2400
            , Baudrate4800 = 4800
            , Baudrate9600 = 9600
            , Baudrate19200 = 19200
            , Baudrate38400 = 38400
            , Baudrate57600 = 57600
            , Baudrate115200 = 115200
            , Baudrate230400 = 230400
            , Baudrate460800 = 460800
            , Baudrate921600 = 921600
        };

        public enum DataBits 
        {
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8
        };

        public RS232Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드 각 컨트롤(ComboBox) 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RS232Form_Load(object sender, EventArgs e)
        {
            string[] PortNames = SerialPort.GetPortNames();
            string[] BaudrateNames = Enum.GetNames(typeof(BaudRates));//Config.GetInformation("SERIAL", "Baudrate").Split(',');
            string[] ParityNames = Enum.GetNames(typeof(Parity));
            string[] DataBitNames = Enum.GetNames(typeof(DataBits));
            string[] StopBitNames = Enum.GetNames(typeof(StopBits));

            PortCbx.Items.AddRange(PortNames);
            BaudrateCbx.Items.AddRange(BaudrateNames);
            ParityCbx.Items.AddRange(ParityNames);
            DataBitsCbx.Items.AddRange(DataBitNames);
            StopBitsCbx.Items.AddRange(StopBitNames);

            LoadiniFile();

            PortCbx.SelectedIndex = PortCbx.Items.IndexOf(PortName);
            BaudrateCbx.SelectedIndex = BaudrateCbx.Items.IndexOf(BaudrateName);
            ParityCbx.SelectedIndex = ParityCbx.Items.IndexOf(ParityName);
            DataBitsCbx.SelectedIndex = DataBitsCbx.Items.IndexOf(DataBitName);
            StopBitsCbx.SelectedIndex = StopBitsCbx.Items.IndexOf(StopBitName);
        }
        
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Config.SaveiniFile(PortCbx.Text, BaudrateCbx.Text, ParityCbx.Text, DataBitsCbx.Text, StopBitsCbx.Text);
            Application.OpenForms["RS232Form"].Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            // 만약 시리얼포트가 열려있으면, 포트를 닫기
            if (serialPort.IsOpen == true)
            {
                serialPort.Close();
            }
            Application.OpenForms["RS232Form"].Close();
        }

        private void LoadiniFile()
        {
            PortName = Config.GetInformation("SERIAL", "Serial_Port");
            BaudrateName = Config.GetInformation("SERIAL", "Baudrate");
            ParityName = Config.GetInformation("SERIAL", "Parity");
            DataBitName = Config.GetInformation("SERIAL", "DataBits");
            StopBitName = Config.GetInformation("SERIAL", "StopBits");
        }
    }
}
