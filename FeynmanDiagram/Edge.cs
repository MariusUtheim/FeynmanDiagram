using System;
using System.Collections.Generic;
using GRaff;
using GRaff.Graphics;
using GRaff.Graphics.Text;
using GRaff.Panels;

namespace FeynmanDiagram
{
    public class Edge : DraggableNode, IPanelMousePressListener
    {
        public const double Radius = 5;
        public const double EdgeHeight = 8;

        public Edge(Vertex from, Vertex to, ParticleType type)
        {
		//	if (type.IsAntiParticle())
		///	{
		//		(from, to) = (to, from);
		//		type = type.AntiParticle();
		//	}
			
            this.From = from;
            From.AddEdge(this);
            this.To = to;
            To.AddEdge(this);
            this.ParticleType = type;
        }

        public override Rectangle Region
        {
            get
            {
                double x = GMath.Min(From.Center.X, To.Center.X), y = GMath.Min(From.Center.Y, To.Center.Y);
                double w = GMath.Abs(From.Center.X - To.Center.X), h = GMath.Abs(From.Center.Y - To.Center.Y);
                if (w < Radius)
                {
                    x -= GMath.Abs(w - Radius) / 2.0;
                    w = Radius;
                }
                if (h < Radius)
                {
                    y -= GMath.Abs(h - Radius) / 2.0;
                    h = Radius;
                }
                return new Rectangle(x, y, w, h);
            }
            set => throw new NotSupportedException();
        }

        public Line Line => new Line(From.Center, To.Center);

		private Polygon _mask =>
			 Matrix
				.Scaling((To.Center - From.Center).Magnitude, EdgeHeight)
                .Rotate((To.Center - From.Center).Direction)
                .Translate((From.Center.X + To.Center.X) / 2, (From.Center.Y + To.Center.Y) / 2) * Polygon.Rectangle(1, 1);

        public Vertex From { get; }

        public Vertex To { get; }

        public ParticleClass EdgeType => ParticleType.Class;

        public ParticleType ParticleType { get; }

        public Vertex OtherEnd(Vertex oneEnd)
        {
            if (From == oneEnd)
                return To;
            else if (To == oneEnd)
                return From;
            else
                throw new InvalidOperationException();
        }

        public bool ConnectsTo(Edge next)
        {
            if (EdgeType.IsFermion())
            {
                if (!(From == next.To || To == next.From))
                    return false;

                var force = ((From == next.To) ? From : To).Decompose()?.f;

                if (force == null)
                    return this.ParticleType.Name == next.ParticleType.Name || this.ParticleType.Complementary == next.ParticleType;
                else if (force == ParticleType.Photon)
                    return this.ParticleType.Name == next.ParticleType.Name && this.ParticleType.ColorCharge == next.ParticleType.ColorCharge;
                else if (force == ParticleType.Gluon)
                    return this.ParticleType.Name == next.ParticleType.Name && this.ParticleType.ColorCharge != next.ParticleType.ColorCharge;
                else if (force == ParticleType.WPlus || force == ParticleType.WMinus)
                    return this.ParticleType.Complementary == next.ParticleType;
                else
                    throw new NotSupportedException();
            }
            else
                return EdgeType == next.EdgeType;
        }

        public void Branch(ParticleType type)
        {
            var loc = ToParent(Mouse.Location);
            var newSourceVertex = Parent.AddChildLast(new Vertex(loc.Project(new Line(From.Center, To.Center))));
            Parent.AddChildFirst(new Edge(From, newSourceVertex, ParticleType));
            Parent.AddChildFirst(new Edge(newSourceVertex, To, ParticleType));

            var newDestVertex = Parent.AddChildLast(new Vertex(loc));
            Parent.AddChildFirst(new Edge(newSourceVertex, newDestVertex, type));
            newDestVertex.Drag(loc);

            this.Parent = null;
        }

        public void Transfer(Vertex src, Vertex dst)
        {
            if (src == this.From)
                Parent.AddChildFirst(new Edge(dst, this.To, ParticleType));
            else if (src == this.To)
                Parent.AddChildFirst(new Edge(this.From, dst, ParticleType));
            else
                throw new InvalidOperationException();
            Parent = null;
        }

        public void OnMousePress(MouseEventArgs e)
        {
            if (_mask.ContainsPoint(e.Location))
                Toolbar.Current.Click(this);
            else
                e.Propagate();
        }

        protected override void OnDrag(Point location)
        {
            if (((Vector)location).Magnitude > 10)
            {
                var loc = ToParent(Mouse.Location);
                var newVertex = Parent.AddChildLast(new Vertex(loc));
                Parent.AddChildFirst(new Edge(this.From, newVertex, ParticleType));
                Parent.AddChildFirst(new Edge(newVertex, this.To, ParticleType));
                Parent = null;
				newVertex.Drag(loc);
            }
        }

        protected override void OnDrop()
        {
            Location = (0, 0);
        }

        public override void OnDraw()
        {
            var t = Matrix.Rotation((To.Center - From.Center).Direction)
                          .Translate((From.Center + To.Center).X / 2, (From.Center + To.Center).Y / 2);

            Color color;
            if (ParticleType == ParticleType.Gluon)
            {
                var ins = From.Decompose();
                var outs = To.Decompose();
                if (ins == null || outs == null)
                    color = Colors.Black;
                else if (ins.Value.i.ColorCharge == ins.Value.o.ColorCharge && outs.Value.i.ColorCharge == outs.Value.o.ColorCharge)
                    color = Colors.Black;
                else if (ins.Value.i.ColorCharge != outs.Value.o.ColorCharge || ins.Value.i.ColorCharge != outs.Value.o.ColorCharge)
                    color = Colors.Black;
                else
                {
                    var p = ins.Value.i.ColorCharge.GetColor();
                    var a = ins.Value.o.ColorCharge.GetColor();
                    color = Color.Rgb(p.R + a.R, p.G + a.G, p.B + a.B);
                }
            }
            else
                color = ParticleType.ColorCharge.GetColor();

            if (To.Center != From.Center)
            {
                var w = Matrix.Scaling((To.Center - From.Center).Magnitude / Assets.EdgeSprites[EdgeType].Width, 1);// / ;
                //var tw = 2 * w / Assets.EdgeSprites[EdgeType].Width;
                //var h = Assets.EdgeSprites[EdgeType].Height / 2;
                //var t2 = t * Matrix.Scaling(w, 1);
                //var polygon = new Polygon(new[] { t * new Point(-w, -5), t * new Point(w, -5), t * new Point(w, 5), t * new Point(-w, 5) });
                //   Draw.Polygon(polygon, Colors.Aqua);

                //Draw.Primitive(PrimitiveType.TriangleStrip, Assets.EdgeSprites[EdgeType].SubImage(0).Texture, 
                              // new (Point, Point)[] {
                              //      (t * new Point(-w, -h), (0, 1)),
                              //      (t * new Point(w, -h), (tw, 1)),
                              //      (t * new Point(-w, h), (0, 0)),
                              //      (t * new Point(w, h), (tw, 0)),
                              //  }, color
                              //);

                Draw.Sprite(Assets.EdgeSprites[EdgeType], 0, t * w, color);
            }

            if (EdgeType.IsFermion())
                Draw.Sprite(Assets.FermionArrowheadSprite, 0, t, color);

#warning Position the symbol more nicely
            Draw.Text(ParticleType.Symbol, Assets.EdgeLabelFont, FontAlignment.Center, t * new Point(0, 15), color);

            //Draw.Polygon(_mask, Colors.Aqua);
            //Draw.Rectangle(Region, Colors.Aqua);
        }

		public override void OnParentChanging(Node parent)
        {
            if (this.Parent != null && parent == null)
            {
                From.RemoveEdge(this);
                To.RemoveEdge(this);
            }
            else if (this.Parent != null)
                throw new InvalidOperationException("Edges cannot change parents once created");
		}

        public override string ToString()
        {
            return string.Format("[Edge: ParticleType={1}]", ParticleType);
        }
    }
}
