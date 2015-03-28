using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelAdventure
{
    class Chest
    {
        public bool Locked;
        public MyItem[] Treasure;
        public int x;
        public int y;

        public Chest(bool Locked, int Seed, int x, int y)
        {
            this.Locked = Locked;
            this.x = x;
            this.y = y;

            byte[] itemBytes = new byte[6];
            if (Seed < 1)
                new Random().NextBytes(itemBytes);
            else
                new Random(Seed).NextBytes(itemBytes);

            Treasure = new MyItem[itemBytes[0] % 5 + 1];
            for (int i = 0; i < itemBytes[0] % 5 + 1; i++)
                Treasure[i] = new MyItem(itemBytes[i + 1] % ItemTable.MyTable.Length, 1);
        }

        public Chest(bool Locked, MyItem[] Items, int x, int y)
        {
            this.Locked = Locked;
            Treasure = Items;
            this.x = x;
            this.y = y;
        }

        public bool Open(bool Key, Player MyPlayer)
        {
            if (Locked)
            {
                if (Key)
                {
                    int itemCount = -1;
                    for (int i = 0; i < MyPlayer.MyInventory.Count; i++)
                        if (MyPlayer.MyInventory[i].ID == 8)
                            itemCount = i;
                    if (itemCount == -1)
                        return false;

                    if (MyPlayer.MyInventory[itemCount].Amount == 1)
                        MyPlayer.MyInventory.RemoveAt(itemCount);
                    else
                        MyPlayer.MyInventory[itemCount].Amount--;

                    foreach (MyItem item in Treasure)
                        MyPlayer.MyWorld.Surface[x, y].Drops.Add(item);
                    Treasure = null;
                    x = y = -1;

                    return true;
                }
                else
                    return false;
            }
            else
            {
                foreach (MyItem item in Treasure)
                    MyPlayer.MyWorld.Surface[x, y].Drops.Add(item);
                Treasure = null;
                x = y = -1;
                return true;
            }
        }
    }

    class Door
    {
        public bool Locked;
        public bool Opened;

        public Door(bool Locked)
        {
            this.Locked = Locked;
            this.Opened = false;
        }

        public bool Open(bool Key, Player MyPlayer)
        {
            if (Locked)
            {
                if (Key)
                {
                    int itemCount = -1;
                    for (int i = 0; i < MyPlayer.MyInventory.Count; i++)
                        if (MyPlayer.MyInventory[i].ID == 8)
                            itemCount = i;
                    if (itemCount == -1)
                        return false;

                    if (MyPlayer.MyInventory[itemCount].Amount == 1)
                        MyPlayer.MyInventory.RemoveAt(itemCount);
                    else
                        MyPlayer.MyInventory[itemCount].Amount--;

                    Locked = false;
                    Opened = true;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                Opened = true;
                return true;
            }
        }

        public bool Close()
        {
            if (Opened)
            {
                Opened = false;
                return true;
            }
            else
                return false;
        }
    }
}
