using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    interface IReadManager
    {
        void SetBlock(byte[] buffer, byte[] compressedData = null);
        void Complete();
    }
}
