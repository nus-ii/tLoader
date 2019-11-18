using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFormat
{
    public interface IDataObject
    {
        string CSVstring { get; }

        string CSVHeader { get; }
    }
}
