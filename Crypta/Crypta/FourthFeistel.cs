using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    class FourthFeistel: ThirdInterface3
    {
        ThirdInterface1 keyGenerator;
        ThirdInterface2 transformation;
        byte[][] roundKeys;
        public FourthFeistel(ThirdInterface1 key, ThirdInterface2 encrypt)
        {
            keyGenerator = key;
            transformation = encrypt;
        }
        
        public virtual byte[] EncryptBlock(byte[] message)
        {
            ulong res = BitConverter.ToUInt64(message, 0);
            uint left = (uint)(res >> 32 );
            uint right = (uint)(res & ((ulong)1 << 32) - 1);
            uint newLeft = 0, newRight = 0;
            for (int round = 0; round < 16; round++)
            {
                newLeft = right;
                newRight = left ^ BitConverter.ToUInt32(transformation.EncryptFunc(BitConverter.GetBytes(right), roundKeys[round]));
                left = newLeft;
                right = newRight;
            }
            res = (ulong)newLeft;
            res = res << 32 | newRight;
            byte[] result = BitConverter.GetBytes(res);
            return result;
        }

        public virtual byte[] DecryptBlock(byte[] message)
        {
            ulong res = BitConverter.ToUInt64(message, 0);
            uint left = (uint)(res >> 32);
            uint right = (uint)(res & ((ulong)1 << 32) - 1);
            uint newLeft = 0, newRight = 0;
            for (int round = 15; round >= 0; round--)
            {
                newRight = left;
                newLeft = right ^ BitConverter.ToUInt32(transformation.EncryptFunc(BitConverter.GetBytes(left), roundKeys[round]));
                left = newLeft;
                right = newRight;
            }
            res = (ulong)newLeft;
            res = res << 32 | newRight;
            byte[] result = BitConverter.GetBytes(res);
            return result;
        }

        public void SetKey(byte[] key)
        {
            roundKeys = keyGenerator.GenerateRoundKeys(key);
        }
    }
}
