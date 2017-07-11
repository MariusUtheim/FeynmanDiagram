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
            : base(1024, 768)
        {
            _name = name;
            _minOrder = minOrder;

            _task = $"{String.Join(" + ", input.Select(p => p.Symbol))} -> {String.Join(" + ", output.Select(p => p.Symbol))}";
            _input = input.OrderBy(p => p).ToList();
            _output = output.OrderBy(p => p).ToList();
        }

        public PuzzleRoom(string name, int minOrder, IEnumerable<ParticleType> input, IEnumerable<ParticleType> output, params Tool[] availableTools)
            : this(name, minOrder, input, output)
        {
            _toolbar = new Toolbar(availableTools);
		}

        public PuzzleRoom(string name, int minOrder, IEnumerable<ParticleType> input, IEnumerable<ParticleType> output, Tool[,] availableTools)
            : this(name, minOrder, input, output)
        {
            _toolbar = new Toolbar(availableTools);
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
        public static PuzzleRoom[] Puzzles = new[] {



            // Electrons
            new PuzzleRoom("Two ways", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new Tool[,] {
                               { Tool.Pointer, Tool.Vertex, Tool.Delete },
                               { Tool.Electron, null, null },
                               { Tool.Photon, null, null }
                           }),

            new PuzzleRoom("Electron in a field", 2,
                           new[] { ParticleType.Electron, ParticleType.Photon },
                           new[] { ParticleType.Electron, ParticleType.Photon },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Photon),

            new PuzzleRoom("Electron-Positron annihilation", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.Photon, ParticleType.Photon },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Photon),


            // Muons
            new PuzzleRoom("Muon pair production", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.Muon, ParticleType.AntiMuon },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Muon, Tool.Photon),

			new PuzzleRoom("Photon-Photon scattering", 4,
						   new[] { ParticleType.Photon, ParticleType.Photon },
						   new[] { ParticleType.Photon, ParticleType.Photon },
						   Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Muon, Tool.Photon),

			new PuzzleRoom("Electron-Positron pair production", 3,
                           new[] { ParticleType.Muon, ParticleType.Photon },
                           new[] { ParticleType.Muon, ParticleType.Electron, ParticleType.AntiElectron },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Muon, Tool.Photon),


            // Exotic diagrams
            new PuzzleRoom("Vacuum fluctuations", 2,
                           new ParticleType[0],
                           new ParticleType[0],
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Muon, Tool.Photon),

            new PuzzleRoom("Tadpole", 1,
                           new[] { ParticleType.Photon },
                           new ParticleType[0],
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.Muon, Tool.Photon),


            // Quarks
            new PuzzleRoom("Quark annihilation", 0,
                           new[] { ParticleType.UpR, ParticleType.AntiUpR },
                           new[] { ParticleType.DownB, ParticleType.AntiDownB },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Up(R), Tool.Down(B), Tool.Photon),

            // Gluons
            new PuzzleRoom("Gluons", 0,
                           new[] { ParticleType.UpR, ParticleType.AntiUpB },
                           new[] { ParticleType.DownR, ParticleType.AntiDownB },
                           new Tool[,] {
                { Tool.Pointer, Tool.Vertex, Tool.Delete },
                { Tool.Up(R), Tool.Up(B), null },
                { Tool.Down(R), Tool.Down(B), null },
                { Tool.Photon, Tool.Gluon, null }
            }),


            new PuzzleRoom("Down quark decay", 0,
                           new[] { ParticleType.DownR },
                           new[] { ParticleType.DownB, ParticleType.UpR, ParticleType.AntiUpB },
                           Tool.Pointer, Tool.Vertex, Tool.Delete,
                           Tool.Up(R), Tool.Up(B), Tool.Down(R), Tool.Down(B), Tool.Photon, Tool.Gluon),

            new PuzzleRoom("Neutral pion generation", 2,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.UpR, ParticleType.AntiUpR, ParticleType.DownB, ParticleType.AntiDownB },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.ENeutrino,
                           Tool.Up(R), Tool.Down(B), Tool.Photon, Tool.Gluon),

            new PuzzleRoom("Charged pion generation", 0,
                           new[] { ParticleType.Electron, ParticleType.AntiElectron },
                           new[] { ParticleType.UpR, ParticleType.AntiDownR, ParticleType.DownB, ParticleType.AntiUpB },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.ENeutrino,
                           Tool.Up(R), Tool.Up(B), Tool.Down(B), Tool.Down(B),
                           Tool.Photon, Tool.Gluon),

            new PuzzleRoom("Gluon three-vertex", 0,
                           new[] { ParticleType.Gluon },
                           new[] { ParticleType.UpR, ParticleType.AntiUpG, ParticleType.DownG, ParticleType.AntiDownR },
						   Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.ENeutrino,
						   Tool.Up(R), Tool.Up(B), Tool.Down(B), Tool.Down(B),
						   Tool.Photon, Tool.Gluon),


            // Weak force
            new PuzzleRoom("Neuton decay", 0,
                           new[] { ParticleType.DownR },
                           new[] { ParticleType.UpR, ParticleType.Electron, ParticleType.AntiENeutrino },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.ENeutrino, Tool.Up(ColorCharge.R), Tool.Down(ColorCharge.R), Tool.Photon, Tool.Gluon, Tool.WBoson),


            new PuzzleRoom("Penguin diagram", 0,
                           new[] { ParticleType.ENeutrino },
                           new[] { ParticleType.ENeutrino, ParticleType.UpR, ParticleType.AntiUpR },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Electron, Tool.ENeutrino, Tool.Up(ColorCharge.R), Tool.Photon, Tool.Gluon, Tool.WBoson),


            new PuzzleRoom("Crossing", 0,
                           new[] { ParticleType.UpR, ParticleType.UpR },
                           new[] { ParticleType.TopR, ParticleType.TopR },
                           Tool.Pointer, Tool.Vertex, Tool.Delete, Tool.Up(R), Tool.Down(R), Tool.Top(R), Tool.Bottom(R), Tool.Photon, Tool.Gluon, Tool.WBoson),



        };


    }

}
