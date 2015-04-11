using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PixelAdventure
{
    enum Classes { Warrior, Magician, Rogue };

    class ClassArray
    {
        public int HpBonus;
        public int MpBonus;
        public int Atk;
        public int Def;
        public int Int;
        public int Dex;

        public ClassArray(int HpBonus, int MpBonus, int Atk, int Def, int Int, int Dex)
        {
            this.HpBonus = HpBonus;
            this.MpBonus = MpBonus;
            this.Atk = Atk;
            this.Def = Def;
            this.Int = Int;
            this.Dex = Dex;
        }
    }

    class Exp
    {
        public int Level;
        public int ExpNext;
        public ClassArray[] Stats;

        public Exp(int Level, int ExpNext, ClassArray Warrior, ClassArray Magician, ClassArray Rogue)
        {
            this.Level = Level;
            this.ExpNext = ExpNext;
            Stats = new ClassArray[3];
            Stats[0] = Warrior;
            Stats[1] = Magician;
            Stats[2] = Rogue;
        }
    }

    class ExpTable
    {
        public static Exp[] MyTable = new Exp[] {
            new Exp(1,15,new ClassArray(2,0,10,7,4,4),new ClassArray(1,1,6,4,10,6), new ClassArray(1,0,7,4,6,10)),
            new Exp(2,25,new ClassArray(2,0,12,8,5,5),new ClassArray(1,1,7,5,12,7), new ClassArray(2,0,8,5,7,12)),
            new Exp(3,40,new ClassArray(3,0,14,9,5,5),new ClassArray(1,2,8,5,14,8), new ClassArray(2,0,9,5,8,14)),
            new Exp(4,70,new ClassArray(3,0,16,10,6,6),new ClassArray(2,3,9,6,16,9), new ClassArray(2,0,10,6,9,16)),
            new Exp(5,120,new ClassArray(3,0,18,11,6,6),new ClassArray(2,4,10,6,18,10), new ClassArray(3,0,11,6,10,18)),
            new Exp(6,200,new ClassArray(4,0,20,12,7,7),new ClassArray(2,6,10,7,20,10), new ClassArray(3,0,12,7,10,20)),
            new Exp(7,320,new ClassArray(4,0,22,13,7,7),new ClassArray(3,9,11,7,22,11), new ClassArray(3,0,13,7,11,22)),
            new Exp(8,540,new ClassArray(5,0,24,14,8,8),new ClassArray(3,14,12,8,24,12), new ClassArray(3,0,14,8,12,24)),
            new Exp(9,880,new ClassArray(5,0,26,15,8,8),new ClassArray(3,20,13,8,26,13), new ClassArray(4,0,15,8,13,26)),
            new Exp(10,1500,new ClassArray(6,0,28,16,9,9),new ClassArray(3,28,13,9,28,13), new ClassArray(4,0,16,9,13,28)),
            new Exp(11,-1,new ClassArray(7,0,30,17,9,9),new ClassArray(3,42,14,9,30,14), new ClassArray(4,0,17,9,14,30))
        };
    }

    class Player
    {
        public string Name;
        public int Level;
        public int Exp;
        public Classes Class;
        public World MyWorld;
        public Cave MyCave;
        public int LocationX;
        public int LocationY;
        public int MaxHP;
        public int CurrentHP;
        public int MaxMP;
        public double CurrentMP;
        public int BaseAtk;
        public int BaseDef;
        public int BaseInt;
        public int BaseDex;
        public Inventory MyInventory;
        public Weapon MyWeapon;
        public Tool MyTool;
        public Armor MyArmor;
        public SpellBook MySpellBook;

        public Player(string Name, World MyWorld) //Defines a player
        {
            this.Name = Name;
            this.Level = 1;
            this.Exp = 0;
            this.Class = GetClass();
            this.MyWorld = MyWorld;
            this.MyCave = null;
            this.MyInventory = new Inventory();
            this.MyWeapon = null;
            this.MyArmor = null;
            this.MySpellBook = new SpellBook();

            SetPlayerLocation();

            switch (Class)
            {
                case Classes.Warrior:
                    MaxHP = new Random().Next(7, 11);
                    CurrentHP = MaxHP;
                    MaxMP = 0;
                    CurrentMP = 0.0;
                    BaseAtk = 10;
                    BaseDef = 7;
                    BaseInt = 4;
                    BaseDex = 4;
                    MyInventory.Add(new MyItem(0, 1));
                    break;

                case Classes.Magician:
                    MaxHP = new Random().Next(4, 6);
                    CurrentHP = MaxHP;
                    MaxMP = 10;
                    CurrentMP = 10.0;
                    BaseAtk = 7;
                    BaseDef = 4;
                    BaseInt = 10;
                    BaseDex = 6;
                    foreach (Spell s in SpellTable.MyTable)
                        if (s.Level == 1)
                            MySpellBook.Add(s);
                    break;

                case Classes.Rogue:
                    MaxHP = new Random().Next(4, 8);
                    CurrentHP = MaxHP;
                    MaxMP = 0;
                    CurrentMP = 0.0;
                    BaseAtk = 6;
                    BaseDef = 4;
                    BaseInt = 6;
                    BaseDex = 10;
                    MyInventory.Add(new MyItem(14, 1));
                    break;
            }
        }

        public Player(string Name, World MyWorld, Classes MyClass, int Level, int EXP, int LocationX, int LocationY, int MaxHP, int CurrentHP, int MaxMP, double CurrentMP)
        {
            this.Name = Name;
            this.Level = Level;
            this.Exp = EXP;
            this.Class = MyClass;
            this.MyWorld = MyWorld;
            this.MyCave = null;
            this.MyInventory = new Inventory();
            this.MyWeapon = null;
            this.MyArmor = null;
            this.MySpellBook = new SpellBook();
            this.LocationX = LocationX;
            this.LocationY = LocationY;

            int LevelIndex = -1;
            switch (Class)
            {
                case Classes.Warrior:
                    this.MaxHP = MaxHP;
                    this.CurrentHP = CurrentHP;
                    this.MaxMP = MaxMP;
                    this.CurrentMP = CurrentMP;

                    for (int i = 0; i < ExpTable.MyTable.Length; i++)
                        if (ExpTable.MyTable[i].Level == Level)
                        {
                            LevelIndex = i;
                            break;
                        }
                    this.BaseAtk = ExpTable.MyTable[LevelIndex].Stats[0].Atk;
                    this.BaseDef = ExpTable.MyTable[LevelIndex].Stats[0].Def;
                    this.BaseInt = ExpTable.MyTable[LevelIndex].Stats[0].Int;
                    this.BaseDex = ExpTable.MyTable[LevelIndex].Stats[0].Dex;

                    MyInventory.Add(new MyItem(0, 1));
                    break;

                case Classes.Magician:
                    this.MaxHP = MaxHP;
                    this.CurrentHP = CurrentHP;
                    this.MaxMP = MaxMP;
                    this.CurrentMP = CurrentMP;
                    
                    for (int i = 0; i < ExpTable.MyTable.Length; i++)
                        if (ExpTable.MyTable[i].Level == Level)
                        {
                            LevelIndex = i;
                            break;
                        }
                    this.BaseAtk = ExpTable.MyTable[LevelIndex].Stats[0].Atk;
                    this.BaseDef = ExpTable.MyTable[LevelIndex].Stats[0].Def;
                    this.BaseInt = ExpTable.MyTable[LevelIndex].Stats[0].Int;
                    this.BaseDex = ExpTable.MyTable[LevelIndex].Stats[0].Dex;

                    foreach (Spell s in SpellTable.MyTable)
                        if (s.Level <= Level)
                            MySpellBook.Add(s);
                    break;

                case Classes.Rogue:
                    this.MaxHP = MaxHP;
                    this.CurrentHP = CurrentHP;
                    this.MaxMP = MaxMP;
                    this.CurrentMP = CurrentMP;
                    
                    for (int i = 0; i < ExpTable.MyTable.Length; i++)
                        if (ExpTable.MyTable[i].Level == Level)
                        {
                            LevelIndex = i;
                            break;
                        }
                    this.BaseAtk = ExpTable.MyTable[LevelIndex].Stats[0].Atk;
                    this.BaseDef = ExpTable.MyTable[LevelIndex].Stats[0].Def;
                    this.BaseInt = ExpTable.MyTable[LevelIndex].Stats[0].Int;
                    this.BaseDex = ExpTable.MyTable[LevelIndex].Stats[0].Dex;

                    MyInventory.Add(new MyItem(14, 1));
                    break;
            }
        }

        static Classes GetClass() //Sets a class according to input
        {
            Console.Write("Choose a class (Warrior, Magician, Rogue): ");
            string str = Console.ReadLine();
            switch (str.ToLower())
            {
                case "warrior":
                    return Classes.Warrior;
                case "magician":
                    return Classes.Magician;
                case "rogue":
                    return Classes.Rogue;
                default:
                    Console.WriteLine("The value you entered is invalid.");
                    return GetClass();
            }

        }

        void SetPlayerLocation() //Sets the location of the player randomly
        {
            int x = new Random().Next(MyWorld.Surface.GetLength(0));
            Thread.Sleep(10);
            int y = new Random().Next(MyWorld.Surface.GetLength(1));
            Thread.Sleep(10);
            Landscapes myLoc = MyWorld.Surface[x, y].Location;

            while (myLoc == Landscapes.Cliffs || myLoc == Landscapes.Ocean || myLoc == Landscapes.RockyMountains)
            {
                x = new Random().Next(MyWorld.Surface.GetLength(0));
                Thread.Sleep(10);
                y = new Random().Next(MyWorld.Surface.GetLength(1));
                Thread.Sleep(10);
                myLoc = MyWorld.Surface[x, y].Location;
            }

            LocationX = x;
            LocationY = y;
        }

        public void PrintInfo() //Prints the info of the player
        {
            //Console.WriteLine("Name: " + Name + "; Class: " + Class + "; Level: " + Level +"; EXP: " + Exp + "; Location: [" + LocationX + "," + LocationY + "]; Landscape: " + MyWorld.Surface[LocationX,LocationY].Location + "; HP: " + CurrentHP + "/" + MaxHP +".");
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Class: " + Class);
            Console.WriteLine("Level: " + Level + ". EXP: " + Exp);
            if (MyCave == null)
                Console.WriteLine("Location: [" + LocationX + "," + LocationY + "]. Landscape: " + MyWorld.Surface[LocationX, LocationY].Location);
            else
                Console.WriteLine("Location: [" + LocationX + "," + LocationY + "]. Landscape: " + MyCave.Layout[LocationX, LocationY].Location);
            if (Class == Classes.Magician)
                Console.WriteLine("ATK: " + BaseAtk + ". DEF: " + BaseDef + ". INT: " + BaseInt + ". DEX: " + BaseDex + ". HP: " + CurrentHP + "/" + MaxHP + ". MP: " + (int)CurrentMP + "/" + MaxMP);
            else
                Console.WriteLine("ATK: " + BaseAtk + ". DEF: " + BaseDef + ". INT: " + BaseInt + ". DEX: " + BaseDex + ". HP: " + CurrentHP + "/" + MaxHP);
        }
    }
}
