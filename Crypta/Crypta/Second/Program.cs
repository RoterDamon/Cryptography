using System;
using System.Collections.Generic;

namespace Second
{
	class Program
	{
		public static void Main()
		{
			var sub = new Dictionary<byte, byte>()
			{
				{ 2, 3},
				{ 3, 2},
				{ 1, 2}
			};
			Console.WriteLine(Substitute(45, sub, 2));
		}
		// 45 = 101101 -->  111010 = 58
		public static ulong Substitute(ulong n, Dictionary<byte, byte> sub, int count)
		{
			if ( (uint)(Math.Log(n, 2.0) + 1) % sub.Count != 0 || count % sub.Count != 0) 
				throw new ArgumentException();
			ulong result = 0;
			int k = 0;
            for (int i = 0; i < sub.Count; i++)
            {
				ulong subElement = sub[(byte)(n & (ulong)((1 << count) - 1))];
                result = (subElement << k) | result;
				n = n >> count;
				k += count;
            }
            return result;
		}
	}
}