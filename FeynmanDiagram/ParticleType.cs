using System;
using System.Collections.Generic;
using System.Linq;
using GRaff;

namespace FeynmanDiagram
{

    public class ParticleType : IComparable<ParticleType>
    {
        private ParticleType(ParticleClass particleClass, string name, string symbol, int charge, ColorCharge colorCharge)
        {
            this.Class = particleClass;
            this.Name = name;
            this.Symbol = symbol;
            this.Charge = charge;
            this.ColorCharge = colorCharge;
            this.Antiparticle = this;
            this.Complementary = null;
        }


        public ParticleClass Class { get; }

        public string Name { get; }
        public string Symbol { get; }
        public int Charge { get; }
        public ColorCharge ColorCharge { get; }
		
        public ParticleType Antiparticle { get; private set; }

        public ParticleType Complementary { get; private set; }

        public double CouplingStrength(ParticleType other, ParticleType forceParticle)
        {
            if (forceParticle == Photon)
                return 30 / GMath.Abs(Charge);
            else if (forceParticle == Gluon)
                return (ColorCharge == ColorCharge.Neutral || other.ColorCharge == ColorCharge.Neutral || ColorCharge == other.ColorCharge) ? 0 : 5;
            else if (forceParticle == WPlus)
                return (this == other.Complementary) ? 50 : (this.Class == other.Class && (GMath.Abs(this.Charge) + GMath.Abs(other.Charge) == 3)) ? 100 : 0;
            else
                return 0;//throw new NotSupportedException();// if (forceParticle == ParticleType.
        }


        //public static ParticleType Fermion { get; } = new ParticleType(ParticleClass.Fermion, "Fermion", "", 1, ColorCharge.Neutral);
        

        private static ParticleType _Lepton(string name, string symbol, int charge) => new ParticleType(ParticleClass.Lepton, name, symbol, charge, ColorCharge.Neutral);
        private static ParticleType _Quark(string name, int charge, ColorCharge color) => new ParticleType(ParticleClass.Quark, name, name[0].ToString(), charge, color);

        private ParticleType _Antiparticle(string name = null)
        {
            var c = (int)ColorCharge;
            c = ((c & 0b111) << 3) | ((c & 0b111000) >> 3);
            Antiparticle = new ParticleType(Class, name ?? "anti" + Name, Symbol + "*", -Charge, (ColorCharge)c);
            Antiparticle.Antiparticle = this;
            return Antiparticle;
        }

        private ParticleType _Complementary(string name, string symbol)
        {
            var c = new ParticleType(Class, name, symbol, Charge >= 0 ? Charge - 3 : Charge + 3, ColorCharge);
            Complementary = c;
            c.Complementary = this;
            return c;
        }


        public static ParticleType Electron { get; } = _Lepton("electron", "e", -3);
		public static ParticleType AntiElectron { get; } = Electron._Antiparticle("positron");
        public static ParticleType ENeutrino { get; } = Electron._Complementary("electron neutrino", "νe");
		public static ParticleType AntiENeutrino { get; } = ENeutrino._Antiparticle("electron antineutrino");

		public static ParticleType Muon { get; } = _Lepton("muon", "µ", -3);
        public static ParticleType AntiMuon { get; } = Muon._Antiparticle();
        public static ParticleType MNeutrino { get; } = Muon._Complementary("muon neutrino", "νµ");
        public static ParticleType AntiMNeutrino { get; } = MNeutrino._Antiparticle("muon antineutrino");

        public static ParticleType UpR { get; } = _Quark("up", 2, ColorCharge.R);
        public static ParticleType UpG { get; } = _Quark("up", 2, ColorCharge.G);
        public static ParticleType UpB { get; } = _Quark("up", 2, ColorCharge.B);
        public static ParticleType Up(ColorCharge color)
            => color == ColorCharge.R ? UpR : color == ColorCharge.G ? UpG : color == ColorCharge.B ? UpB : throw new NotSupportedException();
        public static ParticleType AntiUpR { get; } = UpR._Antiparticle();
        public static ParticleType AntiUpG { get; } = UpG._Antiparticle();
        public static ParticleType AntiUpB { get; } = UpB._Antiparticle();


        public static ParticleType DownR { get; } = UpR._Complementary("down", "d");
		public static ParticleType DownG { get; } = UpG._Complementary("down", "d");
		public static ParticleType DownB { get; } = UpB._Complementary("down", "d");
		public static ParticleType Down(ColorCharge color)
			=> color == ColorCharge.R ? DownR : color == ColorCharge.G ? DownG : color == ColorCharge.B ? DownB : throw new NotSupportedException();
        public static ParticleType AntiDownR { get; } = DownR._Antiparticle();
        public static ParticleType AntiDownG { get; } = DownG._Antiparticle();
        public static ParticleType AntiDownB { get; } = DownB._Antiparticle();

		public static ParticleType TopR { get; } = _Quark("top", 2, ColorCharge.R);
		public static ParticleType TopG { get; } = _Quark("top", 2, ColorCharge.G);
		public static ParticleType TopB { get; } = _Quark("top", 2, ColorCharge.B);
		public static ParticleType Top(ColorCharge color)
			=> color == ColorCharge.R ? TopR : color == ColorCharge.G ? TopG : color == ColorCharge.B ? TopB : throw new NotSupportedException();
		public static ParticleType AntiTopR { get; } = TopR._Antiparticle();
		public static ParticleType AntiTopG { get; } = TopG._Antiparticle();
		public static ParticleType AntiTopB { get; } = TopB._Antiparticle();

		public static ParticleType BottomR { get; } = TopR._Complementary("bottom", "b");
		public static ParticleType BottomG { get; } = TopG._Complementary("bottom", "b");
		public static ParticleType BottomB { get; } = TopB._Complementary("bottom", "b");
		public static ParticleType Bottom(ColorCharge color)
			=> color == ColorCharge.R ? BottomR : color == ColorCharge.G ? BottomG : color == ColorCharge.B ? BottomB : throw new NotSupportedException();
		public static ParticleType AntiBottomR { get; } = BottomR._Antiparticle();
		public static ParticleType AntiBottomG { get; } = BottomG._Antiparticle();
		public static ParticleType AntiBottomB { get; } = BottomB._Antiparticle();

        public int CompareTo(ParticleType other) => this.Name.CompareTo(other.Name);


        public static ParticleType Photon { get; } = new ParticleType(ParticleClass.ZPhoton, "Photon", "γ", 0, ColorCharge.Neutral);
        public static ParticleType Gluon { get; } = new ParticleType(ParticleClass.Gluon, "Gluon", "g", 0, ColorCharge.Neutral);

        public static ParticleType WPlus { get; } = new ParticleType(ParticleClass.WBoson, "W+", "W+", 1, ColorCharge.Neutral);
        public static ParticleType WMinus { get; } = WPlus._Antiparticle("W-");


        public override string ToString()
        {
            return string.Format("[ParticleType: {1}{2}]", Name, ColorCharge == ColorCharge.Neutral ? "" : $"({ColorCharge})");
        }
	}

    [Flags]
    public enum ColorCharge
    {
        Neutral = 0,
        R = 0b000001, G = 0b000010, B = 0b000100, r = 0b001000, g = 0b010000, b = 0b100000,
        Rr = R | r, Rg = R | g, Rb = R | b, Gr = G | r, Gg = G | g, Gb = G | b, Br = B | r, Bg = B | g, Bb = B | b
    }

    public enum ParticleClass
	{
		Quark = 0x101, Lepton = 0x102, ZPhoton = 0x200, WBoson = 0x300, Gluon = 0x400, Higgs = 0x500
	}


    public static class ParticleTypeExtensions
    {
        public static Color GetColor(this ColorCharge charge)
        {
            switch (charge)
            {
                case ColorCharge.Neutral: return Color.Rgb(0, 0, 0);
                case ColorCharge.R: return Color.Rgb(255, 0, 0);
                case ColorCharge.G: return Color.Rgb(0, 255, 0);
                case ColorCharge.B: return Color.Rgb(0, 0, 255);
                default: return Color.Rgb(0, 0, 0);
            }
        }


        public static bool IsFermion(this ParticleClass particleClass)
            => particleClass == ParticleClass.Lepton || particleClass == ParticleClass.Quark;
    }
}
