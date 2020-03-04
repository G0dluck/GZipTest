using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    class CompressReader : IReader
    {
        public string SourceFile { get; set; }
        private readonly int _blockSize = 1048576; //Размер блока в 1 МБ

        public CompressReader (string sourceFile)
        {
            SourceFile = sourceFile;
        }

        public void Read(IReadManager readManager)
        {
            using (FileStream fileToBeCompressed = new FileStream(SourceFile, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                byte[] lastBuffer;

                while (fileToBeCompressed.Position < fileToBeCompressed.Length)
                {
                    if (fileToBeCompressed.Length - fileToBeCompressed.Position <= _blockSize)
                    {
                        bytesRead = (int)(fileToBeCompressed.Length - fileToBeCompressed.Position);
                    }
                    else
                    {
                        bytesRead = _blockSize;
                    }

                    lastBuffer = new byte[bytesRead];
                    fileToBeCompressed.Read(lastBuffer, 0, bytesRead);
                    readManager.SetBlock(lastBuffer);
                    Progress.Show(fileToBeCompressed.Position, fileToBeCompressed.Length);
                }
            }
        }
    }
}
