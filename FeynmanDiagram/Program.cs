using System;
using System.Linq;
using FeynmanDiagram.Tools;
using GRaff;

namespace FeynmanDiagram
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Giraffe.Run(1024, 768, gameStart);
        }

        public static void gameStart()
        {
            GlobalEvent.ExitOnEscape = true;
            Assets.LoadAll();

            //Room.Goto(new Tutorial01());

            Room.Goto(PuzzleRoom.Puzzles[0]);

            return;

            //var center = Instance.Create(new Vertex(null, Room.Current.Center));
            //var in1 = Instance.Create(new Vertex(null, (100, 100)));
            //var in2 = Instance.Create(new Vertex(null, (100, 600)));
            //var @out = Instance.Create(new Vertex(null, (800, 380)));
            //Instance.Create(new Edge(null, in1, center, EdgeType.Fermion));
            //Instance.Create(new Edge(null, center, in2, EdgeType.Fermion));
            //Instance.Create(new Edge(null, center, @out, EdgeType.Photon));

            //Instance.Create(new Toolbar(new PointerTool(), new FermionVertexCreationTool(), new PhotonVertexCreationTool(), new GluonVertexCreationTool()));
        }
    }
}
