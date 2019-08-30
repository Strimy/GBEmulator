using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Emulator.GB.Core
{
    public class GPU : IGpu
    {
        public event EventHandler VBlank;

        private byte[] _vram = new byte[0x2000];
        private byte[] _io = new byte[0x7F];
        private uint _modeClock = 0;
        private byte[] _oam = new byte[0xA0];
        private byte[] _frame = new byte[256 * 256];

        public byte[] VRam => _vram;

        public LCDC LCDC
        {
            get
            {
                return (LCDC)_io[0x40];
            }
            set
            {
                _io[0x40] = (byte)value;
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
        public byte WY => _io[(byte)GPURegister.WY];
        public byte WX => _io[(byte)GPURegister.WX];
        public byte SCY => _io[(byte)GPURegister.SCY];
        public byte SCX => _io[(byte)GPURegister.SCX];
        public byte BGP => _io[(byte)GPURegister.BGP];
        public byte OBP0 => _io[(byte)GPURegister.OBP0];
        public byte OBP1 => _io[(byte)GPURegister.OBP1];

        public ReadOnlySpan<OAM> OAMTable
        {
            get
            {
                return MemoryMarshal.Cast<byte, OAM>(_oam.AsSpan());
            }
        }

        public ReadOnlySpan<BGTile> VRamTiles
        {
            get
            {
                return MemoryMarshal.Cast<byte, BGTile>(_vram.AsSpan().Slice(0, 0x9800 - 0x8000));
            }
        }

        public ReadOnlySpan<byte> BackgroundMap0
        {
            get
            {
                return _vram.AsSpan(0x1800, 1024);
            }
        }

        public ReadOnlySpan<byte> BackgroundMap1
        {
            get
            {
                return _vram.AsSpan(0x1C00, 1024);
            }
        }

        public ReadOnlySpan<BGTile> SelectedBackgroundTileData
        {
            get
            {
                if (LCDC.HasFlag(LCDC.TileDataSelect))
                    return VRamTiles.Slice(0, 0x1000 / 16);
                else
                    return VRamTiles.Slice(0x0800 / 16, 0x1000 / 16);
            }
        }

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

        public GPU()
        {
            Mode = GPUMode.Mode1;
        }

        public byte ReadByte(int address)
        {
            if (address < 0x8000)
                throw new InvalidOperationException("Not a GPU memory portion");
            else if(address < 0x9FFF)
                return _vram[address - 0x8000];
            else if(address < 0xFE00)
                throw new InvalidOperationException("Not a GPU memory portion");
            else if(address < 0xFEA0)
                return _oam[address - 0xFE00];
            



            GPURegister localAddress = (GPURegister)(address - 0xFF00);
            return _io[(byte)localAddress];
        }

        public void WriteByte(int address, byte value)
        {
            if (address < 0x8000)
                throw new InvalidOperationException("Not a GPU memory portion");
            else if (address < 0xA000)
                _vram[address - 0x8000] = value;
            else if (address < 0xFE00)
                throw new InvalidOperationException("Not a GPU memory portion");
            else if (address < 0xFEA0)
                _oam[address - 0xFE00] = value;
            else
            {
                GPURegister localAddress = (GPURegister)(address - 0xFF00);
                _io[(byte)localAddress] = value;

                if (address == 0x46)
                    Debugger.Break();
            }
        }

        internal void Step(uint instructionTime)
        {
            if (!LCDC.HasFlag(LCDC.LCDEnable))
                return;

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

        private void Scanline()
        {
            int line = LY;
            var frameLine = _frame.AsSpan(line * 256, 256);

            ReadOnlySpan<byte> windowTileMap = GetSelectedWindowTileMap();
            ReadOnlySpan<byte> bgTileMap = GetSelectedBackgroundTileMap();
            ReadOnlySpan<BGTile> bgTileData = SelectedBackgroundTileData;

            bool isWindowDisplayEnabled = LCDC.HasFlag(LCDC.WindowDisplayEnabled);

            var tileMapSliced = bgTileMap.Slice(line / 8 * 32, 32);


            if(isWindowDisplayEnabled)
            {
                // TODO : Draw the window over
            }
        }

        private ReadOnlySpan<byte> GetSelectedWindowTileMap()
        {
            if (LCDC.HasFlag(LCDC.WindowTileMapSelect))
                return BackgroundMap1;
            else
                return BackgroundMap0;
        }

        private ReadOnlySpan<byte> GetSelectedBackgroundTileMap()
        {
            if (LCDC.HasFlag(LCDC.BGTileMapSelect))
                return BackgroundMap1;
            else
                return BackgroundMap0;
        }



        /// <summary>
        /// LCD reading from OAM only
        /// </summary>
        private void Mode2()
        {
            if (_modeClock >= 80)
            {
                Mode = GPUMode.Mode3;
            }
        }

        /// <summary>
        /// LCD Transfer of the data from the OAM/Vram to the frame
        /// This is where the frame data is built
        /// </summary>
        private void Mode3()
        {
            if (_modeClock >= 172)
            {
                Scanline();
                Mode = GPUMode.Mode0;
            }
        }



        /// <summary>
        /// VBlank
        /// Reports a new frame to display
        /// </summary>
        private void Mode1()
        {
            var line = _modeClock % 456;

            LY = (byte)(144 + line);

            if (_modeClock >= 4560)
            {
                Mode = GPUMode.Mode2;
                LY = 0;
                VBlank?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// HBlank
        /// </summary>
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

    [StructLayout(LayoutKind.Explicit)]
    public struct OAM
    {
        [FieldOffset(0)]
        private byte _y;

        [FieldOffset(1)]
        private byte _x;

        [FieldOffset(2)]
        private byte _tile;

        [FieldOffset(3)]
        private OAMFlags _flags;

        public byte Y => _y;
        public byte X => _x;
        public byte Tile => _tile;
        public OAMFlags Flags => _flags;

    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BGTile
    {
        [FieldOffset(0)]
        public byte x0y0;

        [FieldOffset(1)]
        public byte x1y0;

        [FieldOffset(2)]
        public byte x0y1;

        [FieldOffset(3)]
        public byte x1y1;

        [FieldOffset(4)]
        public byte x0y2;

        [FieldOffset(5)]
        public byte x1y2;

        [FieldOffset(6)]
        public byte x0y3;

        [FieldOffset(7)]
        public byte x1y3;

        [FieldOffset(8)]
        public byte x0y4;

        [FieldOffset(9)]
        public byte x1y4;

        [FieldOffset(10)]
        public byte x0y5;

        [FieldOffset(11)]
        public byte x1y5;

        [FieldOffset(12)]
        public byte x0y6;

        [FieldOffset(13)]
        public byte x1y6;

        [FieldOffset(14)]
        public byte x0y7;

        [FieldOffset(15)]
        public byte x1y7;

        [FieldOffset(0)]
        public ushort L1;
        [FieldOffset(2)]
        public ushort L2;
        [FieldOffset(4)]
        public ushort L3;
        [FieldOffset(6)]
        public ushort L4;
        [FieldOffset(8)]
        public ushort L5;
        [FieldOffset(10)]
        public ushort L6;
        [FieldOffset(12)]
        public ushort L7;
        [FieldOffset(14)]
        public ushort L8;
    }

    [Flags]
    public enum OAMFlags : byte
    {
        BehindBG =  0b1000_0000,
        YFlip =     0b0100_0000,
        XFlip =     0b0010_0000,
        OBP1 =      0b0001_0000, 
    }

}
