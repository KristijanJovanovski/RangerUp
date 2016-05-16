using System;
using System.Collections.Generic;

namespace RangerUp
{
    public class Player:IComparable<Player>,IComparer<Player>
    {
        public string Name;
        public long Score;
        public string Difficulty;

        public Player(string name , long score,string difficulty)
        {
            Name = name;
            Score = score;
            Difficulty = difficulty;
        }

        public int Compare(Player x, Player y)
        {
            if (x.Score > y.Score)
                return 1;
            if (x.Score < y.Score)
                return -1;
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        public int CompareTo(Player other)
        {
            if (Score > other.Score)
                return 1;
            if (Score < other.Score)
                return -1;
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}
