using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    internal interface IJsonSerializable
    {
        bool LoadJSON(string path);
        bool SaveJSON(string path);
    }
}
