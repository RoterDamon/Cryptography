using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Second
{
    class Attack
    {
        public Tuple<BigInteger, List<Tuple<BigInteger, BigInteger>>> WienerAttack(BigInteger e, BigInteger N)
        {
            int count = 0;
            List<Tuple<BigInteger, BigInteger>> list = new List<Tuple<BigInteger, BigInteger>>();
            BigInteger M, C;
            var random = new Random();
            byte[] buffer = new byte[8];
            random.NextBytes(buffer);
            BigInteger message   = new BigInteger(buffer);
            C = Functions.QuickPow(message, e, N);
            BigInteger limitD = (BigInteger)(0.3333 * Math.Pow((double)N, 0.25));
            List<BigInteger> quotients = ContinuedFraction(e, N);
            for (int i = 1; i < quotients.Count; i += 2)
            {
                if (quotients[i] > limitD)
                    break;
                M = Functions.QuickPow(C, quotients[i], N); 
                list.Add(new Tuple<BigInteger, BigInteger>(quotients[count], quotients[count + 1]));
                if (message == M)
                {
                    return new(quotients[count + 1], list);
                }
                    
                count += 2;
            }

            return new(0, list);
        }
        public List<BigInteger> ContinuedFraction(BigInteger up, BigInteger down)
        {                
            int count = 0;
            BigInteger prevP = 1, prevQ = 0, p, q;
            List<BigInteger> quotients = new List<BigInteger>();
            List<BigInteger> res = new List<BigInteger>();
            BigInteger a = up / down;
            quotients.Add(a);
            while (a * down != up)
            {
                BigInteger tmp = up - a * down;
                up = down;
                down = tmp;
                a = up / down;
                quotients.Add(a);
            }
            p = quotients[0];
            q = 1;
            res.Add(p);
            res.Add(q);
            for (int i = 1; i < quotients.Count; i++)
            {
                p = quotients[i] * p + prevP;
                q = quotients[i] * q + prevQ;
                prevP = res[count];
                prevQ = res[count + 1];
                res.Add(p);
                res.Add(q);
                
                count += 2;
            }
            return res;
        }
    }
}
