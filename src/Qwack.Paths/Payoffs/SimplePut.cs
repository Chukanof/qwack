using System;
using System.Collections.Generic;
using System.Text;
using Qwack.Paths.Features;

namespace Qwack.Paths.Payoffs
{
    public class SimplePut:IPathProcess
    {
        private string _asset;
        private double _strike;
        private DateTime _expiry;
        
        public SimplePut(string asset, double strike, DateTime expiry)
        {
            _asset = asset;
            _strike = strike;
            _expiry = expiry;
        }

        public void Process(PathBlock block)
        {
            throw new NotImplementedException();
        }

        public bool SetupFeatures(FeatureCollection featuresCollection)
        {
            var timeSteps = featuresCollection.GetFeature<ITimeStepsFeature>();
            timeSteps.AddDate(_expiry);
            var pathMap = featuresCollection.GetFeature<IPathMappingFeature>();
            if(pathMap.TryFindDimension(_asset, out int dimensionId))
            {
                throw new ArgumentOutOfRangeException(nameof(_asset), "Could not find the asset in the path map");
            }
            return true;
        }
    }
}
