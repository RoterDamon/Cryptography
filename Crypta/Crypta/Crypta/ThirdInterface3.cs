using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    interface ThirdInterface3
    {
        byte[] EncryptBlock(byte[] message);

        byte[] DecryptBlock(byte[] message);

        void SetKey(byte[] key);

    }
}
