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
        Dictionary<int, byte> _mapWrittenAddress = new Dictionary<int, byte>();

        public AddressReturnedMMU()
        {

        }

        public byte ReadByte(int address)
        {
            if (_mapWrittenAddress.ContainsKey(address))
                return _mapWrittenAddress[address];

            var h = address >> 8 & 0xFF;
            var l = address & 0x00FF;

            address = h ^ l;

            return (byte)(address);
        }

        public ushort ReadWord(int address)
        {
            byte ls = ReadByte(address);
            byte ms = ReadByte(address + 1);
            return (ushort)((ms << 8) | ls);
        }

        public void SetCartridge(ICartridge cartridge)
        {
            

        }

        public void SetGPU(IGpu gpu)
        {
            
        }

        public void WriteByte(int address, byte value)
        {
            _mapWrittenAddress[address] = value;
        }

        public void WriteWord(int address, int value)
        {
            throw new NotImplementedException();
        }
    }
}
