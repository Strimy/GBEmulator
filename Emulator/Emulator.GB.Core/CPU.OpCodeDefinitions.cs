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
            for (int i = 0; i < _opCodes.Length; i++)
            {
                _opCodes[i] = () => { throw new NotImplementedException(); };
            }

            InitLoad8Bits();
            InitLoadRegister8Bits();
            InitLoadAddress();
        }

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
            _opCodes[0xFA] = () => LoadAddress(ref _a, BC);

            _opCodes[0x46] = () => LoadAddress(ref _b, HL);
            _opCodes[0x4E] = () => LoadAddress(ref _c, HL);
            _opCodes[0x56] = () => LoadAddress(ref _d, HL);
            _opCodes[0x5E] = () => LoadAddress(ref _e, HL);
            _opCodes[0x66] = () => LoadAddress(ref _h, HL);
            _opCodes[0x6E] = () => LoadAddress(ref _l, HL);


        }

		protected void InitLoadImmediateAddress()
        {

        }
    }
}
