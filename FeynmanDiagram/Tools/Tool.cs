﻿using System;
using GRaff;
using GRaff.Graphics.Text;

namespace FeynmanDiagram.Tools
{
    public abstract class Tool
    {
        public virtual void DraggingRegionAction(DiagramContainerNode node) { }

        public virtual void VertexAction(Vertex vertex) { }

        public virtual void EdgeAction(Edge edge) { }

        public virtual bool AllowMerge => false;

        public virtual void DrawTo(Rectangle region)
        {
            Draw.Text(Text, Assets.ToolbarFont, FontAlignment.Center, region.Center, Color);
        }


        public abstract Key Hotkey { get;}

        public abstract Color Color { get; }

        public abstract string Text { get; }


        public static Tool Pointer { get; } = new PointerTool();
        public static Tool Vertex { get; } = new VertexCreationTool();
        public static Tool Delete { get; } = new DeleteTool();

        public static Tool Electron { get; } = new EdgeCreationTool(ParticleType.Electron, Key.E, Colors.DarkBlue, "Electron");
        public static Tool Muon { get; } = new EdgeCreationTool(ParticleType.Muon, Key.M, Colors.Purple, "Muon");
        public static Tool ENeutrino { get; } = new EdgeCreationTool(ParticleType.ENeutrino, Key.N, Colors.White, "Electron\nneutrino");
        public static Tool MNeutrino { get; } = new EdgeCreationTool(ParticleType.MNeutrino, Key.N, Colors.NavajoWhite, "Muon\nneutrino");
        public static Tool Up(ColorCharge color) => new EdgeCreationTool(ParticleType.Up(color), Key.U, color.GetColor(), $"Up\n({color})");
        public static Tool Down(ColorCharge color) => new EdgeCreationTool(ParticleType.Down(color), Key.D, color.GetColor(), $"Down\n({color})");
        public static Tool Top(ColorCharge color) => new EdgeCreationTool(ParticleType.Top(color), Key.T, color.GetColor(), $"Top\n({color})");
        public static Tool Bottom(ColorCharge color) => new EdgeCreationTool(ParticleType.Bottom(color), Key.B, color.GetColor(), $"Bottom\n({color})");
        public static Tool Photon { get; } = new EdgeCreationTool(ParticleType.Photon, Key.H, Colors.DarkOrange, "Photon");
        public static Tool Gluon = new EdgeCreationTool(ParticleType.Gluon, Key.G, Colors.SlateBlue, "Gluon");
        public static Tool WBoson = new EdgeCreationTool(ParticleType.WPlus, Key.W, Colors.Black, "W boson");
        public static Tool Higgs = new EdgeCreationTool(ParticleType.Higgs, Key.H, Colors.Yellow, "Higgs");
    }
}