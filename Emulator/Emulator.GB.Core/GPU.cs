using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.GB.Core
{
    public class GPU : IGpu
    {
        private byte[] _vram = new byte[0x2000];
        private byte[] _io = new byte[0x7F];
        private uint _modeClock = 0;

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

        public STAT STAT
        {
            get
            {
                return (STAT)_io[0x41];
            }
            set
            {
                _io[0x41] = (byte)value;
            }
        }

        public byte this[GPURegister register]
        {
            get
            {
                return _io[(byte)register];
            }
            set
            {
                _io[(byte)register] = value;
            }
        }

        public byte LY
        {
            get
            {
                return _io[(byte)GPURegister.LY];
            }
            set
            {
                _io[(byte)GPURegister.LY] = value;
            }
        }
        public byte LYC => _io[(byte)GPURegister.LYC];

        public GPUMode Mode
        {
            get
            {
                var statByte = (byte)STAT;
                return (GPUMode)(byte)(statByte & 0x03);
            }
            set
            {
                _modeClock = 0;
                STAT = (STAT)(((byte)STAT & 0b1111_1100) ^ (byte)value);
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

        internal void Step(uint instructionTime)
        {
            _modeClock += instructionTime;

            GPUMode mode = Mode;

            switch (mode)
            {
                case GPUMode.Mode0: // HBlank
                    Mode0();
                    break;
                case GPUMode.Mode1: // VBlank
                    Mode1();
                    break;
                case GPUMode.Mode2: // OAM
                    Mode2();
                    break;
                case GPUMode.Mode3: // LCD Transfer
                    Mode3();
                    break;
                default:
                    break;
            }
        }

        private void Mode2()
        {
            if (_modeClock >= 80)
            {
                Mode = GPUMode.Mode3;
            }
        }

        private void Mode3()
        {
            if (_modeClock >= 172)
            {
                Mode = GPUMode.Mode0;
            }
        }

        private void Mode1()
        {
            var line = _modeClock % 456;

            LY = (byte)(144 + line);

            if (_modeClock >= 4560)
            {
                Mode = GPUMode.Mode2;
                LY = 0;
            }
        }

        private void Mode0()
        {
            if (_modeClock >= 204)
            {
                var y = LY++;
                if (LY > 143)
                {
                    Mode = GPUMode.Mode1;
#warning Manage VBlank interrupt ?
                }
                else
                    Mode = GPUMode.Mode2;
            }
        }
    }


}
