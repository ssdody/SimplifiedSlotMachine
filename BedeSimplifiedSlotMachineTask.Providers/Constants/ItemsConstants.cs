using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSimplifiedSlotMachineTask.Providers.Constants
{
    public static class ItemsConstants
    {
        public const string AppleName = "Apple";
        public const string BananaName = "Banana";
        public const string PineappleName = "Pineapple";
        public const string WildcardName = "Wildcard";

        public const string AppleSymbol = "a";
        public const string BananaSymbol = "b";
        public const string PineappleSymbol = "p";
        public const string WildcardSymbol = "w";

        public const decimal AppleCoefficients = 0.4m;
        public const decimal BananaCoefficients = 0.6m;
        public const decimal PineappleCoefficients = 0.8m;
        public const decimal WildcardCoef = 0m;

        public const int AppleProbability = 45;
        public const int BananaProbability = 35;
        public const int PineappleProbability = 15;
        public const int WildcardProbability = 5;
    }
}
