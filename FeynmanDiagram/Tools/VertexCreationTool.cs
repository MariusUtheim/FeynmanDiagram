using System;
using GRaff;

namespace FeynmanDiagram.Tools
{
    public class VertexCreationTool : Tool
    {
        public override void DraggingRegionAction(DiagramContainerNode node)
        {
            var newVertex = node.AddChildLast(new Vertex(node.ToClient(Mouse.Location)));
            newVertex.Drag();
        }

		public override void VertexAction(Vertex vertex) => vertex.Drag();

		public override void EdgeAction(Edge edge) => edge.Drag();

        public override bool AllowMerge => true;

		public override Key Hotkey => Key.V;

		public override Color Color => Colors.Black;

		public override string Text => "Vertex";
    }
}
