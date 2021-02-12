using System;
using System.Collections.Generic;

namespace BeeCombGUI
{
    class Hexagon : ICloneable
    {
        private static double _radius = 30 * Math.Sqrt(3);
        private static double _long = Math.Sin(Math.PI * 60 / 180f);
        private static double _short = Math.Sin(Math.PI * 30 / 180f);

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Value { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        #region Constructors
        public Hexagon(int value)
        {
            Value = value;
            X = 0;
            Y = 0;
            Z = 0;
            PosX = 0;
            PosY = 0;
        }
        public Hexagon(int value, double x, double y)
        {
            Value = value;
            X = 0;
            Y = 0;
            Z = 0;
            PosX = x;
            PosY = y;
        }
        #endregion

        //No.2
        private void IncreaseX()
        {
            X++;
            Y--;
            PosX -= _radius * _long;
            PosY -= _radius * _short;
        }
        //No.4
        private void IncreaseY()
        {
            Y++;
            Z--;
            PosX += _radius * _long;
            PosY -= _radius * _short;
        }
        //No.0
        private void IncreaseZ()
        {
            Z++;
            X--;
            PosY += _radius;
        }
        //No.5
        private void DecreaseX()
        {
            X--;
            Y++;
            PosX += _radius * _long;
            PosY += _radius * _short;
        }
        //No.1
        private void DecreaseY()
        {
            Y--;
            Z++;
            PosX -= _radius * _long;
            PosY += _radius * _short;
        }
        //No.3
        private void DecreaseZ()
        {
            Z--;
            X++;

            PosY -= _radius;
        }
        public Hexagon NextHex(StepDirection direction)
        {
            Hexagon newHex = this.Clone() as Hexagon;
            switch (direction)
            {
                case StepDirection.IncreaseZ:
                    newHex.IncreaseZ();
                    break;
                case StepDirection.DecreaseY:
                    newHex.DecreaseY();
                    break;
                case StepDirection.IncreaseX:
                    newHex.IncreaseX();
                    break;
                case StepDirection.DecreaseZ:
                    newHex.DecreaseZ();
                    break;
                case StepDirection.IncreaseY:
                    newHex.IncreaseY();
                    break;
                case StepDirection.DecreaseX:
                    newHex.DecreaseX();
                    break;
            }
            newHex.Value++;

            return newHex;
        }
        public List<Hexagon> GetNeighbours()
        {
            List<Hexagon> neighbours = new List<Hexagon>();
            for (var i = 0; i < 6; i++)
                neighbours.Add(this.Clone() as Hexagon);
            neighbours[0].IncreaseZ();
            neighbours[1].DecreaseY();
            neighbours[2].IncreaseX();
            neighbours[3].DecreaseZ();
            neighbours[4].IncreaseY();
            neighbours[5].DecreaseX();
            return neighbours;
        }

        public override string ToString()
        {
            return $"X:{X},Y:{Y},Z:{Z} => Value: {Value}";
        }

        public static int operator -(Hexagon a, Hexagon b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        public object Clone()
        {
            return (Hexagon)this.MemberwiseClone();
        }

        public override int GetHashCode()
        {
            return (X * 10000, Y * 100, Z).GetHashCode();
            //return base.GetHashCode();
        }
    }
}
