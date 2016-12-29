using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// Program Counter - Instruction Pointer 
        /// Points to the next instruction to be executed in the GB memory
        /// Or the address for the Load,...
        /// </summary>
        int PC { get; }

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

        byte FetchNextOpCode();

        void Init();
        void ExecBios();

        #region Debugs interface
        void SetMMU(IMMU mmu);
        void SetRegister(Expression<Func<ICpu, byte>> b, byte value);
        void SetRegister(Expression<Func<ICpu, short>> b, short value);
        void SetRegister(Expression<Func<ICpu, int>> b, int value);
        #endregion
    }
}
