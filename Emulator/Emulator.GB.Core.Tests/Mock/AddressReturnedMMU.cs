using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core.Tests
{
    /// <summary>
    /// Used to test Load instructions by returning the address read (with according mask)
    /// </summary>
    public class AddressReturnedMMU : IMMU
    {
        public AddressReturnedMMU()
        {

        }

        public byte ReadByte(int address)
        {
            return (byte)(address & 0xFF);
        }

        public int ReadWord(int address)
        {
            byte ls = ReadByte(address);
            byte ms = ReadByte(address + 1);
            return (ms << 8) | ls;
        }

        public void WriteByte(int address, byte value)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(int address, int value)
        {
            throw new NotImplementedException();
        }
    }
}
