using System;
using System.Collections.Generic;
using System.Text;

namespace Qwack.Paths.Features
{
    public class PathMappingFeature : IPathMappingFeature
    {
        private List<string> _dimensionNames = new List<string>();
        
        public int NumberOfDimensions => _dimensionNames.Count;

        public int AddDimension(string dimensionName)
        {
            int index = _dimensionNames.Count;
            _dimensionNames.Add(dimensionName);
            return index;
        }

        public bool TryFindDimension(string dimensionName, out int dimensionId)
        {
            for(int i =0; i< _dimensionNames.Count; i++)
            {
                if(_dimensionNames[i].Equals(dimensionName,StringComparison.OrdinalIgnoreCase))
                {
                    dimensionId = i;
                    return true;
                }
            }
            dimensionId = default(int);
            return false;
        }
    }
}
