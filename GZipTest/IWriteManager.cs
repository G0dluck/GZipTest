using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    interface IWriteManager
    {
        IEnumerable<DataBlock> GetBlocks();
        void Complete();
    }
}
