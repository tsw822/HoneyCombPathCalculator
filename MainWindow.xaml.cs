//Group 4: Shuwen, Smit, Cong, Manpreet
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BeeCombGUI
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Hexagon> hexList = new List<Hexagon>();
        static Dictionary<int, Hexagon> mapDict = new Dictionary<int, Hexagon>();
        private readonly MainProgramVM MainProgramInstance;

        public int Stop { get; set; } = 1;
        public MainWindow()
        {
            InitializeComponent();
            MainProgramInstance = MainProgramVM.MainProgramInstance;
            DataContext = MainProgramInstance;

            var ret = MainProgramInstance.PrepHexMap();
            hexList = ret.Item1;
            mapDict = ret.Item2;

            foreach (var item in hexList.GetRange(0, 271))// display 271 hex on canvas
            {
                var secHex = new HexagonDraw(item);
                var tmpPolygen = secHex.GetHexPolygon(Brushes.Gold);
                ParentCavas.Children.Add(tmpPolygen);
                tmpPolygen.MouseDown += TmpPolygen_MouseDown;
                ParentCavas.Children.Add(secHex.GetLabel());
            }

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black
            };
            this.WindowState = WindowState.Maximized;
            Show();
        }

        private void TmpPolygen_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var noInList = MainProgramInstance.NoInList;
            var hitHex = (sender as Polygon);
            hitHex.Fill = Brushes.SaddleBrown;
            noInList.Add(int.Parse(hitHex.Tag.ToString()));
        }

        private void PathFinding(int StopSteps)
        {
            int count = 0;
            var visited = MainProgramInstance.Visited;
            var frontier = new Queue<int>();
            MainProgramInstance.Visited.Clear();
            frontier.Enqueue(MainProgramInstance.Input);
            while ((!visited.Contains(MainProgramInstance.Target)) && (count < StopSteps))
            {
                Console.WriteLine(frontier.Count);
                var nextFrontier = MainProgramVM.BFSPathFinder(frontier, hexList, mapDict, MainProgramInstance.Target);
                frontier = nextFrontier;
                if (visited.Contains(MainProgramInstance.Target))
                {
                    Tbx.Text = $"We found the minimum Steps of {count}";
                    Btn_Next.IsEnabled = false;
                }
                else
                {
                    Tbx.Text = $"Start: {MainProgramInstance.Input} End: {MainProgramInstance.Target} It is the {count} Step(s)";
                }
                count++;
            }
        }

        private void NextStep()
        {
            PathFinding(Stop);
        }

        private void Btn_Next_OnClick(object sender, RoutedEventArgs e)
        {
            var visited = MainProgramInstance.Visited;
            NextStep();
            foreach (var item in visited)
            {
                var secHex = new HexagonDraw(hexList[item - 1]);
                ParentCavas.Children.Add(secHex.GetHexPolygon(Brushes.Goldenrod));
                ParentCavas.Children.Add(secHex.GetLabel());
            }
            Stop++;
        }

        private void Btn_Reset_OnClick(object sender, RoutedEventArgs e)
        {
            //ParentCavas.Children.Clear();
            foreach (var item in ParentCavas.Children.OfType<Polygon>())
            {
                item.Fill = Brushes.Gold;
                item.MouseDown += TmpPolygen_MouseDown;
            }
            MainProgramInstance.Visited.Clear();
            MainProgramInstance.NoInList.Clear();
            Tbx.Text = String.Empty;
            Btn_Next.IsEnabled = true;
            Stop = 1;
        }

        private void Btn_Result_OnClick(object sender, RoutedEventArgs e)
        {
            PathFinding(int.MaxValue);
        }
    }
}
