namespace BedeSimplifiedSlotMachine.Helpers
{
    using BedeSimplifiedSlotMachine.Models.Contracts;
    using System;
    using System.Collections.Generic;

    public class SlotMachineHelper
    {
        private static Random random = new Random();

        public static List<ISlotMachineItem> Shuffle(List<ISlotMachineItem> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ISlotMachineItem value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
