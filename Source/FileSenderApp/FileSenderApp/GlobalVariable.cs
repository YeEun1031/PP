using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FileSenderApp
{
    public static class GlobalVariable
    {
        public static Action<string> ReportText;
        public static Action<string, bool> StateReport;
        public static Action<int> PrograssBarValue;
        public static Action<int> PrograssBarMaxValue;

        public static int fileLength = 0;
        public static int RxDataCount = 0;
        public static string fileName = string.Empty;
    }
}
