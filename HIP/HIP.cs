using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Diagnostics;


namespace HIP_Utilities
{
    public class HIP
    {

        private BinaryReader hipReader;
        private Dictionary<string, byte[]> hipFile = new Dictionary<string, byte[]>();

        public HIP(string filePath)
        {
            // check that file exists

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Provided file could not be found.", Path.GetFileName(filePath));

            hipReader = new BinaryReader(File.OpenRead(filePath));
            parseHIP();


        }

        private void parseHIP()
        {
            //refering to https://heavyironmodding.org/wiki/HIP_(File_Format)#Overall_Structure there appears to be multiple sections to a HIP file.
            // Top level HIPA, PACK, DICT and STRM. at this stage i only care about these.


            while (hipReader.BaseStream.Position < hipReader.BaseStream.Length)
            {
                string hipSection = new string(hipReader.ReadChars(4));

                switch (hipSection)
                {
                    case "HIPA":
                    case "PACK":
                    case "DICT":
                    case "STRM":

                        // Convert Length to Big Endian

                        byte[] lengthBytes = hipReader.ReadBytes(4);
                        Array.Reverse(lengthBytes);
                        int secLength = BitConverter.ToInt32(lengthBytes, 0);

                        // Write Section to our Dictionary

                        hipFile.Add(hipSection, hipReader.ReadBytes(secLength));

                        break;
                    default:
                        Debug.WriteLine($"Unknown Section at Address: 0x{hipReader.BaseStream.Position.ToString("X2")}\n\t{{Str: '{hipSection}', Hex: '{string.Join("", hipSection.Select(c => ((int)c).ToString("X2")))}'}}");
                        break;
                }
            }

            hipReader.Close();
#if DEBUG
            Debug.WriteLine("HIP File processed, following sections were read into memory:\n\n");

            foreach (string Key in hipFile.Keys)
                Debug.WriteLine($"Section '{Key}' at a size of {hipFile[Key].Length} bytes.");
#endif
        }
    }
}
