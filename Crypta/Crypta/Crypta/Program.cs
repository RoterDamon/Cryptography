using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    class Program
    {
        static void Main(string[] args)
        {
            FifthDES DES = new FifthDES(new Encryption(), new GenerationRoundKeys());
            ThirdClass4 encryptor = new ThirdClass4( ThirdClass4.EncryptionMode.ECB, new byte[8]);
            encryptor.algorithm = DES;
            ulong Key = 11422891502611697239;
            string b = "1234567812345678";
            DES.SetKey(BitConverter.GetBytes(Key));
            byte[] text = new byte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            
            byte[] encryptedText = encryptor.Encrypt(Encoding.Default.GetBytes(b));
            string decryptedText = Encoding.Default.GetString(encryptor.Decrypt(encryptedText));
            

        }
    }
}
