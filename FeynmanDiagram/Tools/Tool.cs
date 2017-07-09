﻿using System;
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


        public static PointerTool Pointer { get; } = new PointerTool();
        public static VertexCreationTool Vertex { get; } = new VertexCreationTool();
        public static DeleteTool Delete { get; } = new DeleteTool();

        public static EdgeCreationTool Electron { get; } = new EdgeCreationTool(ParticleType.Electron, Key.E, Colors.Black, "Electron");
        public static EdgeCreationTool Muon { get; } = new EdgeCreationTool(ParticleType.Muon, Key.M, Colors.Black, "Muon");
        public static EdgeCreationTool ENeutrino { get; } = new EdgeCreationTool(ParticleType.ENeutrino, Key.N, Colors.White, "Neutrino(e)");
        public static EdgeCreationTool Up(ColorCharge color) => new EdgeCreationTool(ParticleType.Up(color), Key.U, color.GetColor(), $"Up ({color})");
        public static EdgeCreationTool Down(ColorCharge color) => new EdgeCreationTool(ParticleType.Down(color), Key.D, color.GetColor(), $"Down ({color})");
        public static EdgeCreationTool Top(ColorCharge color) => new EdgeCreationTool(ParticleType.Top(color), Key.T, color.GetColor(), $"Top ({color})");
        public static EdgeCreationTool Bottom(ColorCharge color) => new EdgeCreationTool(ParticleType.Bottom(color), Key.B, color.GetColor(), $"Bottom ({color})");
        public static EdgeCreationTool Photon { get; } = new EdgeCreationTool(ParticleType.Photon, Key.H, Colors.Black, "Photon");
        public static EdgeCreationTool Gluon = new EdgeCreationTool(ParticleType.Gluon, Key.G, Colors.White, "Gluon");
        public static EdgeCreationTool WBoson = new EdgeCreationTool(ParticleType.WPlus, Key.W, Colors.Black, "W");
    }
}