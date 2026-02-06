using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    internal interface ICsvSerializable
    {
        bool LoadCSV(string path);
        bool SaveCSV(string path);
    }
}
