using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    class DecompressWriter : IWriter
    {
        public string DestinationFile { get; set; }

        public DecompressWriter(string destinationFile)
        {
            DestinationFile = destinationFile;
        }

        public void Write(IWriteManager writeManager)
        {
            using (FileStream decompressedFile = new FileStream(DestinationFile, FileMode.Create))
            {
                foreach (var block in writeManager.GetBlocks())
                {
                    decompressedFile.Write(block.Buffer, 0, block.Buffer.Length);
                }
            }
        }
    }
}
