using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Second
{
    struct Keys
    {
        public BigInteger n;    //p*q
        public BigInteger e;    //public key
        public BigInteger d;   //private key
        
    }
    class RSA
    {
        public enum TestMode { Fermat, MillerRabin, SolovayStrassen };
        Keys keys;
        public RSA(TestMode mode, double minProbability, ulong size)
        {
            KeysGenerator keysGenerator = new KeysGenerator(mode, minProbability, size);
            keys = keysGenerator.GenerateKeys();
        }
        public BigInteger Encrypt(BigInteger message)
        {
            return Functions.QuickPow(message, keys.e, keys.n);
        }
        public BigInteger Decrypt(BigInteger message)
        {
            return Functions.QuickPow(message, keys.d, keys.n);
        }
        class KeysGenerator
        { 
            public TestMode testMode;
            double probability;
            UInt64 numSize;
            public KeysGenerator(TestMode mode, double minProbability, ulong size)
            {
                testMode = mode;
                probability = minProbability;
                numSize = size;
            }

            public Keys GenerateKeys()
            {
                Keys keys;
                BigInteger p = GetPrimeNumber();
                BigInteger q = GetPrimeNumber();
                keys.n = BigInteger.Multiply(p, q);
                BigInteger euler = BigInteger.Multiply(p - 1, q - 1);

                var random = new Random();
                byte[] buffer = new byte[numSize];
                while (true)
                {
                    while (true)
                    {
                        random.NextBytes(buffer);
                        BigInteger e = new BigInteger(buffer);
                        if (e > 3 && e < euler && Functions.EuclideanAlgorithm(e, euler) == 1)
                        {
                            keys.e = e;
                            break;
                        }
                    }
                    BigInteger x;
                    BigInteger y;
                    BigInteger g = Functions.ExtendedEuclideanAlgorithm(keys.e, euler, out x, out y);
                    if (g != 1) throw new ArgumentException();
                    while (x < 0)
                    {
                        x += euler;
                    }
                    if (x > (BigInteger)(0.3333 * Math.Pow((double)keys.n, 0.25)))//винер проверка
                    {
                        keys.d = x;
                        break;
                    }
                }
                
                return keys;
            }

            public BigInteger GetPrimeNumber()
            {
                BigInteger newBigInt;
                var random = new Random();
                byte[] buffer = new byte[numSize];
                while (true)
                {
                    do
                    {
                        random.NextBytes(buffer);
                        newBigInt = new BigInteger(buffer);
                    } while (newBigInt < 2);
                    
                    switch (testMode)
                    {
                        case TestMode.Fermat:
                            {
                                FermatTest test = new FermatTest();
                                if (test.MakeSimplicityTest(newBigInt, probability)) return newBigInt;
                                break;
                            }
                        case TestMode.MillerRabin:
                            {
                                MillerRabinTest test = new MillerRabinTest();
                                if (test.MakeSimplicityTest(newBigInt, probability)) return newBigInt;
                                break;
                            }
                        case TestMode.SolovayStrassen:
                            {
                                SolovayStrassenTest test = new SolovayStrassenTest();
                                if (test.MakeSimplicityTest(newBigInt, probability)) return newBigInt;
                                break;
                            }
                    }
                }
            }
            
        }
    }

}
