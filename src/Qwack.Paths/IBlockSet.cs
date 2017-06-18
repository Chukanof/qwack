using System;
using System.Collections.Generic;

namespace Qwack.Paths
{
    public interface IBlockSet: IEnumerable<IPathBlock>, IDisposable
    {
        int Factors { get; }
        int NumberOfPaths { get; }
        int Steps { get; }
    }
}