using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SpatialAnalysis.Network
{
    public enum NetworkType
    {
        BaseNetwork,
        SimpleNetwork,
        TIN
    }

    public interface INetwork
    {
        List<Node> Nodes { set; get; }
        List<Edge> Edges { set; get; }
        NetworkType NetworkType { get; }
        // void Floyd(INetwork network, ref List<Node>[,] pathMatrix, ref double[,] costMatrix);
    }
}
