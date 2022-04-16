using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First
{
    class ThirdClass4
    {
        public enum EncryptionMode { ECB, CBC, CFB, OFB, CTR, RD, RDH };
        public EncryptionMode encryptionMode;
        public byte[] InitializationVector;
        static int BlockSize = 8;
        public ThirdInterface3 algorithm;
        
        public ThirdClass4( EncryptionMode mode, byte[] vector)
        {
            encryptionMode = mode;
            InitializationVector = vector; 
        }
        public byte[] Encrypt(byte[] data)
        {
            byte[] res = MakkePaddingPKCS7(data);
            List<byte[]> blocks = new List<byte[]>();
            switch (encryptionMode)
            {
                case EncryptionMode.ECB: 
                    {
                        byte[] block = new byte[BlockSize];
                        for(int i = 0; i < res.Length / BlockSize; i++)
                        {
                            Array.Copy(res, i * BlockSize, block, 0, BlockSize);
                            blocks.Add(algorithm.EncryptBlock(block));
                        }
                        break;
                    }
                case EncryptionMode.CBC:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        Array.Copy(InitializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < res.Length / BlockSize; i++)
                        {
                            Array.Copy(res, i * BlockSize, curBlock, 0, BlockSize);
                            blocks.Add(algorithm.EncryptBlock(XOR(curBlock, prevBlock)));
                            Array.Copy(blocks[i], prevBlock, BlockSize);
                        }
                        break;
                    }
                case EncryptionMode.CFB://curEncBlock = E(prevEncBlock) ^ block
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        Array.Copy(InitializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < res.Length / BlockSize; i++)
                        {
                            Array.Copy(res, i * BlockSize, curBlock, 0, BlockSize);
                            blocks.Add(XOR(algorithm.EncryptBlock(prevBlock), curBlock));
                            Array.Copy(blocks[i], prevBlock, BlockSize);
                        }
                        break;
                    }
                case EncryptionMode.OFB:
                    {
                        
                        break;
                    }
                case EncryptionMode.CTR:
                    {
                        break;
                    }
                case EncryptionMode.RD:
                    {
                        break;
                    }
                case EncryptionMode.RDH:
                    {
                        break;
                    }
            }
            return MakeArrayFromList(blocks);
        }

        public byte[] Decrypt(byte[] data)
        {
            List<byte[]> blocks = new List<byte[]>();
            switch (encryptionMode)
            {
                case EncryptionMode.ECB:
                    {
                        byte[] block = new byte[BlockSize];
                        for (int i = 0; i < data.Length / BlockSize; i++)
                        {
                            Array.Copy(data, i * BlockSize, block, 0, BlockSize);
                            blocks.Add(algorithm.DecryptBlock(block));
                        }
                        break;
                    }
                case EncryptionMode.CBC:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        Array.Copy(InitializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < data.Length / BlockSize; i++)
                        {
                            Array.Copy(data, i * BlockSize, curBlock, 0, BlockSize);
                            blocks.Add(XOR(prevBlock, algorithm.DecryptBlock(curBlock)));
                            Array.Copy(curBlock, prevBlock, BlockSize);
                        }
                        break;
                    }
                case EncryptionMode.CFB:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        Array.Copy(InitializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < data.Length / BlockSize; i++)
                        {
                            Array.Copy(data, i * BlockSize, curBlock, 0, BlockSize);
                            blocks.Add(XOR(algorithm.EncryptBlock(prevBlock), curBlock));
                            Array.Copy(curBlock, prevBlock, BlockSize);
                        }
                        break;
                    }
                case EncryptionMode.OFB:
                    {
                        
                        break;
                    }
                case EncryptionMode.CTR:
                    {
                        break;
                    }
                case EncryptionMode.RD:
                    {
                        break;
                    }
                case EncryptionMode.RDH:
                    {
                        break;
                    }
            }
            byte[] array = MakeArrayFromList(blocks);
            byte extraBlocks = array[array.Length - 1];
            var res = new byte[array.Length - extraBlocks];
            Array.Copy(array, res, res.Length);
            return res;
        }

        private byte[] MakkePaddingPKCS7(byte[] data)
        {
            byte mod = (byte)(BlockSize - data.Length % BlockSize);
            mod = (byte)(mod == 0 ? BlockSize : mod);
            byte[] addedData = new byte[data.Length + mod];
            Array.Copy(data, addedData, data.Length);
            Array.Fill(addedData, mod, data.Length, mod); 
            return addedData;
        }

        
        private byte[] MakeArrayFromList(List<byte[]> data)
        {
            byte[] res = new byte[BlockSize * data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                Array.Copy(data[i], 0, res, i * BlockSize, BlockSize);
            }
            return res;
        }

        private byte[] XOR(byte[] left, byte[] right)
        {
            byte[] res = new byte[left.Length];
            for (int i = 0; i < left.Length; i++)
            {
                res[i] = (byte)(left[i] ^ right[i]);
            }
            return res;
        }
    }
}
