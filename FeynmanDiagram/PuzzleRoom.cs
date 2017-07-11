using System;
using System.Collections.Generic;
using System.Linq;
using FeynmanDiagram.Tools;
using GRaff;
using GRaff.Panels;
using GRaff.Synchronization;
using static FeynmanDiagram.ColorCharge;

namespace FeynmanDiagram
{
    public sealed class PuzzleRoom : Room
    {
        Toolbar _toolbar;
        DiagramContainerElement _element;
        string _task, _name;
        int _minOrder;
        IEnumerable<ParticleType> _input, _output;
        Alarm _nextLevelAlarm;

        private Color _color = Colors.AliceBlue;

        private PuzzleRoom(string name, int minOrder, IEnumerable<ParticleType> input, IEnumerable<ParticleType> output)
            : base(Program.WindowWidth, Program.WindowHeight)
        {
            _name = name;
            _minOrder = minOrder;

            _task = $"{String.Join(" + ", input.Select(p => p.Symbol))} -> {String.Join(" + ", output.Select(p => p.Symbol))}";
            _input = input.OrderBy(p => p).ToList();
            _output = output.OrderBy(p => p).ToList();
        }

        public PuzzleRoom(string name, int minOrder, IEnumerable<ParticleType> input, IEnumerable<ParticleType> output, Tool[][] availableTools)
            : this(name, minOrder, input, output)
        {
            var w = availableTools.Length;
            var h = 0;
            for (var i = 0; i < w; i++)
                h = GMath.Max(h, availableTools[i].Length);
            var tools = new Tool[w, h];
            for (var x = 0; x < availableTools.Length; x++)
                for (var y = 0; y < availableTools[x].Length; y++)
                    tools[x, y] = availableTools[x][y];

            _toolbar = new Toolbar(tools);
        }

        public sealed override void OnEnter()
        {
            Window.Title = _name;
            Instance.Create(_toolbar);
            _element = Instance.Create(new DiagramContainerElement(new Rectangle(112, 168, 800, 550)));
        }

        public override void OnDrawBackground()
        {
            //Draw.Clear(_color);
            Draw.FillRectangle(Room.Current.ClientRectangle, _color.Transparent(0.85));
        }

        public override void OnDrawForeground()
        {
            Draw.Text(_task, Assets.TaskFont, GRaff.Graphics.Text.FontAlignment.Bottom, (Center.X, 120));
        }

        public override void OnBeginStep()
        {
            if (Keyboard.IsPressed(Key.Enter))
                _gotoNext();
        }

        public override void OnEndStep()
        {
            var (i, o) = _element.GetEquation();

            if (_nextLevelAlarm == null && DraggableNode.DragNode == null && _input.SequenceEqual(i) && _output.SequenceEqual(o) && _element.GetOrder() >= _minOrder)
            {
                Window.Title = "Clear";
                _color = Colors.LightGoldenrodYellow;
                _nextLevelAlarm = Alarm.Start(60, _gotoNext);
            }
        }

        private void _gotoNext()
        {
            _puzzleIndex++;
            if (_puzzleIndex >= Puzzles.Length)
                Giraffe.Quit();
            else
                Goto(Puzzles[_puzzleIndex]);
        }

        private static int _puzzleIndex = 0;
        private static Tool[] basicTools = new[] { Tool.Pointer, Tool.Vertex, Tool.Delete };

        public static PuzzleRoom[] Puzzles = new[] {



            // Electrons
            new PuzzleRoom("Electron-positron scattering", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Electron },
                               new[] { Tool.Photon }
                           }),

            new PuzzleRoom("Electron in a field", 2,
                           new[] { ParticleType.Electron, ParticleType.Photon },
                           new[] { ParticleType.Electron, ParticleType.Photon },
                           new[] { basicTools, new[] { Tool.Electron }, new[] { Tool.Photon }}),

            new PuzzleRoom("Electron-Positron annihilation", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.Photon, ParticleType.Photon },
                           new[] { basicTools, new[] { Tool.Electron }, new[] { Tool.Photon }}),


            // Muons
            new PuzzleRoom("Muon pair production", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.Muon, ParticleType.AntiMuon },
                           new[] { basicTools, new[] { Tool.Electron, Tool.Muon }, new[] { Tool.Photon }}),

            new PuzzleRoom("Photon-Photon scattering", 4,
                           new[] { ParticleType.Photon, ParticleType.Photon },
                           new[] { ParticleType.Photon, ParticleType.Photon },
                           new[] { basicTools, new[] { Tool.Electron, Tool.Muon }, new[] { Tool.Photon }}),

            new PuzzleRoom("Electron-Positron pair production", 3,
                           new[] { ParticleType.Muon, ParticleType.Photon },
                           new[] { ParticleType.Muon, ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { basicTools, new[] { Tool.Electron, Tool.Muon }, new[] { Tool.Photon }}),


            // Exotic diagrams
            new PuzzleRoom("Vacuum fluctuations", 2,
                           new ParticleType[0],
                           new ParticleType[0],
                           new[] { basicTools, new[] { Tool.Electron, Tool.Muon }, new[] { Tool.Photon }}),

            new PuzzleRoom("Tadpole", 1,
                           new[] { ParticleType.Photon },
                           new ParticleType[0],
                           new[] { basicTools, new[] { Tool.Electron, Tool.Muon }, new[] { Tool.Photon }}),


            // Quarks
            new PuzzleRoom("Quark annihilation", 0,
                           new[] { ParticleType.UpR, ParticleType.AntiUpR },
                           new[] { ParticleType.DownB, ParticleType.AntiDownB },
                           new[] { basicTools, new[] { Tool.Up(R) }, new[] { Tool.Down(B) }, new[] { Tool.Photon } }),

            // Gluons
            new PuzzleRoom("Gluons", 0,
                           new[] { ParticleType.UpR, ParticleType.AntiUpB },
                           new[] { ParticleType.DownR, ParticleType.AntiDownB },
                           new Tool[][] {
                                basicTools,
                                new[] { Tool.Up(R), Tool.Up(B) },
                                new[] { Tool.Down(R), Tool.Down(B) },
                                new[] { Tool.Photon, Tool.Gluon }
                           }),

            new PuzzleRoom("Gluon emission", 0,
                           new[] { ParticleType.DownR },
                           new[] { ParticleType.DownB, ParticleType.UpR, ParticleType.AntiUpB },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon }
                           }),

            new PuzzleRoom("Color conservation", 0,
                           new[] { ParticleType.DownG },
                           new[] { ParticleType.DownG, ParticleType.UpG, ParticleType.AntiUpG },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon }
                           }),

            new PuzzleRoom("Neutral pion generation", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.UpR, ParticleType.AntiUpR, ParticleType.DownB, ParticleType.AntiDownB },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon }
                           }),

            new PuzzleRoom("Charged pion generation", 0,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.UpR, ParticleType.AntiDownR, ParticleType.DownB, ParticleType.AntiUpB },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon }
                           }),

            new PuzzleRoom("Gluon three-vertex", 0,
                           new[] { ParticleType.Gluon },
                           new[] { ParticleType.UpR, ParticleType.AntiUpG, ParticleType.DownG, ParticleType.AntiDownR },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon }
                           }),


            // Weak force
            new PuzzleRoom("Weak Annihilation", 0,
                           new[] { ParticleType.AntiElectron, ParticleType.ENeutrino},
                           new[] { ParticleType.Muon, ParticleType.AntiMNeutrino },
                           new Tool[][]
                           {
                               basicTools,
                               new[] { Tool.Electron, Tool.Muon },
                               new[] { Tool.ENeutrino, Tool.MNeutrino },
                               new[] { Tool.WBoson }
                           }),

            new PuzzleRoom("Weak Scattering", 0,
                           new[] { ParticleType.Muon, ParticleType.ENeutrino},
                           new[] { ParticleType.Electron, ParticleType.MNeutrino },
                           new Tool[][]
                           {
                               basicTools,
                               new[] { Tool.Electron, Tool.Muon },
                               new[] { Tool.ENeutrino, Tool.MNeutrino },
                               new[] { Tool.WBoson }
                           }),

            new PuzzleRoom("Neuton decay", 0,
                           new[] { ParticleType.DownR },
                           new[] { ParticleType.UpR, ParticleType.Electron, ParticleType.AntiENeutrino },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Electron, Tool.Muon },
                               new[] { Tool.ENeutrino, Tool.MNeutrino },
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon, Tool.WBoson }
                           }),


            new PuzzleRoom("Penguin diagram", 0,
                           new[] { ParticleType.ENeutrino },
                           new[] { ParticleType.ENeutrino, ParticleType.UpR, ParticleType.AntiUpR },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Electron, Tool.Muon },
                               new[] { Tool.ENeutrino, Tool.MNeutrino },
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon, Tool.WBoson }
                           }),


            new PuzzleRoom("Crossing", 0,
                           new[] { ParticleType.UpR, ParticleType.UpR },
                           new[] { ParticleType.TopR, ParticleType.TopR },
                           new Tool[][] {
                               basicTools,
                               new[] { Tool.Electron, Tool.Muon },
                               new[] { Tool.ENeutrino, Tool.MNeutrino },
                               new[] { Tool.Up(R), Tool.Up(G), Tool.Up(B) },
                               new[] { Tool.Down(R), Tool.Down(G), Tool.Down(B) },
                               new[] { Tool.Photon, Tool.Gluon, Tool.WBoson }
                           }),



        };


    }

}
