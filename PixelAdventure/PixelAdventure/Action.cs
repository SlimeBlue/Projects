using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PixelAdventure
{
    class Action
    {
        public static string[] actions = { "go", "attack", "help", "equip", "bag", "eat", "cast", "spellbook", /*"save", "load",*/ "pick", "drop" };

        public static bool Start(string Input, Player MyPlayer) //Invokes an action
        {
            /*if (Input == null)
            {
                Method = null;
                Parameters = null;
            }

            else
            {
                string[] Parts = Input.Split(' ');
                Type myType = Parts[0].GetType();

                this.Method = myType.GetMethod(Parts[0]);
                Parts.CopyTo(this.Parameters, 1);

                myType = null;
                Parts = null;
            }

            this.MyPlayer = MyPlayer;*/

            if (Input == null)
                return false;
            string[] Parts = Input.Split(' ');
            bool flag = false;
            //string[] Parameters;

            switch (Parts[0].ToLower())
            {
                case "go":
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a direction as an add!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    flag = Go(Parts[1], MyPlayer);
                    Console.WriteLine();
                    return flag;

                case "attack":
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a target!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    if ((int)Parts[1][0] > 96)
                        Parts[1] = (char)((int)Parts[1][0] - 32) + Parts[1].Substring(1);

                    flag = Attack(Parts[1], MyPlayer, MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities);
                    Console.WriteLine();
                    return flag;

                case "help":
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    if (Parts.Length == 2)
                    {
                        flag = Help(Parts[1]);
                        Console.WriteLine();
                        return flag;
                    }

                    else
                    {
                        flag = Help();
                        Console.WriteLine();
                        return flag;
                    }

                case "equip":
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a name of an item!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    flag = Eat(Parts[1], MyPlayer);
                    Console.WriteLine();
                    return flag;

                case "bag":
                    if (Parts.Length > 1)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    else
                    {
                        flag = Bag(MyPlayer);
                        Console.WriteLine();
                        return true;
                    }

                case "eat":
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a name of an item!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    flag = Eat(Parts[1], MyPlayer);
                    Console.WriteLine();
                    return flag;

                case "cast":
                    if (Parts.Length > 3)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a name of a spell!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length == 2)
                        flag = Cast(Parts[1], MyPlayer, null);
                    if (Parts.Length == 3)
                        flag = Cast(Parts[1], MyPlayer, Parts[2]);
                    Console.WriteLine();
                    return flag;

                case "save":
                    Save(MyPlayer);
                    return true;

                case "spellbook":
                    if (Parts.Length > 1)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }
                    else
                    {
                        flag = SBook(MyPlayer);
                        Console.WriteLine();
                        return true;
                    }

                case "pick":
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a name of an item!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    flag = Pick(Parts[1], MyPlayer);
                    Console.WriteLine();
                    return flag;

                case "drop":
                    if (Parts.Length == 1)
                    {
                        Console.WriteLine("This action needs a name of an item!");
                        Console.WriteLine();
                        return false;
                    }
                    if (Parts.Length > 2)
                    {
                        Console.WriteLine("There are too many adds for this action!");
                        Console.WriteLine();
                        return false;
                    }

                    flag = Drop(Parts[1], MyPlayer);
                    Console.WriteLine();
                    return flag;

                default:
                    Console.WriteLine("There is no such an action!");
                    Console.WriteLine();
                    return false;
            }
        }

        public static string AutoComplete(string Input)
        {
            string[] parts = Input.Split(' ');
            if (parts[parts.Length - 1].Length == 0)
                return null;

            switch (parts.Length)
            {
                case 1:
                    string myPart = parts[0];
                    for (int i = 0; i < actions.Length; i++)
                        if (myPart.Length <= actions[i].Length)
                            if (myPart.ToLower() == actions[i].Substring(0, myPart.Length).ToLower())
                                return actions[i].Substring(myPart.Length).ToLower();
                    break;

                case 2:
                    switch (parts[0].ToLower())
                    {
                        case "go":
                            if (parts[1].Length < 5)
                            {
                                if (parts[1].ToLower() == "north".Substring(0, parts[1].Length))
                                    return "north".Substring(parts[1].Length);
                                if (parts[1].ToLower() == "south".Substring(0, parts[1].Length))
                                    return "south".Substring(parts[1].Length);
                                if (parts[1].Length < 4)
                                {
                                    if (parts[1].ToLower() == "west".Substring(0, parts[1].Length))
                                        return "west".Substring(parts[1].Length);
                                    if (parts[1].ToLower() == "east".Substring(0, parts[1].Length))
                                        return "east".Substring(parts[1].Length);
                                }
                            }
                            break;

                        case "attack":
                            for (int i = 0; i < Animals.AnimalArray.Length; i++)
                                if (parts[1].Length < Animals.AnimalArray[i].Name.Length)
                                    if (parts[1].ToLower() == Animals.AnimalArray[i].Name.Substring(0, parts[1].Length).ToLower())
                                        return Animals.AnimalArray[i].Name.Substring(parts[1].Length).ToLower();
                            break;

                        case "help":
                            for (int i = 0; i < actions.Length; i++)
                                if (parts[1].Length < actions[i].Length)
                                    if (parts[1].ToLower() == actions[i].Substring(0, parts[1].Length).ToLower())
                                        return actions[i].Substring(parts[1].Length).ToLower();
                            break;

                        case "eat":
                            for (int i = 0; i < ItemTable.MyTable.Length; i++)
                                if (parts[1].Length < ItemTable.MyTable[i].Name.Length)
                                    if (parts[1].ToLower() == ItemTable.MyTable[i].Name.Substring(0, parts[1].Length).ToLower())
                                        return ItemTable.MyTable[i].Name.Substring(parts[1].Length).ToLower();
                            break;

                        case "cast":
                            for (int i = 0; i < SpellTable.MyTable.Length; i++)
                                if (parts[1].Length < SpellTable.MyTable[i].Name.Length)
                                    if (parts[1].ToLower() == SpellTable.MyTable[i].Name.Substring(0, parts[1].Length).ToLower())
                                        return SpellTable.MyTable[i].Name.Substring(parts[1].Length).ToLower();
                            break;

                        case "pick":
                            for (int i = 0; i < ItemTable.MyTable.Length; i++)
                                if (parts[1].Length < ItemTable.MyTable[i].Name.Length)
                                    if (parts[1].ToLower() == ItemTable.MyTable[i].Name.Substring(0, parts[1].Length).ToLower())
                                        return ItemTable.MyTable[i].Name.Substring(parts[1].Length).ToLower();
                            break;

                        case "drop":
                            for (int i = 0; i < ItemTable.MyTable.Length; i++)
                                if (parts[1].Length < ItemTable.MyTable[i].Name.Length)
                                    if (parts[1].ToLower() == ItemTable.MyTable[i].Name.Substring(0, parts[1].Length).ToLower())
                                        return ItemTable.MyTable[i].Name.Substring(parts[1].Length).ToLower();
                            break;

                        case "equip":
                            for (int i = 0; i < ItemTable.MyTable.Length; i++)
                                if (parts[1].Length < ItemTable.MyTable[i].Name.Length)
                                    if (parts[1].ToLower() == ItemTable.MyTable[i].Name.Substring(0, parts[1].Length).ToLower())
                                        return ItemTable.MyTable[i].Name.Substring(parts[1].Length).ToLower();
                            break;
                    }
                    break;

                case 3:
                    if (parts[0].ToLower() == "cast")
                    {
                        for (int i = 0; i < Animals.AnimalArray.Length; i++)
                            if (parts[2].Length < Animals.AnimalArray[i].Name.Length)
                                if (parts[2].ToLower() == Animals.AnimalArray[i].Name.Substring(0, parts[2].Length).ToLower())
                                    return Animals.AnimalArray[i].Name.Substring(parts[2].Length).ToLower();
                    }
                    break;
            }

            return null;
        }

        /*public void SetAction(string Input)
        {
            string[] Parts = Input.Split(' ');
            Type myType = Parts[0].GetType();

            this.Method = myType.GetMethod(Parts[0]);

            if (Parts.Length == 1)
                Parameters = null;
            else
                Parts.CopyTo(this.Parameters, 1);

            myType = null;
            Parts = null;
        }

        public void Start() //Starts the action
        {
            Method.Invoke(MyPlayer, Parameters);
        }

        public void Abandon() //Abandons the action (but still saves the player data)
        {
            Method = null;
            Parameters = null;
        }*/

        public static bool CheckCanGo(int Direction, Player MyPlayer) //Checks if the player can move in that direction. 0-north, 1-south, 2-east, 3-west
        {
            Landscapes NextLocation;
            switch (Direction)
            {
                case 0:
                    NextLocation = MyPlayer.MyWorld.Surface[MyPlayer.LocationX - 1, MyPlayer.LocationY].Location;
                    break;

                case 1:
                    NextLocation = MyPlayer.MyWorld.Surface[MyPlayer.LocationX + 1, MyPlayer.LocationY].Location;
                    break;

                case 2:
                    NextLocation = MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY + 1].Location;
                    break;

                default:
                    NextLocation = MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY - 1].Location;
                    break;
            }
            return (NextLocation != Landscapes.Cliffs && NextLocation != Landscapes.Ocean && NextLocation != Landscapes.RockyMountains);
        }

        static bool Go(string Direction, Player MyPlayer) //Moves the player if it can
        {
            switch (Direction.ToLower())
            {
                case "north":
                    if (CheckCanGo(0, MyPlayer))
                        MyPlayer.LocationX--;
                    else
                    {
                        Console.WriteLine("You can't go there!");
                        return false;
                    }
                    break;

                case "south":
                    if (CheckCanGo(1, MyPlayer))
                        MyPlayer.LocationX++;
                    else
                    {
                        Console.WriteLine("You can't go there!");
                        return false;
                    }
                    break;

                case "east":
                    if (CheckCanGo(2, MyPlayer))
                        MyPlayer.LocationY++;
                    else
                    {
                        Console.WriteLine("You can't go there!");
                        return false;
                    }
                    break;

                case "west":
                    if (CheckCanGo(3, MyPlayer))
                        MyPlayer.LocationY--;
                    else
                    {
                        Console.WriteLine("You can't go there!");
                        return false;
                    }
                    break;

                default:
                    Console.WriteLine("The add isn't a proper direction!");
                    return false;
            }

            Console.WriteLine("Your location is: [" + MyPlayer.LocationX + "," + MyPlayer.LocationY + "].");
            Console.WriteLine("You are in: " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Name() + ".");
            /*Console.WriteLine("To your north: " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX - 1, MyPlayer.LocationY].Name() + ".");
            Console.WriteLine("To your south: " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX + 1, MyPlayer.LocationY].Name() + ".");
            Console.WriteLine("To your east: " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY + 1].Name() + ".");
            Console.WriteLine("To your west: " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY - 1].Name() + ".");*/

            for (int i = 0; i < MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities.Count; i++)
                Console.WriteLine("There is a " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities[i].Name + " here, it is " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities[i].CurrentStatus + ".");

            for (int i = 0; i < MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Count; i++)
                Console.WriteLine("There are " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[i].Amount + " " + MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[i].Name + " on the ground.");

            return true;
        }

        static bool Attack(string Name, Player MyPlayer, List<Animal> MyList)
        {
            if (MyList.Count == 0) //Might be deleted and only be informed that there is no the specific animal here
            {
                Console.WriteLine("There are no animals here!");
                return false;
            }

            int Damage, Index = -1, ItemIndex;
            bool IsExist = false, isItemExist = false;
            for (int i = 0; i < MyList.Count && !IsExist; i++)
                if (MyList[i].Name.ToLower() == Name.ToLower())
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no animal with this name here!");
                return false;
            }

            if (MyPlayer.MyWeapon == null)
                Damage = new Random().Next(MyPlayer.BaseAtk / 6, MyPlayer.BaseAtk / 3);
            else
                Damage = new Random().Next(MyPlayer.BaseAtk / 4 + MyPlayer.MyWeapon.MinAtk / 3, MyPlayer.BaseAtk / 2 + (MyPlayer.MyWeapon.MaxAtk + 1) / 3);

            Console.WriteLine("You damaged the " + Name + " by " + Damage + " points.");
            MyList[Index].HP -= Damage;

            if (MyList[Index].HP <= 0 && MyList[Index].CurrentStatus != Status.Dead)
            {
                Console.WriteLine("The " + Name + " has died!");
                GainExp(MyPlayer, MyList[Index].Exp);

                MyList[Index].CurrentStatus = Status.Dead;
                MyList[Index].LocationX = -1;
                MyList[Index].LocationY = -1; //Fix at the Mob class the case of (-1,-1)
                //MyList.RemoveAt(Index);
                //Add function of animal's drop.
                for (int i = 0; i < MyList[Index].Drops.Length; i++)
                {
                    if (ItemTable.MyTable[MyList[Index].Drops[i][0]].IsStackable == StackType.Stackable)
                    {
                        for (ItemIndex = 0; ItemIndex < MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Count; ItemIndex++)
                        {
                            if (MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[ItemIndex].ID == MyList[Index].Drops[i][0])
                            {
                                isItemExist = true;
                                break;
                            }
                        }

                        if (isItemExist)
                            MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[ItemIndex].Amount += MyList[Index].Drops[i][1];
                        else
                            MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Add(new MyItem(MyList[Index].Drops[i][0], MyList[Index].Drops[i][1]));
                    }
                    Console.WriteLine("The " + Name + " dropped " + MyList[Index].Drops[i][1] + " " + ItemTable.MyTable[MyList[Index].Drops[i][0]].Name + ".");
                }
                MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities.Remove(MyList[Index]);
            }
            else if (MyList[Index].Act == Behaviour.Normal)
                MyList[Index].CurrentStatus = Status.Hostile;
            return true;
        }

        static bool Attack(string Name, Player MyPlayer, List<Animal> MyList, int Damage)
        {
            if (MyList.Count == 0) //Might be deleted and only be informed that there is no the specific animal here
            {
                Console.WriteLine("There are no animals here!");
                return false;
            }

            int Index = -1, ItemIndex;
            bool IsExist = false, isItemExist = false;
            for (int i = 0; i < MyList.Count && !IsExist; i++)
                if (MyList[i].Name.ToLower() == Name.ToLower())
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no animal with this name here!");
                return false;
            }

            Console.WriteLine("You damaged the " + Name + " by " + Damage + " points.");
            MyList[Index].HP -= Damage;

            if (MyList[Index].HP <= 0 && MyList[Index].CurrentStatus != Status.Dead)
            {
                Console.WriteLine("The " + Name + " has died!");
                GainExp(MyPlayer, MyList[Index].Exp);

                MyList[Index].CurrentStatus = Status.Dead;
                MyList[Index].LocationX = -1;
                MyList[Index].LocationY = -1; //Fix at the Mob class the case of (-1,-1)
                //MyList.RemoveAt(Index);
                //Add function of animal's drop.
                for (int i = 0; i < MyList[Index].Drops.Length; i++)
                {
                    if (ItemTable.MyTable[MyList[Index].Drops[i][0]].IsStackable == StackType.Stackable)
                    {
                        for (ItemIndex = 0; ItemIndex < MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Count; ItemIndex++)
                        {
                            if (MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[ItemIndex].ID == MyList[Index].Drops[i][0])
                            {
                                isItemExist = true;
                                break;
                            }
                        }

                        if (isItemExist)
                            MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[ItemIndex].Amount += MyList[Index].Drops[i][1];
                        else
                            MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Add(new MyItem(MyList[Index].Drops[i][0], MyList[Index].Drops[i][1]));
                    }
                    Console.WriteLine("The " + Name + " dropped " + MyList[Index].Drops[i][1] + " " + ItemTable.MyTable[MyList[Index].Drops[i][0]].Name + ".");
                }
                MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities.Remove(MyList[Index]);
            }
            else if (MyList[Index].Act == Behaviour.Normal)
                MyList[Index].CurrentStatus = Status.Hostile;
            return true;
        }

        static bool Help(string Function)
        {
            switch (Function.ToLower())
            {
                case "go":
                    Console.WriteLine("Using the command Go and a direction (north, south, east or west), the player will move in that direction.");
                    break;
                case "attack":
                    Console.WriteLine("Using the command Attack and a target, will make the player to attack and damage the target.");
                    break;
                case "help":
                    Console.WriteLine("Using the command Help, the list and information of the commands is shown. With another command it will show the information of it.");
                    break;
                case "bag":
                    Console.WriteLine("Using the command Bag, the list of items the player have in its inventory is shown.");
                    break;
                case "eat":
                    Console.WriteLine("Using the command Eat and an item, will make the player to eat the item.");
                    break;
                case "cast":
                    Console.WriteLine("Using the command Cast, a spell and a target, will make the player cast the spell on that target");
                    break;
                case "spellbook":
                    Console.WriteLine("Using the command Spellbook, the list of spells the player know is shown.");
                    break;
                case "save":
                    //
                    break;
                case "load":
                    //
                    break;
            }
            return true;
        }

        static bool Help()
        {
            Console.WriteLine("The functions in the game are:");
            Console.WriteLine("- Attack (+ Target) : The player attacks the target with that name (if possible).");
            Console.WriteLine("- Bag : Shows the content of the player's inventory.");
            Console.WriteLine("- Cast (+ Spell + Target) : The player casts the spell on the target with that name (if possible).");
            Console.WriteLine("- Eat (+ Item) : The player eats the item (if possible).");
            Console.WriteLine("- Go (+ Direction) : The player moves to that direction (if possible).");
            Console.WriteLine("- Help : Shows all the commands in the game.");
            Console.WriteLine("- Help (+ Command) : Shows the explanation and syntax of a command.");
            Console.WriteLine("- Spellbook : Shows the spells the player knows.");
            return true;
        }

        /*static bool Equip(string Name, Player MyPlayer)
        {
            if (MyPlayer.MyInventory.Count == 0)
            {
                Console.WriteLine("There is no item in your inventory!");
                return false;
            }

            int Index = -1;
            bool IsExist = false;
            for (int i = 0; i < MyPlayer.MyInventory.Count && !IsExist; i++)
                if (MyPlayer.MyInventory[i].Name == Name)
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no item with this name in your inventory!");
                return false;
            }

            for (int i = 0; i < MyPlayer.MyInventory[Index].Types.Length; i++)
            {
                if (MyPlayer.MyInventory[Index].Types[i].Type == ItemType.Weapon)
                {
                    if (MyPlayer.MyWeapon != null)
                        MyPlayer.MyWeapon = null;
                    if (MyPlayer.MyTool != null)
                        MyPlayer.MyTool = null;
                    MyPlayer.MyWeapon = new Weapon(MyPlayer.MyInventory[Index].ID);
                }

                else if (MyPlayer.MyInventory[Index].Types[i].Type == ItemType.Tool)
                {
                    if (MyPlayer.MyWeapon != null)
                        MyPlayer.MyWeapon = null;
                    if (MyPlayer.MyTool != null)
                        MyPlayer.MyTool = null;
                    MyPlayer.MyTool = new Tool(MyPlayer.MyInventory[Index].ID);
                }
            }

            //put that item on you

            return true;
        }*/

        static bool Bag(Player MyPlayer)
        {
            Console.Write("Your Inventory: ");
            for (int i = 0; i < MyPlayer.MyInventory.Count - 1; i++)
                Console.Write(MyPlayer.MyInventory[i].Amount + " " + MyPlayer.MyInventory[i].Name + ", ");
            if (MyPlayer.MyInventory.Count != 0)
                Console.WriteLine(MyPlayer.MyInventory[MyPlayer.MyInventory.Count - 1].Amount + " " + MyPlayer.MyInventory[MyPlayer.MyInventory.Count - 1].Name + ".");
            return true;
        }

        static bool Eat(string Name, Player MyPlayer)
        {
            if (MyPlayer.MyInventory.Count == 0) //Might be deleted and only be informed that there is no the specific animal here
            {
                Console.WriteLine("There are no items in your inventory!");
                return false;
            }

            int Index = -1;
            bool IsExist = false;
            for (int i = 0; i < MyPlayer.MyInventory.Count && !IsExist; i++)
                if (MyPlayer.MyInventory[i].Name.ToLower() == Name.ToLower())
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no such an item with this name in your inventory!");
                return false;
            }

            int IndexType = -1;
            bool IsTypeExist = false;

            if (MyPlayer.MyInventory[Index].Types == null)
            {
                Console.WriteLine("You can't eat this item!");
                return false;
            }

            for (int i = 0; i < MyPlayer.MyInventory[Index].Types.Length; i++)
            {
                if (MyPlayer.MyInventory[Index].Types[i].Type == ItemType.Food)
                {
                    IndexType = i;
                    IsTypeExist = true;
                }
            }

            if (!IsTypeExist)
            {
                Console.WriteLine("You can't eat this item!");
                return false;
            }

            MyPlayer.CurrentHP += int.Parse(MyPlayer.MyInventory[Index].Types[IndexType].Props[0].Split('=')[1]);
            if (MyPlayer.CurrentHP > MyPlayer.MaxHP)
                MyPlayer.CurrentHP = MyPlayer.MaxHP;
            Console.WriteLine("You ate " + MyPlayer.MyInventory[Index].Name + " and gained " + int.Parse(MyPlayer.MyInventory[Index].Types[IndexType].Props[0].Split('=')[1]) + " HP.");
            Console.WriteLine("Your HP is now " + MyPlayer.CurrentHP + "/" + MyPlayer.MaxHP + ".");
            if (MyPlayer.MyInventory[Index].Amount > 1)
                MyPlayer.MyInventory[Index].Amount--;
            else
                MyPlayer.MyInventory.RemoveAt(Index);
            //לסיים!

            return true;
        }

        static void GainExp(Player MyPlayer, int MoreExp)
        {
            MyPlayer.Exp += MoreExp;
            Console.WriteLine("You earned " + MoreExp + " experience.");

            if (MyPlayer.Exp >= ExpTable.MyTable[MyPlayer.Level - 1].ExpNext)
            {
                MyPlayer.Exp -= ExpTable.MyTable[MyPlayer.Level - 1].ExpNext;
                MyPlayer.Level++;

                int ClassNum;
                switch (MyPlayer.Class)
                {
                    case Classes.Warrior:
                        ClassNum = 0;
                        break;
                    case Classes.Magician:
                        ClassNum = 1;
                        foreach (Spell s in SpellTable.MyTable)
                            if (s.Level == MyPlayer.Level)
                                MyPlayer.MySpellBook.Add(s);
                        break;
                    default:
                        ClassNum = 2;
                        break;
                }

                Console.WriteLine("Congrats! Your new level is " + MyPlayer.Level + "!");
                MyPlayer.MaxHP = MyPlayer.MaxHP + ExpTable.MyTable[MyPlayer.Level - 1].Stats[ClassNum].HpBonus;
                MyPlayer.CurrentHP = MyPlayer.CurrentHP + ExpTable.MyTable[MyPlayer.Level - 1].Stats[ClassNum].HpBonus;
                MyPlayer.BaseAtk = ExpTable.MyTable[MyPlayer.Level - 1].Stats[ClassNum].Atk;
                MyPlayer.BaseDef = ExpTable.MyTable[MyPlayer.Level - 1].Stats[ClassNum].Def;
                MyPlayer.BaseInt = ExpTable.MyTable[MyPlayer.Level - 1].Stats[ClassNum].Int;
                MyPlayer.BaseDex = ExpTable.MyTable[MyPlayer.Level - 1].Stats[ClassNum].Dex;
            }
        }

        static bool Cast(string Name, Player MyPlayer, string MyTarget)
        {
            foreach (Spell s in MyPlayer.MySpellBook)
            {
                if (s.Name.ToLower() == Name.ToLower())
                {
                    if (s.ManaCost > MyPlayer.CurrentMP)
                    {
                        Console.WriteLine("You don't have enough mana!");
                        return false;
                    }

                    if (s.MyType == SpellType.Attack && MyTarget != null)
                    {
                        if (Attack(MyTarget, MyPlayer, MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Entities, s.Amount))
                            MyPlayer.CurrentMP -= s.ManaCost;
                    }

                    if (s.MyType == SpellType.Heal)
                    {
                        MyPlayer.CurrentHP += Math.Min(s.Amount, MyPlayer.MaxHP - MyPlayer.CurrentHP);
                        MyPlayer.CurrentMP -= s.ManaCost;
                    }

                    return true;
                }
            }
            Console.WriteLine("This spell does not exist in your spellbook!");
            return false;
        }

        static bool SBook(Player MyPlayer)
        {
            Console.WriteLine("Your Spell Book:");
            for (int i = 0; i < MyPlayer.MySpellBook.Count; i++)
                Console.WriteLine(MyPlayer.MySpellBook[i].Name + ". MP: " + MyPlayer.MySpellBook[i].ManaCost + ".");
            return true;
        }

        static bool Save(Player MyPlayer)
        {
            StreamWriter saver = new StreamWriter("save " + DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + ".sav");

            saver.WriteLine(MyPlayer.MyWorld.Surface.GetLength(0) + "," + MyPlayer.MyWorld.Surface.GetLength(1));
            saver.WriteLine("end_size");

            //player
            saver.WriteLine(MyPlayer.Name);
            saver.WriteLine(MyPlayer.LocationX);
            saver.WriteLine(MyPlayer.LocationY);
            saver.WriteLine(MyPlayer.Class.ToString());
            saver.WriteLine(MyPlayer.Level);
            saver.WriteLine(MyPlayer.Exp);
            saver.WriteLine(MyPlayer.LocationX);
            saver.WriteLine(MyPlayer.LocationY);
            saver.WriteLine(MyPlayer.BaseAtk);
            saver.WriteLine(MyPlayer.BaseDef);
            saver.WriteLine(MyPlayer.BaseDex);
            saver.WriteLine(MyPlayer.BaseInt);
            saver.WriteLine(MyPlayer.MaxHP);
            saver.WriteLine(MyPlayer.CurrentHP);
            saver.WriteLine(MyPlayer.MaxMP);
            saver.WriteLine(MyPlayer.CurrentMP);
            saver.WriteLine("end_player");

            //inventory
            for (int i = 0; i < MyPlayer.MyInventory.Count - 1; i++)
                saver.Write(MyPlayer.MyInventory[i].ID + ",");
            if (MyPlayer.MyInventory.Count != 0)
                saver.WriteLine(MyPlayer.MyInventory[MyPlayer.MyInventory.Count - 1].ID);
            saver.WriteLine("end_inventory");

            //tools
            if (MyPlayer.MyWeapon != null)
                saver.WriteLine(MyPlayer.MyWeapon.ID);
            if (MyPlayer.MyTool != null)
                saver.WriteLine(MyPlayer.MyTool.ID);
            if (MyPlayer.MyArmor != null)
                saver.WriteLine(MyPlayer.MyArmor.ID);
            saver.WriteLine("end_tools");

            //spellbook
            for (int i = 0; i < MyPlayer.MySpellBook.Count - 1; i++)
                saver.Write(MyPlayer.MySpellBook[i].ID + ",");
            if (MyPlayer.MySpellBook.Count != 0)
                saver.WriteLine(MyPlayer.MySpellBook[MyPlayer.MySpellBook.Count - 1].ID);
            saver.WriteLine("end_spellbook");

            //world
            Plot[,] surf = MyPlayer.MyWorld.Surface;
            for (int i = 0; i < surf.GetLength(0); i++)
                for (int j = 0; j < surf.GetLength(1); j++)
                {
                    saver.Write(i + "," + j + ";" + surf[i, j].Location.ToString());
                    if (surf[i, j].Entities.Count != 0)
                        saver.Write(";");

                    for (int k = 0; k < surf[i, j].Entities.Count - 1; k++)
                    {
                        saver.Write(surf[i, j].Entities[k].ID + "-");
                        saver.Write(surf[i, j].Entities[k].HP + "-");
                        saver.Write(surf[i, j].Entities[k].CurrentStatus.ToString() + "-");

                        for (int l = 0; l < surf[i, j].Entities[k].Drops.Length - 1; l++)
                            saver.Write(surf[i, j].Entities[k].Drops[l][0] + "/" + surf[i, j].Entities[k].Drops[l][1] + "+");
                        if (surf[i, j].Entities[k].Drops.Length != 0)
                            saver.Write(surf[i, j].Entities[k].Drops[surf[i, j].Entities[k].Drops.Length - 1][0] + "/" + surf[i, j].Entities[k].Drops[surf[i, j].Entities[k].Drops.Length - 1][1] + ".");
                    }

                    if (surf[i, j].Entities.Count != 0)
                    {
                        saver.Write(surf[i, j].Entities[surf[i, j].Entities.Count - 1].ID + "-");
                        saver.Write(surf[i, j].Entities[surf[i, j].Entities.Count - 1].HP + "-");
                        saver.Write(surf[i, j].Entities[surf[i, j].Entities.Count - 1].CurrentStatus.ToString() + "-");

                        for (int l = 0; l < surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops.Length - 1; l++)
                            saver.Write(surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops[l][0] + "/" + surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops[l][1] + "+");
                        if (surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops.Length != 0)
                            saver.Write(surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops[surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops.Length - 1][0] + "/" + surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops[surf[i, j].Entities[surf[i, j].Entities.Count - 1].Drops.Length - 1][1]);
                    }

                    saver.WriteLine();
                }
            saver.WriteLine("end_world");

            saver.Close();

            return true;
        }

        static bool Pick(string Name, Player MyPlayer)
        {
            int Index = -1;
            bool IsExist = false;
            for (int i = 0; i < MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Count && !IsExist; i++)
                if (MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[i].Name.ToLower() == Name.ToLower())
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no such item with this name at your location!");
                return false;
            }

            MyPlayer.MyInventory.Add(new MyItem(MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[Index].ID, MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops[Index].Amount));
            MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.RemoveAt(Index);
            Console.WriteLine("You picked the " + Name + ".");
            return true;
        }

        static bool Drop(string Name, Player MyPlayer)
        {
            int Index = -1;
            bool IsExist = false;
            for (int i = 0; i < MyPlayer.MyInventory.Count && !IsExist; i++)
                if (MyPlayer.MyInventory[i].Name.ToLower() == Name.ToLower())
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no such item in your inventory!");
                return false;
            }

            MyPlayer.MyWorld.Surface[MyPlayer.LocationX, MyPlayer.LocationY].Drops.Add(new MyItem(MyPlayer.MyInventory[Index].ID, MyPlayer.MyInventory[Index].Amount));

            if (MyPlayer.MyArmor.ID == MyPlayer.MyInventory[Index].ID)
            {
                MyPlayer.MyArmor = null;
                Console.WriteLine("You dequipped the " + Name + " as you armor.");
            }
            if (MyPlayer.MyTool.ID == MyPlayer.MyInventory[Index].ID)
            {
                MyPlayer.MyTool = null;
                Console.WriteLine("You dequipped the " + Name + " as you tool.");
            }
            if (MyPlayer.MyWeapon.ID == MyPlayer.MyInventory[Index].ID)
            {
                MyPlayer.MyWeapon = null;
                Console.WriteLine("You dequipped the " + Name + " as you weapon.");
            }

            MyPlayer.MyInventory.RemoveAt(Index);
            Console.WriteLine("You dropped the " + Name + ".");
            return true;
        }

        static bool Equip(string Name, Player MyPlayer)
        {
            int Index = -1;
            bool IsExist = false;
            for (int i = 0; i < MyPlayer.MyInventory.Count && !IsExist; i++)
                if (MyPlayer.MyInventory[i].Name.ToLower() == Name.ToLower())
                {
                    Index = i;
                    IsExist = true;
                }

            if (!IsExist)
            {
                Console.WriteLine("There is no such item in your inventory!");
                return false;
            }

            bool IsEquip = false;
            for (int i = 0; i < MyPlayer.MyInventory[Index].Types.Length; i++)
                switch (MyPlayer.MyInventory[Index].Types[i].Type)
                {
                    case ItemType.Armor:
                        MyPlayer.MyArmor = new Armor(MyPlayer.MyInventory[Index].ID);
                        Console.WriteLine("You equipped the " + Name + " as you armor.");
                        IsEquip = true;
                        break;
                    case ItemType.Tool:
                        MyPlayer.MyTool = new Tool(MyPlayer.MyInventory[Index].ID);
                        Console.WriteLine("You equipped the " + Name + " as you tool.");
                        IsEquip = true;
                        break;
                    case ItemType.Weapon:
                        MyPlayer.MyWeapon = new Weapon(MyPlayer.MyInventory[Index].ID);
                        Console.WriteLine("You equipped the " + Name + " as you weapon.");
                        IsEquip = true;
                        break;
                }
            if (!IsEquip)
            {
                Console.WriteLine("This item can't be equipped!");
                return false;
            }
            return true;
        }

        static bool Open(string Name, Player MyPlayer)
        {
            switch (Name.ToLower())
            {
                case "chest":
                    
                    break;


                case "door":

                    break;
            }

            return true;
        }
    }
}
