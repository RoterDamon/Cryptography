using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace Second
{
    class Program
    {
        static void Main(string[] args)
        {
            /*FermatTest test = new FermatTest();
            SolovayStrassenTest test2 = new SolovayStrassenTest();
            MillerRabinTest test3 = new MillerRabinTest();

            bool fermat = test.MakeSimplicityTest(211, 1);
            bool solovay = test2.MakeSimplicityTest(211, 1);
            bool miller = test3.MakeSimplicityTest(211, 1);

            BigInteger resL = Functions.Legendre(126, 53);
            BigInteger resJ = Functions.Jacobi(7, 143);

            Console.WriteLine("символ Лежандра: ", resL);
            Console.WriteLine("символ Якоби: ", resJ);*/
            
            Attack attack1 = new();
            BigInteger a = 16843009;
            BigInteger e = 6792605526025;
            BigInteger n = 9449868410449;

            RSA rsa = new RSA(RSA.TestMode.MillerRabin, 0.7, 30);
            BigInteger encryptInt = rsa.Encrypt(123123123);
            BigInteger decryptInt = rsa.Decrypt(encryptInt);

            BigInteger enA = Functions.QuickPow(a, e, n);
            List<Tuple<BigInteger, BigInteger>> attack = attack1.WienerAttack(a, e, n);
        }
        
    }
}
