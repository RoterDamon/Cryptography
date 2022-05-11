using System;
using System.Numerics;

namespace Second
{
    interface IProbabilisticSimplicityTest
    {//проверять уникальность а 
        bool MakeSimplicityTest(BigInteger value, double minProbability);
    }
}
