using BedeSimplifiedSlotMachine.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BedeSimplifiedSlotMachineTask.Providers
{
    public class MatrixProvider
    {
        public ISlotMachineItem[,] GenerateMatrix(IList<ISlotMachineItem> items, int rows, int cols)
        {
            var randomNumberProvider = new RandomNumberProvider();
            var slotMachiteItems = items;

            var matrix = new ISlotMachineItem[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var randomNumber = randomNumberProvider.GetRandomNumber(0, 100);

                    matrix[row, col] = slotMachiteItems[randomNumber];
                }
            }

            return matrix;
        }

        public PictureBox[,] GetPictureBoxMatrix(IEnumerable<PictureBox> pictureBoxes, int rows, int cols)
        {
            var picBoxes = new Stack<PictureBox>(pictureBoxes.OrderByDescending(x => x.Name));
            var picBoxMatrix = new PictureBox[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    picBoxMatrix[row, col] = picBoxes.Pop();
                }
            }

            return picBoxMatrix;
        }
    }
}
