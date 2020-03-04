using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public static class Validator
    {
        public static void ArgumentValidation(string[] args)
        {

            if (args.Length != 3)
            {
                throw new ArgumentException("Пожалуйста, введите параметры следующим образом:\n compress/decompress [исходный файл] [результирующий файл].");
            }

            if (args[0].ToLower() != "compress" && args[0].ToLower() != "decompress")
            {
                throw new ArgumentException("Первый параметр должен быть \"compress\" или \"decompress\".");
            }

            if (args[1] == args[2])
            {
                throw new ArgumentException("Имена исходного и результирующего файла должны быть разными.");
            }

            if (!File.Exists(args[1]))
            {
                throw new FileNotFoundException("Исходный файл не найден.");
            }

            FileInfo fileIn = new FileInfo(args[1]);
            FileInfo fileOut = new FileInfo(args[2]);

            if (!Directory.Exists(fileOut.DirectoryName))
            {
                throw new DirectoryNotFoundException(string.Format("Указанного пути не существует ({0}).", fileOut.DirectoryName));
            }

            if (fileIn.Extension == ".gz" && args[0].ToLower() == "compress")
            {
                throw new ArgumentException("Файл уже был сжат.");
            }

            if (fileIn.Extension != ".gz" && args[0].ToLower() == "decompress")
            {
                throw new ArgumentException("Файл для распаковки должен быть с раширением .gz.");
            }

            if ((fileOut.Extension == ".gz" || fileIn.Extension == ".gz") && fileOut.Exists)
            {
                throw new ArgumentException("Результирующий файл уже существует.");
            }
        }
    }
}
