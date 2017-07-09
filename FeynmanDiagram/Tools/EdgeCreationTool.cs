using System;
using GRaff;

namespace FeynmanDiagram.Tools
{
    public class EdgeCreationTool : Tool
    {
        public EdgeCreationTool(ParticleType type, Key hotkey, Color color, string text)
        {
            this.Type = type;
            this.Hotkey = hotkey;
            this.Color = color;
            this.Text = text;
        }

        public override void DraggingRegionAction(DiagramContainerNode node)
		{
            var loc = node.ToClient(Mouse.Location);
            var fromVertex = node.AddChildLast(new Vertex(loc));
            var toVertex = node.AddChildLast(new Vertex(loc));
			node.AddChildFirst(new Edge(fromVertex, toVertex, Type));
            toVertex.Drag(loc);
		}

		public override void VertexAction(Vertex vertex)
		{
			vertex.MakeVertex(Type);
		}

		public override void EdgeAction(Edge edge)
		{
			edge.Branch(Type);
		}

        public ParticleType Type { get; }

        public override bool AllowMerge => true;

        public override Key Hotkey { get; }

        public override Color Color { get; }

        public override string Text { get; }


	}

}
