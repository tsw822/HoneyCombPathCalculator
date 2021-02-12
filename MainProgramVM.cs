using BeeCombGUI.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BeeCombGUI
{
    internal enum StepDirection
    {
        IncreaseZ,
        DecreaseY,
        IncreaseX,
        DecreaseZ,
        IncreaseY,
        DecreaseX
    }

    internal class MainProgramVM : INotifyPropertyChanged
    {
        private const int _initPosX = 400;
        private const int _initPosY = 300;
        private const int _hexCount = 100000;

        private static MainProgramVM instance;
        public static MainProgramVM MainProgramInstance => instance ?? (instance = new MainProgramVM());

        public HashSet<int> Visited { get; set; } = new HashSet<int>();

        public HashSet<int> NoInList { get; set; } = new HashSet<int>();

        #region VM properties
        private int input = 1;
        public int Input
        {
            get => input;
            set { input = value; OnPropertyChanged(); }
        }

        private int target = 1;
        public int Target
        {
            get => target;
            set { target = value; OnPropertyChanged(); }
        }

        private bool greedy;
        public bool Greedy
        {
            get => greedy;
            set { greedy = value; OnPropertyChanged(); }
        }
        #endregion

        public static List<int> StepsOpsForCircles(int n) //direction of each hex of circle
        {
            if (n == 0)
            {
                return new List<int> { 0 };
            }
            else
            {
                List<int> list = new List<int>();
                list.AddRange(Enumerable.Repeat(1, n - 1));
                foreach (int item in new List<int> { 2, 3, 4, 5, 0 })
                {
                    list.AddRange(Enumerable.Repeat(item, n));
                }
                list.Add(0);
                return list;
            }
        }
        public static List<Hexagon> ListGenerator(int n)
        {
            List<Hexagon> valueList = new List<Hexagon>();

            Hexagon startHex = new Hexagon(1, _initPosX, _initPosY);//setting position of first hexagon

            valueList.Add(startHex);

            int mapCount = n;
            List<int> stepDirectionList = new List<int>();
            int circle = 0;


            while (stepDirectionList.Count <= mapCount)
            {
                stepDirectionList.AddRange(StepsOpsForCircles(circle));
                circle++;
                //Console.WriteLine(string.Join(",",StepsOpsForCircles(circle)));
                //Console.WriteLine(circle);
            }

            for (int i = 2; i < mapCount; i++) //db
            {
                Hexagon tmp = valueList.Last();
                valueList.Add(tmp.NextHex((StepDirection)stepDirectionList[i - 2]));
            }
            return valueList;
        }

        public Tuple<List<Hexagon>, Dictionary<int, Hexagon>> PrepHexMap()
        {

            List<Hexagon> hexList = ListGenerator(_hexCount);

            Dictionary<int, Hexagon> mapDict = new Dictionary<int, Hexagon>();

            foreach (Hexagon item in hexList)
            {
                mapDict[item.GetHashCode()] = item;
            }

            return new Tuple<List<Hexagon>, Dictionary<int, Hexagon>>(hexList, mapDict);
        }

        public static List<int> GetNeighbourList(List<Hexagon> hexList, Dictionary<int, Hexagon> mapDict, int input)
        {
            Hexagon startpoint = hexList[input - 1];

            List<Hexagon> neibourList = startpoint.GetNeighbours();
            List<int> neibourValueList = new List<int>();

            foreach (Hexagon item in neibourList)
            {
                try
                {
                    neibourValueList.Add(mapDict[(item.X * 10000, item.Y * 100, item.Z).GetHashCode()].Value);
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException("not in the map");
                }
            }
            return neibourValueList;
        }

        public static Queue<int> BFSPathFinder(Queue<int> frontier, List<Hexagon> hexList, Dictionary<int, Hexagon> mapDict, int target)
        {
            Queue<int> nextFrontier = new Queue<int>();

            while (frontier.Count != 0)
            {
                int item = frontier.Dequeue();
                int currentDistance = hexList[item - 1] - hexList[target - 1];
                bool condition;
                if (currentDistance == 0)
                {
                    MainProgramInstance.Visited.Add(item);
                    break;
                }
                foreach (int node in GetNeighbourList(hexList, mapDict, item))
                {
                    if (MainProgramInstance.Greedy)
                    {
                        condition = hexList[node - 1] - hexList[target - 1] < currentDistance;
                    }
                    else
                    {
                        condition = hexList[node - 1] - hexList[target - 1] <= currentDistance;
                    }
                    if (condition && (!MainProgramInstance.Visited.Contains(node)) && (!MainProgramInstance.NoInList.Contains(node)) && (!nextFrontier.Contains(node)))
                    {
                        nextFrontier.Enqueue(node);
                        //pathLists.Add(new List<int>{item,node});
                    }
                }
                MainProgramInstance.Visited.Add(item);
            }
            return nextFrontier;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
