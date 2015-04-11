using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelAdventure
{
    enum CaveType { Cave, Entrance, Wall } ;

    class CavePlot
    {
        public CaveType Location;
        public List<MyItem> Drops;
        public List<Animal> Entities;
        public List<Chest> Chests;
        public List<Door> Doors;

        public bool Seen;

        public CavePlot(Plot MyPlot, CaveType Location)
        {
            this.Location = Location;
            this.Drops = MyPlot.Drops;
            this.Entities = MyPlot.Entities;
            this.Chests = MyPlot.Chests;
            this.Doors = MyPlot.Doors;
            this.Seen = false;
        }
        public CavePlot(CaveType Location)
        {
            this.Location = Location;
            this.Drops = new List<MyItem>();
            this.Entities = new List<Animal>();
            this.Chests = new List<Chest>();
            this.Doors = new List<Door>();
            this.Seen = false;
        }

        public string Name() //Returns the name of the landscape
        {
            switch (Location)
            {
                case CaveType.Cave:
                    return "Cave";

                case CaveType.Entrance:
                    return "Entrance";

                case CaveType.Wall:
                    return "Wall";

                default:
                    return "???";
            }
        }
        public char Symbol() //Returns the symbol of the landscape
        {
            switch (Location)
            {
                case CaveType.Cave:
                    return 'c';

                case CaveType.Entrance:
                    return 'e';

                case CaveType.Wall:
                    return 'w';

                default:
                    return ' ';
            }
        }
        public ConsoleColor Color() //Returns the color of the landscape
        {
            switch (Location)
            {
                case CaveType.Cave:
                    return ConsoleColor.Gray;

                case CaveType.Entrance:
                    return ConsoleColor.Cyan;

                case CaveType.Wall:
                    return ConsoleColor.DarkGray;

                default:
                    return ConsoleColor.Black;
            }
        }
    }

    class Cave
    {
        public CavePlot[,] Layout;
        public Player MyPlayer;
        public int caveLocationX;
        public int caveLocationY;

        static byte[] caveRand;

        static int birthLimit = 4;
        static int deathLimit = 3;
        static float aliveChance = 0.35f;
        static int stepsNumber = 100;
        static int entranceLimit = 6;

        bool[,] GenerateMap(int width, int height)
        {
            bool[,] cellMap = new bool[width, height];
            cellMap = InitializeMap(cellMap);
            for (int i = 0; i < stepsNumber; i++)
                cellMap = DoSimulationStep(cellMap);
            return cellMap;
        }

        bool[,] InitializeMap(bool [,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (caveRand[i * map.GetLength(1) + j] < byte.MaxValue * aliveChance)
                        map[i, j] = true;
                    else
                        map[i, j] = false;
            return map;
        }

        bool[,] DoSimulationStep(bool[,] oldMap)
        {
            bool[,] newMap = new bool[oldMap.GetLength(0),oldMap.GetLength(1)];
            int nbs;
            for (int i = 0; i < oldMap.GetLength(0); i++)
                for (int j = 0; j < oldMap.GetLength(1); j++)
                {
                    nbs = CountAliveNeighbours(oldMap, i, j);
                    if (oldMap[i, j])
                        if (nbs < deathLimit)
                            newMap[i, j] = false;
                        else
                            newMap[i, j] = true;
                    else
                        if (nbs > birthLimit)
                            newMap[i, j] = true;
                        else
                            newMap[i, j] = false;
                }
            return newMap;
        }

        int CountAliveNeighbours(bool[,] map, int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j<2; j++)
                    if (i != 0 || j != 0)
                        if (x+i < 0 || y+j < 0 || x+i>=map.GetLength(0) || y+j >=map.GetLength(1))
                            count = count + 1;
                        else if (map[x+i,y+j])
                            count = count + 1;
            return count;
        }

        public Cave(int seed, int caveLocationX, int caveLocationY)
        {
            this.caveLocationX = caveLocationX;
            this.caveLocationY = caveLocationY;

            int x, y;

            if (seed < 1)
            {
                x = new Random().Next(15, 25);
                y = new Random().Next(10, 20);
                caveRand = new byte[x * y];
                new Random().NextBytes(caveRand);
            }
            else
            {
                x = new Random(seed).Next(15, 25);
                y = new Random(seed).Next(10, 20);
                caveRand = new byte[x * y];
                new Random(seed).NextBytes(caveRand);
            }
            Layout = new CavePlot[x, y];
            
            bool[,] boolLayout = GenerateMap(x, y);
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    if (!boolLayout[i, j])
                        Layout[i, j] = new CavePlot(CaveType.Cave);
                    else
                        Layout[i, j] = new CavePlot(CaveType.Wall);

            for (int i = 0; i < Layout.GetLength(0); i++)
            {
                Layout[i, 0] = new CavePlot(CaveType.Wall);
                Layout[i, Layout.GetLength(1) - 1] = new CavePlot(CaveType.Wall);
            }
            for (int j = 0; j < Layout.GetLength(1); j++)
            {
                Layout[0, j] = new CavePlot(CaveType.Wall);
                Layout[Layout.GetLength(0) - 1, j] = new CavePlot(CaveType.Wall);
            }

            int nbs;
            while (true)
            {
                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                        if (!boolLayout[i, j])
                        {
                            nbs = CountAliveNeighbours(boolLayout, i, j);
                            if (8 - nbs >= entranceLimit)
                            {
                                Layout[i, j] = new CavePlot(CaveType.Entrance);
                                caveRand = null;
                                boolLayout = null;
                                return;
                            }
                        }
            }
        }
        public Cave(Player MyPlayer, int seed, int caveLocationX, int caveLocationY)
            : this(seed, caveLocationX, caveLocationY)
        {
            this.MyPlayer = MyPlayer;
        }
        public Cave(CavePlot[,] MyLayout, int caveLocationX, int caveLocationY)
        {
            this.Layout = MyLayout;
            this.caveLocationX = caveLocationX;
            this.caveLocationY = caveLocationY;
        }

        public int[] FindEntrance()
        {
            for (int i = 0; i < Layout.GetLength(0); i++)
                for (int j = 0; j < Layout.GetLength(1); j++)
                    if (Layout[i, j].Location == CaveType.Entrance)
                        return new int[2] { i, j };
            return null;
        }

        public void Print()
        {
            for (int i = 0; i < Layout.GetLength(0); i++)
            {
                for (int j = 0; j < Layout.GetLength(1); j++)
                {
                    if (Layout[i, j].Seen)
                        Console.BackgroundColor = Layout[i, j].Color();

                    else
                        Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                }

                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
