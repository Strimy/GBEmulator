using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core
{
    public partial class CPU
    {
        private void InitializeOpCodes()
        {
            // Used mainly for debugging
            // Fill the opcodes with a not implemented exception so we can test which op code doesn't exist
            for (int i = 0; i < _opCodes.Length; i++)
            {
                var opCode = i;
                _opCodes[i] = () => { throw new NotImplementedException("OPCode not implemented " + opCode); };
            }
            for (int i = 0; i < _opCodesExt.Length; i++)
            {
                var opCode = i;
                _opCodesExt[i] = () => { throw new NotImplementedException("OPCode Ext not implemented CB " + opCode); };
            }

            _opCodes[0] = () => { _lastOpTime = 4; };
            InitIncrement();
            InitDecrement();
            InitLoad8Bits();
            InitLoadRegister8Bits();
            InitLoadAddress();
            InitLoadIntoMemory();
            InitLoadImmediateAddress();
            InitLoadC();
            InitLoadHL();
            InitLoadFFA();
            InitLoad16bits();
            InitXor();
            InitJumps();
            InitExtentedOpCodes();
            InitMisc();
            InitPush();
            InitPop();
            InitReturn();
            InitRL();
            InitCP();
            InitSub();
            InitAdd();

            _opCodes[0xCD] = Call;
        }


        private void InitRL()
        {
            _opCodesExt[0x17] = () => { RL(ref _a); };
            _opCodesExt[0x10] = () => { RL(ref _b); };
            _opCodesExt[0x11] = () => { RL(ref _c); };
            _opCodesExt[0x12] = () => { RL(ref _d); };
            _opCodesExt[0x13] = () => { RL(ref _e); };
            _opCodesExt[0x14] = () => { RL(ref _h); };
            _opCodesExt[0x15] = () => { RL(ref _l); };
            _opCodesExt[0x16] = () => 
            {
                var value = _mmu.ReadByte(HL);
                RL(ref value);
                _mmu.WriteByte(HL, value);
                _lastOpTime = 16;
            };

            _opCodes[0x17] = () => { RL(ref _a); _lastOpTime = 4; };

        }

        private void InitReturn()
        {
            _opCodes[0xC9] = Return;
        }

        private void InitPop()
        {
            _opCodes[0xF1] = () => { Pop(ref _a, ref _f); };
            _opCodes[0xC1] = () => { Pop(ref _b, ref _c); };
            _opCodes[0xD1] = () => { Pop(ref _d, ref _e); };
            _opCodes[0xE1] = () => { Pop(ref _l, ref _l); };
        }

        private void InitPush()
        {
            _opCodes[0xF5] = () => { Push(A, F); };
            _opCodes[0xC5] = () => { Push(B, C); };
            _opCodes[0xD5] = () => { Push(D, E); };
            _opCodes[0xE5] = () => { Push(H, L); };
        }

        private void InitIncrement()
        {
            _opCodes[0x3C] = () => Increment(ref _a);
            _opCodes[0x04] = () => Increment(ref _b);
            _opCodes[0x0C] = () => Increment(ref _c);
            _opCodes[0x14] = () => Increment(ref _d);
            _opCodes[0x1C] = () => Increment(ref _e);
            _opCodes[0x24] = () => Increment(ref _h);
            _opCodes[0x2C] = () => Increment(ref _l);

            _opCodes[0x03] = () => Increment(ref _b, ref _c);
            _opCodes[0x13] = () => Increment(ref _d, ref _e);
            _opCodes[0x23] = () => Increment(ref _h, ref _l);
            _opCodes[0x33] = () => Increment(ref _sp);

        }

        private void InitDecrement()
        {
            _opCodes[0x3D] = () => Decrement(ref _a);
            _opCodes[0x05] = () => Decrement(ref _b);
            _opCodes[0x0D] = () => Decrement(ref _c);
            _opCodes[0x15] = () => Decrement(ref _d);
            _opCodes[0x1D] = () => Decrement(ref _e);
            _opCodes[0x25] = () => Decrement(ref _h);
            _opCodes[0x2D] = () => Decrement(ref _l);

            _opCodes[0x2B] = () => Decrement(ref _h, ref _l);
            _opCodes[0x0B] = () => Decrement(ref _b, ref _c);
            _opCodes[0x1B] = () => Decrement(ref _d, ref _e);
            _opCodes[0x3B] = () => Decrement(ref _sp);

        }

        private void InitAdd()
        {
            _opCodes[0x87] = () => Add(_a);
            _opCodes[0x80] = () => Add(_b);
            _opCodes[0x81] = () => Add(_c);
            _opCodes[0x82] = () => Add(_d);
            _opCodes[0x83] = () => Add(_e);
            _opCodes[0x84] = () => Add(_h);
            _opCodes[0x85] = () => Add(_l);

            _opCodes[0x86] = AddFromHL;
            _opCodes[0xC6] = AddFromImmediate;
        }

        private void InitSub()
        {
            _opCodes[0x97] = () => Sub(_a);
            _opCodes[0x90] = () => Sub(_b);
            _opCodes[0x91] = () => Sub(_c);
            _opCodes[0x92] = () => Sub(_d);
            _opCodes[0x93] = () => Sub(_e);
            _opCodes[0x94] = () => Sub(_h);
            _opCodes[0x95] = () => Sub(_l);

            _opCodes[0x96] = () => SubFromAddress(HL);
            _opCodes[0xD6] = SubFromImmediate;
        }

        private void InitMisc()
        {
            _opCodes[0xFB] = () => { _interruptsEnabled = true; };
        }

        private void InitJumps()
        {
            _opCodes[0x18] = JumpImmediateRelative;
            _opCodes[0x20] = JmpNZ;
            _opCodes[0x28] = JmpZ;
            _opCodes[0x30] = JmpNC;
            _opCodes[0x38] = JmpC;
            _opCodes[0xC3] = JumpImmediate;
        }

        private void InitExtentedOpCodes()
        {
            InitTestBit();
        }

        private void InitTestBit()
        {
            // Bit 1
            _opCodesExt[0x47] = () => TestBit(0, A);
            _opCodesExt[0x40] = () => TestBit(0, B);
            _opCodesExt[0x41] = () => TestBit(0, C);
            _opCodesExt[0x42] = () => TestBit(0, D);
            _opCodesExt[0x43] = () => TestBit(0, E);
            _opCodesExt[0x44] = () => TestBit(0, H);
            _opCodesExt[0x45] = () => TestBit(0, L);

            // Bit 2
            _opCodesExt[0x4F] = () => TestBit(1, A);
            _opCodesExt[0x48] = () => TestBit(1, B);
            _opCodesExt[0x49] = () => TestBit(1, C);
            _opCodesExt[0x4A] = () => TestBit(1, D);
            _opCodesExt[0x4B] = () => TestBit(1, E);
            _opCodesExt[0x4C] = () => TestBit(1, H);
            _opCodesExt[0x4D] = () => TestBit(1, L);


            _opCodesExt[0x57] = () => TestBit(2, A);
            _opCodesExt[0x50] = () => TestBit(2, B);
            _opCodesExt[0x51] = () => TestBit(2, C);
            _opCodesExt[0x52] = () => TestBit(2, D);
            _opCodesExt[0x53] = () => TestBit(2, E);
            _opCodesExt[0x54] = () => TestBit(2, H);
            _opCodesExt[0x55] = () => TestBit(2, L);

            _opCodesExt[0x5F] = () => TestBit(3, A);
            _opCodesExt[0x58] = () => TestBit(3, B);
            _opCodesExt[0x59] = () => TestBit(3, C);
            _opCodesExt[0x5A] = () => TestBit(3, D);
            _opCodesExt[0x5B] = () => TestBit(3, E);
            _opCodesExt[0x5C] = () => TestBit(3, H);
            _opCodesExt[0x5D] = () => TestBit(3, L);

            _opCodesExt[0x67] = () => TestBit(4, A);
            _opCodesExt[0x60] = () => TestBit(4, B);
            _opCodesExt[0x61] = () => TestBit(4, C);
            _opCodesExt[0x62] = () => TestBit(4, D);
            _opCodesExt[0x63] = () => TestBit(4, E);
            _opCodesExt[0x64] = () => TestBit(4, H);
            _opCodesExt[0x65] = () => TestBit(4, L);

            _opCodesExt[0x6F] = () => TestBit(5, A);
            _opCodesExt[0x68] = () => TestBit(5, B);
            _opCodesExt[0x69] = () => TestBit(5, C);
            _opCodesExt[0x6A] = () => TestBit(5, D);
            _opCodesExt[0x6B] = () => TestBit(5, E);
            _opCodesExt[0x6C] = () => TestBit(5, H);
            _opCodesExt[0x6D] = () => TestBit(5, L);

            _opCodesExt[0x77] = () => TestBit(6, A);
            _opCodesExt[0x70] = () => TestBit(6, B);
            _opCodesExt[0x71] = () => TestBit(6, C);
            _opCodesExt[0x72] = () => TestBit(6, D);
            _opCodesExt[0x73] = () => TestBit(6, E);
            _opCodesExt[0x74] = () => TestBit(6, H);
            _opCodesExt[0x75] = () => TestBit(6, L);

            _opCodesExt[0x7F] = () => TestBit(7, A);
            _opCodesExt[0x78] = () => TestBit(7, B);
            _opCodesExt[0x79] = () => TestBit(7, C);
            _opCodesExt[0x7A] = () => TestBit(7, D);
            _opCodesExt[0x7B] = () => TestBit(7, E);
            _opCodesExt[0x7C] = () => TestBit(7, H);
            _opCodesExt[0x7D] = () => TestBit(7, L);
        }

        /// <summary>
        /// Prepare the basic 8 bits loads to a specific register
        /// </summary>
        private void InitLoad8Bits()
        {
            _opCodes[0x3E] = () => LoadImmediate(ref _a);
            _opCodes[0x06] = () => LoadImmediate(ref _b);
            _opCodes[0x0E] = () => LoadImmediate(ref _c);
            _opCodes[0x16] = () => LoadImmediate(ref _d);
            _opCodes[0x1E] = () => LoadImmediate(ref _e);
            _opCodes[0x26] = () => LoadImmediate(ref _h);
            _opCodes[0x2E] = () => LoadImmediate(ref _l);
        }

        private void InitLoadRegister8Bits()
        {
            _opCodes[0x7F] = () => LoadRegister(ref _a, _a);
            _opCodes[0x78] = () => LoadRegister(ref _a, _b);
            _opCodes[0x79] = () => LoadRegister(ref _a, _c);
            _opCodes[0x7a] = () => LoadRegister(ref _a, _d);
            _opCodes[0x7b] = () => LoadRegister(ref _a, _e);
            _opCodes[0x7c] = () => LoadRegister(ref _a, _h);
            _opCodes[0x7d] = () => LoadRegister(ref _a, _l);

            _opCodes[0x47] = () => LoadRegister(ref _b, _a);
            _opCodes[0x40] = () => LoadRegister(ref _b, _b);
            _opCodes[0x41] = () => LoadRegister(ref _b, _c);
            _opCodes[0x42] = () => LoadRegister(ref _b, _d);
            _opCodes[0x43] = () => LoadRegister(ref _b, _e);
            _opCodes[0x44] = () => LoadRegister(ref _b, _h);
            _opCodes[0x45] = () => LoadRegister(ref _b, _l);

            _opCodes[0x4F] = () => LoadRegister(ref _c, _a);
            _opCodes[0x48] = () => LoadRegister(ref _c, _b);
            _opCodes[0x49] = () => LoadRegister(ref _c, _c);
            _opCodes[0x4A] = () => LoadRegister(ref _c, _d);
            _opCodes[0x4b] = () => LoadRegister(ref _c, _e);
            _opCodes[0x4c] = () => LoadRegister(ref _c, _h);
            _opCodes[0x4d] = () => LoadRegister(ref _c, _l);

            _opCodes[0x57] = () => LoadRegister(ref _d, _a);
            _opCodes[0x50] = () => LoadRegister(ref _d, _b);
            _opCodes[0x51] = () => LoadRegister(ref _d, _c);
            _opCodes[0x52] = () => LoadRegister(ref _d, _d);
            _opCodes[0x53] = () => LoadRegister(ref _d, _e);
            _opCodes[0x54] = () => LoadRegister(ref _d, _h);
            _opCodes[0x55] = () => LoadRegister(ref _d, _l);


            _opCodes[0x5F] = () => LoadRegister(ref _e, _a);
            _opCodes[0x58] = () => LoadRegister(ref _e, _b);
            _opCodes[0x59] = () => LoadRegister(ref _e, _c);
            _opCodes[0x5A] = () => LoadRegister(ref _e, _d);
            _opCodes[0x5B] = () => LoadRegister(ref _e, _e);
            _opCodes[0x5C] = () => LoadRegister(ref _e, _h);
            _opCodes[0x5D] = () => LoadRegister(ref _e, _l);


            _opCodes[0x67] = () => LoadRegister(ref _h, _a);
            _opCodes[0x60] = () => LoadRegister(ref _h, _b);
            _opCodes[0x61] = () => LoadRegister(ref _h, _c);
            _opCodes[0x62] = () => LoadRegister(ref _h, _d);
            _opCodes[0x63] = () => LoadRegister(ref _h, _e);
            _opCodes[0x64] = () => LoadRegister(ref _h, _h);
            _opCodes[0x65] = () => LoadRegister(ref _h, _l);

            _opCodes[0x6F] = () => LoadRegister(ref _l, _a);
            _opCodes[0x68] = () => LoadRegister(ref _l, _b);
            _opCodes[0x69] = () => LoadRegister(ref _l, _c);
            _opCodes[0x6A] = () => LoadRegister(ref _l, _d);
            _opCodes[0x6B] = () => LoadRegister(ref _l, _e);
            _opCodes[0x6C] = () => LoadRegister(ref _l, _h);
            _opCodes[0x6D] = () => LoadRegister(ref _l, _l);
        }

        protected void InitLoadAddress()
        {
            _opCodes[0x0A] = () => LoadAddress(ref _a, BC);
            _opCodes[0x1A] = () => LoadAddress(ref _a, DE);

            _opCodes[0x7E] = () => LoadAddress(ref _a, HL);
            _opCodes[0x46] = () => LoadAddress(ref _b, HL);
            _opCodes[0x4E] = () => LoadAddress(ref _c, HL);
            _opCodes[0x56] = () => LoadAddress(ref _d, HL);
            _opCodes[0x5E] = () => LoadAddress(ref _e, HL);
            _opCodes[0x66] = () => LoadAddress(ref _h, HL);
            _opCodes[0x6E] = () => LoadAddress(ref _l, HL);
        }

        protected void InitLoadImmediateAddress()
        {
            _opCodes[0xFA] = () => LoadImmediateAddress(ref _a);
        }

        protected void InitLoadIntoMemory()
        {
            _opCodes[0x70] = () => LoadIntoMemory(HL, B);
            _opCodes[0x71] = () => LoadIntoMemory(HL, C);
            _opCodes[0x72] = () => LoadIntoMemory(HL, D);
            _opCodes[0x73] = () => LoadIntoMemory(HL, E);
            _opCodes[0x74] = () => LoadIntoMemory(HL, H);
            _opCodes[0x75] = () => LoadIntoMemory(HL, L);
            _opCodes[0x02] = () => LoadIntoMemory(BC, A);
            _opCodes[0x12] = () => LoadIntoMemory(DE, A);
            _opCodes[0x77] = () => LoadIntoMemory(HL, A);
            _opCodes[0x36] = () => { LoadIntoMemory(HL, _mmu.ReadByte(PC++)); _lastOpTime += 4; };
            _opCodes[0xEA] = () => { LoadIntoMemory(_mmu.ReadWord(PC), A); PC += 2; _lastOpTime += 8; };
        }

        protected void InitLoadC()
        {
            _opCodes[0xF2] = () => LoadAFromC();
            _opCodes[0xE2] = () => LoadCFromA();
        }

        protected void InitLoadHL()
        {
            _opCodes[0x3A] = () => { LoadAddress(ref _a, HL--); _lastOpTime = 8; };
            _opCodes[0x32] = () => { LoadIntoMemory(HL--, A);  };
            _opCodes[0x2A] = () => { LoadAddress(ref _a, HL++); _lastOpTime = 8; };
            _opCodes[0x22] = () => { LoadIntoMemory(HL++, A);  };
        }

        protected void InitLoadFFA()
        {
            _opCodes[0xE0] = () => { LoadIntoMemory(0xFF00 | _mmu.ReadByte(PC++), _a); _lastOpTime = 12; };
            _opCodes[0xF0] = () => { LoadAddress(ref _a, 0xFF00 | _mmu.ReadByte(PC++)); _lastOpTime = 12; };
        }

        protected void InitLoad16bits()
        {
            _opCodes[0x01] = () => LoadImmediate16Bits(ref _b, ref _c);
            _opCodes[0x11] = () => LoadImmediate16Bits(ref _d, ref _e);
            _opCodes[0x21] = () => LoadImmediate16Bits(ref _h, ref _l);
            _opCodes[0x31] = () => LoadImmediate16Bits(ref _sp);

        }

        protected void InitXor()
        {
            _opCodes[0xAF] = () => Xor(ref _a);
            _opCodes[0xA8] = () => Xor(ref _b);
            _opCodes[0xA9] = () => Xor(ref _c);
            _opCodes[0xAA] = () => Xor(ref _d);
            _opCodes[0xAB] = () => Xor(ref _e);
            _opCodes[0xAC] = () => Xor(ref _h);
            _opCodes[0xAD] = () => Xor(ref _l);
            _opCodes[0xAE] = () => Xor(HL);
        }

        protected void InitCP()
        {
            _opCodes[0xBF] = () => { CP(_a); };
            _opCodes[0xB8] = () => { CP(_b); };
            _opCodes[0xB9] = () => { CP(_c); };
            _opCodes[0xBA] = () => { CP(_d); };
            _opCodes[0xBB] = () => { CP(_e); };
            _opCodes[0xBC] = () => { CP(_h); };
            _opCodes[0xBD] = () => { CP(_l); };
            _opCodes[0xBE] = () => { CP(HL); };
            _opCodes[0xFE] = () => { CPImmediate(); };
        }
    }
}
