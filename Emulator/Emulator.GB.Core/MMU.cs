using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.GB.Core
{
    public partial class MMU : IMMU
    {
        private byte[] _highRam = new byte[127];
        private byte[] _lowRam = new byte[0xDFFF-0xC000];
        private ICartridge _cartridge;
        private IGpu _gpu;
        private byte[] _sound = new byte[0x40];

        public void SetGPU(IGpu gpu)
        {
            _gpu = gpu;
        }

        public byte ReadByte(int address)
        {
            if(address < 256)
            {
                return _bios[address];
            }

            if (address < 0x8000)
                return _cartridge.ReadByte(address);
            if (address < 0xA000)
                return _gpu.ReadByte(address);
            if (address < 0xC000)
                return _cartridge.ReadByte(address);
            if (address < 0xFE00)
                return _lowRam[address];
            if (address < 0xFEA0)
                return _gpu.ReadByte(address);
            if (address < 0xFF40)
                return _sound[address - 0xFF00];
            if (address < 0xFF80)
                return _gpu.ReadByte(address);
            if (address < 0xFFFF) // FF80 -> FFFE
                return _highRam[address & 0x7F];



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
            if (address < 0x8000)
                _cartridge.WriteByte(address, value);
            else if (address < 0xA000)
                _gpu.WriteByte(address, value);
            else if (address < 0xC000)
                _cartridge.WriteByte(address, value);
            else if (address < 0xFE00)
                _lowRam[address] = value;
            else if (address < 0xFEA0)
                _gpu.WriteByte(address, value);
            else if (address < 0xFF40)
                _sound[address - 0xFF00] = value;
            else if (address < 0xFF80)
                _gpu.WriteByte(address, value);
            else if (address < 0xFFFF) // FF80 -> FFFE
                _highRam[address & 0x7F] = value;
            else
                throw new InvalidOperationException("Non managed MMU access");
            //if (address >= 0x8000 && address < 0xA000)
            //{
            //    int shiftedAddress = address & 0x1FFF;
            //    _gpu.VRam[shiftedAddress] = value;
            //}
            //else if (address >= 0xFF40 && address < 0xFF7F)
            //    _gpu.WriteByte(address, value);
            //else
            //{
            //    var area = GetBufferArea(address);
            //    area[0] = value;
            //}
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
            else if(address > 0xFF00 && address < 0xFF40)
            {
                return _sound.AsSpan(address - 0xFF00, 1);
            }

            throw new NotImplementedException();
        }

        public void SetCartridge(ICartridge cartridge)
        {
            _cartridge = cartridge;
        }
    }
}
