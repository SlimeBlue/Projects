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

        public bool Explore()
        {
            Seen = true;
            return true;
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
        static byte[] caveRand;
        static int randCount;

        public Cave(int seed)
        {
            int x, y;

            if (seed < 1)
            {
                x = new Random().Next(15, 25);
                y = new Random().Next(10, 20);
                caveRand = new byte[4 * x * y];
                new Random().NextBytes(caveRand);
            }
            else
            {
                x = new Random(seed).Next(15, 25);
                y = new Random(seed).Next(10, 20);
                caveRand = new byte[4 * x * y];
                new Random(seed).NextBytes(caveRand);
            }
            Layout = new CavePlot[x, y];

            int xEntrance, yEntrance;
            if (seed < 1)
            {
                xEntrance = caveRand[0] % x;
                yEntrance = caveRand[1] % y;
            }
            int randCount = 2;

            int numDirec = 0;
            int[] turns; // 0-forward, 1-turn left, 2-turn right
            numDirec = caveRand[randCount] % 3;
            turns = new int[numDirec];
            for (int i = 0; i < numDirec; i++)
            {
                randCount++;
                turns[i] = caveRand[randCount] % 4;
            }

            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    if (Layout[i, j] == null)
                        Layout[i, j] = new CavePlot(CaveType.Wall);

                    caveRand = null;
        }
        public Cave(Player MyPlayer, int seed)
            : this(seed)
        {
            this.MyPlayer = MyPlayer;
        }
        public Cave(CavePlot[,] MyLayout)
        {
            this.Layout = MyLayout;
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
