using System;
using System.Collections.Generic;
using System.Linq;
using FeynmanDiagram.Analysis;
using GRaff;
using GRaff.Panels;

namespace FeynmanDiagram
{
    public class DiagramContainerElement : PanelElement, IGlobalMousePressListener
    {
        public DiagramContainerElement(Rectangle region)
            : base(new DiagramContainerNode(region))
        {
        }

        public override void OnDraw()
        {
            Draw.Rectangle(Root.Region, Colors.Black);
            base.OnDraw();
        }

        public Diagram MakeDiagram()
        {
            var vertexIndices = new Dictionary<Vertex, int>();
            var vertices = new List<VertexData>();
            var edges = new List<EdgeData>();
            var index = 0;

            foreach (var vertex in Root.Children.OfType<Vertex>())
            {
                vertices.Add(new VertexData(index, vertex.IsInput ? VertexType.Input : vertex.IsOutput ? VertexType.Output : VertexType.Internal));
                vertexIndices[vertex] = index;
                index++;
            }

            foreach (var edge in Root.Children.OfType<Edge>())
            {
                edges.Add(new EdgeData(vertexIndices[edge.From], vertexIndices[edge.To], edge.EdgeType));
            }

            return new Diagram(vertices, edges);
        }

        public (IEnumerable<ParticleType> input, IEnumerable<ParticleType> output) GetEquation()
        {
            var input = new List<ParticleType>();
            var output = new List<ParticleType>();

            foreach (var vertex in Root.Children.OfType<Vertex>())
            {
                if (vertex.IsInput)
                    input.Add(vertex.GetEndpointType());
                else if (vertex.IsOutput)
                    output.Add(vertex.GetEndpointType());
            }

            return (input.OrderBy(p => p), output.OrderBy(p => p));
        }

        public double GetCouplingCost()
        {
            var cost = 0.0;

            foreach (var vertex in Root.Children.OfType<Vertex>())
            {
                var es = vertex.Decompose();
                if (es == null)
                    continue;
                cost += (double)es?.i.CouplingStrength(es?.o, es?.f);
            }

            return cost;
        }

        public int GetOrder() => Root.Children.OfType<Vertex>().Where(v => v.IsValid && v.Edges.Count() >= 3).Count();

        public bool IsValid
        {
            get
            {
#warning Also make sure it is connected
                return Root.Children.OfType<Vertex>().All(v => v.IsValid);
            }
        }


    }
}
