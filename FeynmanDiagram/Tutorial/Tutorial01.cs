//﻿using System;
//using System.Linq;
//using FeynmanDiagram.Tools;
//using GRaff;
//using GRaff.Graphics.Text;

//namespace FeynmanDiagram.Tutorial
//{
//    public class Tutorial01 : Room
//    {
//		private DiagramContainerElement[] _regions;
//		private DiagramContainerElement _region;

//        public Tutorial01()
//            : base(Program.WindowWidth, Program.WindowHeight)
//        {
//            GlobalEvent.KeyPressed += key =>
//            {
//                if (key == Key.Right)
//                    Room.Goto(new Tutorial02());
//            };
//        }

//        public override void OnEnter()
//        {
//            Instance.Create(new Toolbar(new PointerTool()));

//            _region = Instance.Create(new DiagramContainerElement(new Rectangle(312, 160, 400, 200)));
//            var root = _region.Root;

//            var center = root.AddChildLast(new Vertex(200, 100));
//            var in1 = root.AddChildLast(new Vertex(50, 25));
//            var in2 = root.AddChildLast(new Vertex(50, 175));
//            var @out = root.AddChildLast(new Vertex(350, 100));
//            root.AddChildFirst(new Edge(in1, center, ParticleType.Fermion));
//            root.AddChildFirst(new Edge(center, in2, ParticleType.Fermion));
//            root.AddChildFirst(new Edge(center, @out, ParticleType.Photon));


//		    _regions = new DiagramContainerElement[4];

//            for (var i = 0; i < _regions.Length; i++)
//                _regions[i] = Instance.Create(new DiagramContainerElement(new Rectangle(50 + 240 * i, 525, 200, 200)));

//			Vertex c, v1, v2, v3;

//            root = _regions[0].Root;
//			c = root.AddChildLast(new Vertex(100, 100));
//			v1 = root.AddChildLast(new Vertex(25, 25));
//			v2 = root.AddChildLast(new Vertex(25, 175));
//			v3 = root.AddChildLast(new Vertex(175, 100));
//			root.AddChildFirst(new Edge(v1, c, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(v2, c, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

//            root = _regions[1].Root;
//			c = root.AddChildLast(new Vertex(100, 100));
//			v1 = root.AddChildLast(new Vertex(25, 25));
//			v2 = root.AddChildLast(new Vertex(175, 25));
//			v3 = root.AddChildLast(new Vertex(100, 175));
//			root.AddChildFirst(new Edge(c, v1, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(c, v2, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

//            root = _regions[2].Root;
//			c = root.AddChildLast(new Vertex(100, 175));
//			v1 = root.AddChildLast(new Vertex(25, 100));
//			v2 = root.AddChildLast(new Vertex(175, 100));
//			v3 = root.AddChildLast(new Vertex(100, 25));
//			root.AddChildFirst(new Edge(v1, c, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(c, v2, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(c, v3, ParticleType.Fermion));

//            root = _regions[3].Root;
//			c = root.AddChildLast(new Vertex(100, 100));
//			v1 = root.AddChildLast(new Vertex(25, 175));
//			v2 = root.AddChildLast(new Vertex(100, 25));
//			v3 = root.AddChildLast(new Vertex(175, 100));
//			root.AddChildFirst(new Edge(v1, c, ParticleType.Fermion));
//			root.AddChildFirst(new Edge(c, v2, ParticleType.Photon));
//			root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

//		}

//        public override void OnDrawForeground()
//        {
//            Draw.Text("This is a vertex. Vertices are the fundamental parts of this game. You can move it around and stuff.", Assets.TutorialTextRenderer, (50, 50));
//            Draw.Text("A vertex contains an incoming and an outgoing particle, as well as a force carrier particle. These are not valid vertices:", Assets.TutorialTextRenderer, (50, 350));
//        }


//    }
//}
