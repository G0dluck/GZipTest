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
    class Compressor : GZip, ICommand
    {
        public IReader Reader { get; set; }
        public IWriter Writer { get; set; }
        public int Success { get; set; }

        private bool _isDelete = false;
        public Compressor(IReader reader, IWriter writer) : base()
        {
            Reader = reader;
            Writer = writer;
            Success = 1;
        }

        private void Read()
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
            Console.WriteLine("Сжатие...\n");

            Thread _reader = new Thread(new ThreadStart(Read));
            _reader.Start();

            var tpool = new Thread[_threads];

            for (int i = 0; i < _threads; i++)
            {
                tpool[i] = new Thread(Compress);
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
                Console.WriteLine("\nСжатие успешно завершено.");
                Success = 0;
            }
        }

        private void Write()
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
                    File.Delete(Writer.DestinationFile + ".gz");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nОшибка удаления файла. \nОписание ошибки: {0}", ex.Message);
                }
            }
        }

        private void Compress(object i)
        {
            try
            {
                foreach (var block in _poolReader.GetBlocks())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (GZipStream cs = new GZipStream(memoryStream, CompressionMode.Compress))
                        {

                            cs.Write(block.Buffer, 0, block.Buffer.Length);
                        }

                        byte[] compressedData = memoryStream.ToArray();
                        _poolWriter.SetBlockForWrite(block.ID, compressedData);
                    }            
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка в потоке номер {0}. \nОписание ошибки: {1}", i, ex.Message);
                _poolWriter.Complete();
                _isDelete = true;
            }

        }
    }
}
