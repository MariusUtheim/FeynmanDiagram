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

        public override void DrawTo(Rectangle region)
        {
            Draw.Sprite(Assets.PointerSprite, 0, Matrix.Scaling(0.25, 0.25).Translate(region.Center.X, region.Center.Y));
        }
    }
}
