using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GZipTest
{
    public class BlockManager : IReadManager, IWriteManager
    {
        private readonly object locker = new object();
        private BlockingCollection<DataBlock> _pool;
        private int blockId = 0;

        public BlockManager(int capacity)
        {
            _pool = new BlockingCollection<DataBlock>(capacity);
        }

        public void SetBlockForWrite(int id, byte[] buffer)
        {
            if (_pool.IsCompleted)
                return;
            var block = new DataBlock(id, buffer);
            lock (locker)
            {
                while (id != blockId)
                {
                    Monitor.Wait(locker);
                }

                _pool.Add(block);
                blockId++;
                Monitor.PulseAll(locker);
            }
        }

        public void SetBlock(byte[] buffer, byte[] compressedData = null)
        {
            var block = new DataBlock(blockId, buffer, compressedData);
            _pool.Add(block);
            blockId++;
        }


        public IEnumerable<DataBlock> GetBlocks()
        {
            return _pool.GetConsumingEnumerable();
        }

        public void Complete()
        {
            _pool.CompleteAdding();
        }
    }
}
