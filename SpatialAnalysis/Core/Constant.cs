using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Core
{
    public static class Constant
    {
        public static long INF = 10000000000000L;
        public static long NAN = - INF;
    }

    public enum RelPointAndLine
    {
        LineRight,
        LineLeft,
        LineOn
    }
}
