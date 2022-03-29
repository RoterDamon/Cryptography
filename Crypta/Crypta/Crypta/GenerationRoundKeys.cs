using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    class GenerationRoundKeys : ThirdInterface1
	{
        #region Choice of replacement, Shift to the left
        /* 64-битный ключ становится 56-битным */
		static byte[] PC_1 =
		{
			57, 49, 41, 33, 25, 17, 9,
			1, 58, 50, 42, 34, 26, 18,
			10,  2, 59, 51, 43, 35, 27,
			19, 11,  3, 60, 52, 44, 36,
			63, 55, 47, 39, 31, 23, 15,
			7, 62, 54, 46, 38, 30, 22,
			14,  6, 61, 53, 45, 37, 29,
			21, 13,  5, 28, 20, 12,  4
		};
		 /* Выбор замены PC-2 */
		 /* 56-битный ключ, сжатый в 48-битный подключ */
		static byte[] PC_2 =
		{
			14, 17, 11, 24,  1,  5,
			3, 28, 15,  6, 21, 10,
			23, 19, 12,  4, 26,  8,
			16,  7, 27, 20, 13,  2,
			41, 52, 31, 37, 47, 55,
			30, 40, 51, 45, 33, 48,
			44, 49, 39, 56, 34, 53,
			46, 42, 50, 36, 29, 32
		};
 
		 /* Циклический сдвиг влево */
		static byte[] shiftBits = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
		#endregion
		public byte[][] GenerateRoundKeys(byte[] key)
        {
			
			uint temp = 0;

			byte[][] roundKeys = null;
			Encryption permute = new Encryption();
			byte[] PermutedKey = permute.EncryptFunc(PC_1, key);
			ulong res = BitConverter.ToUInt64(PermutedKey, 0);
			uint C = (uint)(res >> 28 & ((1 << 28) - 1));
			uint D = (uint)(res & ((1 << 28) - 1));
			for (int round = 0, Shift; round < 16; round++)
			{
				Shift = shiftBits[round];
				C = (C << Shift) | (C >> (28 - Shift));
				D = (D << Shift) | (D >> (28 - Shift));

				roundKeys[round] = permute.EncryptFunc(PC_2, BitConverter.GetBytes((C << 28) | D));
			}
			return roundKeys;
		}
    }
}
