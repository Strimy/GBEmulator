using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core
{
    public class CPU : ICpu
    {
        public byte A
        {
            get;  private set;
        }

        public short AF
        {
            get
            {
                return (short)(A << 8 | F);
            }
        }

        public byte B
        {
            get; private set;
        }

        public short BC
        {
            get
            {
                return (short)(B << 8 | C);
            }
        }

        public byte C
        {
            get; set;
        }

        public bool CarryFlag
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte D
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public short DE
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte E
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte F
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte H
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HalfCarryFlag
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public short HL
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte L
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int LastOpTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMMU MMU
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public short PC
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public short SP
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool SubstractFlag
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool ZeroFlag
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Exec(int opCode)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void SetMMU(IMMU mmu)
        {
            throw new NotImplementedException();
        }

        public void SetRegister(ref byte b, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
