﻿using Emulator.GB.Interfaces;
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

        public ushort AF
        {
            get
            {
                return (ushort)(A << 8 | F);
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

        public ushort BC
        {
            get
            {
                return (ushort)(B << 8 | C);
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

        public ushort DE
        {
            get
            {
                return (ushort)(D << 8 | E);
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

        public ushort HL
        {
            get
            {
                return (ushort)(H << 8 | L);

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

        public ushort SP
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
            // Reset the last OpTime, this should be updated by each op code
            _lastOpTime = 0;
            // Direct array access should be quicker than Dictionary or predicates
            // TODO : Bench for Action overhead
            _opCodes[opCode]();
        }

        public byte FetchNextOpCode()
        {
            return _mmu.ReadByte(PC++);
        }

        /// <summary>
        /// Initialize the registers to simulate the "BIOS" code
        /// </summary>
        public void Init()
        {
            
        }

        /// <summary>
        /// Execs the "BIOS" code for real
        /// </summary>
        public void ExecBios()
        {

        }


        public void SetMMU(IMMU mmu)
        {
            _mmu = mmu;
        }

        public void SetRegister(Expression<Func<ICpu, byte>> inExpr, byte value)
        {
            SetRegister<byte>(inExpr, value);
        }

        public void SetRegister(Expression<Func<ICpu, ushort>> inExpr, ushort value)
        {
            SetRegister<ushort>(inExpr, value);
        }

        public void SetRegister(Expression<Func<ICpu, int>> inExpr, int value)
        {
            SetRegister<int>(inExpr, value);
        }

        public void SetRegister<T>(Expression<Func<ICpu, T>> inExpr, T value) where T : struct
        {
            var expr = (MemberExpression)inExpr.Body;
            var prop = (PropertyInfo)expr.Member;

            prop = typeof(CPU).GetProperty(prop.Name);

            prop.SetValue(this, value);
        }
    }
}
