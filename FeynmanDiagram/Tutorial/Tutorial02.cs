//using System;
//using GRaff;
//using GRaff.Panels;
//using FeynmanDiagram.Tools;

//namespace FeynmanDiagram.Tutorial
//{
//    public class Tutorial02 : Room
//    {
       
//        public Tutorial02()
//            : base(1024, 768) {}



//        public override void OnEnter()
//        {

//			//Instance.Create(new Toolbar(new PointerTool(), new VertexCreationTool(), new DeleteTool(), new ElectronEdgeCreationTool(), new PhotonEdgeCreationTool()));

//            Node root;
//            Vertex c, v1, v2, v3;


//            root = Instance.Create(new DiagramContainerElement(new Rectangle(262, 160, 400, 300))).Root;

//            c = root.AddChildLast(new Vertex(100, 150));
//            v1 = root.AddChildLast(new Vertex(50, 50));
//            v2 = root.AddChildLast(new Vertex(50, 250));
//            v3 = root.AddChildLast(new Vertex(150, 150));
//            root.AddChildFirst(new Edge(v1, c, ParticleType.Fermion));
//            root.AddChildFirst(new Edge(c, v2, ParticleType.Fermion));
//            root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

//            c = root.AddChildLast(new Vertex(300, 150));
//            v1 = root.AddChildLast(new Vertex(350, 250));
//            v2 = root.AddChildLast(new Vertex(350, 50));
//            v3 = root.AddChildLast(new Vertex(250, 150));
//            root.AddChildFirst(new Edge(v1, c, ParticleType.Fermion));
//            root.AddChildFirst(new Edge(c, v2, ParticleType.Fermion));
//            root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

//        }


//        public override void OnDrawForeground()
//        {
//            Draw.Text("Vertices can be connected by dragging and dropping.", Assets.TutorialTextRenderer, (50, 50));
//			Draw.Text("By combining vertices like these, you can create a diagram", Assets.TutorialTextRenderer, (50, 500));
//		}

//    }
//}
