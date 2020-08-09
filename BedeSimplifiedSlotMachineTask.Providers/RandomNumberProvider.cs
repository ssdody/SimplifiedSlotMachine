using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSimplifiedSlotMachineTask.Providers
{
    public class RandomNumberProvider
    {
        private Random random;

        public RandomNumberProvider()
        {
            this.random = new Random();
        }

        public int GetRandomNumber(int minRange, int maxRange)
        {
            return this.random.Next(minRange, maxRange);
        }

    }
}
