using BedeSimplifiedSlotMachine.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSimplifiedSlotMachine.Models
{
    class Pineapple : ISlotMachineItem
    {
        public string Name { get; set; }
        public decimal Coefficient { get; set; }
        public int Probability { get; set; }
        public string Image { get; set; }
        public bool IsWildCard { get; set; }
    }
}
