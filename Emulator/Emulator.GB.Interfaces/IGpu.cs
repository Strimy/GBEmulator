using System;

namespace Emulator.GB.Interfaces
{
    public interface IGpu
    {
        byte this[GPURegister register] { get; set; }

        LCDC LCDC { get; set; }
        byte LY { get; set; }
        byte LYC { get; }
        GPUMode Mode { get; set; }
        STAT STAT { get; set; }
        byte[] VRam { get; }

        byte ReadByte(int address);
        void WriteByte(int address, byte value);
    }

    public enum GPURegister : byte
    {
        LDLC = 0x40,
        STAT = 0x41,
        SCY = 0x42,
        SCX = 0x43,
        LY = 0x44,
        LYC = 0x45,
        BGP = 0x47,
        OBP0 = 0x48,
        OBP1 = 0x49,
        WY = 0x4A,
        WX = 0x4B,
    }

    [Flags]
    public enum LCDC : byte
    {
        LCDEnable = 0b1000_0000,
        /// <summary>
        /// 0=9800-9BFF, 1=9C00-9FFF
        /// </summary>
        WindowTileMapSelect = 0b0100_0000,

        WindowDisplayEnabled = 0b0010_0000,

        /// <summary>
        /// 0=8800-97FF, 1=8000-8FFF
        /// </summary>
        TileDataSelect = 0b0001_0000,

        /// <summary>
        /// 0=9800-9BFF, 1=9C00-9FFF
        /// </summary>
        BGTileMapSelect = 0b0000_1000,

        /// <summary>
        /// If flag is present, sprites are 8x16
        /// </summary>
        SpriteSize8x16 = 0b0000_0100,


        SpriteEnabled = 0b0000_0010,
        BGDisplayEnabled = 0b0000_0001,
    }

    [Flags]
    public enum STAT : byte
    {
        Mode2_OAM_Interrupt = 0b0010_0000,
        Mode1_VBlank_Interrupt = 0b0001_0000,
        Mode0_HBlank_Interrupt = 0b0000_1000,
        Coincidence = 0b0000_0100,
        Mode0 = 0b0000_0000, // HBlank
        Mode1 = 0b0000_0001, // Vblank
        Mode2 = 0b0000_0010, // OAM Search
        Mode3 = 0b0000_0011, // LCD Transfer
    }

    public enum GPUMode : byte
    {
        /// <summary>
        /// HBlank
        /// </summary>
        Mode0,
        /// <summary>
        /// VBlank
        /// </summary>
        Mode1,
        Mode2,
        Mode3
    }
}