﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComputerAlgebra;

namespace Circuit
{
    /// <summary>
    /// Ideal current source.
    /// </summary>
    [Category("Standard")]
    [DisplayName("Current Source")]
    [DefaultProperty("Current")]
    [Description("Ideal current source.")]
    public class CurrentSource : TwoTerminal
    {
        private Quantity current = new Quantity(1, Units.A);
        [Serialize, Description("Current generated by this current source.")]
        public Quantity Current { get { return current; } set { if (current.Set(value)) NotifyChanged("Current"); } }

        public CurrentSource() { Name = "I1"; }

        public static void Analyze(Analysis Mna, Node Anode, Node Cathode, Expression Current)
        {
            Mna.AddPassiveComponent(Anode, Cathode, Current);
            // Add initial conditions, if necessary.
            Expression i0 = Current.Evaluate(t, 0);
            if (!(i0 is Constant))
                Mna.AddInitialConditions(Arrow.New(i0, 0));
        }

        public override void Analyze(Analysis Mna) { Analyze(Mna, Anode, Cathode, Current.Value); }

        public override void LayoutSymbol(SymbolLayout Sym)
        {
            base.LayoutSymbol(Sym);

            int r = 10;
            Sym.AddWire(Anode, new Coord(0, r));
            Sym.AddWire(Cathode, new Coord(0, -r));

            Sym.AddCircle(EdgeType.Black, new Coord(0, 0), r);
            Sym.DrawArrow(EdgeType.Black, new Coord(0, -7), new Coord(0, 7), 0.2f);

            Sym.DrawText(() => Current.ToString(), new Point(r * 0.7, r * 0.7), Alignment.Near, Alignment.Near);
            Sym.DrawText(() => Name, new Point(r * 0.7, r * -0.7), Alignment.Near, Alignment.Far); 
        }
    }
}
