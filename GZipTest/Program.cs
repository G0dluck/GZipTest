using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                Validator.ArgumentValidation(args);
                ICommand command;

                if (args[0].ToLower() == "compress")
                {
                    command = new Compressor(new CompressReader(args[1]), new CompressWriter(args[2]));
                }
                else
                {
                    command = new Decompressor(new DecompressReader(args[1]), new DecompressWriter(args[2]));
                }

                command.Run();

                return command.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Обнаружена ошибка!\n Метод: {0}\n Описание ошибки: {1}", ex.TargetSite.Name, ex.Message);
                return 1;
            }
        }
    }
}
