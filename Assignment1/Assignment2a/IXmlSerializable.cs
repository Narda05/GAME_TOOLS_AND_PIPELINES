using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    internal interface IXmlSerializable
    {
        bool LoadXML(string path);
        bool SaveXML(string path);
    }
}
