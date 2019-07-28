using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.GB.Core
{
    public class GPU
    {
        private byte[] _vram = new byte[0x2000];
        private byte[] _io = new byte[0x7F];

        public byte[] VRam => _vram;

        public byte LCDC
        {
            get
            {
                return _io[0x40];
            }
            set
            {
                _io[0x40] = value;
            }
        }

        public byte STAT
        {
            get
            {
                return _io[0x41];
            }
            set
            {
                _io[0x41] = value;
            }
        }

        public byte ReadByte(int address)
        {
            GPURegister localAddress = (GPURegister)(address - 0xFF00); 

            return _io[(byte)localAddress];
        }

        public void WriteByte(int address, byte value)
        {
            GPURegister localAddress = (GPURegister)(address - 0xFF00);

            _io[(byte)localAddress] = value;
        }
    }

    public enum GPURegister : byte
    {
        LDLC = 0x40,
        STAT = 0x41,
        SCY = 0x42,
        SCX = 0x43,
        LY = 0x44,
        LYC = 0x45,
        WY = 0x4A,
        WX = 0x4B,
    }

    [Flags]
    public enum STAT : byte
    {
        Mode2OAM = 0b0010_0000,
        Mode1VBlank = 0b0001_0000,
        Mode0HBlank = 0b0000_1000,
        Coincidence = 0b0000_0100,
        Mode0 = 0b0000_0000, // HBlank
        Mode1 = 0b0000_0001, // Vblank
        Mode2 = 0b0000_0010, // OAM Search
        Mode3 = 0b0000_0011, // LCD Transfer
    }
}
