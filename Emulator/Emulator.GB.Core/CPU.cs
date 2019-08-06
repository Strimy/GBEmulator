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
        public uint ClockFrequency => 4194304;
        public decimal ClockFrequencyMhz => 4.194304M;
        public decimal LastInstructionMicroSec => 1.0M / ClockFrequencyMhz * _lastOpTime;
        public int LastInstructionClockTime => (int)_lastOpTime;

        #region Private Fields
        private uint _lastOpTime;
        private IMMU _mmu;
        private byte _a, _b, _c, _d, _e, _h, _f, _l;
        private bool _interruptsEnabled;
        private uint _intructionsCounter = 0;
        private int _lastOpCode;
        private GPU _gpu;

        #endregion

        public IGpu GPU => _gpu;

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
        [Flags]
        private enum FlagEnum : byte
        {
            Zero =          0b1000_0000,
            Sub =           0b0100_0000,
            HalfCarry =     0b0010_0000,
            Carry =         0b0001_0000,
        }


        public bool ZeroFlag
        {
            get
            {
                return GetFlag(FlagEnum.Zero);
            }
            private set
            {
                SetFlag(FlagEnum.Zero, value);
            }
        }



        public bool SubstractFlag
        {
            get
            {
                return GetFlag(FlagEnum.Sub);
            }
            private set
            {
                SetFlag(FlagEnum.Sub, value);
            }
        }


        public bool HalfCarryFlag
        {
            get
            {
                return GetFlag(FlagEnum.HalfCarry);
            }
            private set
            {
                SetFlag(FlagEnum.HalfCarry, value);
            }
        }

        public bool CarryFlag
        {
            get
            {
                return GetFlag(FlagEnum.Carry);
            }
            private set
            {
                SetFlag(FlagEnum.Carry, value);
            }
        }

        #endregion

        private Action[] _opCodes = new Action[0xFF];
        private Action[] _opCodesExt = new Action[0xFF];

        public uint InstructionsCount => _intructionsCounter;


        public uint LastOpTime
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

        public CPU(GPU gpu)
        {
            _gpu = gpu;
            InitializeOpCodes();
        }

        public void Exec(int opCode)
        {
            // Reset the last OpTime, this should be updated by each op code
            _lastOpTime = 0;
            _lastOpCode = opCode & 0xFF;
            // Direct array access should be quicker than Dictionary or predicates
            // TODO : Bench for Action overhead

            byte extOpCode = 0;

            try
            {
                _intructionsCounter++;
                if (opCode == 0xCB) // Extended op codes
                {
                    extOpCode = FetchNextOpCode();
                    _lastOpCode |= extOpCode << 8;
                    _opCodesExt[extOpCode]();
                }
                else
                    _opCodes[opCode]();

                _gpu.Step(_lastOpTime);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"CPU thrown an exception with state : \n" +
                                                $"OP Code: 0x{opCode:X} - Ext : {extOpCode:X} \n" +
                                                $"PC: 0x{PC-1:X} \n A: {A} \n B: {B} \n C: {C}\n D: {D}\n E: {E}\n"+
                                                $"Instructions run : {_intructionsCounter}", e);
            }
        }

        public void ExecExtOpCode(int extOpCode)
        {
            // Reset the last OpTime, this should be updated by each op code
            _lastOpTime = 0;

            _lastOpCode = 0xCB;

            try
            {
                _intructionsCounter++;

                _lastOpCode |= extOpCode << 8;
                _opCodesExt[extOpCode]();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"CPU thrown an exception with state : \n" +
                                                $"OP Code: {_lastOpCode:X} - Ext : {extOpCode:X} \n" +
                                                $"PC: {PC} \n A: {A} \n B: {B} \n C: {C}\n D: {D}\n E: {E}\n" +
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
            _mmu.SetGPU(_gpu);
        }

        public void SetRegister<T>(Expression<Func<ICpu, T>> inExpr, T value) where T : struct
        {
            var expr = (MemberExpression)inExpr.Body;
            var prop = (PropertyInfo)expr.Member;

            prop = typeof(CPU).GetProperty(prop.Name);

            prop.SetValue(this, value);
        }

        public override string ToString()
        {
            return $"PC: {PC} \n A: {A:X} \n B: {B:X} \n C: {C:X}\n D: {D:X}\n E: {E:X}\n F: {F:X}\n H: {H:X}\n L: {L:X}\n" +
                   $"Instructions run : {_intructionsCounter}";

        }

        #region Private
        private void SetFlag(FlagEnum flag, bool value)
        {
            if (value)
            {
                _f = (byte)(_f | (byte)flag);
            }
            else
            {
                //left-shift 1, then take complement, then bitwise AND
                _f = (byte)(_f & ~(byte)flag);
            }
        }

        private bool GetFlag(FlagEnum flag)
        {
            return (F & (byte)flag) > 0;
        }
        #endregion
    }
}
