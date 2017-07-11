using System;
using GRaff;

namespace FeynmanDiagram.Tutorial
{
    public class TutorialRoom : Room
    {
        public TutorialRoom(string txt, params DiagramContainerElement[] figures)
            : base(Program.WindowWidth, Program.WindowHeight) { }


        private static TutorialRoom Tutorial01()
        {
            var elements = new DiagramContainerElement[5];

            elements[0] = new DiagramContainerElement(new Rectangle(312, 160, 400, 200));
            var root = elements[0].Root;

            var center = root.AddChildLast(new Vertex(200, 100));
            var in1 = root.AddChildLast(new Vertex(50, 25));
            var in2 = root.AddChildLast(new Vertex(50, 175));
            var @out = root.AddChildLast(new Vertex(350, 100));
            root.AddChildFirst(new Edge(in1, center, ParticleType.Electron));
            root.AddChildFirst(new Edge(center, in2, ParticleType.Electron));
            root.AddChildFirst(new Edge(center, @out, ParticleType.Photon));


            for (var i = 1; i < 5; i++)
                elements[i] = new DiagramContainerElement(new Rectangle(50 + 240 * i, 525, 200, 200));

            Vertex c, v1, v2, v3;

            root = elements[1].Root;
            c = root.AddChildLast(new Vertex(100, 100));
            v1 = root.AddChildLast(new Vertex(25, 25));
            v2 = root.AddChildLast(new Vertex(25, 175));
            v3 = root.AddChildLast(new Vertex(175, 100));
            root.AddChildFirst(new Edge(v1, c, ParticleType.Electron));
            root.AddChildFirst(new Edge(v2, c, ParticleType.Electron));
            root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

            root = elements[2].Root;
            c = root.AddChildLast(new Vertex(100, 100));
            v1 = root.AddChildLast(new Vertex(25, 25));
            v2 = root.AddChildLast(new Vertex(175, 25));
            v3 = root.AddChildLast(new Vertex(100, 175));
            root.AddChildFirst(new Edge(c, v1, ParticleType.Electron));
            root.AddChildFirst(new Edge(c, v2, ParticleType.Electron));
            root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

            root = elements[3].Root;
            c = root.AddChildLast(new Vertex(100, 175));
            v1 = root.AddChildLast(new Vertex(25, 100));
            v2 = root.AddChildLast(new Vertex(175, 100));
            v3 = root.AddChildLast(new Vertex(100, 25));
            root.AddChildFirst(new Edge(v1, c, ParticleType.Electron));
            root.AddChildFirst(new Edge(c, v2, ParticleType.Electron));
            root.AddChildFirst(new Edge(c, v3, ParticleType.Electron));

            root = elements[4].Root;
            c = root.AddChildLast(new Vertex(100, 100));
            v1 = root.AddChildLast(new Vertex(25, 175));
            v2 = root.AddChildLast(new Vertex(100, 25));
            v3 = root.AddChildLast(new Vertex(175, 100));
            root.AddChildFirst(new Edge(v1, c, ParticleType.Electron));
            root.AddChildFirst(new Edge(c, v2, ParticleType.Photon));
            root.AddChildFirst(new Edge(c, v3, ParticleType.Photon));

            return new TutorialRoom("This is a vertex", elements);
        }

    }
}
