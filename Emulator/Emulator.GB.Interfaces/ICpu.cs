using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emulator.GB.Interfaces
{
    public interface ICpu
    {
        #region Registers
        short AF { get; }
        short BC { get; }
        short DE { get; }
        short HL { get; }

        byte A { get; }
        byte B { get; }
        byte C { get; }
        byte D { get; }
        byte E { get; }
        byte F { get; }
        byte H { get; }
        byte L { get; }


        /// <summary>
        /// Program Counter - Instruction Pointer / Points to the next instruction to be executed in the GB memory
        /// </summary>
        short PC { get; }

        /// <summary>
        /// Stack pointer
        /// </summary>  
        short SP { get; }

        bool ZeroFlag { get; }

        bool SubstractFlag { get; }

        bool HalfCarryFlag { get; }

        bool CarryFlag { get; }
        #endregion

        IMMU MMU { get; }

        int LastOpTime { get; }
        void Exec(int opCode);

        void Init();

        #region Debugs interface
        void SetMMU(IMMU mmu);
        void SetRegister(ref byte b, byte value);
        #endregion
    }
}
