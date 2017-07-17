using System;
using System.Collections.Generic;
using GRaff;
using GRaff.Graphics;
using GRaff.Graphics.Text;

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


        private static Sprite _genPhotonSprite()
        {
            int w = 155, h = 25;
            var sineData = new Color[h + 1, w];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                    sineData[y, x] = Colors.White.Transparent(1 - GMath.Abs(y - h / 2 + (h / 2 - 1) * GMath.Sin(2 * x * GMath.Tau / w)) / 2.0);
            }
            var sineTexture = new Texture(sineData);

            return new Sprite(sineTexture.SubTexture());
        }

        private static Sprite _genWBosonSprite()
        {
            var h = 25;
            var dx = h - 3;
            var w = dx * 9;
            var wData = new Color[h + 1, w];
            for (var x = 0; x < w; x++)
            {
                var y0 = GMath.Abs(GMath.Remainder(x + dx/2, 2 * dx) - dx) + 1;
                for (var y = 0; y < h; y++)
                    wData[y, x] = Colors.White.Transparent(GMath.Exp(-GMath.Sqr(y - y0)));
            }
            var wTexture = new Texture(wData);

            return new Sprite(wTexture.SubTexture());
        }

        private static Sprite _genHiggsSprite()
        {
            var h = 15;
            var dx = 15;
            var w = dx * 15;
            var wData = new Color[h + 1, w];
            for (var x = 0; x < w; x++)
            {
                if (x % (2 * dx) >= dx)
                    continue;
                for (var y = 0; y < h; y++)
                    wData[y, x] = Colors.White.Transparent(GMath.Exp(-GMath.Sqr(4 * (y - h/2))));
            }
            var wTexture = new Texture(wData);

            return new Sprite(wTexture.SubTexture());
        }
    }
}
