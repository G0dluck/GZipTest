using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public abstract class GZip
    {
        protected readonly int _threads = Environment.ProcessorCount;

        protected BlockManager _poolReader;
        protected BlockManager _poolWriter;

        public GZip()
        {
            _poolReader = new BlockManager(_threads * 2);
            _poolWriter = new BlockManager(_threads * 2);
        }
    }
}
