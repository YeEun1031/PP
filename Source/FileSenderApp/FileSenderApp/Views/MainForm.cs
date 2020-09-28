using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSenderApp
{
    public partial class MainForm : MetroForm
    {
        // 라디오버튼 상태 초기화
        bool RS232 = false;                       // RS232 라디오버튼이 눌러졌는지 확인하는 변수
        bool Ethernet = false;                    // Ethernet 라디오버튼이 눌러졌는지 확인하는 변수
        public static bool Master = false;        // Master 라디오버튼이 눌러졌는지 확인하는 변수, 다른 폼에서도 쓰임
        public static bool Slave = false;         // Slave 라디오버튼이 눌러졌는지 확인하는 변수, 다른 폼에서도 쓰임

        // Ehternet 통신에 필요한 변수
        string filePath;
        Socket receiveSocket, sendSocket;
        Thread listenThread;
        bool conCheck;
        bool listenCheck;


        public string ServerIPName { get; set; }

        public int Ethernet_PortNum { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }
        public void InitValue()                             // 초기화 메서드
        {
            ConnectStateBtn.BackColor = Color.LightGray;    // 연결상태버튼 초기상태: 회색
            OpenSaveBtn.Enabled = false;                    // 열기/저장버튼 비활성화
            ConfigBtn.Enabled = false;                      // 설정버튼 비활성화
            RecvSendBtn.Enabled = false;                    // 수신/송신버튼 비활성화
            SendRateTbx.Text = String.Empty;                // 전송률 초기값
            SendRateBar.Minimum = 0;                        // 전송률 상태진행바 최소값
            SendRateBar.Maximum = 100;                      // 전송률 상태진행바 최대값
            SendRateBar.Value = 0;                          // 전송률 상태진행바 초기값


            // Ethernet 통신 정보
            ServerIPName = Config.GetInformation("SERVER", "ServerIP");
            Ethernet_PortNum = int.Parse(Config.GetInformation("SERVER", "Ethernet_Port"));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitValue();    // 초기화 메서드 호출
        }

        private void ConfigBtn_Click(object sender, EventArgs e)
        {
            if (RS232 == true)
            {
                RS232Form form1 = new RS232Form();          // RS232 라디오버튼이 선택된 상태에서 Config 버튼을 클릭하면 RS232Form 생성
                form1.Show();                               // RS232Form 호출
            }
            else if (Ethernet == true)
            {
                EthernetForm form2 = new EthernetForm();    // Ethernet 라디오버튼이 선택된 상태에서 Config 버튼을 클릭하면 EthernetForm 생성
                form2.Show();                               // EthernetForm 호출
            }
        }

        public void OpenSaveBtn_Click(object sender, EventArgs e)
        {
            if (Slave == true)
            {
                FileNameTbx.Clear();
                filePath = null;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog1.FileName;
                    FileNameTbx.Text = filePath.Split('\\')[filePath.Split('\\').Length - 1];
                    FileLocTbx.Text = filePath;
                }

                // file 정보가 있어야만 전송버튼이 활성화 됨
                if (FileNameTbx.Text == null || FileLocTbx.Text == null)
                    RecvSendBtn.Enabled = false;
                else
                {
                    RecvSendBtn.Text = "Send";
                    RecvSendBtn.Enabled = true;
                    RecvSendBtn.Update();
                }
                    RecvSendBtn.Enabled = true;

                TcpSocketConnect();
            }
        }

        private void RecvSendBtn_Click(object sender, EventArgs e)
        {
            if (Ethernet == true)
            {
                if (Master == true)
                {
                    TcpSocketOpen();
                    Listen();

                    ConnectStateBtn.BackColor = Color.Green;
                    ConnectStateBtn.Update();

                    MessageBox.Show("파일을 수신합니다.");
                }
                else if (Slave == true)
                {
                    ConnectStateBtn.BackColor = Color.Green;
                    ConnectStateBtn.Update();
                    try
                    {
                        int persent;
                        FileInfo fi = new FileInfo(filePath);

                        byte[] fileNameSize = BitConverter.GetBytes(fi.Name.Length);
                        byte[] fileName = Encoding.UTF8.GetBytes(fi.Name);
                        byte[] fileSizeRealSize = File.ReadAllBytes(filePath);
                        byte[] file = BitConverter.GetBytes(fileSizeRealSize.Length);
                        byte[] sendBuffer = new byte[fileName.Length + fileNameSize.Length + file.Length + fileSizeRealSize.Length];

                        Buffer.BlockCopy(fileNameSize, 0, sendBuffer, 0, fileNameSize.Length);
                        Buffer.BlockCopy(fileName, 0, sendBuffer, fileNameSize.Length, fileName.Length);
                        Buffer.BlockCopy(file, 0, sendBuffer, fileNameSize.Length + fileName.Length, file.Length);
                        Buffer.BlockCopy(fileSizeRealSize, 0, sendBuffer, fileNameSize.Length + fileName.Length + file.Length, fileSizeRealSize.Length);

                        byte[] temp = new byte[30000];
                        int fullCount = sendBuffer.Length / 30000;
                        int count = 0;
                        SendRateTbx.Text = "송신 중";
                        SendRateTbx.Update();

                        if (fullCount != 0)
                        {
                            persent = (count * 100) / fullCount;
                            SendRateBar.Value = persent;
                            SendRateBar.Update();
                        }
                        else
                        {
                            SendRateBar.Value = 100;
                            SendRateBar.Update();
                        }

                        // 큰 경우에 나눠서 전송
                        while (sendBuffer.Length > 30000)
                        {
                            count++;
                            persent = (count * 100) / fullCount;
                            SendRateBar.Value = persent;
                            SendRateBar.Update();
                            Buffer.BlockCopy(sendBuffer, 0, temp, 0, temp.Length);
                            sendBuffer = ByteMove(sendBuffer, temp.Length, sendBuffer.Length - temp.Length);
                            sendSocket.Send(temp);
                        }
                        sendSocket.Send(sendBuffer);
                        SendRateTbx.Text = "송신 완료";
                        SendRateTbx.Update();
                    }
                    catch (Exception ex)
                    {
                        ConnectStateBtn.BackColor = Color.Red;
                        ConnectStateBtn.Update();

                        if (MessageBox.Show(ex.Message) == DialogResult.OK)
                        {
                            Application.Exit();
                        }
                    }
                }
            }
        }

        // 소켓 오픈, 포트바인딩
        private void TcpSocketOpen()
        {
            try
            {
                // 소켓 객체 생성, IPAddress.Any: 아무 IP주소나 다 받음
                receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Ethernet_PortNum);
                receiveSocket.Bind(endPoint);

                SendRateTbx.Text = "대기 중";
                SendRateTbx.Update();
            }
            catch (Exception ex)
            {
                ConnectStateBtn.BackColor = Color.Red;
                ConnectStateBtn.Update();

                MessageBox.Show(ex.Message);
            }
        }

        // 서버에 연결
        private void TcpSocketConnect()
        {
            if (conCheck) return;

            try
            {
                // Ethernet 통신 정보 Update
                ServerIPName = Config.GetInformation("SERVER", "ServerIP");
                Ethernet_PortNum = int.Parse(Config.GetInformation("SERVER", "Ethernet_Port"));

                // 소켓 객체 생성
                sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sendSocket.Connect(IPAddress.Parse(ServerIPName), Ethernet_PortNum);
                conCheck = true;
            }
            catch (Exception ex)
            {
                ConnectStateBtn.BackColor = Color.Red;
                ConnectStateBtn.Update();

                MessageBox.Show(ex.Message);
            }
        }

        // 포트 리스닝 시작
        private void Listen()
        {
            if (listenCheck) return;

            try
            {
                // 연결 허용
                receiveSocket.Listen(10);
                receiveSocket = receiveSocket.Accept();

                // 실제로 통신을 담당하는 쓰레드 생성
                listenThread = new Thread(Receive_Data);
                listenThread.Start();
                listenCheck = true;

                ConnectStateBtn.BackColor = Color.Green;
                ConnectStateBtn.Update();
            }
            catch (Exception ex)
            {
                ConnectStateBtn.BackColor = Color.Red;
                ConnectStateBtn.Update();

                MessageBox.Show(ex.Message);

                if (listenCheck)
                    listenCheck = false;
            }
        }

        private void Receive_Data()
        {
            byte[] receiveBuffer = new byte[4096];
            byte[] packetRealSizeBuffer;
            byte[] packetSizeBuffer = new byte[4];
            byte[] fileNameSizeBuffer = new byte[4];
            byte[] fileNameBuffer;
            byte[] temp;
            string fileName;
            string dirName;
            int fileNameLength;
            int fileRealSize;
            int fileSizeSum;
            int receiveSize;
            int persent;

            try
            {
                while (listenCheck)
                {
                    receiveSize = receiveSocket.Receive(receiveBuffer);
                    if (receiveSize > 0)
                    {
                        Buffer.BlockCopy(receiveBuffer, 0, fileNameSizeBuffer, 0, 4);
                        fileNameLength = BitConverter.ToInt32(fileNameSizeBuffer, 0);
                        fileNameBuffer = new byte[fileNameLength];
                        Buffer.BlockCopy(receiveBuffer, fileNameSizeBuffer.Length, fileNameBuffer, 0, fileNameBuffer.Length);
                        fileName = Encoding.Default.GetString(fileNameBuffer);
                        Buffer.BlockCopy(receiveBuffer, fileNameSizeBuffer.Length + fileNameBuffer.Length, packetSizeBuffer, 0, packetSizeBuffer.Length);
                        fileRealSize = BitConverter.ToInt32(packetSizeBuffer, 0);
                        packetRealSizeBuffer = ByteMove(receiveBuffer, fileNameSizeBuffer.Length + fileNameBuffer.Length + packetSizeBuffer.Length,
                            receiveSize - (fileNameSizeBuffer.Length + fileNameBuffer.Length + packetSizeBuffer.Length));
                        fileSizeSum = packetRealSizeBuffer.Length;
                        persent = (fileSizeSum * 100) / fileRealSize;

                        //크로스 쓰레드 에러 방지 대리자 호출
                        Invoke((MethodInvoker)delegate
                        {
                            SendRateTbx.Text = "수신 중";
                        });
                        while (fileRealSize > fileSizeSum)
                        {
                            receiveSize = receiveSocket.Receive(receiveBuffer);
                            fileSizeSum += receiveSize;
                            if (fileRealSize != 0)
                                persent = (fileSizeSum * 100) / fileRealSize;
                            Invoke((MethodInvoker)delegate
                            {
                                SendRateBar.Value = persent;
                            });
                            temp = new byte[receiveSize + packetRealSizeBuffer.Length];
                            Buffer.BlockCopy(packetRealSizeBuffer, 0, temp, 0, packetRealSizeBuffer.Length);
                            Buffer.BlockCopy(receiveBuffer, 0, temp, packetRealSizeBuffer.Length, receiveSize);
                            packetRealSizeBuffer = temp;
                        }
                        if (fileRealSize != fileSizeSum)
                        {
                            MessageBox.Show("파일이 손상되었습니다.");
                        }

                        // 다운로드가 완료되면 saveFileDialog로 파일 저장하기
                        Invoke((MethodInvoker)delegate
                        {
                            SendRateTbx.Text = "수신 완료";
                            if (fileRealSize == fileSizeSum)
                            {
                                var result = saveFileDialog1.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    filePath = saveFileDialog1.FileName;
                                    FileNameTbx.Text = filePath.Split('\\')[filePath.Split('\\').Length - 1];
                                    FileLocTbx.Text = filePath;
                                }
                            }
                        });

                        dirName = filePath + fileName;               
                        using (FileStream fs = new FileStream(dirName, FileMode.Create, FileAccess.Write))
                            fs.Write(packetRealSizeBuffer, 0, packetRealSizeBuffer.Length);
                        MessageBox.Show(dirName + " 파일 생성 완료");
                    }
                }
            }
            catch
            {
                // 소켓 닫기
                if (receiveSocket != null)
                    receiveSocket.Close();
                if (sendSocket != null)
                    sendSocket.Close();

                Application.Exit();
            }
        }

        private byte[] ByteMove(byte[] afterByte, int movePoint, int moveCount)
        {
            byte[] resultByte = new byte[moveCount];
            Buffer.BlockCopy(afterByte, movePoint, resultByte, 0, moveCount);
            return resultByte;
        }

        private void RBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;

            //FileNameTbx.Text = "파일명";
            //FileLocTbx.Clear();

            switch (rbtn.Name)
            {
                case "MasterRBtn":
                    {
                        if (Master == false)
                            Master = true;
                        if (Slave == true)
                            Slave = false;

                        RecvSendBtn.Text = "Receive";
                        RecvSendBtn.Enabled = true;
                        RecvSendBtn.Update();

                        break;
                    }
                case "SlaveRBtn":
                    {
                        FileNameTbx.Text = "파일명";
                        FileLocTbx.Clear();

                        if (Slave == false)
                            Slave = true;
                        if (Master == true)
                            Master = false;

                        OpenSaveBtn.Enabled = true;
                        OpenSaveBtn.Text = "Open";

                        break;
                    }
                case "EthernetRBtn":
                    {
                        RS232 = false;
                        Ethernet = true;
                        ConfigBtn.Enabled = true;
                        break;
                    }
                case "RS232Rbtn":
                    {
                        Ethernet = false;
                        RS232 = true;
                        ConfigBtn.Enabled = true;
                        break;
                    }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            listenCheck = false;

            // 소켓 닫기
            if (receiveSocket != null)
                receiveSocket.Close();
            if (sendSocket != null)
                sendSocket.Close();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            #region 버튼 이벤트
            this.ConfigBtn.Click += new System.EventHandler(this.ConfigBtn_Click);
            #endregion

            #region 라디오 체인지 이벤트
            this.MasterRBtn.CheckedChanged += new System.EventHandler(this.RBtn_CheckedChanged);
            this.SlaveRBtn.CheckedChanged += new System.EventHandler(this.RBtn_CheckedChanged);
            this.EthernetRBtn.CheckedChanged += new System.EventHandler(this.RBtn_CheckedChanged);
            this.RS232Rbtn.CheckedChanged += new System.EventHandler(this.RBtn_CheckedChanged);
            #endregion
        }
    }
}