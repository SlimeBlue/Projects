using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PixelAdventure
{
    enum Landscapes { Planes, RockyMountains, Garden, Hut, Graveyard, Ocean, Cliffs, Beach, Hills, Forest, Cave }

    class Plot
    {
        //public int ID;
        public Landscapes Location;
        public List<MyItem> Drops;
        public List<Animal> Entities;
        public List<Chest> Chests;
        public List<Door> Doors;
        public List<Cave> Caves;

        public Plot(/*int ID, */Landscapes Location) //Creates a plot
        {
            //this.ID = ID;
            this.Location = Location;
            this.Drops = new List<MyItem>();
            this.Entities = new List<Animal>();
            this.Chests = new List<Chest>();
            this.Doors = new List<Door>();
            this.Caves = new List<Cave>();
        }

        public string Name() //Returns the name of the landscape
        {
            switch (Location)
            {
                case Landscapes.RockyMountains:
                    return "Rocky Mountains";

                case Landscapes.Ocean:
                    return "Ocean";

                case Landscapes.Cliffs:
                    return "Cliffs";

                case Landscapes.Planes:
                    return "Planes";

                case Landscapes.Beach:
                    return "Beach";

                case Landscapes.Hills:
                    return "Hills";

                case Landscapes.Forest:
                    return "Forest";

                case Landscapes.Hut:
                    return "Hut";

                case Landscapes.Garden:
                    return "Garden";

                case Landscapes.Cave:
                    return "Cave";

                default:
                    return "???";
            }
        }
        public char Symbol() //Returns the symbol of the landscape
        {
            switch (Location)
            {
                case Landscapes.RockyMountains:
                    return 'm';

                case Landscapes.Ocean:
                    return 'o';

                case Landscapes.Cliffs:
                    return 'c';

                case Landscapes.Planes:
                    return 'p';

                case Landscapes.Beach:
                    return 'b';

                case Landscapes.Hills:
                    return 'h';

                default:
                    return ' ';
            }
        }
        public ConsoleColor Color() //Returns the color of the landscape
        {
            switch (Location)
            {
                case Landscapes.RockyMountains:
                    return ConsoleColor.DarkGray;

                case Landscapes.Ocean:
                    return ConsoleColor.Blue;

                case Landscapes.Cliffs:
                    return ConsoleColor.Gray;

                case Landscapes.Planes:
                    return ConsoleColor.Green;

                case Landscapes.Beach:
                    return ConsoleColor.Yellow;

                case Landscapes.Hills:
                    return ConsoleColor.DarkYellow;

                case Landscapes.Forest:
                    return ConsoleColor.DarkGreen;

                case Landscapes.Hut:
                    return ConsoleColor.DarkRed;

                case Landscapes.Garden:
                    return ConsoleColor.Magenta;

                case Landscapes.Cave:
                    return ConsoleColor.DarkCyan;

                default:
                    return ConsoleColor.Black;
            }
        }
    }

    class World
    {
        public Plot[,] Surface; //The first index is the column, and the second is the row
        public Player MyPlayer; //In the multiplayer version change it to a List<Player>
        static byte[] worldRand;
        static int randCount;

        static Landscapes SetLocationBorder() //Returns a random landscape
        {
            //int p = new Random().Next(2);
            if (worldRand[randCount] % 2 == 0)
                return Landscapes.Ocean;
            return Landscapes.RockyMountains;
        }
        static Landscapes SetLocationBorder(Landscapes Before) //Returns a landscape knowing the landscape besides
        {
            //int p = new Random().Next(20);
            if (worldRand[randCount] % 10 != 0)
                return Before;
            return Landscapes.Cliffs;
        }
        static Landscapes SetLocationBorder(Landscapes Before, Landscapes After) //Returns a landscape knowing the two landscapes besides
        {
            if (Before == After)
                return Before;
            return Landscapes.Cliffs;
        }
        static Landscapes SetLocationBorderNearCliff(Landscapes Before2) //Returns a landscape knowing the beside is cliff and the one before it
        {
            if (Before2 == Landscapes.Ocean)
                return Landscapes.RockyMountains;
            return Landscapes.Ocean;
        }

        static Landscapes SetLocationInnerCorner(Landscapes Outside1, Landscapes Outside2) //Return a landscape knowing the landscapes outside
        {
            //int p;
            switch (Outside1)
            {
                case Landscapes.Cliffs:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        default:
                            return Outside2;
                    }
                case Landscapes.Ocean:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Beach;
                        case Landscapes.Ocean:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        case Landscapes.RockyMountains:
                            return Landscapes.Cliffs;
                        case Landscapes.Beach:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        default:
                            return Landscapes.Beach;
                    }
                case Landscapes.RockyMountains:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Cliffs;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        case Landscapes.Hills:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        default:
                            return Landscapes.Hills;
                    }
                case Landscapes.Beach:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Ocean;
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            return Landscapes.Hills;
                        case Landscapes.Beach:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        case Landscapes.Planes:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        default:
                            return Landscapes.Planes;
                    }
                case Landscapes.Hills:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.RockyMountains;
                            return Landscapes.Hills;
                        case Landscapes.Hills:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        case Landscapes.Planes:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        default:
                            return Landscapes.Planes;
                    }
                default:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            return Landscapes.Hills;
                        case Landscapes.Hills:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        case Landscapes.Beach:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        default:
                            return Landscapes.Planes;
                    }
            }
        }
        static Landscapes SetLocationInner(Landscapes Outside, Landscapes Before) //Return a landscape knowing the landscape outside and the one beside
        {
            //int p;
            switch (Outside)
            {
                case Landscapes.Cliffs:
                    switch (Before)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        default:
                            return Before;
                    }
                case Landscapes.Ocean:
                    switch (Before)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Beach;
                        case Landscapes.Ocean:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        case Landscapes.RockyMountains:
                            return Landscapes.Cliffs;
                        case Landscapes.Beach:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        default:
                            return Landscapes.Beach;
                    }
                case Landscapes.RockyMountains:
                    switch (Before)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Cliffs;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        case Landscapes.Hills:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        default:
                            return Landscapes.Hills;
                    }
                case Landscapes.Beach:
                    switch (Before)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Ocean;
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            return Landscapes.Hills;
                        case Landscapes.Beach:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        case Landscapes.Planes:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        default:
                            return Landscapes.Planes;
                    }
                case Landscapes.Hills:
                    switch (Before)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.RockyMountains;
                            return Landscapes.Hills;
                        case Landscapes.Hills:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        case Landscapes.Planes:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        default:
                            return Landscapes.Planes;
                    }
                default:
                    switch (Before)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            return Landscapes.Hills;
                        case Landscapes.Hills:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        case Landscapes.Beach:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        default:
                            return Landscapes.Planes;
                    }
            }
        }
        static Landscapes SetLocationInnerCorner(Landscapes Outside1, Landscapes Outside2, Landscapes Before) //Need to be fixed
        {
            //int p;
            switch (Outside1)
            {
                case Landscapes.Cliffs:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        default:
                            return Outside2;
                    }
                case Landscapes.Ocean:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Beach;
                        case Landscapes.Ocean:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        case Landscapes.RockyMountains:
                            return Landscapes.Cliffs;
                        case Landscapes.Beach:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Beach;
                            return Landscapes.Ocean;
                        default:
                            return Landscapes.Beach;
                    }
                case Landscapes.RockyMountains:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Cliffs;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        case Landscapes.Hills:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Hills;
                            return Landscapes.RockyMountains;
                        default:
                            return Landscapes.Hills;
                    }
                case Landscapes.Beach:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.Ocean;
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            return Landscapes.Hills;
                        case Landscapes.Beach:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        case Landscapes.Planes:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        default:
                            return Landscapes.Planes;
                    }
                case Landscapes.Hills:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            //p = new Random().Next(3);
                            if (worldRand[randCount] % 3 == 0)
                                return Landscapes.RockyMountains;
                            return Landscapes.Hills;
                        case Landscapes.Hills:
                            //p = new Random().Next(6);
                            if (worldRand[randCount] % 6 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        case Landscapes.Planes:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        default:
                            return Landscapes.Planes;
                    }
                default:
                    switch (Outside2)
                    {
                        case Landscapes.Cliffs:
                            return Landscapes.Hills;
                        case Landscapes.Ocean:
                            return Landscapes.Beach;
                        case Landscapes.RockyMountains:
                            return Landscapes.Hills;
                        case Landscapes.Hills:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Hills;
                        case Landscapes.Beach:
                            //p = new Random().Next(2);
                            if (worldRand[randCount] % 2 == 0)
                                return Landscapes.Planes;
                            return Landscapes.Beach;
                        default:
                            return Landscapes.Planes;
                    }
            }
        }

        private int RecursiveForest(int x, int y, int size, int count)
        {
            if (count >= size)
                return count;

            if (Surface[x - 1, y].Location == Landscapes.Planes)
            {
                randCount++;
                if (worldRand[randCount] % 6 != 0)
                {
                    Surface[x - 1, y].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x - 1, y, size, count);
                }
                //Thread.Sleep(10);
            }
            if (Surface[x - 1, y].Location == Landscapes.Hills)
            {
                randCount++;
                if (worldRand[randCount] % 4 != 0)
                {
                    Surface[x - 1, y].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x - 1, y, size, count);
                }
                //Thread.Sleep(10);
            }

            if (count >= size)
                return count;

            if (Surface[x + 1, y].Location == Landscapes.Planes)
            {
                randCount++;
                if (worldRand[randCount] % 6 != 0)
                {
                    Surface[x + 1, y].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x + 1, y, size, count);
                }
                //Thread.Sleep(10);
            }
            if (Surface[x + 1, y].Location == Landscapes.Hills)
            {
                randCount++;
                if (worldRand[randCount] % 4 != 0)
                {
                    Surface[x + 1, y].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x + 1, y, size, count);
                }
                //Thread.Sleep(10);
            }

            if (count >= size)
                return count;

            if (Surface[x, y + 1].Location == Landscapes.Planes)
            {
                randCount++;
                if (worldRand[randCount] % 6 != 0)
                {
                    Surface[x, y + 1].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x, y + 1, size, count);
                }
                //Thread.Sleep(10);
            }
            if (Surface[x, y + 1].Location == Landscapes.Hills)
            {
                randCount++;
                if (worldRand[randCount] % 4 != 0)
                {
                    Surface[x, y + 1].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x, y + 1, size, count);
                }
                //Thread.Sleep(10);
            }

            if (count >= size)
                return count;

            if (Surface[x, y - 1].Location == Landscapes.Planes)
            {
                randCount++;
                if (worldRand[randCount] % 6 != 0)
                {
                    Surface[x - 1, y].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x, y - 1, size, count);
                }
                //Thread.Sleep(10);
            }
            if (Surface[x, y - 1].Location == Landscapes.Hills)
            {
                randCount++;
                if (worldRand[randCount] % 4 != 0)
                {
                    Surface[x - 1, y].Location = Landscapes.Forest;
                    count++;
                    count = RecursiveForest(x, y - 1, size, count);
                }
                //Thread.Sleep(10);
            }

            return count;
        }
        private void SetForest() //Defines the places of the forest
        {
            int size = 0, count = 0;
            for (int i = 1; i < Surface.GetLength(0) - 1; i++)
                for (int j = 1; j < Surface.GetLength(1) - 1; j++)
                    if (Surface[i, j].Location == Landscapes.Planes || Surface[i, j].Location == Landscapes.Hills)
                        size++;
            size = size / 3;

            while (count < size)
            {
                /*for (int i = 1; i < Surface.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < Surface.GetLength(1) - 1; j++)
                    {
                        if (Surface[i, j].Location == Landscapes.Planes)
                        {
                            if (new Random().Next(6) == 5)
                            {
                                Surface[i, j].Location = Landscapes.Forest;
                                count++;
                                count = RecursiveForest(i, j, size, count);
                            }
                            Thread.Sleep(10);
                        }
                        
                        else if (Surface[i, j].Location == Landscapes.Hills)
                        {
                            if (new Random().Next(9) == 8)
                            {
                                Surface[i, j].Location = Landscapes.Forest;
                                count++;
                                count = RecursiveForest(i, j, size, count);
                            }
                            Thread.Sleep(10);
                        }
                    }
                }*/

                int i, j;

                do
                {
                    //i = new Random().Next(Surface.GetLength(0) - 1);
                    //j = new Random().Next(Surface.GetLength(1) - 1);
                    //Thread.Sleep(4);

                    randCount++;
                    i = worldRand[randCount] % (Surface.GetLength(0) - 1);
                    randCount++;
                    j = worldRand[randCount] % (Surface.GetLength(1) - 1);
                }
                while (Surface[i, j].Location != Landscapes.Planes && Surface[i, j].Location != Landscapes.Hills);

                Surface[i, j].Location = Landscapes.Forest;
                count++;
                count = RecursiveForest(i, j, size, count);
                Thread.Sleep(10);
            }
        }

        private void SetCave(int seed) //Defines the place of the cave
        {
            bool flag = false;
            while (!flag)
            {
                int x = worldRand[randCount] % Surface.GetLength(0);
                int y = worldRand[randCount + 1] % Surface.GetLength(1);
                randCount += 2;
                if (Surface[x, y].Location == Landscapes.Hills)
                {
                    Surface[x, y].Location = Landscapes.Cave;
                    Surface[x, y].Caves.Add(new Cave(seed, x, y));
                    flag = true;
                }
            }
        }

        private void SetBorders() //Defines the plots of the borders of the world
        {
            //Defines the plot of the left-upper corner
            Surface[0, 0] = new Plot(SetLocationBorder());
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the first row
            for (int i = 1; i < Surface.GetLength(1) - 1; i++)
            {
                if (Surface[0, i - 1].Location == Landscapes.Cliffs)
                    Surface[0, i] = new Plot(SetLocationBorderNearCliff(Surface[0, i - 2].Location));
                else
                    Surface[0, i] = new Plot(SetLocationBorder(Surface[0, i - 1].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the right-upper corner
            if (Surface[0, Surface.GetLength(1) - 2].Location == Landscapes.Cliffs)
                Surface[0, Surface.GetLength(1) - 1] = new Plot(SetLocationBorderNearCliff(Surface[0, Surface.GetLength(1) - 3].Location));
            else
                Surface[0, Surface.GetLength(1) - 1] = new Plot(Surface[0, Surface.GetLength(1) - 2].Location);
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the last column
            for (int i = 1; i < Surface.GetLength(0) - 1; i++)
            {
                if (Surface[i - 1, Surface.GetLength(1) - 1].Location == Landscapes.Cliffs)
                    Surface[i, Surface.GetLength(1) - 1] = new Plot(SetLocationBorderNearCliff(Surface[i - 2, Surface.GetLength(1) - 1].Location));
                else
                    Surface[i, Surface.GetLength(1) - 1] = new Plot(SetLocationBorder(Surface[i - 1, Surface.GetLength(1) - 1].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the right-bottom corner
            if (Surface[Surface.GetLength(0) - 2, Surface.GetLength(1) - 1].Location == Landscapes.Cliffs)
                Surface[Surface.GetLength(0) - 1, Surface.GetLength(1) - 1] = new Plot(SetLocationBorderNearCliff(Surface[Surface.GetLength(0) - 3, Surface.GetLength(1) - 1].Location));
            else
                Surface[Surface.GetLength(0) - 1, Surface.GetLength(1) - 1] = new Plot(Surface[Surface.GetLength(0) - 2, Surface.GetLength(1) - 1].Location);
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the last row
            for (int i = Surface.GetLength(1) - 2; i > 0; i--)
            {
                if (Surface[Surface.GetLength(0) - 1, i + 1].Location == Landscapes.Cliffs)
                    Surface[Surface.GetLength(0) - 1, i] = new Plot(SetLocationBorderNearCliff(Surface[Surface.GetLength(0) - 1, i + 2].Location));
                else
                    Surface[Surface.GetLength(0) - 1, i] = new Plot(SetLocationBorder(Surface[Surface.GetLength(0) - 1, i + 1].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the left-bottom corner
            if (Surface[Surface.GetLength(0) - 1, 1].Location == Landscapes.Cliffs)
                Surface[Surface.GetLength(0) - 1, 0] = new Plot(SetLocationBorderNearCliff(Surface[Surface.GetLength(0) - 1, 2].Location));
            else
                Surface[Surface.GetLength(0) - 1, 0] = new Plot(Surface[Surface.GetLength(0) - 1, 1].Location);
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the first column
            for (int i = Surface.GetLength(0) - 2; i > 1; i--)
            {
                if (Surface[i + 1, 0].Location == Landscapes.Cliffs)
                    Surface[i, 0] = new Plot(SetLocationBorderNearCliff(Surface[i + 2, 0].Location));
                else
                    Surface[i, 0] = new Plot(SetLocationBorder(Surface[i + 1, 0].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the point besides the left-upper plot
            Surface[1, 0] = new Plot(SetLocationBorder(Surface[2, 0].Location, Surface[0, 0].Location));
            //Thread.Sleep(10);
            randCount++;
        }
        private void SetInner(int Inner) //Defines the plot of the x-inner border
        {
            //Defines the plot of the first row
            for (int i = Inner; i < Surface.GetLength(1) - Inner - 1; i++)
            {
                Surface[Inner, i] = new Plot(SetLocationInner(Surface[Inner - 1, i].Location, Surface[Inner, i - 1].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the right-upper corner
            Surface[Inner, Surface.GetLength(1) - Inner - 1] = new Plot(SetLocationInnerCorner(Surface[Inner - 1, Surface.GetLength(1) - Inner - 1].Location, Surface[Inner, Surface.GetLength(1) - Inner - 2].Location, Surface[Inner, Surface.GetLength(1) - Inner].Location));
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the last column
            for (int i = Inner; i < Surface.GetLength(0) - Inner - 1; i++)
            {
                Surface[i, Surface.GetLength(1) - Inner - 1] = new Plot(SetLocationInner(Surface[i - 1, Surface.GetLength(1) - Inner - 1].Location, Surface[i, Surface.GetLength(1) - Inner].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the right-bottom corner
            Surface[Surface.GetLength(0) - Inner - 1, Surface.GetLength(1) - Inner - 1] = new Plot(SetLocationInnerCorner(Surface[Surface.GetLength(0) - Inner, Surface.GetLength(1) - Inner - 1].Location, Surface[Surface.GetLength(0) - Inner - 2, Surface.GetLength(1) - Inner - 1].Location, Surface[Surface.GetLength(0) - Inner - 1, Surface.GetLength(1) - Inner].Location));
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the last row
            for (int i = Surface.GetLength(1) - Inner - 2; i > 0; i--)
            {
                Surface[Surface.GetLength(0) - Inner - 1, i] = new Plot(SetLocationInner(Surface[Surface.GetLength(0) - Inner - 1, i + 1].Location, Surface[Surface.GetLength(0) - Inner, i].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the left-bottom corner
            Surface[Surface.GetLength(0) - Inner - 1, Inner] = new Plot(SetLocationInnerCorner(Surface[Surface.GetLength(0) - Inner - 1, Inner + 1].Location, Surface[Surface.GetLength(0) - Inner - 1, Inner - 1].Location, Surface[Surface.GetLength(0) - Inner, Inner].Location));
            //Thread.Sleep(10);
            randCount++;

            //Defines the plot of the first column
            for (int i = Surface.GetLength(0) - Inner - 2; i > 1; i--)
            {
                Surface[i, Inner] = new Plot(SetLocationInner(Surface[i + 1, Inner].Location, Surface[i, Inner - 1].Location));
                //Thread.Sleep(10);
                randCount++;
            }

            //Defines the plot of the point besides the left-upper plot
            Surface[Inner + 1, Inner] = new Plot(SetLocationInnerCorner(Surface[Inner + 2, Inner].Location, Surface[Inner, Inner].Location, Surface[Inner + 1, Inner - 1].Location));
            //Thread.Sleep(10);
            randCount++;
        }

        public World(int seed) //Defines the plots of the whole world
        {
            int x, y;

            if (seed < 1)
            {
                x = new Random().Next(40, 60);
                y = new Random().Next(30, 40);
                worldRand = new byte[2 * x * y];
                new Random().NextBytes(worldRand);
            }
            else
            {
                x = new Random(seed).Next(40, 60);
                y = new Random(seed).Next(30, 40);
                worldRand = new byte[2 * x * y];
                new Random(seed).NextBytes(worldRand);
            }
            Surface = new Plot[x, y];

            SetBorders();

            for (int i = 1; i < (Surface.GetLength(0) - 1) / 2 || i < (Surface.GetLength(1) - 1) / 2; i++)
                SetInner(i);

            SetCave(seed);
            SetForest();
            worldRand = null;
        }
        public World(Player MyPlayer, int seed)
            : this(seed)
        {
            this.MyPlayer = MyPlayer;
            for (int i = 0; i < Surface.GetLength(0); i++)
                for (int j = 0; j < Surface.GetLength(1); j++)
                    if (Surface[i, j].Caves.Count > 0)
                        Surface[i, j].Caves[0].MyPlayer = MyPlayer;
        }
        public World(Plot[,] MySurface)
        {
            this.Surface = MySurface;
        }

        public void Print() //Prints the map of the world
        {
            for (int i = 0; i < Surface.GetLength(0); i++)
            {
                for (int j = 0; j < Surface.GetLength(1); j++)
                {
                    Console.BackgroundColor = Surface[i, j].Color();
                    Console.Write(/*Surface[i, j].Symbol() + */"  ");
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
