using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    class GF256
    {
        private byte Polynom;
        private readonly uint basePolynome = 0b100011011; // For GF(2^8)



        public GF256(byte Polynom)
        {
            this.Polynom = Polynom;
        }

        public GF256(byte Polynom, uint basePolynome)
        {
            this.Polynom = Polynom;
            this.basePolynome = basePolynome;
        }

        public static bool CheckIrr(uint P)
        {
            for (uint i = 2; i < P; i++)
            {
                for (uint j = 2; j < P; j++)
                {

                    uint l = 0b10000000000;
                    uint r = 0b10000000000;

                    uint midResult = 0;
                    while (l != 0)
                    {
                        while (r != 0)
                        {
                            midResult ^= XMultiply((uint)(i & l), (uint)(j & r));

                            r >>= 1;
                        }
                        l >>= 1;
                        r = 0b10000000;
                    }

                    if (midResult == P) return false;
                }

            }
            return true;
        }

        public GF256 Inverse()
        {
            uint degree = 254;
            GF256 result = new GF256(0b1);
            uint bitesForMask = degree;
            GF256 b = this;
            while (bitesForMask > 0)
            {
                if ((bitesForMask & 0b01) == 1)
                {
                    result = new GF256(Module(result.Multiply(b).Polynom));
                }
                bitesForMask >>= 1;

                b = b.Multiply(b);
            }
            return result;
        }
        public GF256 Sum(GF256 right)
        {
            return new GF256((byte)(Polynom ^ right.Polynom));

        }
        public GF256 Multiply(GF256 right)
        {
            GF256 result = new GF256(0);
            if (right.Polynom == 0) return result;
            byte l = 0b10000000;
            byte r = 0b10000000;

            uint midResult = 0;
            while (l != 0)
            {
                while (r != 0)
                {
                    midResult ^= XMultiply((uint)(Polynom & l), (uint)(right.Polynom & r));

                    r >>= 1;
                }
                l >>= 1;
                r = 0b10000000;
            }


            midResult = Module(midResult);

            return new GF256((byte)midResult);
        }

        public GF256 Multiply(byte right)
        {
            return Multiply(new GF256(right));
        }

        public byte GetPolynom()
        {
            return Polynom;
        }

        #region MathHelpers
        private static uint XMultiply(uint left, uint right) // x^4 * x^2 = x^6
        {
            uint result = left;
            if (right == 0) return 0;
            while (right != 1)
            {
                result <<= 1;
                right >>= 1;
            }
            return result;
        }

        private uint GetHigherBitMask(uint n)
        {
            if (n == 0) return 0;
            uint count = 1;
            while (n != 1)
            {
                n >>= 1;
                count <<= 1;
            }

            return count;
        }

        private byte Module(uint num)
        {
            uint lDeg = GetHigherBitMask(num);
            uint baseDeg = GetHigherBitMask(basePolynome);

            while (lDeg >= baseDeg)
            {
                uint tmpLeft = lDeg;
                uint tmpRight = baseDeg;
                while (tmpRight != 1)
                {
                    tmpLeft >>= 1;
                    tmpRight >>= 1;
                }

                uint tmpMulResult = XMultiply(basePolynome, tmpLeft);
                num ^= tmpMulResult;
                lDeg = GetHigherBitMask(num);
            }

            return (byte)num;
        }


        #endregion



        public override string ToString()
        {
            return Polynom.ToString();
        }
        public static void Main(string[] args)
        {
            GF256 num = new GF256(0b100011);
            GF256 inv = num.Inverse();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GF256);
        }

        public bool Equals(GF256 obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (this.GetType() != obj.GetType()) return false;
            return Polynom == obj.Polynom;

        }
    }
}
