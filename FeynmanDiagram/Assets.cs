using System;
using System.Collections.Generic;
using GRaff;
using GRaff.Graphics;
using GRaff.Graphics.Text;
using GRaff.Effects;

namespace FeynmanDiagram
{
    public static class Assets
	{
	    public static readonly ISet<char> ParticleSymbols = new HashSet<char>("RGBrgb() eµνudtbγZgW+-H*->".ToCharArray());

        public static Font ToolbarFont { get; private set; }

		public static Font EdgeLabelFont { get; private set; }
		public static Font VertexLabelFont { get; private set; }
        public static Font TaskFont { get; private set; }

        public static TextRenderer TutorialTextRenderer { get; private set; }


       // public static Texture StraightEdgeTexture { get; private set; }
        public static Sprite FermionArrowheadSprite { get; private set; }

        public static Sprite ToolBackgroundSprite { get; private set; }

        public static Sprite PointerSprite { get; private set; }
        public static Sprite DeleteSprite { get; private set; }

        public static Dictionary<ParticleClass, Sprite> EdgeSprites { get; private set; }


        public static void LoadAll()
        {
            ToolbarFont = Font.LoadTrueType("Arial", 12, Font.ASCIICharacters, FontOptions.Bold);
            EdgeLabelFont = Font.LoadTrueTypeFromFile("Assets/cambriai.ttf", 18, ParticleSymbols);
            VertexLabelFont = Font.LoadTrueTypeFromFile("Assets/cambriai.ttf", 32, ParticleSymbols);
            TaskFont = Font.LoadTrueTypeFromFile("Assets/cambriai.ttf", 36, ParticleSymbols);
            
            TutorialTextRenderer = new TextRenderer(ToolbarFont, FontAlignment.TopLeft, 900);

            PointerSprite = Sprite.Load("Assets/Pointer.png");
            DeleteSprite = Sprite.Load("Assets/Delete.png");
            FermionArrowheadSprite = Sprite.Load("Assets/FermionArrowhead.png");

            ToolBackgroundSprite = Sprite.Load("Assets/ToolBackground.png", imageCount: 3, origin: (0, 0));

            var fermionSprite = Sprite.Load("Assets/StraightEdge.png");
            EdgeSprites = new Dictionary<ParticleClass, Sprite>
            {
                { ParticleClass.Quark, fermionSprite },
                { ParticleClass.Lepton, fermionSprite },
                { ParticleClass.ZPhoton, _genPhotonSprite() },
                { ParticleClass.Gluon, Sprite.Load("Assets/GluonEdge.png") },
                { ParticleClass.WBoson, _genWBosonSprite() },
                { ParticleClass.Higgs, _genHiggsSprite() },
            };
        }

        private const double Thickness = 5.0;
        private static Sprite _generateSprite(IntVector size, Func<double, double> wave, double waveNumber)
        {
            var data = new Color[size.Y, size.X];
            for (var x = 0; x < size.X; x++)
            {
                var y0 = (size.Y / 2.0 + 0.5 * (size.Y - Thickness - 1) * wave(x * waveNumber / size.X));
                for (int y = 0; y < size.Y; y++)
                    data[y, x] = Colors.White.Transparent(GMath.Exp(-GMath.Sqr(y - y0)));
            }

            var texture = new Texture(data);
            return new Sprite(texture.SubTexture());
        }


        private static Sprite _genPhotonSprite()
        {
            return _generateSprite((600, 35), WaveGenerator.Sine(1), 2);
        }

        private static Sprite _genWBosonSprite()
        {
            return _generateSprite((300, 30), WaveGenerator.Triangle(1), 5);
        }

        private static Sprite _genHiggsSprite()
        {
            return _generateSprite((300, 10), WaveGenerator.Binary(1, 100), 10);
        }
    }
}
