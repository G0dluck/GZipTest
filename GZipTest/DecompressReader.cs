using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    class DecompressReader : IReader
    {
        public string SourceFile { get; }

        public DecompressReader(string sourceFile)
        {
            SourceFile = sourceFile;
        }

        public void Read(IReadManager readManager)
        {
            using (FileStream compressedFile = new FileStream(SourceFile, FileMode.Open, FileAccess.Read))
            {
                while (compressedFile.Position < compressedFile.Length)
                {
                    byte[] lengthBuffer = new byte[8];
                    compressedFile.Read(lengthBuffer, 0, lengthBuffer.Length);
                    int blockLength = BitConverter.ToInt32(lengthBuffer, 4);
                    byte[] compressedData = new byte[blockLength];
                    lengthBuffer.CopyTo(compressedData, 0);

                    compressedFile.Read(compressedData, 8, blockLength - 8);
                    int dataSize = BitConverter.ToInt32(compressedData, blockLength - 4);
                    byte[] lastBuffer = new byte[dataSize];

                    readManager.SetBlock(lastBuffer, compressedData);
                    Progress.Show(compressedFile.Position, compressedFile.Length);
                }
            }
        }
    }
}
