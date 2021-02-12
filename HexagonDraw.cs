using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BeeCombGUI
{
    class HexagonDraw
    {
        // Hexagon Size
        private const int Radius = 30;
        public double PosX { get; set; }

        public double PosY { get; set; }

        public string Value { get; set; }

        public HexagonDraw(double x, double y)
        {
            PosX = x;
            PosY = y;
        }

        public HexagonDraw(Hexagon hex)
        {
            Value = hex.Value.ToString();
            PosX = hex.PosX;
            PosY = hex.PosY;
        }

        public PointCollection Points { get; set; }

        public PointCollection GetPoints()
        {
            Points = new PointCollection();
            for (int i = 0; i < 6; i++)
            {
                Points.Add(new Point(
                    PosX + Radius * Math.Cos(i * 60 * Math.PI / 180f),
                    PosY + Radius * Math.Sin(i * 60 * Math.PI / 180f)
                    ));
            }
            return Points;
        }

        public Polygon GetHexPolygon(Brush color)
        {
            return new Polygon
            {
                Stroke = Brushes.Honeydew,
                Fill = color,
                StrokeThickness = 1,
                //HorizontalAlignment = HorizontalAlignment.Right,
                //VerticalAlignment = VerticalAlignment.Center,
                Points = GetPoints(),
                Tag = Value,
            };
        }

        public Label GetLabel()
        {
            var lb = new Label();
            lb.Content = Value;
            //lb.Padding = new Thickness(0);
            lb.Margin = new Thickness(PosX - 5, PosY - 5, PosX + 5, PosY + 5);
            return lb;
        }
    }
}
