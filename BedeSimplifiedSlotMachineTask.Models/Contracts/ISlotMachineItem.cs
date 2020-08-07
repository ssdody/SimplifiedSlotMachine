using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSimplifiedSlotMachine.Models.Contracts
{
    public interface ISlotMachineItem
    {
        string Name { get; set; }
        string Symbol { get; set; }
        decimal Coefficient { get; set; }
        int ProbabilityPercent { get; set; }
        string Image { get; set; }
        bool IsWildCard { get; set; }
    }
}
