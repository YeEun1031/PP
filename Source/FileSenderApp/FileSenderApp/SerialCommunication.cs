using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSenderApp
{
    public class SerialCommunication
    {
        #region 선언부

        private bool IsThreadRun = true;

        private Thread SerialReceiveThread = null;

        private Thread SerialSendThread = null;

        private Thread SerialConnectCheck = null;

        private Queue<byte[]> SendDataQueue = new Queue<byte[]>();

        private SerialPort SerialPort = null;

        public bool MS = false;


        public string RS232_PortNum { get; set; }
        public string Baudrate { get; set; }
        public string Parity { get; set; }
        public string DataBits { get; set; }
        public string StopBits { get; set; }

        #endregion



        #region 초기화 작업

        private void ThreadInit(object sender, EventArgs e)
        {
            if (this.SerialSendThread == null)
            {
                this.SerialSendThread = new Thread(new ThreadStart(SendData));
                this.SerialSendThread.IsBackground = true;
            }

            if (this.SerialReceiveThread == null)
            {
                this.SerialReceiveThread = new Thread(new ThreadStart(ReceiveData));
                this.SerialReceiveThread.IsBackground = true;
            }

            if (this.SerialConnectCheck == null)
            {
                this.SerialConnectCheck = new Thread(new ThreadStart(ConnectCheck));
                this.SerialConnectCheck.IsBackground = true;
            }
        }


        private void ConnectCheck()
        {
            while (IsThreadRun)
            {

            }
        }

        public void InitValue()
        {
            // 시리얼통신 정보
            RS232_PortNum = Config.GetInformation("SERIAL", "Serial_Port");
            Baudrate = Config.GetInformation("SERIAL", "Baudrate");
            Parity = Config.GetInformation("SERIAL", "Parity");
            DataBits = Config.GetInformation("SERIAL", "DataBits");
            StopBits = Config.GetInformation("SERIAL", "StopBits");
        }

        #endregion

        #region 커넥션 체크부
        private void SerialConnect()
        {
            InitValue();

            // 시리얼포트가 열려있지 않으면
            if (!SerialPort.IsOpen)
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

                SerialPort.PortName = RS232_PortNum;
                SerialPort.BaudRate = int.Parse(Baudrate);
                SerialPort.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                SerialPort.DataBits = int.Parse(DataBits);

                // StopBits가 None일 때, 예외처리(Default값 One)
                try
                {
                    SerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);
                }
                catch { }
                SerialPort.Open();
                //버튼 초록색으로 만든다
                GlobalVariable.StateReport?.Invoke("ConnectStateBtn", true);
                MessageBox.Show("포트가 열렸습니다");

                if (SerialPort.IsOpen)
                {
   
                    GlobalVariable.StateReport?.Invoke("ConfigBtn", false);
                    GlobalVariable.ReportText?.Invoke("대기중");
                }
            }
            else
            {
                MessageBox.Show("포트가 열려있습니다");
            }
        }
        #endregion

        #region 송신부
        public void SendMessage(byte[] _senData)
        {
            //동작 중 일때만 큐에 메세지를 씁니다.
            if (IsThreadRun)
                this.SendDataQueue.Enqueue(_senData);
        }

        private void SendData()
        {
            while (IsThreadRun)
            {
                try
                {
                    while (this.SendDataQueue != null && this.SendDataQueue.Count > 0)
                    {
                        byte[] sendData = this.SendDataQueue.Dequeue();

                        if (sendData != null)
                        {
                            //보낼 데이터를 처리하세요
                            SerialPort.Write(sendData, 0, sendData.Length);
                        }
                        Thread.Sleep(100);
                    }
                }
                catch { }
            }
        }
        #endregion

        #region 수신부
        private void ReceiveData()
        {
            if (this.IsThreadRun)
            {
                try
                {
                    if (MS)
                        SerialPort.DataReceived += DataFileReceivedEvent;
                    else
                        SerialPort.DataReceived += AckReceivedEvent;

                }
                catch (ThreadAbortException) { }

                catch (Exception ex)
                {
                    //Error Log를 작성하세요

                    Console.WriteLine("ReceiveData(object) :: {0}", ex.Message);
                }
            }
        }

        #endregion

        #region 수신 처리부
        private void AckReceivedEvent(object sender, SerialDataReceivedEventArgs e)
        {
            //데이터를 검증 하는 로직을 작성하시오.
        }

        // 패킷을 조정 할 변수
        int RxOffSet = 0;

        byte[] receiveData = new byte[4096];
        byte[] ByteRxData = new byte[4096];

        private void DataFileReceivedEvent(object sender, SerialDataReceivedEventArgs e)
        {
            // 수신 버퍼에 있는 데이터의 바이트 수를 가져옴
            int recvSize = SerialPort.BytesToRead;
            // string strRxData; // richTextBox에 출력해주려고 했음

            if (recvSize != 0)
            {
                // strRxData = "";
                // 버퍼를 수신된 만큼의 크기로 생성해줌
                byte[] buffer = new byte[recvSize];
                SerialPort.Read(buffer, 0, recvSize);

                // 버퍼에 수신된 데이터 쪼개기(최초 한 번만 수행)
                try
                {
                    if (GlobalVariable.RxDataCount == 0)
                    {
                        // 파일명길이
                        int fileNameLength;
                        fileNameLength = Convert.ToInt32(buffer[0]);

                        // 파일명 - nameLengthBuffer의 내용을 받아와서 사이즈를 선언하고 fileNameBuffer에 옮김
                        byte[] fileNameBuffer = new byte[fileNameLength];
                        Array.Copy(buffer, 1, fileNameBuffer, 0, fileNameLength);
                        GlobalVariable.fileName = Encoding.ASCII.GetString(fileNameBuffer);

                        // 파일길이
                        byte[] fileLengthBuffer = new byte[4];
                        Array.Copy(buffer, 1 + fileNameLength, fileLengthBuffer, 0, 4);
                        GlobalVariable.fileLength = BitConverter.ToInt32(fileLengthBuffer, 0);
                        GlobalVariable.PrograssBarMaxValue?.Invoke(GlobalVariable.fileLength);

                        // 실제 파일 데이터
                        ByteRxData = new byte[GlobalVariable.fileLength];
                        Array.Copy(buffer, 1 + fileNameLength + 4, ByteRxData, GlobalVariable.RxDataCount, recvSize - (1 + fileNameLength + 4));
                        GlobalVariable.RxDataCount = (recvSize - (1 + fileNameLength + 4));
                    }
                    // 최초 첫 수신 이후에는 버퍼의 0번지부터 저장해야 함
                    else
                    {
                        // 실제 파일 데이터
                        Array.Copy(buffer, 0, ByteRxData, GlobalVariable.RxDataCount, recvSize);
                        GlobalVariable.RxDataCount += recvSize;
                    }

                    GlobalVariable.PrograssBarValue?.Invoke(GlobalVariable.RxDataCount);
                    // 총 수신된 데이터 카운터가 실제 파일 데이터 크기보다 크면 저장
                    if (GlobalVariable.RxDataCount >= GlobalVariable.fileLength)
                    {
                        // saveFileDialog 띄우기
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        if (!string.IsNullOrEmpty(GlobalVariable.fileName))
                            saveFileDialog1.FileName = GlobalVariable.fileName;

                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            BinaryWriter writer = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                            writer.Write(ByteRxData, 0, GlobalVariable.RxDataCount);
                            writer.Close();
                        }
                        GlobalVariable.ReportText?.Invoke("수신 완료");
                    }
                    GlobalVariable.ReportText?.Invoke("수신 중");
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

            //받은 데이터를 처리하세요
            if (this.SerialPort != null && this.SerialPort.IsOpen)
            {
                try
                {
                    int inputCount = this.SerialPort.BytesToRead;

                    if ((inputCount + RxOffSet) > 4096)
                    {
                        this.SerialPort.DiscardInBuffer();

                        RxOffSet = 0;

                        return;
                    }
                    else
                    {
                        this.SerialPort.Read(receiveData, RxOffSet, inputCount);

                        RxOffSet += inputCount;
                    }
                }
                catch (Exception ex)
                {
                    //Error Log를 작성하세요
                    Console.WriteLine("DataReceivedEvent :: {0}", ex.Message);
                }

                //답장할 메시지를 작성하고 큐에 담아주세요
                byte[] replyMessage = new byte[1];
                SendMessage(replyMessage);
            }
        }

        #endregion

        #region Thread Control Start() , Stop() { DisposeSerialPort() >> DisposeThread(Thread) }

        public void Start()
        {
            //스타트 조건을 작성하세요
            if (SerialSendThread == null && SerialReceiveThread == null)
            {
                try
                {
                    ThreadInit(null, null);
                    this.IsThreadRun = true;
                    this.SerialSendThread.Start();
                    this.SerialReceiveThread.Start();
                    this.SerialConnectCheck.Start();
                }

                catch (ThreadAbortException) { }

                catch (Exception ex)
                {
                    //Error Log를 작성하세요

                    Console.WriteLine("Start() :: {0}", ex.Message);
                }
            }
        }

        public void Stop()
        {
            this.IsThreadRun = false;
            DisposeSerialPort();
            DisposeThread(this.SerialSendThread);
            DisposeThread(this.SerialReceiveThread);
        }

        private void DisposeSerialPort()
        {
            try
            {
                if (this.SerialPort != null)
                {
                    if (this.SerialPort.IsOpen)
                    {
                        this.SerialPort.Close();
                    }
                    this.SerialPort.Dispose();
                }
            }
            catch { }

            finally
            {
                this.SerialPort = null;
            }
        }

        private void DisposeThread(Thread thread)
        {
            try
            {
                if (thread != null)
                {
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                    }
                    thread = null;
                }
            }

            catch { }

            finally
            {
                thread = null;
            }
        }
        #endregion Thread Control

    }
}
