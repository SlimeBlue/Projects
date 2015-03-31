using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelAdventure
{
    enum NPCTypes { Merchant, Mercenary, King, Bodyguard, Gatekeeper, Innkeeper, Drunk };

    class NPC
    {
        public string Name;
        public NPCTypes[] Types;
        public int HP;
        //מה עוד?
    }
}
