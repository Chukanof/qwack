﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Qwack.Paths
{
    public interface IPathProcess
    {
        void SetupFeatures(FeatureCollection pathProcessFeaturesCollection);
        void Process(IPathBlock block);
    }
}
