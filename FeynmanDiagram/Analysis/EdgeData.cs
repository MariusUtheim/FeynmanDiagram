using System;
namespace FeynmanDiagram.Analysis
{
    public class EdgeData
    {

        public EdgeData(int from, int to, ParticleClass type)
        {
            this.From = from;
            this.To = to;
            this.Type = type;
        }

        public int From { get; }

        public int To { get; }

        public ParticleClass Type { get; }


        public char ToSymbol()
        {
            switch (Type)
            {
                case ParticleClass.Quark:
                case ParticleClass.Lepton:
                    return '>';
                case ParticleClass.ZPhoton: return '~';
                case ParticleClass.Gluon: return '@';
                case ParticleClass.WBoson: return '-';
                case ParticleClass.Higgs: return ':';
                default: throw new NotSupportedException("Invalid enum: " + Type.ToString());
            }
        }
    }
}
