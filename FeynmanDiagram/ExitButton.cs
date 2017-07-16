using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff;

namespace FeynmanDiagram
{
    class ExitButton : GameObject, IMousePressListener
    {
        public ExitButton()
        {
            Mask.Shape = MaskShape.Rectangle(0, 0, 40, 40);
        }

        public void OnMousePress(MouseButton button)
        {
            if (button == MouseButton.Left)
                Giraffe.Quit();
        }

        public override void OnDraw()
        {
            Draw.FillRectangle(Location, (40, 40), Colors.DarkRed);
            Draw.Text("Quit", Assets.ToolbarFont, Location);
        }
    }
}
