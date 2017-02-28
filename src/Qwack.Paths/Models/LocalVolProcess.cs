using System;
using System.Collections.Generic;
using System.Text;
using Qwack.Core.Instruments;
using Qwack.Options.VolSurfaces;

namespace Qwack.Paths.Models
{
    public class LocalVolProcess : IPathProcess
    {
        private IVolSurface _volSurface;
        private IAsset _asset;
        private string _assetName;

        public LocalVolProcess(string assetName, IAsset asset, IVolSurface volSurface)
        {
            _volSurface = volSurface;
            _asset = asset;
            _assetName = assetName;
        }

        public void Process(PathBlock block)
        {
            throw new NotImplementedException();
        }
        
        public bool SetupFeatures(FeatureCollection pathProcessFeaturesCollection)
        {
            throw new NotImplementedException();
        }
    }
}
