﻿using Qwack.Options.VolSurfaces;
using Qwack.Paths.Features;
using System;
using System.Collections.Generic;
using System.Text;
using Qwack.Core.Underlyings;

namespace Qwack.Paths.Processes
{
    public class BlackSingleAsset : IPathProcess
    {
        private IVolSurface _surface;
        private DateTime _expiryDate;
        private int _nTimeSteps;
        
        public BlackSingleAsset(IVolSurface volSurface, DateTime expiryDate, int nTimeSteps)
        {
            _surface = volSurface;
            _expiryDate = expiryDate;
            _nTimeSteps = nTimeSteps;
        }
        public void Process(IPathBlock block)
        {
            for (var i = 0; i < block.TotalBlockSize; i++)
            {
                throw new NotImplementedException();

            }
            throw new NotImplementedException();
        }

        public void SetupFeatures(FeatureCollection pathProcessFeaturesCollection)
        {
            var mappingFeature = pathProcessFeaturesCollection.GetFeature<IPathMappingFeature>();
            mappingFeature.AddDimension("Black_1");

            var dates = pathProcessFeaturesCollection.GetFeature<ITimeStepsFeature>();
            var stepSize = (_expiryDate - _surface.OriginDate).TotalDays;
            for (var i = 0; i < _nTimeSteps; i++)
            {
                dates.AddDate(_surface.OriginDate.AddDays(i * stepSize));
            }
        }
    }
}
