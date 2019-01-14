using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Emulator.GB.Core.Cartridge
{
    public class ROM : ICartridge
    {
        private byte[] _rom;

        public Stream GetRAM()
        {
            throw new NotImplementedException();
        }

        public void LoadRAM(Stream ramStream)
        {
            throw new NotImplementedException();
        }

        public void LoadROM(byte[] rom)
        {
            _rom = rom;
        }

        public void LoadROM(Stream romStream)
        {
            _rom = new byte[romStream.Length];
            romStream.Read(_rom, 0, _rom.Length);
        }

        public byte ReadByte(int address)
        {
            return _rom[address];
        }

        public void WriteByte(int address, byte value)
        {
            _rom[address] = value;
        }
    }
}
