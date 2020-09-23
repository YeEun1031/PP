using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FileSenderApp
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        
        /*
        static void EthernetConfigCall()
        {
            try
            {
                string UpdateInfo = Config.ReadUpdate();

                Config.WriteUpdate(false);

                if (UpdateInfo == "true")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    int count = 0;

                    Process[] process = Process.GetProcesses();

                    foreach(Process p in process)
                    {
                        if (p.ProcessName.Equals("FileSenderApp") == true)
                            count++;
                    }

                    if(count > 1)
                    {
                        MessageBox.Show("이미 실행 중입니다.");
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new MainForm());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Program - Error");
            }
        }
        */
    }
}
