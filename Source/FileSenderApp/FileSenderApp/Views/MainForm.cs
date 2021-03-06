﻿using MetroFramework.Forms;
using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Data;
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

        SerialCommunication serialCommunication = new SerialCommunication();


        // Ehternet 통신에 필요한 변수
        string filePath;
        Socket receiveSocket, sendSocket;
        Thread listenThread;
        bool conCheck;
        bool listenCheck;

        // RS232 통신에 필요한 변수
        FileInfo FileInfo;
        string fileName;
        int fileLength;
        int TotalSize;
        int RxDataCount = 0;
        byte[] ByteSendData;
        byte[] ByteRxData;

        public string ServerIPName { get; set; }
        public int Ethernet_PortNum { get; set; }

        public MainForm()
        {
            InitializeComponent();

            GlobalVariable.ReportText += WriteStateText;
            GlobalVariable.StateReport += StateReport;
            GlobalVariable.PrograssBarValue += ProgressBarUpdate;
            GlobalVariable.PrograssBarMaxValue += PrograssBarMaxValueUpdate;
        }

        private void WriteStateText(string reporttext)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    SendRateTbx.Text = reporttext;
                }));
            }
            else
            {
                SendRateTbx.Text = reporttext;
            }
        }

        private void StateReport(string controlname, bool statecheck)
        {
            switch (controlname)
            {
                case "ConnectStateBtn":
                    ConnectStateBtn.BackColor = statecheck == true ? Color.Green : Color.Red;
                break;

                case "ConfigBtn":
                    ConfigBtn.Enabled = statecheck;
                    break;
                    
                default:
                    break;
            }
        }

        private void ProgressBarUpdate(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    SendRateBar.Value = value;
                }));
            }
            else
            {
                SendRateBar.Value = value;
            }
        }

        private void PrograssBarMaxValueUpdate(int maxvalue)
        {
            SendRateBar.Maximum = maxvalue;
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
                            FileInfo = new FileInfo(openFileDialog1.FileName);

                            FileNameTbx.Text = (FileInfo.ToString()).Split('\\')[FileInfo.ToString().Split('\\').Length - 1];
                            FileLocTbx.Text = FileInfo.ToString();

                            string fileName = FileNameTbx.Text;
                            int fileNameLen = fileName.Length;
                            int fileSize = Convert.ToInt32(FileInfo.Length);

                            // byte형 변수는 1Byte, int형 변수는 4Byte를 직접 더해줌...
                            TotalSize = 1 + fileNameLen + 4 + fileSize;

                            // 파일명길이
                            byte[] byteNameSize = new byte[1];
                            byteNameSize[0] = (byte)fileNameLen;
                            // 파일명
                            byte[] byteFileName = Encoding.ASCII.GetBytes(fileName);
                            // 파일길이
                            byte[] byteFileSize = BitConverter.GetBytes(fileSize);
                            // 실제 파일 데이터
                            byte[] byteFileInfo;

                            // 패킷크기만큼 생성
                            ByteSendData = new Byte[TotalSize];

                            BinaryReader reader = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open));

                            byteFileInfo = reader.ReadBytes(Convert.ToInt32(fileSize));

                            // 패킷생성 : 파일명길이 + 파일명 + 파일길이 + 실제 파일 데이터


                            Array.Copy(byteNameSize, 0, ByteSendData, 0, byteNameSize.Length);
                            Array.Copy(byteFileName, 0, ByteSendData, byteNameSize.Length, byteFileName.Length);
                            Array.Copy(byteFileSize, 0, ByteSendData, (byteNameSize.Length + byteFileName.Length), byteFileSize.Length);
                            Array.Copy(byteFileInfo, 0, ByteSendData, (byteNameSize.Length + byteFileName.Length + byteFileSize.Length), byteFileInfo.Length);

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
                    serialCommunication.MS = true;
                    serialCommunication.Start();
                }
                else if (Slave == true)
                {
                    // 실제로 보내는 부분
                    try
                    {
                        serialCommunication.MS = false;
                        serialCommunication.Start();
                        serialPort1.Write(ByteSendData, 0, TotalSize);
                    }
                    finally
                    {
                        SendRateTbx.Text = "송신 완료";
                    }
                }
            }
        }

        private void SerialConnect()
        {

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
            
            // 수신 버퍼에 있는 데이터의 바이트 수를 가져옴
            int recvSize = serialPort1.BytesToRead;
            // string strRxData; // richTextBox에 출력해주려고 했음

            if (recvSize != 0)
            {
                // strRxData = "";
                // 버퍼를 수신된 만큼의 크기로 생성해줌
                byte[] buffer = new byte[recvSize];
                serialPort1.Read(buffer, 0, recvSize);

                // 버퍼에 수신된 데이터 쪼개기(최초 한 번만 수행)
                try
                {
                    if (RxDataCount == 0)
                    {
                        // 파일명길이
                        int fileNameLength;
                        fileNameLength = Convert.ToInt32(buffer[0]);

                        // 파일명 - nameLengthBuffer의 내용을 받아와서 사이즈를 선언하고 fileNameBuffer에 옮김
                        byte[] fileNameBuffer = new byte[fileNameLength];
                        Array.Copy(buffer, 1, fileNameBuffer, 0, fileNameLength);
                        fileName = Encoding.ASCII.GetString(fileNameBuffer);

                        // 파일길이
                        byte[] fileLengthBuffer = new byte[4];
                        Array.Copy(buffer, 1 + fileNameLength, fileLengthBuffer, 0, 4);
                        fileLength = BitConverter.ToInt32(fileLengthBuffer, 0);
                        SendRateBar.Maximum = fileLength;

                        // 실제 파일 데이터
                        ByteRxData = new byte[fileLength];
                        Array.Copy(buffer, 1 + fileNameLength + 4, ByteRxData, RxDataCount, recvSize - (1 + fileNameLength + 4));
                        RxDataCount = (recvSize - (1 + fileNameLength + 4));
                    }
                    // 최초 첫 수신 이후에는 버퍼의 0번지부터 저장해야 함
                    else
                    {
                        // 실제 파일 데이터
                        Array.Copy(buffer, 0, ByteRxData, RxDataCount, recvSize);
                        RxDataCount += recvSize;
                    }

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            SendRateBar.Value = RxDataCount;
                        }));
                    }
                    else
                    {
                        SendRateBar.Value = RxDataCount;
                    }

                    // 총 수신된 데이터 카운터가 실제 파일 데이터 크기보다 크면 저장
                    if (RxDataCount >= fileLength)
                    {
                        // saveFileDialog 띄우기
                        if (!string.IsNullOrEmpty(fileName))
                            saveFileDialog1.FileName = fileName;

                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            BinaryWriter writer = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                            writer.Write(ByteRxData, 0, RxDataCount);
                            writer.Close();
                        }

                        SendRateTbx.Text = "수신 완료";
                    }
                    SendRateTbx.Text = "수신 중";
                    /*
                    // 다소 의미없는 richTextBox 출력 => 저장과 상관없는 부분이드라
                    // 아니 이 부분 때문에 패킷손실되고 난리도 아니였음
                    for (int temp = 0; temp < recvSize; temp++)
                    {
                        strRxData += " " + buffer[temp].ToString("X2");
                    }
                    HexaCodeTbx.Text += strRxData;
                    */
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
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
                                if (!string.IsNullOrEmpty(fileName))
                                    saveFileDialog1.FileName = fileName;

                                var result = saveFileDialog1.ShowDialog();

                                if (result == DialogResult.OK)
                                {
                                    filePath = saveFileDialog1.FileName;
                                    FileNameTbx.Text = filePath.Split('\\')[filePath.Split('\\').Length - 1];
                                    FileLocTbx.Text = filePath;
                                }
                            }
                        });
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            fs.Write(packetRealSizeBuffer, 0, packetRealSizeBuffer.Length);
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