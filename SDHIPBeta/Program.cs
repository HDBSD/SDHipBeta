using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIP_Utilities;

namespace SDHIPBeta
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            string filePath = @"G:\SD\ISOs\Extracted\PS2 Beta 2\FOO1.HIP";
#else
            System.Windows.Forms.OpenFileDialog fileBrowser = new System.Windows.Forms.OpenFileDialog();
            fileBrowser.Filter = "*.HIP (*.HIP)";
            fileBrowser.ShowDialog();
            string filePath = fileBrowser.FileName;
#endif

            HIP gameHip = new HIP(filePath);

            Console.ReadKey();
        }
    }
}
