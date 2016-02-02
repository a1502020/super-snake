using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeStandalone
{
    public class Key
    {
        public void Update()
        {
            flip = 1 - flip;
            DX.GetHitKeyStateAll(out keys[flip][0]);
            Parallel.ForEach(Enumerable.Range(0, 256), i =>
            {
                if (keys[flip][i] == 0 ^ keys[1 - flip][i] == 0)
                {
                    times[i] = 1;
                }
                else
                {
                    ++times[i];
                }
            });
        }

        public bool IsPressing(int key)
        {
            return keys[flip][key] != 0;
        }

        public int GetPressingTime(int key)
        {
            return IsPressing(key) ? times[key] : 0;
        }

        public bool IsPressed(int key)
        {
            return keys[flip][key] != 0 && keys[1 - flip][key] == 0;
        }

        public bool IsReleasing(int key)
        {
            return keys[flip][key] == 0;
        }

        public int GetReleasingTime(int key)
        {
            return IsReleasing(key) ? times[key] : 0;
        }

        public bool IsReleased(int key)
        {
            return keys[flip][key] == 0 && keys[1 - flip][key] != 0;
        }

        private byte[][] keys = { new byte[256], new byte[256] };
        private int flip = 0;
        private int[] times = new int[256];
    }
}
