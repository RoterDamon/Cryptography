using System;
using System.Linq;

namespace First
{
    public class Program
    {
        static void Main(string[] args)
        {
            byte[] permRule =
            {
                3, 2, 1
            };
            Console.WriteLine(Permute(4, permRule));
        }

        public static ulong Permute(ulong n, byte[] permRule)
        {
            ulong res = 0;
            uint count = (uint)Math.Log(n, 2.0) + 1; // number of bits in n
            if (count != permRule.Length || permRule.Max() > count)
                throw new ArgumentException("*!duren'!*");
            for (int i = 0; i < permRule.Length; i++)
            {
                res |= ((n >> (permRule[i] - 1)) & 1) << (permRule.Length - 1 - i);
            }
            return res;
        }
    }
}
