﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Qwack.Paths.Features;

namespace Qwack.Paths
{
    public class FasterPathEngine : IEnumerable<IPathBlock>, IDisposable, IEngineFeature
    {
        private List<IPathProcess> _pathProcesses = new List<IPathProcess>();
        private List<object> _pathProcessFeatures = new List<object>();
        private int _numberOfPaths;
        private FeatureCollection _featureCollection = new FeatureCollection();
        private int _dimensions;
        private int _steps;
        private FasterBlockSet _blockset;

        public FasterPathEngine(int numberOfPaths)
        {
            _numberOfPaths = numberOfPaths;
            _featureCollection.AddFeature<IPathMappingFeature>(new PathMappingFeature());
            _featureCollection.AddFeature<ITimeStepsFeature>(new TimeStepsFeature());
            _featureCollection.AddFeature<IEngineFeature>(this);
        }

        public FasterBlockSet BlockSet => _blockset;
        public int NumberOfPaths => _numberOfPaths;

        public void RunProcess()
        {
            _blockset = new FasterBlockSet(_numberOfPaths, _dimensions, _steps);

            foreach (var block in _blockset)
            {
                foreach (var process in _pathProcesses)
                {
                    process.Process(block);
                }
            }
        }

        public FeatureCollection Features => _featureCollection;

        public void AddPathProcess(IPathProcess process) => _pathProcesses.Add(process);

        public void SetupFeatures()
        {
            foreach (var pp in _pathProcesses)
            {
                pp.SetupFeatures(_featureCollection);
            }
            _dimensions = _featureCollection.GetFeature<IPathMappingFeature>().NumberOfDimensions;
            _steps = _featureCollection.GetFeature<ITimeStepsFeature>().TimeStepCount;
            _featureCollection.FinishSetup();
            foreach(var process in _pathProcesses)
            {
                if (process is IFeatureRequiresFinish finishProcess)
                {
                    finishProcess.Finish(_featureCollection);
                }
            }
        }

        public IEnumerator<IPathBlock> GetEnumerator() => _blockset.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _blockset.GetEnumerator();

        public void Dispose()
        {
            foreach (var block in _blockset)
            {
                block.Dispose();
            }
        }
    }
}
