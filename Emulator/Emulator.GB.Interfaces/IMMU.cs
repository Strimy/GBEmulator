using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Interfaces
{
    /// <summary>
    /// The memory management unit interface
    /// Defines how memory is accessed
    /// </summary>
    public interface IMMU
    {
        byte ReadByte(int address);
        ushort ReadWord(int address);

        void WriteByte(int address, byte value);
        void WriteWord(int address, int value);

        void SetCartridge(ICartridge cartridge);
        void SetGPU(IGpu gpu);
    }
}
