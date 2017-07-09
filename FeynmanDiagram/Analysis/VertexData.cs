using System;
namespace FeynmanDiagram.Analysis
{

    public enum VertexType
    {
        Input, Internal, Output
    }

    public class VertexData
    {
        public VertexData(int index, VertexType type)
        {
            this.Index = index;
            this.Type = type;
        }

        public int Index { get; }

        public VertexType Type { get; }
    }
}
