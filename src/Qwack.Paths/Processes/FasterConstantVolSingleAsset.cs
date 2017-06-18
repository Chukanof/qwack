﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Qwack.Paths.Features;
using static System.Math;

namespace Qwack.Paths.Processes
{
    public class FasterConstantVolSingleAsset : IPathProcess
    {
        private DateTime _startDate;
        private DateTime _expiry;
        private double _vol;
        private double _scaledVol;
        private double _spot;
        private double _drift;
        private int _numberOfSteps;
        private string _name;
        private int _factorIndex;
        private ITimeStepsFeature _timesteps;

        public FasterConstantVolSingleAsset(DateTime startDate, DateTime expiry, double vol, double spot, double drift, int numberOfSteps, string name)
        {
            _startDate = startDate;
            _expiry = expiry;
            _vol = vol;
            _spot = spot;
            _drift = drift;
            _numberOfSteps = numberOfSteps;
            _name = name;
            _scaledVol = _vol / Sqrt(365.0);
        }

        public void Process(IPathBlock block)
        {
            var driftLocal = _drift;
            var scaledLocal = _scaledVol;
            var timeStepsLocal = _timesteps.TimeSteps;
            for (var path = 0; path < block.NumberOfPaths; path += Vector<double>.Count)
            {
                //This should be set to the spot price here
                var previousStep = new Vector<double>(_spot);
                var steps = block.GetStepsForFactor(path, _factorIndex);
                steps[0] = previousStep;
                for (var step = 1; step < block.NumberOfSteps; step++)
                {
                    var drift = driftLocal * timeStepsLocal[step] * previousStep;
                    var delta = scaledLocal * steps[step] * previousStep;

                    previousStep = (previousStep + drift + delta);
                    steps[step] = previousStep;
                }
            }
        }

        public void SetupFeatures(FeatureCollection pathProcessFeaturesCollection)
        {
            var mappingFeature = pathProcessFeaturesCollection.GetFeature<IPathMappingFeature>();
            _factorIndex = mappingFeature.AddDimension(_name);

            _timesteps = pathProcessFeaturesCollection.GetFeature<ITimeStepsFeature>();
            var stepSize = (_expiry - _startDate).TotalDays / _numberOfSteps;
            for (var i = 0; i < _numberOfSteps - 1; i++)
            {
                _timesteps.AddDate(_startDate.AddDays(i * stepSize));
            }
            _timesteps.AddDate(_expiry);
        }
    }
}
