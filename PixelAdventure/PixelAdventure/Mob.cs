using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PixelAdventure
{
    enum Behaviour { Hostile, Normal, Peaceful };
    enum Status { Peaceful, Hostile, Dead };

    class MobDrop
    {
        public int DropID;
        public int[][] Chances;

        public MobDrop(int DropID, int[][] Chances)
        {
            this.DropID = DropID;
            this.Chances = Chances;
        }
    }

    class Animals
    {
        public static Mob[] AnimalArray = new Mob[] {
            new Mob (0, "Bear", 20, 20, 30, Behaviour.Normal, Status.Peaceful, new MobDrop[] { 
                new MobDrop(5, new int[][] { new int[] {3,4}, new int[] {4,4}, new int[] {5,2} }), 
                new MobDrop(7, new int[][] { new int[] {5,10} }) }),
            
            new Mob (1, "Pigeon", 6, 3, 3, Behaviour.Peaceful, Status.Peaceful, new MobDrop[] { 
                new MobDrop(3, new int[][] { new int[] {1,2}, new int[] {2,7}, new int[] {3,1} }), 
                new MobDrop(7, new int[][] { new int[] {2,10} }) }),
            
            new Mob (2, "Chicken", 4, 2, 2, Behaviour.Peaceful, Status.Peaceful, new MobDrop[] { 
                new MobDrop(3, new int[][] { new int[] {1,7}, new int[] {2,3} }), 
                new MobDrop(7, new int[][] { new int[] {2,10} }) }),
            
            new Mob (3, "Mouse", 2, 1, 1, Behaviour.Peaceful, Status.Peaceful, new MobDrop[] {
                new MobDrop(5, new int[][] { new int[] {1,10} }), 
                new MobDrop(7, new int[][] { new int[] {1,10} }) }),
            
            new Mob (4, "Raven", 6, 6, 9, Behaviour.Normal, Status.Peaceful, new MobDrop[] { 
                new MobDrop(3, new int[][] { new int[] {3,3}, new int[] {4,4}, new int[] {5,3} }), 
                new MobDrop(7, new int[][] { new int[] {2,10} }) }),
            
            new Mob (5, "Snake", 8, 8, 12, Behaviour.Normal, Status.Peaceful, new MobDrop[] {
                new MobDrop(6, new int[][] { new int[] {1,10} }) }),
            
            new Mob (6, "Wolf", 12, 12, 18, Behaviour.Normal, Status.Peaceful, new MobDrop[] {
                new MobDrop(5, new int[][] { new int[] {2,6}, new int[] {3,4} }), 
                new MobDrop(7, new int[][] { new int[] {3,10} }) }),
        };
    }

    class Mob
    {
        public string Name;
        public int ID;
        public int HP;
        public int Atk;
        //public int Def;
        public int Exp;
        public Behaviour Act;
        public Status CurrentStatus;
        public MobDrop[] ChancesStats;
        //public Landscapes[] Locations;
        //public 

        //public int Speed;
        //public World MyWorld;
        //public int LocationX;
        //public int LocationY;

        public Mob(int ID, string Name, int HP, int Atk, int Exp, Behaviour Act, Status CurrentStatus, MobDrop[] Chances)
        {
            this.ID = ID;
            this.Name = Name;
            this.HP = HP;
            this.Atk = Atk;
            this.Exp = Exp;
            this.Act = Act;
            this.CurrentStatus = CurrentStatus;
            this.ChancesStats = Chances;
        }
    }

    class Animal : Mob
    {
        public World MyWorld;
        public int LocationX;
        public int LocationY; //Don't forget the case of a dead animal that is in (-1,-1)!
        public int[][] Drops;

        public int[] SetLocation(World MyWorld, byte[] AnimalRand)
        {
            int[] loc = new int[2];
            loc[0] = AnimalRand[0] % MyWorld.Surface.GetLength(0);
            loc[1] = AnimalRand[1] % MyWorld.Surface.GetLength(1);
            Landscapes myLoc = MyWorld.Surface[loc[0], loc[1]].Location;

            int count = 1;
            while (myLoc != Landscapes.Hills && myLoc != Landscapes.Planes && myLoc != Landscapes.Forest)
            {
                loc[0] = AnimalRand[2 * count] % MyWorld.Surface.GetLength(0);
                loc[1] = AnimalRand[2 * count + 1] % MyWorld.Surface.GetLength(1);
                myLoc = MyWorld.Surface[loc[0], loc[1]].Location;
                count++;
            }

            return loc;

        }
        public int[][] SetDrops(MobDrop[] MyChances)
        {
            Drops = new int[MyChances.Length][];
            int ChanceSum, RandNum, ChanceCount;

            for (int i = 0; i < MyChances.Length; i++)
            {
                ChanceCount = 0;
                ChanceSum = 0;
                for (int j = 0; j < MyChances[i].Chances.Length; j++)
                    ChanceSum += MyChances[i].Chances[j][1];

                RandNum = new Random().Next(ChanceSum);
                for (ChanceCount = 0; RandNum >= MyChances[i].Chances[ChanceCount][1]; ChanceCount++)
                    RandNum -= MyChances[i].Chances[ChanceCount][1];

                Drops[i] = new int[2] { MyChances[i].DropID, MyChances[i].Chances[ChanceCount][0] };
            }

            return Drops;
        }

        public Animal(int ID, World MyWorld, byte[] AnimalRand)
            : base(ID, Animals.AnimalArray[ID].Name, Animals.AnimalArray[ID].HP, Animals.AnimalArray[ID].Atk, Animals.AnimalArray[ID].Exp, Animals.AnimalArray[ID].Act, Status.Peaceful, Animals.AnimalArray[ID].ChancesStats)
        {
            this.MyWorld = MyWorld;
            int[] loc = SetLocation(MyWorld, AnimalRand);
            LocationX = loc[0];
            LocationY = loc[1];
            this.MyWorld.Surface[LocationX, LocationY].Entities.Add(this);
            this.Drops = SetDrops(base.ChancesStats);
        }

        public Animal(int ID, World MyWorld, int locationX, int locationY, int HP, Status myStatus)
            : base(ID, Animals.AnimalArray[ID].Name, HP, Animals.AnimalArray[ID].Atk, Animals.AnimalArray[ID].Exp, Animals.AnimalArray[ID].Act, myStatus, Animals.AnimalArray[ID].ChancesStats)
        {
            this.MyWorld = MyWorld;
            LocationX = locationX;
            LocationY = locationY;
            this.MyWorld.Surface[LocationX, LocationY].Entities.Add(this);
            this.Drops = SetDrops(base.ChancesStats);
        }

        public void Action()
        {
            if (this.CurrentStatus == Status.Dead)
                return;

            if (this.MyWorld.MyPlayer == null)
            {
                Move();
                return;
            }

            if (this.CurrentStatus == Status.Hostile && this.LocationX == this.MyWorld.MyPlayer.LocationX && this.LocationY == this.MyWorld.MyPlayer.LocationY)
                Attack();
            else
                Move();
        }

        bool CheckCanMove(int Direction)
        {
            Landscapes NextLocation;
            switch (Direction)
            {
                case 0:
                    NextLocation = this.MyWorld.Surface[this.LocationX - 1, this.LocationY].Location;
                    break;

                case 1:
                    NextLocation = this.MyWorld.Surface[this.LocationX + 1, this.LocationY].Location;
                    break;

                case 2:
                    NextLocation = this.MyWorld.Surface[this.LocationX, this.LocationY + 1].Location;
                    break;

                default:
                    NextLocation = this.MyWorld.Surface[this.LocationX, this.LocationY - 1].Location;
                    break;
            }
            return (NextLocation != Landscapes.Cliffs && NextLocation != Landscapes.Ocean && NextLocation != Landscapes.RockyMountains);
        }
        void Move()
        {
            if (LocationX < 0 || LocationY < 0)
                return;

            if (new Random().Next(4) < 3)
                return;

            Thread.Sleep(4);

            switch (new Random().Next(4))
            {
                case 0:
                    if (CheckCanMove(0))
                    {
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Remove(this);
                        this.LocationX--;
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Add(this);
                    }
                    else
                        return;
                    break;

                case 1:
                    if (CheckCanMove(1))
                    {
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Remove(this);
                        this.LocationX++;
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Add(this);
                    }
                    else
                        return;
                    break;

                case 2:
                    if (CheckCanMove(2))
                    {
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Remove(this);
                        this.LocationY++;
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Add(this);
                    }
                    else
                        return;
                    break;

                default:
                    if (CheckCanMove(3))
                    {
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Remove(this);
                        this.LocationY--;
                        this.MyWorld.Surface[LocationX, LocationY].Entities.Add(this);
                    }
                    else
                        return;
                    break;
            }

            Thread.Sleep(4);
        }

        void Attack()
        {
            int Damage = new Random().Next(this.Atk / 6, this.Atk / 3);
            int Defence = 0;
            if (this.MyWorld.MyPlayer.MyArmor == null)
                Defence = new Random().Next(this.MyWorld.MyPlayer.BaseDef / 10, this.MyWorld.MyPlayer.BaseDef / 6);
            else
                Defence = new Random().Next((this.MyWorld.MyPlayer.BaseDef + this.MyWorld.MyPlayer.MyArmor.Def) / 8, (this.MyWorld.MyPlayer.BaseDef + this.MyWorld.MyPlayer.MyArmor.Def) / 5);
            Damage = Math.Max(Damage - Defence, 0);
            Console.WriteLine("The " + Name + " damaged you by " + Damage + " points.");
            this.MyWorld.MyPlayer.CurrentHP -= Damage;
        }
    }
}
