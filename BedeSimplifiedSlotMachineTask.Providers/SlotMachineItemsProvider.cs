using BedeSimplifiedSlotMachine.Models.Contracts;
using BedeSimplifiedSlotMachineTask.Models.Models;
using BedeSimplifiedSlotMachineTask.Providers.Constants;
using System.Collections.Generic;

namespace BedeSimplifiedSlotMachineTask.Providers
{
    public class SlotMachineItemsProvider
    {

        public List<ISlotMachineItem> GetSlotMachineItems()
        {
            var itemsArray = new List<ISlotMachineItem>();
           
            var apple = new SlotMachineItem(ItemsConstants.AppleName, ItemsConstants.AppleCoefficients, ItemsConstants.AppleProbability, null, false, ItemsConstants.AppleSymbol);
            var banana = new SlotMachineItem(ItemsConstants.BananaName, ItemsConstants.BananaCoefficients, ItemsConstants.BananaProbability, null, false, ItemsConstants.BananaSymbol);
            var pineapple = new SlotMachineItem(ItemsConstants.PineappleName, ItemsConstants.PineappleCoefficients, ItemsConstants.PineappleProbability, null, false, ItemsConstants.PineappleSymbol);
            var wildcard = new SlotMachineItem(ItemsConstants.WildcardName, ItemsConstants.WildcardCoef, ItemsConstants.WildcardProbability, null, true, ItemsConstants.WildcardSymbol);

            var slotMachineItems = new List<ISlotMachineItem>
            {
                apple,
                banana,
                pineapple,
                wildcard
            };


            foreach (var item in slotMachineItems)
            {
                var imageProvider = new ImageProvider();

                item.Image = imageProvider.GetImageLocation(item.Symbol);
                for (int i = 0; i < item.ProbabilityPercent; i++)
                {
                    itemsArray.Add(item);
                }

            }

            return itemsArray;
        }
    }
}
