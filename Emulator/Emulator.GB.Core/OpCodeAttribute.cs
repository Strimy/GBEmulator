using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core
{
    public class OpCodeAttribute : Attribute
    {
        public byte OpCode { get; set; }
        

    }
}
