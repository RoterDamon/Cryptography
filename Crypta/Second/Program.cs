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
            FermatTest test = new FermatTest();
            SolovayStrassenTest test2 = new SolovayStrassenTest();
            MillerRabinTest test3 = new MillerRabinTest();

            bool fermat = test.MakeSimplicityTest(1, 0.7);
            bool solovay = test2.MakeSimplicityTest(1, 0.7);
            bool miller = test3.MakeSimplicityTest(1, 0.7);

            BigInteger resL = Functions.Legendre(126, 53);
            BigInteger resJ = Functions.Jacobi(7, 143);

            Console.WriteLine("символ Лежандра: ", resL);
            Console.WriteLine("символ Якоби: ", resJ);

            Attack attack1 = new();
            BigInteger e = 6792605526025;
            BigInteger n = 9449868410449;

            RSA rsa = new RSA(RSA.TestMode.MillerRabin, 0.7, 30);
            BigInteger encryptInt = rsa.Encrypt(123123123);
            Console.WriteLine("Зашифрованное ", encryptInt);
            Console.WriteLine("Расшифрованное ", rsa.Decrypt(encryptInt));

            Tuple<BigInteger, List<Tuple<BigInteger, BigInteger>>> attack = attack1.WienerAttack(e, n);
        }
        
    }
}
