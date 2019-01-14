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
        private bool _interruptsEnabled;
        private uint _intructionsCounter = 0;
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

        #region Flags
        private bool _fz;

        public bool ZeroFlag
        {
            get { return _fz; }
            private set { _fz = value; }
        }

        private bool _fn;

        public bool SubstractFlag
        {
            get { return _fn; }
            set { _fn = value; }
        }

        private bool _fh;

        public bool HalfCarryFlag
        {
            get { return _fh; }
            set { _fh = value; }
        }

        private bool _carryFlag;

        public bool CarryFlag
        {
            get { return _carryFlag; }
            set { _carryFlag = value; }
        }

        #endregion

        private Action[] _opCodes = new Action[0xFF];
        private Action[] _opCodesExt = new Action[0xFF];


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

        private int _pc = 0;
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

        private ushort _sp;
        public ushort SP
        {
            get
            {
                return _sp;
            }
            set
            {
                _sp = value;
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

            byte extOpCode = 0;

            try
            {
                _intructionsCounter++;
                if (opCode == 0xCB)
                {
                    extOpCode = FetchNextOpCode();
                    _opCodesExt[extOpCode]();
                }
                else
                    _opCodes[opCode]();
            }
            catch(Exception e)
            {
                throw new InvalidOperationException($"CPU thrown an exception with state : \n" +
                                                $"OP Code: {opCode:X} - Ext : {extOpCode:X} \n" +
                                                $"PC: {PC} \n A: {A} \n B: {B} \n C: {C}\n D: {D}\n E: {E}\n"+
                                                $"Instructions run : {_intructionsCounter}", e);
            }
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
            PC = 0;


        }

        public void Step()
        {
            var opCode = FetchNextOpCode();
            Exec(opCode);
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
