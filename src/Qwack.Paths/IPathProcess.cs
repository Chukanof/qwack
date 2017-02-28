using System;
using System.Collections.Generic;
using System.Text;

namespace Qwack.Paths
{
    public interface IPathProcess
    {
        bool SetupFeatures(FeatureCollection pathProcessFeaturesCollection);
        void Process(PathBlock block);
    }
}
