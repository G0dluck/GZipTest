using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public class DataBlock
    {
        public int ID { get; }
        public byte[] Buffer { get; }
        public byte[] CompressedBuffer { get; }

        public DataBlock(int id, byte[] buffer) : this(id, buffer, new byte[0])
        {
        }

        public DataBlock(int id, byte[] buffer, byte[] compressedBuffer)
        {
            ID = id;
            Buffer = buffer;
            CompressedBuffer = compressedBuffer;
        }
    }
}
