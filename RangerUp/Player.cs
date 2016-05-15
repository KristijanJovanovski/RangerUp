using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RangerUp
{
    public class Player:IComparer<Player>
    {
        public String name;
        public long score;

        public Player(string _name , long _score)
        {
            name = _name;
            score = _score;
        }
        public int Compare(Player x, Player y)
        {
            if (x.score > y.score)
                return 1;
            if (x.score < y.score)
                return -1;
            return x.name.CompareTo(y.name);
        }
    }
}
