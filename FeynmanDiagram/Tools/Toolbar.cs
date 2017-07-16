using System;
using System.Linq;
using GRaff;
using GRaff.Graphics.Text;
using FeynmanDiagram.Tools;
using System.Collections.Generic;

namespace FeynmanDiagram
{
    public class Toolbar : GameElement, IKeyPressListener, IGlobalMousePressListener
    {
        private const double width = 64, height = 64;

        private Tool[,] _tools;
        private (int x, int y) _activeTool = (0, 0);

        public Toolbar(params Tool[] tools)
        {
            //TODO// assert tools.Select(tool => tool.Hotkey).Distinct().Count() == tools.Length
            this._tools = new Tool[tools.Length, 1];
            for (var i = 0; i < tools.Length; i++)
                _tools[i, 0] = tools[i];
        }

        public Toolbar(Tool[,] tools)
        {
            this._tools = tools;
            //Location = (0, Room.Current.Height);
        }

        public static Toolbar Current => Instance<Toolbar>._;

        public Tool CurrentTool => _tools[_activeTool.x, _activeTool.y];

        public IEnumerable<Tool> Tools
        {
            get
            {
                for (var x = 0; x < _tools.GetLength(0); x++)
                    for (var y = 0; y < _tools.GetLength(1); y++)
                        yield return _tools[x, y];
            }
        }

        public void Click(DiagramContainerNode node)
        {
            CurrentTool.DraggingRegionAction(node);
        }

        public void Click(Vertex vertex)
        {
            CurrentTool.VertexAction(vertex);
        }

        public void Click(Edge edge)
        {
            CurrentTool.EdgeAction(edge);
        }

        public Point Location { get; }

        public (int x, int y) MouseIndex => ((int)((Mouse.Y - Location.Y) / height), (int)((Mouse.X - Location.X) / width));

        public override void OnDraw()
        {
            var targetRegion = new Rectangle(Location, (width, height));
            var idx = MouseIndex;

            for (var x = 0; x < _tools.GetLength(0); x++)
                for (var y = 0; y < _tools.GetLength(1); y++)
                {
                    if (_tools[x, y] == null)
                        continue;
                    var region = targetRegion + new Vector(width * y, height * x);
                    Draw.Sprite(Assets.ToolBackgroundSprite, (x, y).Equals(_activeTool) ? 2 : (x, y).Equals(idx) ? 1 : 0, region.TopLeft);  
                    Draw.Text(_tools[x, y].Text, Assets.ToolbarFont, FontAlignment.Center, region.Center, _tools[x, y].Color);
                    Draw.Text(_tools[x, y].Hotkey.ToString(), Assets.ToolbarFont, FontAlignment.BottomRight, region.BottomRight, Colors.Black);
                }
        }

        public void OnGlobalMousePress(MouseButton button)
        {
            var (xIndex, yIndex) = MouseIndex;
            if (xIndex >= 0 && xIndex < _tools.GetLength(0) && yIndex >= 0 && yIndex < _tools.GetLength(1) && _tools[xIndex, yIndex] != null)
                _activeTool = (xIndex, yIndex);
        }

        public void OnKeyPress(Key key)
        {
            for (var x = 0; x < _tools.GetLength(0); x++)
                for (var y = 0; y < _tools.GetLength(1); y++)
                    if (_tools[x, y] != null && _tools[x, y].Hotkey == key)
                    {
                        _activeTool = (x, y);
                        break;
                    }
        }
    }
}
