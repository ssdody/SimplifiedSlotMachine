using BedeSimplifiedSlotMachine.Models.Contracts;
using System;

namespace BedeSimplifiedSlotMachine.Models
{
    public class Banana : ISlotMachineItem
    {

        public string Name { get; set; }
        public decimal Coefficient { get; set; }
        public int Probability { get; set; }
        public string Image { get; set; }
        public bool IsWildCard { get; set; }
    }
}
