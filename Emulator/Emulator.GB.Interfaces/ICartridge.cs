using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Emulator.GB.Interfaces
{
    public interface ICartridge
    {
        void LoadROM(byte[] rom);
        void LoadROM(Stream romStream);

        byte ReadByte(int address);
        void WriteByte(int address, byte value);

        void LoadRAM(Stream ramStream);
        Stream GetRAM();
    }
}
