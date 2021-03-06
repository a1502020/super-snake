﻿using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Util
{
    public class PlayerInfo
    {
        public string Name { get; private set; }

        public PlayerInfo(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PlayerInfo);
        }

        public bool Equals(PlayerInfo other)
        {
            if ((object)other == null) return false;
            return this.Name.Equals(other.Name);
        }

        public static bool operator ==(PlayerInfo l, PlayerInfo r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(PlayerInfo l, PlayerInfo r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
