using System;
using System.Text;

namespace FileSenderApp
{
    class Config
    {
        // DLLImport를 이용해 Kernel32함수를 사용함 - 파일에 있는 데이터를 읽기 위한 함수
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern int GetPrivateProfileString(string SectionName, string KeyName, string DefaultValue, StringBuilder stringBuilder, int size, string FileName);

        // DLLImport를 이용해 Kernel32함수를 사용함 - 파일에 데이터를 쓰기 위한 함수
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern int WritePrivateProfileString(string SectionName, string KeyName, string Value, string FileName);

        // .ini파일 생성 경로: FileSenderApp/FileSenderApp/bin/Debug 안에 있음
        public static string path = Environment.CurrentDirectory + "\\FileSenderApp.ini";

        //RS232 통신에 필요한 변수
        public static string Serial_Port = String.Empty;
        public static string Baudrate = String.Empty;
        public static string Parity = String.Empty;
        public static string DataBits = String.Empty;
        public static string StopBits = String.Empty;

        //Ethernet 통신에 필요한 변수
        public static string ServerIP = String.Empty;
        public static string Ethernet_Port = String.Empty;

        public static bool check = false;

        public static string GetInformation(string _section, string _key)
        {
            StringBuilder buffer = new StringBuilder(1024);

            GetPrivateProfileString(_section, _key, "NONE", buffer, 1024, path);
            return buffer.ToString();
        }
        public static void SaveiniFile(string _port, string _baudrate, string _pariry, string _databits, string _stopbits)
        {
            // RS232 통신에 필요한 정보
            WritePrivateProfileString("SERIAL", "Serial_Port", _port, path);
            WritePrivateProfileString("SERIAL", "Baudrate", _baudrate, path);
            WritePrivateProfileString("SERIAL", "Parity", _pariry, path);
            WritePrivateProfileString("SERIAL", "DataBits", _databits, path);
            WritePrivateProfileString("SERIAL", "StopBits", _stopbits, path);
        }
        public static void SaveiniFile(string _serverIP, string _port)
        {
            // Ethernet 통신에 필요한 정보
            WritePrivateProfileString("SERVER", "ServerIP", _serverIP, path);
            WritePrivateProfileString("SERVER", "Ethernet_Port", _port, path);
        }

        /*
        public static void WriteUpdate(bool b)
        {
            string value = b == true ? "true" : "false";
            WritePrivateProfileString("UPDATE", "UPDATING", value, path);
        }

        public static string ReadUpdate()
        {
            StringBuilder buffer = new StringBuilder(1024);
            GetPrivateProfileString("UPDATE", "UPDATING", "", buffer, 1024, path);

            return buffer.ToString();
        }
        */
    }
}
