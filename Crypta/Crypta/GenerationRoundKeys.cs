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
			50, 43, 36, 29, 22, 15,  8,  1, 51, 44, 37, 30, 23, 16,
			 9,  2, 52, 45, 38, 31, 24, 17, 10,  3, 53, 46, 39, 32,
			56, 49, 42, 35, 28, 21, 14,  7, 55, 48, 41, 34, 27, 20,
			13,  6, 54, 47, 40, 33, 26, 19, 12,  5, 25, 18, 11,  4
		};
		/* Выбор замены PC-2 */
		/* 56-битный ключ, сжатый в 48-битный подключ */
		static byte[] PC_2 =
		{
			14, 17, 11, 24,  1,  5,  3, 28, 15,  6, 21, 10,
			23, 19, 12,  4, 26,  8, 16,  7, 27, 20, 13,  2,
			41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
			44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
		};

		/* Циклический сдвиг влево */
		static byte[] shiftBits = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
		#endregion
		public byte[][] GenerateRoundKeys(byte[] key)
        {
			byte[][] roundKeys = new byte[16][];
			byte[] PermutedKey = Permute(PC_1, key);
			ulong res = BitConverter.ToUInt64(PermutedKey, 0);
			ulong C = (ulong)(res >> 28);
			ulong D = (ulong)(res & ((1 << 28) - 1));
			for (int round = 0, Shift; round < 16; round++)
			{
				Shift = shiftBits[round];
				C = ((C << Shift) | (C >> (28 - Shift))) & ((1 << 28) - 1);
				D = ((D << Shift) | (D >> (28 - Shift))) & ((1 << 28) - 1);

				roundKeys[round] = Permute(PC_2, BitConverter.GetBytes((C << 28) | D));
			}
			return roundKeys;
		}

		private static byte[] Permute(byte[] permRule, byte[] block)
		{
			ulong res = 0;
			ulong n = BitConverter.ToUInt64(block, 0);
			for (int i = 0; i < permRule.Length; i++)
			{
				res |= ((n >> (permRule[i] - 1) & 1) << i);
			}
			return BitConverter.GetBytes(res);
		}
	}
}
