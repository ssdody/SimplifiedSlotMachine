using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSimplifiedSlotMachine.Models.Contracts
{
    public interface ISlotMachineItem
    {
        string Name { get; set; }
        decimal Coefficient { get; set; }
        int Probability { get; set; }
        string Image { get; set; }
        bool IsWildCard { get; set; }
    }
}
