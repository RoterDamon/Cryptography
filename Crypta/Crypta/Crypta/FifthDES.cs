using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    class FifthDES: FourthFeistel
    {
        public FifthDES(EncryptionConversion name1, GenerationRoundKeys name2) : base(name2, name1)
        {

        }
    }
}
