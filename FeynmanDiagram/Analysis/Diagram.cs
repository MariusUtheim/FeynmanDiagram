using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeynmanDiagram.Analysis
{
    public sealed class Diagram
    {

        public Diagram(IEnumerable<VertexData> vertices, IEnumerable<EdgeData> edges)
        {
            this.Vertices = vertices;
            this.Edges = edges;
        }

        public IEnumerable<VertexData> Vertices { get; }

        public IEnumerable<EdgeData> Edges { get; }

        

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine($"{Vertices.Count()},{Edges.Count()}");

            foreach (var vertex in Vertices)
                str.AppendLine($"{(vertex.Type == VertexType.Input ? '>' : vertex.Type == VertexType.Output ? '<' : '-')}{vertex.Index}");

            foreach (var edge in Edges)
                str.AppendLine($"{edge.From}{edge.ToSymbol()}{edge.To}");

            return str.ToString();
        }


        private (VertexData[] ins, VertexData[] mids, VertexData[] outs) _partitionVertices()
        {
            return (
                Vertices.Where(v => v.Type == VertexType.Input).ToArray(),
                Vertices.Where(v => v.Type == VertexType.Internal).ToArray(),
                Vertices.Where(v => v.Type == VertexType.Output).ToArray()
            );
        }

        public bool Equivalent(Diagram other)
        {
            var (leftIns, leftMids, leftOuts) = _partitionVertices();
            var (rightIns, rightMids, rightOuts) = other._partitionVertices();

            if (leftIns.Length != rightIns.Length || leftMids.Length != rightMids.Length || leftOuts.Length != rightOuts.Length)
                return false;
            

            throw new NotImplementedException();
		}


    }
}
