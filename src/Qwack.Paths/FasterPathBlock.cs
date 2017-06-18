﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Qwack.Paths
{
    public class FasterPathBlock : IDisposable, IPathBlock
    {
        private readonly int _numberOfPaths;
        private readonly int _numberOfFactors;
        private readonly int _numberOfSteps;
        private GCHandle _handle;
        private double[] _backingArray;
        private static readonly int _vectorShift = (int)System.Math.Log(Vector<double>.Count, 2);
        private readonly int _factorJumpSize;
        private readonly int _pathJumpSize;


        public FasterPathBlock(int numberOfPaths, int factors, int numberOfSteps)
        {
            _numberOfPaths = numberOfPaths;
            _numberOfFactors = factors;
            _numberOfSteps = numberOfSteps;
            _factorJumpSize = Vector<double>.Count* _numberOfSteps;
            _pathJumpSize = _factorJumpSize * _numberOfFactors;
            _backingArray = new double[numberOfPaths * factors * numberOfSteps];
            _handle = GCHandle.Alloc(_backingArray, GCHandleType.Pinned);
        }

        public int NumberOfPaths => _numberOfPaths;
        public int Factors => _numberOfFactors;
        public int NumberOfSteps => _numberOfSteps;
        public static int MinNumberOfPaths => Vector<double>.Count;
        public int TotalBlockSize => _numberOfPaths * _numberOfFactors * _numberOfSteps;
        public double[] RawData => _backingArray;

        public double this[int index] { get => _backingArray[index]; set => _backingArray[index] = value; }

        public unsafe Span<Vector<double>> GetStepsForFactor(int pathId, int factorId)
        {
            var byteOffset = GetIndexOfPathStart(pathId, factorId) << 3;
            var pointer = (void*)IntPtr.Add(_handle.AddrOfPinnedObject(), byteOffset);
            var span = new Span<Vector<double>>(pointer, _numberOfSteps);
            return span;
        }
               
        public int GetIndexOfPathStart(int pathId, int factorId)
        {
            var pathIndex = pathId >> _vectorShift;
            var pathDelta = pathIndex * _pathJumpSize;
            var factorDelta = _factorJumpSize * factorId;

            var totalIndex = pathDelta + factorDelta;
            return totalIndex;
        }

        public void Dispose()
        {
            if (_handle.IsAllocated)
            {
                _handle.Free();
            }
            GC.SuppressFinalize(this);
        }

        ~FasterPathBlock() => Dispose();
    }
}
