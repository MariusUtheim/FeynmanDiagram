using System;
using GRaff;

namespace FeynmanDiagram.Tools
{
    public class DeleteTool : Tool
    {


        public override void VertexAction(Vertex vertex)
        {
            vertex.Parent = null;
        }

        public override void EdgeAction(Edge edge)
        {
            edge.Parent = null;
        }

        public override Key Hotkey => Key.D;

        public override Color Color => Colors.Red;

        public override string Text => "Delete";

        public override void DrawTo(Rectangle region)
        {
            Draw.Sprite(Assets.DeleteSprite, 0, Matrix.Scaling(0.3, 0.3).Translate(region.Center.X, region.Center.Y));
        }
    }
}
