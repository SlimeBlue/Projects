﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace PixelAdventure
{
    //לסיים עם ממשק החפצים (להוסיף עוד חפצים), ולהכניס אותם למשחק (בעיקר הכנסה של עניין הקרפטינג)
    //לתת פירוט על כל המשחק והסבר על איך משחקים כשנכנסים להלפ
    //לבצע פונקציות של ספאון של חיות נוספות אחרי שכמה מתו ("מחזור")
    //קסמים, קסמים, קסמים (אחרת למה עשיתי קוסם? XD)

    //להכניס את ההשפעה של ה-INT וה-DEX למשחק
    //עליית המאנה של קוסם היא כל תור או כל 10 תורות, ואם כל תור איך לעשות שיהיה הבדל בין רמות שונות של INT

    //לחשוב, לכתוב ולממש את האלוריתם לייצור מערות (ואם המשחק מיוצר על ידי סיד, איך לעשות שהמערות יהיו שונות זו מזו אבל יהיו זהות בין משחקים)
    //לסיים את הממשק של הדלתות ותיבות האוצר ולהכניס אותם למשחק
    //להוסיף את ממשק ה-NPC
    //לעשות אפשרות של קניה ומכירה אצל סוחרים (וכמובן לתת לכל סוחר מבחר חפצים שהוא מוכר, האם לעשות הגבלה לכמות שלהם?)
    //להכניס עוד אזורים חדשים כמו ערים, כניסות למערות וכו' (כדי לממש את כל הדברים הנוספים שעושים)
    //לעשות שבתוך העיר תהיה מפה נפרדת לעיר שהולכים בה ולמשל אפשר ללכת מהשער לשוק או לטירה וכו' (כלומר לעשות מחלקה נפרדת לעיר כמו שעשינו למערה)
    //לעדכן את כל הפעולות שהשחקן יכול לעשות כך שאפשר יהיה להשתמש בהן במשחק

    //לעשות שכל המשתנים האקראיים במשחק יהיו מאותו האחד (כדי למנוע כפילויות)

    class Program
    {
        public static bool stopMelody = false;

        #region melody
        public static int[][] melodyArray = new int[][] {
            new int[] { 262, 500 },
            new int[] { 784, 500 },
            new int[] { 262, 500 },
            new int[] { 784, 250 },
            new int[] { 784, 250 },

            new int[] { 262, 500 },
            new int[] { 784, 500 },
            new int[] { 262, 500 },
            new int[] { 784, 500 },

            new int[] { 262, 500 },
            new int[] { 784, 500 },
            new int[] { 262, 500 },
            new int[] { 784, 250 },
            new int[] { 784, 250 },

            new int[] { 262, 500 },
            new int[] { 784, 500 },
            new int[] { 262, 750 },
            new int[] { 262, 250 }
        };

        public static void Melody()
        {
            int index = 0;
            while (!stopMelody)
            {
                if (index == melodyArray.Length)
                    index = 0;
                Console.Beep(melodyArray[index][0], melodyArray[index][1]);
                index++;
            }
        }
        #endregion

        public static void PrintMap(Player MyPlayer, Animal[] AnimalList, World MyWorld)
        {
            for (int i = 0; i < MyWorld.Surface.GetLength(0); i++)
            {
                for (int j = 0; j < MyWorld.Surface.GetLength(1); j++)
                {
                    Console.BackgroundColor = MyWorld.Surface[i, j].Color();
                    for (int k = 0; k < AnimalList.Length; k++)
                        if (i == AnimalList[k].LocationX && j == AnimalList[k].LocationY)
                            Console.BackgroundColor = ConsoleColor.Black;
                    if (i == MyPlayer.LocationX && j == MyPlayer.LocationY)
                        if (Console.BackgroundColor == ConsoleColor.Black)
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        else
                            Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void PrintCaveMap(Player MyPlayer, Cave MyCave)
        {
            for (int i = 0; i < MyCave.Layout.GetLength(0); i++)
            {
                for (int j = 0; j < MyCave.Layout.GetLength(1); j++)
                {
                    Console.BackgroundColor = MyCave.Layout[i, j].Color();
                    if (i == MyPlayer.LocationX && j == MyPlayer.LocationY)
                        Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void Main(string[] args)
        {
            string input = null, oldinput = null;
            bool isAct = false;
            byte[][] animalBytes;
            byte[] resurrectAnimalBytes;

            World myWorld = null;
            Player myPlayer = null;
            Animal[] AnimalList = null;

            Thread MyMelody = new Thread(new ThreadStart(Melody));
            MyMelody.Start();

            #region logo
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            string logo = @"d8888b. d888888b db    db d88888b db                                        
88  `8D   `88'   `8b  d8' 88'     88                                        
88oodD'    88     `8bd8'  88ooooo 88                                        
88~~~      88     .dPYb.  88~~~~~ 88                                        
88        .88.   .8P  Y8. 88.     88booo.                                   
88      Y888888P YP    YP Y88888P Y88888P                                   
                                                                            
                                                                            
 .d8b.  d8888b. db    db d88888b d8b   db d888888b db    db d8888b. d88888b 
d8' `8b 88  `8D 88    88 88'     888o  88 `~~88~~' 88    88 88  `8D 88'     
88ooo88 88   88 Y8    8P 88ooooo 88V8o 88    88    88    88 88oobY' 88ooooo 
88~~~88 88   88 `8b  d8' 88~~~~~ 88 V8o88    88    88    88 88`8b   88~~~~~ 
88   88 88  .8D  `8bd8'  88.     88  V888    88    88b  d88 88 `88. 88.     
YP   YP Y8888D'    YP    Y88888P VP   V8P    YP    ~Y8888P' 88   YD Y88888P";

            Console.Write(logo);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" :-)");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            #endregion

            Console.Write("Start/Load: ");
            bool load = false;

            while (true)
            {
                ConsoleKey myKey = Console.ReadKey(true).Key;
                if (myKey == ConsoleKey.L)
                {
                    load = true;
                    Console.WriteLine();
                    break;
                }
                if (myKey == ConsoleKey.S)
                {
                    Console.WriteLine();
                    break;
                }
            }

            Console.Clear();

            if (load)
            {
                #region loadfunction

                Console.Write("Enter the file name: ");
                string fileName = Console.ReadLine();
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }

                StreamReader loader = new StreamReader(fileName);

                string temp;
                string[] parts;
                int sizeX, sizeY;
                List<Animal> animalTemp;

                temp = loader.ReadLine();
                parts = temp.Split(',');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(parts[0], out sizeX) || !int.TryParse(parts[1], out sizeY))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                loader.ReadLine();

                myWorld = new World(new Plot[sizeX, sizeY]);

                string myName = loader.ReadLine();
                string myClass = loader.ReadLine();

                if (myClass != "Warrior" && myClass != "Magician" && myClass != "Rogue")
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }

                int myLevel, myExp, myLocationX, myLocationY, myBaseAtk, myBaseDef, myBaseDex, myBaseInt, myMaxHP, myCurrentHP, myMaxMP;
                double myCurrentMP;

                if (!int.TryParse(loader.ReadLine(), out myLevel))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myExp))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myLocationX))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myLocationY))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myBaseAtk))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myBaseDef))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myBaseDex))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myBaseInt))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myMaxHP))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myCurrentHP))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!int.TryParse(loader.ReadLine(), out myMaxMP))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
                if (!double.TryParse(loader.ReadLine(), out myCurrentMP))
                {
                    Console.WriteLine("Failed to load!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }

                switch (myClass)
                {
                    case "Warrior":
                        myPlayer = new Player(myName, myWorld, Classes.Warrior, myLevel, myExp, myLocationX, myLocationY, myMaxHP, myCurrentHP, myMaxMP, myCurrentMP);
                        break;

                    case "Magician":
                        myPlayer = new Player(myName, myWorld, Classes.Magician, myLevel, myExp, myLocationX, myLocationY, myMaxHP, myCurrentHP, myMaxMP, myCurrentMP);
                        break;
                    case "Rogue":
                        myPlayer = new Player(myName, myWorld, Classes.Rogue, myLevel, myExp, myLocationX, myLocationY, myMaxHP, myCurrentHP, myMaxMP, myCurrentMP);
                        break;
                }

                loader.ReadLine();

                int itemID, itemCount;

                temp = loader.ReadLine();
                while (temp != "end_inventory")
                {
                    parts = temp.Split(',');

                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Failed to load!");
                        Console.ReadKey();
                        stopMelody = true;
                        return;
                    }
                    if (!int.TryParse(parts[0], out itemID) || !int.TryParse(parts[1], out itemCount))
                    {
                        Console.WriteLine("Failed to load!");
                        Console.ReadKey();
                        stopMelody = true;
                        return;
                    }

                    myPlayer.MyInventory.Add(new MyItem(itemID, itemCount));
                    temp = loader.ReadLine();
                }
                loader.ReadLine();

                temp = loader.ReadLine();
                parts = temp.Split(',');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (!int.TryParse(parts[i], out itemID))
                    {
                        Console.WriteLine("Failed to load!");
                        Console.ReadKey();
                        stopMelody = true;
                        return;
                    }
                    myPlayer.MySpellBook.Add(new Spell(itemID));
                }
                loader.ReadLine();

                string[] parts1, parts2, parts3;
                animalTemp = new List<Animal>();

                temp = loader.ReadLine();
                while (temp != "end_world")
                {
                    parts = temp.Split(';');
                    parts1 = parts[0].Split(',');

                    switch (parts[1])
                    {
                        case "Cliffs":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.Cliffs);
                            break;
                        case "Ocean":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.Ocean);
                            break;
                        case "RockyMountains":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.RockyMountains);
                            break;
                        case "Beach":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.Beach);
                            break;
                        case "Hills":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.Hills);
                            break;
                        case "Planes":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.Planes);
                            break;
                        case "Forest":
                            myWorld.Surface[int.Parse(parts1[0]), int.Parse(parts1[1])] = new Plot(Landscapes.Forest);
                            break;
                        default:
                            Console.WriteLine("Failed to load!");
                            Console.ReadKey();
                            stopMelody = true;
                            return;
                    }

                    parts2 = parts[2].Split('.');
                    for (int i = 0; i < parts2.Length; i++)
                    {
                        parts3 = parts2[i].Split('-');
                        if (parts3[0] != "")
                            switch (parts3[2])
                            {
                                case "Peaceful":
                                    animalTemp.Add(new Animal(int.Parse(parts3[0]), myWorld, int.Parse(parts1[0]), int.Parse(parts1[1]), int.Parse(parts3[1]), Status.Peaceful));
                                    break;
                                case "Hostile":
                                    animalTemp.Add(new Animal(int.Parse(parts3[0]), myWorld, int.Parse(parts1[0]), int.Parse(parts1[1]), int.Parse(parts3[1]), Status.Hostile));
                                    break;
                                case "Dead":
                                    animalTemp.Add(new Animal(int.Parse(parts3[0]), myWorld, int.Parse(parts1[0]), int.Parse(parts1[1]), int.Parse(parts3[1]), Status.Dead));
                                    break;
                            }
                    }

                    temp = loader.ReadLine();
                }

                AnimalList = animalTemp.ToArray();

                #endregion
            }

            else
            {
                #region startregion

                Console.Write("Enter a world seed (-1 for a random seed): ");
                string myseedstr = Console.ReadLine();
                int myseednum;
                while (!int.TryParse(myseedstr, out myseednum))
                {
                    Console.WriteLine("You entered a string which is not an int.");
                    Console.Write("Enter a world seed (-1 for a random seed): ");
                    myseedstr = Console.ReadLine();
                }

                DateTime myDate = DateTime.Now;
                myWorld = new World(myseednum);
                Console.WriteLine(DateTime.Now.Millisecond - myDate.Millisecond);

                Console.Write("Enter the player name: ");
                string myname = Console.ReadLine();
                myPlayer = new Player(myname, myWorld);

                myWorld.MyPlayer = myPlayer;

                /*Console.WriteLine("Player Info:");
                myPlayer.PrintInfo();

                Console.WriteLine("World Map:");
                PrintMap(myPlayer, myWorld);*/

                AnimalList = new Animal[myWorld.Surface.GetLength(0) * myWorld.Surface.GetLength(1) / 40];
                resurrectAnimalBytes = new byte[AnimalList.Length * myWorld.Surface.GetLength(0) * myWorld.Surface.GetLength(1)];
                animalBytes = new byte[AnimalList.Length][];
                new Random().NextBytes(resurrectAnimalBytes);
                for (int i = 0; i < AnimalList.Length; i++)
                {
                    animalBytes[i] = new byte[myWorld.Surface.GetLength(0) * myWorld.Surface.GetLength(1)];
                    for (int j = 0; j < myWorld.Surface.GetLength(0) * myWorld.Surface.GetLength(1); j++)
                        animalBytes[i][j] = resurrectAnimalBytes[i * myWorld.Surface.GetLength(0) * myWorld.Surface.GetLength(1) + j];
                    AnimalList[i] = new Animal(new Random().Next(2) + 1, myWorld, animalBytes[i]);
                }
                resurrectAnimalBytes = null;
                animalBytes = null;

                #endregion
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            while (true)
            {
                Console.WriteLine("Player Info:");
                myPlayer.PrintInfo();
                Console.WriteLine();

                if (myPlayer.MyCave == null)
                {
                    Console.WriteLine("World Map:");
                    PrintMap(myPlayer, AnimalList, myWorld);
                }
                else
                {
                    Console.WriteLine("Cave Map:");
                    PrintCaveMap(myPlayer, myPlayer.MyCave);
                }
                Console.WriteLine();

                Console.WriteLine("What will you do? ");

                Console.ForegroundColor = ConsoleColor.White;

                oldinput = input;
                input = "";
                ConsoleKeyInfo c = Console.ReadKey();
                while (c.Key != ConsoleKey.Enter)
                {
                    #region specialcontrols

                    switch (c.Key)
                    {
                        case ConsoleKey.RightArrow:
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            string tempAuto = Action.AutoComplete(input);
                            if (tempAuto != null)
                            {
                                Console.Write(tempAuto);
                                input += tempAuto;
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            Console.SetCursorPosition(0, Console.CursorTop);
                            Console.Write(oldinput);
                            input = oldinput;
                            break;

                        case ConsoleKey.DownArrow:
                        case ConsoleKey.LeftArrow:
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            break;

                        case ConsoleKey.Backspace:
                            if (input != "")
                            {
                                Console.Write(" ");
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                input = input.Substring(0, input.Length - 1);
                            }
                            break;

                        case ConsoleKey.Tab:
                            Console.SetCursorPosition(input.Length, Console.CursorTop);
                            break;

                        default:
                            input += c.KeyChar;
                            break;
                    }

                    c = Console.ReadKey();

                    #endregion
                }

                //input = Console.ReadLine();
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Gray;
                isAct = Action.Start(input, myPlayer);
                Console.WriteLine();

                if (isAct)
                {
                    for (int i = 0; i < AnimalList.Length; i++)
                    {
                        if (AnimalList[i].CurrentStatus == Status.Dead)
                        {
                            resurrectAnimalBytes = new byte[myWorld.Surface.GetLength(0) * myWorld.Surface.GetLength(1)];
                            new Random().NextBytes(resurrectAnimalBytes);
                            AnimalList[i] = new Animal(4, myWorld, resurrectAnimalBytes);
                        }
                        AnimalList[i].Action();
                    }

                    myPlayer.CurrentMP = Math.Min(myPlayer.MaxMP, myPlayer.CurrentMP + myPlayer.BaseInt / 10.0);
                }

                if (myPlayer.CurrentHP <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("You Died! Game Over!");
                    Console.ReadKey();
                    stopMelody = true;
                    return;
                }
            }

            //Console.ReadKey();
        }
    }
}
