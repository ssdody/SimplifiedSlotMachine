using BedeSimplifiedSlotMachine.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSimplifiedSlotMachineTask.Models.Models
{
    public class SlotMachineItem : ISlotMachineItem
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Coefficient { get; set; }
        public int ProbabilityPercent { get; set; }
        public string Image { get; set; }
        public bool IsWildCard { get; set; }

        public SlotMachineItem(string name, decimal coefficient, int probabilityPercent, string image, bool isWildCard, string symbol)
        {
            Name = name;
            Coefficient = coefficient;
            ProbabilityPercent = probabilityPercent;
            Image = image;
            IsWildCard = isWildCard;
            Symbol = symbol;
        }
    }
}
