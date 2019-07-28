using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.GB.Core
{
    public partial class MMU : IMMU
    {
        private byte[] _highRam = new byte[127];
        private ICartridge _cartridge;
        private GPU _gpu = new GPU();

        public byte ReadByte(int address)
        {
            if(address < 256)
            {
                return _bios[address];
            }

            // Cartridge
            if (address < 0x8000)
                return _cartridge.ReadByte(address);
            if (address >= 0x8000 && address < 0xA000)
            {
                int shiftedAddress = address & 0x1FFF;
                return _gpu.VRam[shiftedAddress];
            }

            // GPU
            if (address > 0xFF00 && address < 0xFF7F)
                return _gpu.ReadByte(address);

            return GetBufferArea(address)[0];

            throw new NotImplementedException("MMU failed at address "+ address);
        }

        public ushort ReadWord(int address)
        {
            var lowByte = ReadByte(address++);
            var highByte = ReadByte(address);

            return (ushort)((highByte << 8) + lowByte);
        }

        public void WriteByte(int address, byte value)
        {
            if(address >= 0x8000 && address < 0xA000)
            {
                int shiftedAddress = address & 0x1FFF;
                _gpu.VRam[shiftedAddress] = value;
            }
            else if (address > 0xFF00 && address < 0xFF7F)
                _gpu.WriteByte(address, value);
            else
            {
                var area = GetBufferArea(address);
                area[0] = value;
            }
        }

        public void WriteWord(int address, int value)
        {
            WriteByte(address + 1, (byte)(value >> 8));
            WriteByte(address, (byte)(value & 0xFF));
        }

        private Span<byte> GetBufferArea(int address)
        {
            if(address > 0xFF80)
            {
                return _highRam.AsSpan(address & 0x007F, 1);
            }

            throw new NotImplementedException();
        }

        public void SetCartridge(ICartridge cartridge)
        {
            _cartridge = cartridge;
        }
    }
}
