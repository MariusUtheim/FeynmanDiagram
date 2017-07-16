using System;
using System.Collections.Generic;
using GRaff;
using GRaff.Graphics;
using GRaff.Graphics.Text;

namespace FeynmanDiagram
{
    public static class Assets
	{
	    public static readonly ISet<char> ParticleSymbols = new HashSet<char>("eµνudtbγZgW+-H*-> ".ToCharArray());

        public static Font ToolbarFont { get; private set; }

		public static Font EdgeLabelFont { get; private set; }
		public static Font VertexLabelFont { get; private set; }
        public static Font TaskFont { get; private set; }

        public static TextRenderer TutorialTextRenderer { get; private set; }


       // public static Texture StraightEdgeTexture { get; private set; }
        public static Sprite FermionArrowheadSprite { get; private set; }

        public static Sprite ToolBackgroundSprite { get; private set; }

        public static Dictionary<ParticleClass, Sprite> EdgeSprites { get; private set; }


        public static void LoadAll()
        {
            ToolbarFont = Font.LoadTrueType("Arial", 12, Font.ASCIICharacters, FontOptions.Bold);
            EdgeLabelFont = Font.LoadTrueTypeFromFile("Assets/cambriai.ttf", 18, ParticleSymbols);
            VertexLabelFont = Font.LoadTrueTypeFromFile("Assets/cambriai.ttf", 32, ParticleSymbols);
            TaskFont = Font.LoadTrueTypeFromFile("Assets/cambriai.ttf", 36, ParticleSymbols);

            TutorialTextRenderer = new TextRenderer(ToolbarFont, FontAlignment.TopLeft, 900);

            FermionArrowheadSprite = Sprite.Load("Assets/FermionArrowhead.png");

            ToolBackgroundSprite = Sprite.Load("Assets/ToolBackground.png", imageCount: 3, origin: (0, 0));

            int w = 155, h = 25;
            var sineData = new Color[h + 1, w];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                    sineData[y, x] = Colors.White.Transparent(1 - GMath.Abs(y - h / 2  + (h / 2 - 1) * GMath.Sin(2 * x * GMath.Tau / w)) / 2.0);
            }
            var texture = new Texture(sineData);

            var fermionSprite = Sprite.Load("Assets/StraightEdge.png");
            EdgeSprites = new Dictionary<ParticleClass, Sprite>
            {
                { ParticleClass.Quark, fermionSprite },
                { ParticleClass.Lepton, fermionSprite },
                { ParticleClass.ZPhoton, new Sprite(texture.SubTexture()) },
                { ParticleClass.Gluon, Sprite.Load("Assets/GluonEdge.png") },
                { ParticleClass.WBoson, new Sprite(texture.SubTexture()) },
                { ParticleClass.Higgs, fermionSprite },
            };
        }


    }
}
