using System;
using System.Linq;
using GRaff;
using GRaff.Graphics.Text;
using FeynmanDiagram.Tools;

namespace FeynmanDiagram
{
    public class Toolbar : GameElement, IKeyPressListener, IGlobalMousePressListener
    {
        private const double width = 50, height = 50;

        private Tool[] _tools;
        private int _activeTool = 0;

        public Toolbar(params Tool[] tools)
        {
            //TODO// assert tools.Select(tool => tool.Hotkey).Distinct().Count() == tools.Length
            this._tools = tools;
        }

        public static Toolbar Current => Instance<Toolbar>._;

        public Tool CurrentTool => _tools[_activeTool];

        public void Click(DiagramContainerNode node)
        {
            _tools[_activeTool].DraggingRegionAction(node);
        }

        public void Click(Vertex vertex)
        {
            _tools[_activeTool].VertexAction(vertex);
        }

        public void Click(Edge edge)
        {
            _tools[_activeTool].EdgeAction(edge);
        }

        public Point Location { get; }

        public override void OnDraw()
        {
            var targetRegion = new Rectangle(Location, (width, height));

            for (var i = 0; i < _tools.Length; i++)
            {
                Draw.FillRectangle(targetRegion, i == _activeTool ? Colors.LightGray : Colors.Gray);
                Draw.Text(_tools[i].Text, Assets.ToolbarFont, FontAlignment.Center, targetRegion.Center, _tools[i].Color);
                Draw.Text(_tools[i].Hotkey.ToString(), Assets.ToolbarFont, FontAlignment.BottomRight, targetRegion.BottomRight, Colors.Black);
                targetRegion += new Vector(width, 0);
            }
        }

        public void OnGlobalMousePress(MouseButton button)
        {
            var index = (int)((Mouse.X - Location.X) / width);
            if (index >= 0 && index < _tools.Length && Mouse.Y >= Location.Y && Mouse.Y < Location.Y + height)
                _activeTool = index;
        }

        public void OnKeyPress(Key key)
        {
            for (var i = 0; i < _tools.Length; i++)
                if (_tools[i].Hotkey == key)
                {
                    _activeTool = i;
				    break;
                }
        }
    }
}
