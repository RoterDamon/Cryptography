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
        ThirdInterface2 chipher;
        byte[][] roundKeys;
        public FourthFeistel(ThirdInterface1 key, ThirdInterface2 encrypt)
        {
            keyGenerator = key;
            chipher = encrypt;
        }
        
        public byte[] EncryptBlock(byte[] message)
        {
            ulong res = BitConverter.ToUInt64(message, 0);
            uint left = (uint)(res >> 32 & ((1 << 32) - 1));
            uint right = (uint)(res & ((1 << 32) - 1));
            uint temp = 0;
            for (int round = 0; round < 16; round++)
            {
                temp = right ^ BitConverter.ToUInt32(chipher.EncryptFunc(BitConverter.GetBytes(left), roundKeys[round]));
                right = left;
                left = temp;
            }
            res = (ulong)left << 32 | right;
            byte[] result = BitConverter.GetBytes(res);
            return result;
        }

        public byte[] DecryptBlock(byte[] message)
        {
            ulong res = BitConverter.ToUInt64(message, 0);
            uint left = (uint)(res >> 32 & ((1 << 32) - 1));
            uint right = (uint)(res & ((1 << 32) - 1));
            uint temp = 0;
            for (int round = 15; round >= 0; round--)
            {
                temp = left ^ BitConverter.ToUInt32(chipher.EncryptFunc(BitConverter.GetBytes(right), roundKeys[round]));
                left = right;
                right = temp;
            }
            res = (ulong)left << 32 | right;
            byte[] result = BitConverter.GetBytes(res);
            return result;
        }

        public void SetKey(byte[] key)
        {
            roundKeys = keyGenerator.GenerateRoundKeys(key);
        }
    }
}
