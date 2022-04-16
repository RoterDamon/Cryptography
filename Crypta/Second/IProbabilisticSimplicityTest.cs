using System;
using System.Numerics;

namespace Second
{
    interface IProbabilisticSimplicityTest
    {
        bool MakeSimplicityTest(BigInteger value, double minProbability);
    }
}
