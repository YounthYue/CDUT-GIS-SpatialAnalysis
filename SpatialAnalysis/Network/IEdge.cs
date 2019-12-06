using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Network
{
    public interface IEdge
    {
        int StartNodeId
        {
            set;
            get;
        }
        int EndNodeId
        {
            set;
            get;
        }
        string EdgeName
        {
            set;
            get;
        }
        double EdgeValue
        {
            set;
            get;
        }
    }
}
