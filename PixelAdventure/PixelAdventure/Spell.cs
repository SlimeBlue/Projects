using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelAdventure
{
    enum SpellType { Attack, Defence, Heal, Effect, Enviromental, Other } ;
    enum Distance { Touch, Close, Medium, Far } ;
    enum Target { Self, Ally, Foe } ;

    class SpellTable
    {
        public static Spell[] MyTable = new Spell[] {
            new Spell(0, "Fireball", SpellType.Attack, 1, 5, 3, Distance.Medium, 0, Target.Foe),
            new Spell(1, "ManaShield", SpellType.Defence, 2, 10, 3, Distance.Medium, 0, Target.Self),
            new Spell(2, "HealingTouch", SpellType.Heal, 4, 10, 5, Distance.Medium, 0, Target.Ally),
            new Spell(3, "FrostTouch", SpellType.Attack, -1, 10, 8, Distance.Medium, 0, Target.Foe)
        };
    }

    class SpellBook : List<Spell>
    {
        public SpellBook() : base() { }
        public SpellBook(int size) : base(size) { }
    }

    class Spell
    {
        public int ID;
        public string Name;
        public SpellType MyType;
        public int Level; //if -1 means it can't be obtainable through leveling up
        public int ManaCost;
        public int Amount;
        public Distance Dis;
        public int Duration;
        public Target Tar;

        public Spell(int ID, string Name, SpellType MyType, int Level, int ManaCost, int Amount, Distance Dis, int Duration, Target Tar)
        {
            this.ID = ID;
            this.Name = Name;
            this.MyType = MyType;
            this.Level = Level;
            this.ManaCost = ManaCost;
            this.Amount = Amount;
            //this.Types = Types;
            this.Dis = Dis;
            this.Duration = Duration;
            this.Tar = Tar;
        }

        public Spell(int ID)
        {
            int index = -1;
            for (int i = 0; i < SpellTable.MyTable.Length; i++)
            {
                if (SpellTable.MyTable[i].ID == ID)
                {
                    index = ID;
                    break;
                }
            }

            if (index == -1)
            {
                this.ID = -1;
                return;
            }

            this.ID = ID;
            this.Name = SpellTable.MyTable[index].Name;
            this.MyType = SpellTable.MyTable[index].MyType;
            this.Level = SpellTable.MyTable[index].Level;
            this.ManaCost = SpellTable.MyTable[index].ManaCost;
            this.Amount = SpellTable.MyTable[index].Amount;
            this.Dis = SpellTable.MyTable[index].Dis;
            this.Duration = SpellTable.MyTable[index].Duration;
            this.Tar = SpellTable.MyTable[index].Tar;
        }

        public override string ToString()
        {
            return ("Name: " + Name + "; ID: " + ID + ".");
        }
    }
}
