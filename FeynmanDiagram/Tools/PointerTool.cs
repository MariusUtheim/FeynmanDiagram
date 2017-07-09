using System;
using GRaff;

namespace FeynmanDiagram.Tools
{
    public class PointerTool : Tool
    {

        public override void VertexAction(Vertex vertex) => vertex.Drag();

        public override Key Hotkey => Key.Escape;

        public override Color Color => Colors.Black;

        public override string Text => "Pointer";
    }
}
