using System;
using System.Numerics;

namespace Qwack.Paths
{
    public interface IPathBlock:IDisposable
    {
        double this[int index] { get; set; }

        int Factors { get; }
        int NumberOfPaths { get; }
        int NumberOfSteps { get; }
        double[] RawData { get; }
        int TotalBlockSize { get; }

        int GetIndexOfPathStart(int pathId, int factorId);
        Span<Vector<double>> GetStepsForFactor(int pathId, int factorId);
    }
}