using System;
using System.Collections.Generic;
using System.Linq;
using GRaff;
using GRaff.Graphics.Text;
using GRaff.Panels;

namespace FeynmanDiagram
{
    public class Vertex : DraggableNode, IPanelMousePressListener
	{
		public const double Radius = 12;
        private static readonly Vector CenterOffset = (Radius, Radius);
		private List<Edge> _edges = new List<Edge>();

	
        public Vertex(Point location) 
        {
            Center = location;
        }

        public Vertex(double x, double y)
            : this((x, y)) { }

        public IEnumerable<Edge> Edges => _edges.ToList();

        public Point Center { get; set; }

        public override Rectangle Region
        {
            get => new Rectangle(Center - CenterOffset, 2 * CenterOffset);
            set => throw new NotSupportedException();
        }


		public void AddEdge(Edge edge)
		{
			_edges.Add(edge);
		}

		public void RemoveEdge(Edge edge)
		{
			_edges.Remove(edge);
		}

		public Edge GetEdge(Vertex other)
		{
            if (other == this)
                return null;
			var result = _edges.SingleOrDefault(e => e.From == other || e.To == other);
			return result;
		}


		public override void OnDraw()
        {
            Color color;
            if (!IsValid)
                color = Colors.Red;
            else
                color = Colors.Black;

            if ((ToParent((Point)Mouse.WindowLocation) - Center).Magnitude <= Radius)
                Draw.FillCircle(Center, Radius, color, color.Transparent(0.1));
            else
                Draw.FillCircle(Center, Radius, color, color.Transparent(0));


            if (IsLeaf)
            {
                var textColor = GetEndpointType().ColorCharge.GetColor();
                if (IsInput)
                    Draw.Text(GetEndpointType().Symbol, Assets.VertexLabelFont, FontAlignment.Right, (Center.X - 1.5 * Radius, Center.Y), textColor);
                else if (IsOutput)
                    Draw.Text(GetEndpointType().Symbol, Assets.VertexLabelFont, FontAlignment.Left, (Center.X + 1.5 * Radius, Center.Y), textColor);
            }
        }

		public override void OnParentChanging(Node parent)
        {
            if (_edges.Count == 2 && _edges[0].ConnectsTo(_edges[1]))
                _edges[0].Transfer(this, _edges[1].OtherEnd(this));
            

            while (_edges.Any())
                _edges[_edges.Count - 1].Parent = null;
		}

        public void OnMousePress(MouseEventArgs e)
		{
            if ((e.Location - Center).Magnitude <= Radius)
                Toolbar.Current.Click(this);
            else
                e.Propagate();
		}

		internal void MakeVertex(ParticleType type)
		{
            var newVertex = Parent.AddChildLast(new Vertex(ToClient(Mouse.Location)));
            Parent.AddChildFirst(new Edge(this, newVertex, type));
            newVertex.Drag(ToClient(Mouse.Location));
		}


        protected override void OnDrag(Point location)
        {
            Center = (location + CenterOffset).Confine(new Rectangle(Point.Zero, Parent.Region.Size));
        }

		protected override void OnDrop()
		{
            //if (!Toolbar.Current.CurrentTool.AllowMerge)
            //    return;
            
            var vertices = Parent.Children
                                 .OfType<Vertex>()
                                 .Where(v => v != this && (Center - v.Center).Magnitude < Radius);

            foreach (var vertex in vertices)
            {
                if (_edges.Count == 0 && vertex._edges.Count == 0)
                {
                    Parent = null;
                    break;
                }
                else if (IsLeaf)
                {
                    var interEdge = _edges[0].OtherEnd(this).GetEdge(vertex);

                    if (_edges[0].OtherEnd(this) != vertex)
                    {
                        _edges[0].Transfer(this, vertex);
                        if (interEdge != null)
                            interEdge.Parent = null;
                        Parent = null;
                        break;
                    }
                }
                else if (_edges.Count == 2 && _edges[0].ConnectsTo(_edges[1]))
                {
                    var edge = GetEdge(vertex);
                    if (edge == null)
                        continue;

                    if (edge == _edges[0])
                        _edges[1].Transfer(this, vertex);
                    else
                        _edges[0].Transfer(this, vertex);
                    Parent = null;
                    break;
                }
            }
		}



        #region Diagram analysis
        
        public bool IsLeaf => _edges.Count == 1;
        public bool IsInput => IsLeaf && _edges[0].OtherEnd(this).X >= this.X;
        public bool IsOutput => IsLeaf && _edges[0].OtherEnd(this).X < this.X;

        public ParticleType GetEndpointType()
        {
            if (!IsLeaf)
                throw new InvalidOperationException("Cannot get endpoint type of non-endpoint vertex");

            if ((_edges[0].From == this && IsInput) || (_edges[0].To == this && IsOutput))
                return _edges[0].ParticleType;
            else
                return _edges[0].ParticleType.Antiparticle;
        }

        public (ParticleType i, ParticleType o, ParticleType f)? Decompose()
        {
            if (_edges.Count != 3)
                return null;
            
            var input = _edges.FirstOrDefault(e => e.EdgeType.IsFermion() && e.To == this);
            var output = _edges.FirstOrDefault(e => e.EdgeType.IsFermion() && e.From == this);
			var force = _edges.FirstOrDefault(e => !e.EdgeType.IsFermion());
            if (input == null || output == null || force == null)
                return null;
            else
                return (input.ParticleType, output.ParticleType, force.ParticleType);
        }
        
        public bool IsValid
        {
            get
            {
                switch (_edges.Count)
                {
                    case 0:
                    case 1:
                        return true;

                    case 2:
                        return (_edges[0].ConnectsTo(_edges[1]));

                    case 3:
                        if (_edges.All(e => e.EdgeType == ParticleClass.Gluon))
                            return true;
                        if (_edges.Any(e => e.EdgeType == ParticleClass.Higgs))
                        {
                            var others = _edges.Where(e => e.EdgeType != ParticleClass.Higgs).ToArray();
                            return others[0].ConnectsTo(others[1]) && others[0].ParticleType.CouplingStrength(others[1].ParticleType, ParticleType.Higgs) > 0;
                        }

                        var force = _edges.FirstOrDefault(e => !e.EdgeType.IsFermion());
                        var input = _edges.FirstOrDefault(e => e.EdgeType.IsFermion() && e.To == this);
                        var output = _edges.FirstOrDefault(e => e.EdgeType.IsFermion() && e.From == this);
                        return (force != null && input != null && output != null)
                            && (input.ParticleType.CouplingStrength(output.ParticleType, force.ParticleType) > 0);


                    default:
                        return _edges.All(e => e.EdgeType == ParticleClass.Gluon);
                }

            }
        }

        public double CouplingCost()
        {
            if (_edges.Count != 3)
                return 0;

            if (_edges.Any(e => e.EdgeType == ParticleClass.Higgs))
            {
                var others = _edges.Where(e => e.EdgeType != ParticleClass.Higgs).ToArray();
                return others[0].ParticleType.CouplingStrength(others[1].ParticleType, ParticleType.Higgs);
            }
            else
            {
                var p = Decompose();
                return p?.i.CouplingStrength(p?.o, p?.f) ?? 0;
            }
        }

        #endregion

    }
}
