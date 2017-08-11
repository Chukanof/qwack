using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Math;

namespace Qwack.Random.Sobol
{
    public class Sobol
    {
        //private SobolDirectionNumbers _directionNumbers;
        private static readonly double _convertToDoubleConstant = Pow(2.0, -32.0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double ConvertRandToDouble(uint rand) => rand * _convertToDoubleConstant;

        private static int NumberOfBitsNeeded(int numberOfPoints)
        {
            var numberOfBits = Ceiling(Log(numberOfPoints) / Log(2.0));
            return (int)numberOfBits;
        }

        
    }
}
