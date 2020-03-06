using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    interface ICommand
    {
        int Success { get; set; }

        void Run();
    }
}
