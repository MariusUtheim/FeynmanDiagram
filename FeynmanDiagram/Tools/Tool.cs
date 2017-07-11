﻿﻿using System;
using GRaff;

namespace FeynmanDiagram.Tools
{
    public abstract class Tool
    {
        public virtual void DraggingRegionAction(DiagramContainerNode node) { }

        public virtual void VertexAction(Vertex vertex) { }

        public virtual void EdgeAction(Edge edge) { }

        public virtual bool AllowMerge => false;

        public abstract Key Hotkey { get;}

        public abstract Color Color { get; }

        public abstract string Text { get; }


        public static Tool Pointer { get; } = new PointerTool();
        public static Tool Vertex { get; } = new VertexCreationTool();
        public static Tool Delete { get; } = new DeleteTool();

        public static Tool Electron { get; } = new EdgeCreationTool(ParticleType.Electron, Key.E, Colors.Black, "Electron");
        public static Tool Muon { get; } = new EdgeCreationTool(ParticleType.Muon, Key.M, Colors.Black, "Muon");
        public static Tool ENeutrino { get; } = new EdgeCreationTool(ParticleType.ENeutrino, Key.N, Colors.White, "Neutrino(e)");
        public static Tool MNeutrino { get; } = new EdgeCreationTool(ParticleType.MNeutrino, Key.N, Colors.White, "Neutrino(m)");
        public static Tool Up(ColorCharge color) => new EdgeCreationTool(ParticleType.Up(color), Key.U, color.GetColor(), $"Up ({color})");
        public static Tool Down(ColorCharge color) => new EdgeCreationTool(ParticleType.Down(color), Key.D, color.GetColor(), $"Down ({color})");
        public static Tool Top(ColorCharge color) => new EdgeCreationTool(ParticleType.Top(color), Key.T, color.GetColor(), $"Top ({color})");
        public static Tool Bottom(ColorCharge color) => new EdgeCreationTool(ParticleType.Bottom(color), Key.B, color.GetColor(), $"Bottom ({color})");
        public static Tool Photon { get; } = new EdgeCreationTool(ParticleType.Photon, Key.H, Colors.Black, "Photon");
        public static Tool Gluon = new EdgeCreationTool(ParticleType.Gluon, Key.G, Colors.White, "Gluon");
        public static Tool WBoson = new EdgeCreationTool(ParticleType.WPlus, Key.W, Colors.Black, "W");
    }
}