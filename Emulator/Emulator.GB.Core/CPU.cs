using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core
{
    public partial class CPU : ICpu
    {
        #region Private Fields
        private int _lastOpTime;
        private IMMU _mmu;
        private byte _a, _b, _c, _d, _e, _h, _f, _l;
        #endregion

        #region Registers
        public byte A
        {
            get
            {
                return _a;
            }
            private set
            {
                _a = value;
            }
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
            get
            {
                return _b;
            }
            private set
            {
                _b = value;
            }
        }

        public short BC
        {
            get
            {
                return (short)(B << 8 | C);
            }
            private set
            {
                B = (byte)(value >> 8 & 0xFF);
                C = (byte)(value & 0xFF);
            }
        }

        public byte C
        {
            get
            {
                return _c;
            }
            private set
            {
                _c = value;
            }
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
                return _d;
            }
            private set
            {
                _d = value;
            }
        }

        public short DE
        {
            get
            {
                return (short)(D << 8 | E);
            }
            private set
            {
                D = (byte)(value >> 8 & 0xFF);
                E = (byte)(value & 0xFF);
            }
        }

        public byte E
        {
            get
            {
                return _e;
            }
            private set
            {
                _e = value;
            }
        }

        public byte F
        {
            get
            {
                return _f;
            }
            private set
            {
                _f = value;
            }
        }

        public byte H
        {
            get
            {
                return _h;
            }
            private set
            {
                _h = value;
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
                return (short)(H << 8 | L);

            }
            private set
            {
                H = (byte)(value >> 8 & 0xFF);
                L = (byte)(value & 0xFF);
            }
        }

        public byte L
        {
            get
            {
                return _l;
            }
            private set
            {
                _l = value;
            }
        }
        #endregion

        private Action[] _opCodes = new Action[0xFF];

        
        public int LastOpTime
        {
            get
            {
                return _lastOpTime;
            }
        }

        public IMMU MMU
        {
            get
            {
                return _mmu;
            }
        }

        private int _pc;
        public int PC
        {
            get
            {
                return _pc;
            }
            private set
            {
                _pc = value;
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

        public CPU()
        {
            InitializeOpCodes();
        }

        public void Exec(int opCode)
        {
            // Direct array access should be quicker than Dictionary or predicates
            // Bench for Action overhead
            _opCodes[opCode]();
        }

        public void Init()
        {
            
        }

        public void SetMMU(IMMU mmu)
        {
            _mmu = mmu;
        }

        public void SetRegister(Expression<Func<ICpu, byte>> inExpr, byte value)
        {
            var expr = (MemberExpression)inExpr.Body;
            var prop = (PropertyInfo)expr.Member;

            prop = typeof(CPU).GetProperty(prop.Name);

            prop.SetValue(this, value);
        }

        public void SetRegister(Expression<Func<ICpu, short>> inExpr, short value)
        {
            var expr = (MemberExpression)inExpr.Body;
            var prop = (PropertyInfo)expr.Member;

            prop = typeof(CPU).GetProperty(prop.Name);

            prop.SetValue(this, value);
        }

        public void SetRegister(Expression<Func<ICpu, int>> inExpr, int value)
        {
            var expr = (MemberExpression)inExpr.Body;
            var prop = (PropertyInfo)expr.Member;

            prop = typeof(CPU).GetProperty(prop.Name);

            prop.SetValue(this, value);
        }
    }
}
