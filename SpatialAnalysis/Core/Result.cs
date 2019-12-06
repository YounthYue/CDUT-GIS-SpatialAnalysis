using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Core
{
    [Serializable]
    public class Result
    {
        public struct IntersectOfLines
        {
            public Core.Point intersectPoint;
            public Core.SimpleLine intersectSimpleLine;
        }
    }
}
