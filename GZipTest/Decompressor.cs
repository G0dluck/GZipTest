using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GZipTest
{
    class Decompressor : GZip, ICommand
    {
        public IReader Reader { get; set; }
        public IWriter Writer { get; set; }
        public int Success { get; set; }

        private bool _isDelete = false;
        public Decompressor(IReader reader, IWriter writer) : base()
        {
            Reader = reader;
            Writer = writer;
            Success = 1;
        }

        public void Read()
        {
            try
            {
                Reader.Read(_poolReader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка чтения файла. \nОписание ошибки: {0}", ex.Message);
                _isDelete = true;
            }
            finally
            {
                _poolReader.Complete();
            }
        }

        public void Run()
        {
            Console.WriteLine("Распаковка...\n");

            Thread _reader = new Thread(new ThreadStart(Read));
            _reader.Start();

            var tpool = new Thread[_threads];

            for (int i = 0; i < _threads; i++)
            {
                tpool[i] = new Thread(Decompress);
                tpool[i].Start(i);
            }

            Thread _writer = new Thread(new ThreadStart(Write));
            _writer.Start();

            _reader.Join();
            foreach (var t in tpool)
            {
                t.Join();
            }
            _poolWriter.Complete();
            _writer.Join();

            if (!_isDelete)
            {
                Console.WriteLine("\nРаспаковка успешно завершена.");
                Success = 0;
            }
        }

        public void Write()
        {
            try
            {
                Writer.Write(_poolWriter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка записи в файл. \nОписание ошибки: {0}", ex.Message);
                _poolWriter.Complete();
                _isDelete = true;
            }

            if (_isDelete)
            {
                try
                {
                    File.Delete(Writer.DestinationFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nОшибка удаления файла. \nОписание ошибки: {0}", ex.Message);
                }
            }
        }

        private void Decompress(object i)
        {
            try
            {
                foreach (var block in _poolReader.GetBlocks())
                {
                    using (MemoryStream ms = new MemoryStream(block.CompressedBuffer))
                    {
                        using (GZipStream cs = new GZipStream(ms, CompressionMode.Decompress))
                        {
                            cs.Read(block.Buffer, 0, block.Buffer.Length);
                            byte[] decompressedData = block.Buffer.ToArray();
                            _poolWriter.SetBlockForWrite(block.ID, decompressedData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка в потоке номер {0}. \nОписание ошибки: {1}", i, ex.Message);
                _isDelete = true;
            }
        }
    }
}
