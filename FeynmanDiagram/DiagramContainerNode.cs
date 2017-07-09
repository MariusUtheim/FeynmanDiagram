using System;
using GRaff;
using GRaff.Panels;

namespace FeynmanDiagram
{
    public class DiagramContainerNode : Node, IPanelMousePressListener
    {
        public DiagramContainerNode(Rectangle region)
        {
            this.Region = region;
        }

        public void OnMousePress(MouseEventArgs e)
        {
            Toolbar.Current.Click(this);
        }
    }
}
