using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public static class Progress
    {
        public static void Show(long current, long overall)
        {
            Console.CursorVisible = false;
            Console.CursorLeft = 0;
            long percentage = overall / 100;

            Console.Write(" " + current / percentage + "%" + " " + current.ToString() + " of " + overall.ToString());

            if (current / percentage == 100)
            {
                Console.CursorLeft = 0;
                Console.Write(" " + "Завершено" + " " + current.ToString() + " из " + overall.ToString());
                Console.WriteLine("\nПожалуйста подождите завершение процесса.");
            }
        }
    }
}
