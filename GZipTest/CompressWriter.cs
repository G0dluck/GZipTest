using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    class CompressWriter : IWriter
    {
        public string DestinationFile { get; }

        public CompressWriter(string destinationFile)
        {
            DestinationFile = destinationFile;
        }

        public void Write(IWriteManager writeManager)
        {
            using (FileStream fileCompressed = new FileStream(DestinationFile + ".gz", FileMode.Create))
            {
                foreach (var block in writeManager.GetBlocks())
                {
                    BitConverter.GetBytes(block.Buffer.Length).CopyTo(block.Buffer, 4);
                    fileCompressed.Write(block.Buffer, 0, block.Buffer.Length);
                }
            }
        }
    }
}
