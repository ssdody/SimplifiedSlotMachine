namespace BedeSimplifiedSlotMachineTask
{
    using System;
    using System.Linq;
    using System.Drawing;
    using System.Resources;
    using System.Collections;
    using System.Windows.Forms;
    using System.Globalization;
    using System.Collections.Generic;
    using BedeSimplifiedSlotMachine.Helpers;
    using BedeSimplifiedSlotMachineTask.Providers;
    using BedeSimplifiedSlotMachineTask.Models.Enum;
    using BedeSimplifiedSlotMachine.Models.Contracts;
    using BedeSimplifiedSlotMachineTask.Models.Models;
    using BedeSimplifiedSlotMachineTask.Providers.Constants;

    public partial class SlotMachine : Form
    {
        private decimal bet;
        private decimal credits;
        private readonly RandomNumberProvider randomProvider;
        private readonly MatrixProvider matrixProvider;
        private readonly SlotMachineItemsProvider slotMachineItemsProvider;



        public SlotMachine()
        {
            InitializeComponent();

            this.randomProvider = new RandomNumberProvider();
            this.matrixProvider = new MatrixProvider();
            this.slotMachineItemsProvider = new SlotMachineItemsProvider();

            var dialogResult = Prompt.ShowEnterCreditsDialog("Deposit amount", "Enter Deposit");
            SetCreditsAmount(dialogResult);

            SetDefaultSlotMachineImages();
        }

        private void SetDefaultSlotMachineImages()
        {
            var picBoxes = new Stack<PictureBox>(Controls.OfType<PictureBox>());
            foreach (var box in picBoxes)
            {
                box.Image = Properties.Resources.q;
            }
        }

        private void SetCreditsAmount(decimal dialogResult)
        {
            credits = dialogResult;

            CreditsVal.Text = dialogResult.ToString();
        }

        private void FillSlotMachinePictireBoxesWithSlotMAchineItemsImages(ISlotMachineItem[,] matrix, PictureBox[,] pictureBoxMatrix)
        {
            ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, false, true);
            var imageDictionary = new Dictionary<string, Bitmap>();

            foreach (DictionaryEntry entry in resourceSet)
            {
                String name = entry.Key.ToString();
                var resource = (Bitmap)entry.Value;
                imageDictionary.Add(name, resource);
            }

            for (int row = 0; row < MatrixSizeConstants.Rows; row++)
            {
                for (int col = 0; col < MatrixSizeConstants.Cols; col++)
                {
                    pictureBoxMatrix[row, col].Image = imageDictionary[(matrix[row, col].Symbol)];
                }
            }

        }

        //private PictureBox[,] GetPictureBoxMatrix(int rows, int cols)
        //{
        //    var picBoxes = new Stack<PictureBox>(Controls.OfType<PictureBox>().OrderByDescending(x => x.Name));
        //    var picBoxMatrix = new PictureBox[MatrixSizeConstants.Rows, MatrixSizeConstants.Cols];

        //    for (int row = 0; row < MatrixSizeConstants.Rows; row++)
        //    {
        //        for (int col = 0; col < MatrixSizeConstants.Cols; col++)
        //        {
        //            picBoxMatrix[row, col] = picBoxes.Pop();
        //        }
        //    }

        //    return picBoxMatrix;
        //}

        private static void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SpinBtn_Click(object sender, EventArgs e)
        {
            this.bet = betNumericUpDown.Value;

            this.credits = Convert.ToDecimal(CreditsVal.Text);

            if (bet <= this.credits && bet > 0)
            {
                RemoveBetFromCredits(this.credits, this.bet);

                var slotMachineItems = this.slotMachineItemsProvider.GetSlotMachineItems();
                var SlotMachineItemsArray = SlotMachineHelper.Shuffle(slotMachineItems);

                var slotMachineItemsMatrix = this.matrixProvider.GenerateMatrix(SlotMachineItemsArray, MatrixSizeConstants.Rows, MatrixSizeConstants.Cols);
                var pictureBoxes = Controls.OfType<PictureBox>();

                var picBoxMatrix = matrixProvider.GetPictureBoxMatrix(pictureBoxes, MatrixSizeConstants.Rows, MatrixSizeConstants.Cols);
                FillSlotMachinePictireBoxesWithSlotMAchineItemsImages(slotMachineItemsMatrix, picBoxMatrix);

                var winCoef = GetTotalWinningRowsCoefficient(slotMachineItemsMatrix);
                if (winCoef > 0)
                {
                    var winAmount = AddCreditsToCreditsAmount(this.bet, winCoef);

                    SetSpinResultText(SpinResultText.Win, winAmount);
                }
                else
                {
                    SetSpinResultText(SpinResultText.Loss, bet);
                }
            }
            else if (this.credits <= 0)
            {
                decimal dialogResult = Prompt.ShowEnterCreditsDialog("You don't have enough credits. Please add more to continue playing", "Not enough credits");

                SetCreditsAmount(dialogResult);
            }
            else if (this.bet > this.credits)
            {
                Prompt.ShowInformationDialog("You're bet is bigger than your credits. Please lower your bet to continue playing", "Bet bigger than credits");

            }
        }

        private void SetSpinResultText(SpinResultText result, decimal credits)
        {
            WinLabel.Text = result.ToString();
            WinVal.Text = credits.ToString();
        }

        private void RemoveBetFromCredits(decimal creditsAmount, decimal betAmount)
        {
            this.credits = creditsAmount - betAmount;
            CreditsVal.Text = credits.ToString();
        }

        private decimal AddCreditsToCreditsAmount(decimal bet, decimal winCoef)
        {
            decimal winAmount = Math.Round((bet * winCoef), 2);
            this.credits += winAmount;

            CreditsVal.Text = Math.Round(this.credits, 2).ToString();

            return winAmount;
        }

        private decimal GetTotalWinningRowsCoefficient(ISlotMachineItem[,] matrix)
        {
            decimal totalCoefficient = 0;

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                decimal rowCoefficient = 0;
                bool isWinningRow = true;
                ISlotMachineItem prevItem = null;

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    var currentItem = matrix[row, col];

                    if (currentItem.IsWildCard)
                    {
                        continue;
                    }

                    if (prevItem != null && prevItem != currentItem)
                    {
                        isWinningRow = false;

                        break;
                    }

                    rowCoefficient += currentItem.Coefficient;
                    prevItem = currentItem;

                }
                if (isWinningRow)
                {
                    totalCoefficient += rowCoefficient;
                }

            }
            return totalCoefficient;
        }

        private void betNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.bet = betNumericUpDown.Value;
        }

        public void SetMatrixItemsImages()
        {

        }

        //public ISlotMachineItem[,] GenerateMatrix(IList<ISlotMachineItem> items)
        //{
        //    var slotMachiteItems = items;

        //    int rows = MatrixSizeConstants.Rows;
        //    int cols = MatrixSizeConstants.Cols;

        //    var matrix = new ISlotMachineItem[rows, cols];
        //    for (int row = 0; row < rows; row++)
        //    {
        //        for (int col = 0; col < cols; col++)
        //        {
        //            var randomNumber = GetRandomNumber(0, 100);

        //            matrix[row, col] = slotMachiteItems[randomNumber];
        //        }
        //    }

        //    return matrix;
        //}

        //int GetRandomNumber(int minRange, int maxRange)
        //{
        //    return this.randomProvider.Next(minRange, maxRange);
        //}

        //List<ISlotMachineItem> GetSlotMachineItems()
        //{
        //    var itemsArray = new List<ISlotMachineItem>();

        //    var apple = new SlotMachineItem("Apple", 0.4m, 45, null, false, "a");
        //    var banana = new SlotMachineItem("Banana", 0.6m, 35, null, false, "b");
        //    var pineapple = new SlotMachineItem("Pineapple", 0.8m, 15, null, false, "p");
        //    var wildcard = new SlotMachineItem("Wildcard", 0, 5, null, true, "w");

        //    var slotMachineItems = new List<ISlotMachineItem>
        //    {
        //        apple,
        //        banana,
        //        pineapple,
        //        wildcard
        //    };


        //    foreach (var item in slotMachineItems)
        //    {
        //        var imageProvider = new ImageProvider();

        //        item.Image = imageProvider.GetImageLocation(item.Symbol);
        //        for (int i = 0; i < item.ProbabilityPercent; i++)
        //        {
        //            itemsArray.Add(item);
        //        }

        //    }

        //    return itemsArray;
        //}

        private static Random random = new Random();

        public List<ISlotMachineItem> Shuffle(List<ISlotMachineItem> list)
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
