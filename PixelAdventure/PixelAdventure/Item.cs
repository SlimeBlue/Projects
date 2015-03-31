using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelAdventure
{
    enum ItemType { Weapon, Tool, Armor, Food } ;
    enum StackType { Stackable, NotStackable } ;

    class ItemTable
    {
        public static Item[] MyTable = new Item[] {
            new Item(0, "Wooden Sword", StackType.NotStackable, new TypeProps[] { new TypeProps(ItemType.Weapon, new string[] { "MinAtk=1", "MaxAtk=3", "AtkSpd=5" }) }, new int[] { 0, 0, 5 }),
            new Item(1, "Leather Armor", StackType.NotStackable, new TypeProps[] { new TypeProps(ItemType.Armor, new string[] { "Def=3", "Spd=1" }) }, new int[] { 0, 3, 0 }),
            new Item(2, "Iron Axe", StackType.NotStackable, new TypeProps[] { new TypeProps(ItemType.Weapon, new string[] { "MinAtk=6", "MaxAtk=15", "AtkSpd=2"}), new TypeProps(ItemType.Tool, new string[] { "Use=Tree" }) }, new int[] { 0, 5, 0 }),
            new Item(3, "Feather", StackType.Stackable, null, new int[] { 0, 0, 1 }),
            new Item(4, "Apple", StackType.Stackable, new TypeProps[] { new TypeProps (ItemType.Food, new string[] { "Restore=3" }) }, new int[] { 0, 0, 3 }),
            new Item(5, "Leather", StackType.Stackable, null, new int[] { 0, 0, 10 }),
            new Item(6, "Snakeskin", StackType.Stackable, null, new int[] { 0, 0, 6 }),
            new Item(7, "Meat", StackType.Stackable, new TypeProps[] { new TypeProps (ItemType.Food, new string[] { "Restore=5" }) }, new int[] { 0, 0, 6 }),
            new Item(8, "Key", StackType.Stackable, null, new int[] { 0, 1, 0 }),
            new Item(9, "Short Sword", StackType.NotStackable, new TypeProps[] { new TypeProps(ItemType.Weapon, new string[] { "MinAtk=9", "MaxAtk=11", "AtkSpd=4" }) }, new int[] { 0, 3, 0 }),
            new Item(10, "Glass Bottle", StackType.Stackable, null, new int[] { 0, 0, 3 }),
            new Item(11, "Gold Coin", StackType.Stackable, null, new int[] { 1, 0, 0 }),
            new Item(12, "Silver Coin", StackType.Stackable, null, new int[] { 0, 1, 0 }),
            new Item(13, "Copper Coin", StackType.Stackable, null, new int[] { 0, 0, 1 } ),
            new Item(14, "Wooden Dagger", StackType.NotStackable, new TypeProps[] { new TypeProps(ItemType.Weapon, new string[] { "MinAtk=1", "MaxAtk=1", "AtkSpd=8" }) }, new int[] { 0, 0, 3 }),
            new Item(15, "Iron Dagger", StackType.NotStackable, new TypeProps[] { new TypeProps(ItemType.Weapon, new string[] { "MinAtk=6", "MaxAtk=8", "AtkSpd=8" }), new TypeProps(ItemType.Tool, new string[] { "Use=Leather;Snakeskin" }) }, new int[] { 0, 2, 5 })
        };
    }

    class TypeProps
    {
        public ItemType Type;
        public string[] Props;

        public TypeProps(ItemType Type, string[] Props)
        {
            this.Type = Type;
            this.Props = Props;
        }
    }

    class Item
    {
        public int ID;
        public string Name;
        public StackType IsStackable;
        public TypeProps[] Types;
        //public int Amount;
        public int[] Worth;

        public Item(int ID, string Name, StackType IsStackable, TypeProps[] Types, int[] Worth) //Defines an item info object
        {
            this.ID = ID;
            this.Name = Name;
            this.IsStackable = IsStackable;
            this.Types = Types;
            //this.Amount = Amount;
            this.Worth = Worth;
        }

        public override string ToString()
        {
            return ("Name: " + Name + "; ID: " + ID /*+ "; Amount: " + Amount*/ + ".");
        }
    }

    class MyItem
    {
        public int ID;
        public string Name;
        public StackType IsStackable;
        public TypeProps[] Types;
        public int Amount;
        public int[] Worth;

        public MyItem(int ID, int Amount) //Defines an item to be used in the game
        {
            this.ID = ID;
            this.Name = ItemTable.MyTable[ID].Name;
            this.IsStackable = ItemTable.MyTable[ID].IsStackable;
            this.Types = ItemTable.MyTable[ID].Types;
            this.Amount = Amount;
            this.Worth = ItemTable.MyTable[ID].Worth;
        }

        public Item ConvertToItem()
        {
            return new Item(ID, Name, IsStackable, Types, Worth);
        }
    }

    class Weapon
    {
        public int ID;
        public string Name;
        public TypeProps[] Types;
        public int[] Worth;
        public int MinAtk;
        public int MaxAtk;
        public int AtkSpd;

        /*public Weapon(string Name, int ID, int Amount, int MinAtk, int MaxAtk, int AtkSpeed) //Defines a weapon 
            : base(Name, ID, Amount)
        {
            this.Amount = 1;
            this.MinAtk = MinAtk;
            this.MaxAtk = MaxAtk;
            this.AtkSpeed = AtkSpeed;
        }*/

        public Weapon(int ID)
        {
            int PropCount;

            this.ID = ID;
            this.Name = ItemTable.MyTable[ID].Name;
            this.Types = ItemTable.MyTable[ID].Types;
            this.Worth = ItemTable.MyTable[ID].Worth;

            for (PropCount = 0; PropCount < ItemTable.MyTable[ID].Types.Length; PropCount++)
                if (ItemTable.MyTable[ID].Types[PropCount].Type == ItemType.Weapon)
                    break;

            if (PropCount == ItemTable.MyTable[ID].Types.Length)
            {
                this.MinAtk = 0;
                this.MaxAtk = 0;
                this.AtkSpd = 0;
            }
            else
            {
                this.MinAtk = int.Parse(ItemTable.MyTable[ID].Types[PropCount].Props[0].Split('=')[1]);
                this.MaxAtk = int.Parse(ItemTable.MyTable[ID].Types[PropCount].Props[1].Split('=')[1]);
                this.AtkSpd = int.Parse(ItemTable.MyTable[ID].Types[PropCount].Props[2].Split('=')[1]);
            }
        }

        public override string ToString()
        {
            return ("Name: " + Name + "; ID: " + ID /*+ "; Amount: " + Amount*/ + "; Attack: " + MinAtk + "~" + MaxAtk + "; Speed: " + AtkSpd + ".");
        }
    }

    class Armor
    {
        public int ID;
        public string Name;
        public TypeProps[] Types;
        public int[] Worth;
        public int Def;
        public int Spd;

        /*public Armor(string Name, int ID, int Amount, int Def, int Speed) //Defines an armor
            : base(Name, ID, Amount)
        {
            this.Amount = 1;
            this.Def = Def;
            this.Speed = Speed;
        }*/

        public Armor(int ID)
        {
            int PropCount;

            this.ID = ID;
            this.Name = ItemTable.MyTable[ID].Name;
            this.Types = ItemTable.MyTable[ID].Types;
            this.Worth = ItemTable.MyTable[ID].Worth;

            for (PropCount = 0; PropCount < ItemTable.MyTable[ID].Types.Length; PropCount++)
                if (ItemTable.MyTable[ID].Types[PropCount].Type == ItemType.Armor)
                    break;

            if (PropCount == ItemTable.MyTable[ID].Types.Length)
            {
                this.Def = 0;
                this.Spd = 0;
            }
            else
            {
                this.Def = int.Parse(ItemTable.MyTable[ID].Types[PropCount].Props[0].Split('=')[1]);
                this.Spd = int.Parse(ItemTable.MyTable[ID].Types[PropCount].Props[1].Split('=')[1]);
            }
        }
    }

    enum ToolUse { Mining, Digging, Chopping, Hoeing, Error };

    class Tool
    {
        public int ID;
        public string Name;
        public TypeProps[] Types;
        public int[] Worth;
        public ToolUse MyUse;

        public Tool(int ID)
        {
            int PropCount;

            this.ID = ID;
            this.Name = ItemTable.MyTable[ID].Name;
            this.Types = ItemTable.MyTable[ID].Types;
            this.Worth = ItemTable.MyTable[ID].Worth;

            for (PropCount = 0; PropCount < ItemTable.MyTable[ID].Types.Length; PropCount++)
                if (ItemTable.MyTable[ID].Types[PropCount].Type == ItemType.Armor)
                    break;

            if (PropCount == ItemTable.MyTable[ID].Types.Length)
            {
                this.MyUse = ToolUse.Error;
            }
            else
            {
                this.MyUse = (ToolUse)Enum.Parse(typeof(ToolUse), ItemTable.MyTable[ID].Types[PropCount].Props[0].Split('=')[1]);
            }
        }
    }

    class Inventory : List<MyItem>
    {
        public Inventory() : base() { }
        public Inventory(int Size) : base(Size) { }
    }
}
