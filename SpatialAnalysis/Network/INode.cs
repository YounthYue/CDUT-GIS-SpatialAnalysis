using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Network
{
    public interface INode
    {
        int NodeId
        {
            set;
            get;
        }
        string NodeName
        {
            set;
            get;
        }
        double NodeValue
        {
            set;
            get;
        }
    }
}
