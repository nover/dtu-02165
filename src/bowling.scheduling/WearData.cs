using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public static class LaneWearData
    {
        private static List<int> wear = new List<int>();

        public static void Populate(int numLanes)
        {
            for (int i = 0; i < numLanes; i++)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100);
                wear.Add(randomNumber);
            }
        }

        public static void CountUpWearForLane(int lane)
        {
            wear[lane] = +1;
        }

        public static int GetWearForLane(int lane)
        {
            return wear[lane];
        }

        public static int GetWearForLanes(int lane, int numOfLanes)
        {
            int wearVal = 0;
            for (int i = lane; i < lane + numOfLanes; i++)
            {
                try
                {
                    wearVal += wear[i];
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    // Do nothing
                }
            }
            return wearVal;
        }
    }
}
