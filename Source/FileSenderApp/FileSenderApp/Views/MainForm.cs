using MetroFramework.Forms;
using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        // RS232 통신에 필요한 변수
        FileInfo fileInfo;
        int fileSize;
        int RxDataCount = 0;
        byte[] ByteSendData;
        byte[] ByteRxData = new byte[1000000 * 1024];

        public string ServerIPName { get; set; }
        public int Ethernet_PortNum { get; set; }
        public string RS232_PortNum { get; set; }
        public string Baudrate { get; set; }
        public string Parity { get; set; }
        public string DataBits { get; set; }
        public string StopBits { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }
        public void InitValue()                             // 초기화 메서드
        {
            ConnectStateBtn.BackColor = Color.LightGray;    // 연결상태버튼 초기상태: 회색
            OpenFileBtn.Enabled = false;                    // 열기/저장버튼 비활성화
            ConfigBtn.Enabled = false;                      // 설정버튼 비활성화
            RecvSendBtn.Enabled = false;                    // 수신/송신버튼 비활성화
            SendRateTbx.Text = String.Empty;                // 전송률 초기값
            SendRateBar.Minimum = 0;                        // 전송률 상태진행바 최소값
            SendRateBar.Maximum = 100;                      // 전송률 상태진행바 최대값
            SendRateBar.Value = 0;                          // 전송률 상태진행바 초기값


            // 소켓통신 정보
            ServerIPName = Config.GetInformation("SERVER", "ServerIP");
            Ethernet_PortNum = int.Parse(Config.GetInformation("SERVER", "Ethernet_Port"));

            // 시리얼통신 정보
            RS232_PortNum = Config.GetInformation("SERIAL", "Serial_Port");
            Baudrate = Config.GetInformation("SERIAL", "Baudrate");
            Parity = Config.GetInformation("SERIAL", "Parity");
            DataBits = Config.GetInformation("SERIAL", "DataBits");
            StopBits = Config.GetInformation("SERIAL", "StopBits");
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

        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            // 소켓통신 클라이언트
            if (Ethernet == true && Slave == true)
            {
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
                    RecvSendBtn.Enabled = true;
                    RecvSendBtn.Update();
                }

                // 소켓 연결 시도
                TcpSocketConnect();
            }

            // 시리얼통신 클라이언트
            if (RS232 == true)
            {
                if (Slave == true)
                {
                    try
                    {
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            fileInfo = new FileInfo(openFileDialog1.FileName);
                            fileSize = Convert.ToInt32(fileInfo.Length);
                            ByteSendData = new byte[fileSize];

                            FileNameTbx.Text = (fileInfo.ToString()).Split('\\')[fileInfo.ToString().Split('\\').Length - 1];
                            FileLocTbx.Text = fileInfo.ToString();

                            BinaryReader reader = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open));
                            ByteSendData = reader.ReadBytes(Convert.ToInt32(fileSize));
                            reader.Close();
                        }

                        // file 정보가 있어야만 전송버튼이 활성화 됨
                        if (FileNameTbx.Text == null || FileLocTbx.Text == null)
                            RecvSendBtn.Enabled = false;
                        else
                        {
                            RecvSendBtn.Enabled = true;
                            RecvSendBtn.Update();
                        }

                        // 시리얼 포트 연결
                        SerialConnect();
                    }
                    catch { }
                }
            }
        }

        private void RecvSendBtn_Click(object sender, EventArgs e)
        {
            // 소켓통신
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

            // 시리얼통신
            if (RS232 == true)
            {
                if (Master == true)
                {
                    SerialConnect();
                }
                else if (Slave == true)
                {
                    serialPort1.Write(ByteSendData, 0, fileSize);
                    for (int i = 0; i < fileSize; i++)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            SendRateBar.Value = fileSize / SendRateBar.Maximum;
                        });
                    }
                }
            }
        }

        private void SerialConnect()
        {
            // 시리얼통신 정보 Update
            RS232_PortNum = Config.GetInformation("SERIAL", "Serial_Port");
            Baudrate = Config.GetInformation("SERIAL", "Baudrate");
            Parity = Config.GetInformation("SERIAL", "Parity");
            DataBits = Config.GetInformation("SERIAL", "DataBits");
            StopBits = Config.GetInformation("SERIAL", "StopBits");

            // 시리얼포트가 열려있지 않으면
            if (!serialPort1.IsOpen)
            {
                // Baudrate 데이터 전처리
                Baudrate = Baudrate.Substring(8);

                // DataBits 데이터 전처리
                switch (DataBits)
                {
                    case "Five":
                        DataBits = "5";
                        break;
                    case "Six":
                        DataBits = "6";
                        break;
                    case "Seven":
                        DataBits = "7";
                        break;
                    case "Eight":
                        DataBits = "8";
                        break;
                }

                serialPort1.PortName = RS232_PortNum;
                serialPort1.BaudRate = int.Parse(Baudrate);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                serialPort1.DataBits = int.Parse(DataBits);

                // StopBits가 None일 때, 예외처리(Default값 One)
                try
                {
                    serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);
                }
                catch { }

                serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialReceivedData);
                serialPort1.Open();

                ConnectStateBtn.BackColor = Color.Green;
                MessageBox.Show("포트가 열렸습니다");
            }
            else
            {
                // MessageBox.Show("포트가 열려있습니다");
            }
        }

        // 시리얼통신의 수신이벤트가 발생하면 실행
        private void SerialReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            // 메인스레드와 수신스레드의 충돌방지, Serial_Received 메서드로 이동하여 추가 작업
            this.Invoke(new EventHandler(Serial_Received));
        }

        // 수신데이터를 처리
        private void Serial_Received(object s, EventArgs e)
        {
            // 수신된 데이타를 읽어와 string형식으로 변환하여 출력
            int recvSize = serialPort1.BytesToRead;
            string strRxData;

            if (recvSize != 0)
            {
                strRxData = "";
                byte[] buffer = new byte[recvSize];

                serialPort1.Read(buffer, 0, recvSize);

                Array.Copy(buffer, 0, ByteRxData, RxDataCount, recvSize);
                RxDataCount += recvSize;

                for (int temp = 0; temp < recvSize; temp++)
                {
                    strRxData += " " + buffer[temp].ToString("X2");
                }
                HexaCodeTbx.Text += strRxData;
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

                ConnectStateBtn.BackColor = Color.Green;
                ConnectStateBtn.Update();
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
                listenThread = new Thread(SocketReceiveData);
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

        // 소켓통신으로 데이타 수신
        private void SocketReceiveData()
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

        /// <summary>
        /// 바이트를 뒤로 이동시켜주는 메서드
        /// </summary>
        /// <param name="afterByte">처음 바이트</param>
        /// <param name="movePoint">이동할 시작위치</param>
        /// <param name="moveCount">이동할 마지막위치</param>
        /// <returns></returns>
        private byte[] ByteMove(byte[] afterByte, int movePoint, int moveCount)
        {
            byte[] resultByte = new byte[moveCount];
            Buffer.BlockCopy(afterByte, movePoint, resultByte, 0, moveCount);
            return resultByte;
        }

        private void RBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;

            switch (rbtn.Name)
            {
                case "MasterRBtn":
                    {
                        if (Master == false)
                            Master = true;
                        if (Slave == true)
                            Slave = false;

                        // Master 모드일 때는 파일을 다 받은 뒤 자동으로 Dialog 띄움
                        OpenFileBtn.Enabled = false;
                        OpenFileBtn.Update();

                        // Master 모드일 때는 Receive 버튼 활성화
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

                        // Slave 모드일 때는 Dialog를 이용해 파일을 선택하기 때문에 버튼 활성화
                        OpenFileBtn.Enabled = true;
                        OpenFileBtn.Update();

                        // Slave 모드일 때는 Send 버튼 활성화
                        RecvSendBtn.Text = "Send";
                        RecvSendBtn.Enabled = false;
                        RecvSendBtn.Update();

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

            // 시리얼포트 닫기
            if (serialPort1.IsOpen)
                serialPort1.Close();
        }

        private void SaveFileBtn_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryWriter writer = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                writer.Write(ByteRxData, 0, RxDataCount);
                writer.Close();
            }
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